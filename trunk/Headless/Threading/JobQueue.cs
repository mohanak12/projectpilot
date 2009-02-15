using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using log4net;

namespace Headless.Threading
{
    [SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
    public class JobQueue<TJob> : IDisposable 
        where TJob : Job 
    {
        public JobQueue(string queueName, WaitHandle stopWorkingSignal)
        {
            this.queueName = queueName;
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

        public void AddWorker (QueuedWorker<TJob> worker)
        {
            if (log.IsDebugEnabled)
                log.DebugFormat("JobQueue '{0}': added worker '{1}'", queueName, worker.WorkerName);

            workers.Add(worker);
        }

        public void AssertAllThreadsAlive()
        {
            foreach (QueuedWorker<TJob> worker in workers)
            {
                if (false == worker.Thread.IsAlive)
                    throw new InvalidOperationException("At least one worker thread is dead");
            }            
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

        public void Enqueue (TJob job)
        {
            lock (this)
            {
                if (log.IsDebugEnabled)
                    log.DebugFormat("JobQueue '{0}': added job, {1} jobs in queue now", queueName, queue.Count);

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
                    if (queue.Count > 0)
                    {
                        job = queue.Dequeue();
                        if (queue.Count > 0)
                            jobsInQueueSignal.Set();
                        else
                            emptyQueueSignal.Set();
                    }
                    else
                    {
                        result = WaitHandle.WaitTimeout;
                    }
                }
            }

            return result;
        }

        public bool WaitForQueueToEmpty(TimeSpan timeout)
        {
            return emptyQueueSignal.WaitOne(timeout);
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
        private EventWaitHandle jobsInQueueSignal = new AutoResetEvent(false);
        private static readonly ILog log = LogManager.GetLogger(typeof(JobQueue<TJob>));
        private Queue<TJob> queue = new Queue<TJob>();
        private readonly string queueName;
        private WaitHandle[] signals;
        private readonly WaitHandle stopWorkingSignal;
        private List<QueuedWorker<TJob>> workers = new List<QueuedWorker<TJob>>();
    }
}