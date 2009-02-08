using System;
using System.Threading;
using Headless.Threading;

namespace Headless.Threading
{
    public abstract class QueuedWorker<TJob> : Worker where TJob : Job
    {
        protected QueuedWorker(string workerName, JobQueue<TJob> jobQueue, IWorkerMonitor workerMonitor)
            : base(workerName, jobQueue.StopWorkingSignal, workerMonitor)
        {
            this.jobQueue = jobQueue;
        }

        public JobQueue<TJob> JobQueue
        {
            get { return jobQueue; }
        }

        protected abstract void ExecuteJob(TJob job);

        protected override void Run()
        {
            while (true)
            {
                WorkerMonitor.SendReport(this.Thread.Name, WorkerActivityType.WaitingForWork, null);

                TJob job;
                int result = jobQueue.WaitForJob(TimeSpan.FromSeconds(10), out job);

                if (result == 0)
                    break;
                if (result == WaitHandle.WaitTimeout)
                    continue;

                WorkerMonitor.SendReport(this.Thread.Name, WorkerActivityType.ExecutingWork, null);
                ExecuteJob(job);
            }
        }

        private readonly JobQueue<TJob> jobQueue;
    }
}