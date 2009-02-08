using Headless.Threading;

namespace Headless
{
    public class CheckTriggersWorker : QueuedWorker<ProjectRelatedJob>
    {
        public CheckTriggersWorker(
            string workerName,
            JobQueue<ProjectRelatedJob> jobQueue, 
            IWorkerMonitor workerMonitor) : base(workerName, jobQueue, workerMonitor)
        {
        }

        protected override void ExecuteJob(ProjectRelatedJob job)
        {
            throw new System.NotImplementedException();
        }
    }
}