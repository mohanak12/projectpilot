using System;
using Headless.Configuration;
using Headless.Threading;

namespace Headless
{
    public class BuildWorker : QueuedWorker<ProjectRelatedJob>
    {
        public BuildWorker(
            string workerName, 
            JobQueue<ProjectRelatedJob> buildQueue, 
            IThreadFactory threadFactory,
            IProjectRegistry projectRegistry,
            IWorkerMonitor workerMonitor) 
            : base(workerName, buildQueue, threadFactory, workerMonitor)
        {
            this.projectRegistry = projectRegistry;
        }

        protected override void ExecuteJob(ProjectRelatedJob job)
        {
            Project project = projectRegistry.GetProject(job.ProjectId);

            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(2));
            project.Status = ProjectStatus.Sleeping;
        }

        private readonly IProjectRegistry projectRegistry;
    }
}