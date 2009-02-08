using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace Headless.Threading
{
    [SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
    public class JobQueue<TJob> : IDisposable 
        where TJob : Job 
    {
        public JobQueue(WaitHandle stopWorkingSignal)
        {
            this.stopWorkingSignal = stopWorkingSignal;
            signals = new WaitHandle[2];
            signals[0] = stopWorkingSignal;
            signals[1] = jobsInQueueSignal;
        }

        public bool IsEmpty
        {
            get { return queue.Count == 0; }
        }

        public WaitHandle StopWorkingSignal
        {
            get { return stopWorkingSignal; }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or
        /// resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void AddWorker (QueuedWorker<TJob> worker)
        {
            workers.Add(worker);
        }

        public void Enqueue (TJob job)
        {
            lock (this)
            {
                queue.Enqueue(job);
                jobsInQueueSignal.Set();
                emptyQueueSignal.Reset();
            }
        }

        public bool IsInQueue (string correlationId)
        {
            lock (this)
            {
                foreach (TJob workerRequest in queue)
                {
                    if (workerRequest.CorrelationId == correlationId)
                        return true;
                }

                return false;
            }
        }

        public void StartWorkers()
        {
            foreach (QueuedWorker<TJob> worker in workers)
            {
                worker.Start();
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#")]
        public int WaitForJob(TimeSpan timeout, out TJob job)
        {
            job = null;

            int result = WaitHandle.WaitAny(signals, timeout);
            if (result == 1)
            {
                lock (this)
                {
                    job = queue.Dequeue();
                    if (queue.Count > 0)
                        jobsInQueueSignal.Set();
                    else
                        emptyQueueSignal.Set();
                }
            }

            return result;
        }

        public bool WaitForQueueToEmpty(TimeSpan timeout)
        {
            return emptyQueueSignal.WaitOne(timeout);
        }

        public void WaitForWorkersToStop (TimeSpan workerStoppingTimeout)
        {
            List<QueuedWorker<TJob>> workersStilLRunning = new List<QueuedWorker<TJob>>();

            foreach (QueuedWorker<TJob> worker in workers)
            {
                if (worker.Thread.IsAlive)
                    workersStilLRunning.Add(worker);
            }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            while (workersStilLRunning.Count > 0 && stopwatch.Elapsed < workerStoppingTimeout)
            {
                for (int i = 0; i < workersStilLRunning.Count;)
                {
                    Worker worker = workersStilLRunning[i];
                    if (false == worker.Thread.IsAlive)
                        workersStilLRunning.RemoveAt(i);
                    else
                        i++;
                }

                Thread.Sleep(TimeSpan.FromSeconds(1));
            }

            foreach (QueuedWorker<TJob> worker in workersStilLRunning)
                worker.Thread.Abort();
        }

        /// <summary>
        /// Disposes the object.
        /// </summary>
        /// <param name="disposing">If <code>false</code>, cleans up native resources. 
        /// If <code>true</code> cleans up both managed and native resources</param>
        protected virtual void Dispose(bool disposing)
        {
            if (false == disposed)
            {
                if (disposing)
                {
                    jobsInQueueSignal.Close();
                }

                disposed = true;
            }
        }

        private bool disposed;
        private EventWaitHandle emptyQueueSignal = new ManualResetEvent(true);
        private Queue<TJob> queue = new Queue<TJob>();
        private EventWaitHandle jobsInQueueSignal = new AutoResetEvent(false);
        private WaitHandle[] signals;
        private readonly WaitHandle stopWorkingSignal;
        private List<QueuedWorker<TJob>> workers = new List<QueuedWorker<TJob>>();
    }
}