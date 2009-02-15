using Headless.Configuration;
using Headless.Threading;

namespace Headless
{
    public class CheckTriggersWorker : QueuedWorker<ProjectRelatedJob>
    {
        public CheckTriggersWorker(
            string workerName,
            JobQueue<ProjectRelatedJob> jobQueue,
            JobQueue<ProjectRelatedJob> buildQueue,
            IThreadFactory threadFactory,
            IProjectRegistry projectRegistry,
            IWorkerMonitor workerMonitor) : base(workerName, jobQueue, threadFactory, workerMonitor)
        {
            this.buildQueue = buildQueue;
            this.projectRegistry = projectRegistry;
        }

        protected override void ExecuteJob(ProjectRelatedJob job)
        {
            Project project = projectRegistry.GetProject(job.ProjectId);

            foreach (ITrigger trigger in project.Triggers)
            {
                if (trigger.IsTriggered())
                {
                    project.Status = ProjectStatus.Building;
                    buildQueue.Enqueue(new ProjectRelatedJob(project.ProjectId));
                }
            }
        }

        private readonly JobQueue<ProjectRelatedJob> buildQueue;
        private readonly IProjectRegistry projectRegistry;
    }
}