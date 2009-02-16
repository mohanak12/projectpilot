using System;
using System.Diagnostics.CodeAnalysis;
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
            IWorkerMonitor workerMonitor,
            IHeadlessLogger headlessLogger) 
            : base(workerName, buildQueue, threadFactory, workerMonitor)
        {
            this.projectRegistry = projectRegistry;
            this.headlessLogger = headlessLogger;
        }

        protected override void ExecuteJob(ProjectRelatedJob job)
        {
            using (IBuildRunner buildRunner = projectRegistry.RegisterBuild(job.ProjectId))
            {
                BuildReport report = buildRunner.Run();
            }

            Project project = projectRegistry.GetProject(job.ProjectId);
            project.Status = ProjectStatus.Sleeping;
        }

        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private readonly IHeadlessLogger headlessLogger;
        private readonly IProjectRegistry projectRegistry;
    }
}