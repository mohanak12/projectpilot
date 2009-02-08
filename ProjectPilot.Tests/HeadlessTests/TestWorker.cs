using System;
using System.Threading;
using Headless.Threading;
using log4net;

namespace ProjectPilot.Tests.HeadlessTests
{
    public class TestWorker : QueuedWorker<Job>
    {
        public TestWorker(string workerName, JobQueue<Job> jobQueue, IWorkerMonitor workerMonitor) : base(workerName, jobQueue, workerMonitor)
        {
        }

        protected override void ExecuteJob(Job job)
        {
            log.DebugFormat("Thread '{0}': Executing job", Thread.Name);
            Thread.Sleep(TimeSpan.FromSeconds(1));
        }

        private static readonly ILog log = LogManager.GetLogger(typeof(TestWorker));
    }
}