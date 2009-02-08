using System;
using System.Threading;
using Headless.Configuration;
using Headless.Threading;

namespace Headless
{
    public class CheckTriggersQueueFeeder : Worker
    {
        public CheckTriggersQueueFeeder(
            WaitHandle stopSignal, 
            IWorkerMonitor workerMonitor,
            IProjectRegistryProvider projectRegistryProvider)
            : base("CheckTriggersQueueFeeder", stopSignal, workerMonitor)
        {
            this.projectRegistryProvider = projectRegistryProvider;
            this.checkTriggersQueue = new JobQueue<ProjectRelatedJob> (stopSignal);
        }

        protected override void Run()
        {
            while (true)
            {
                if (true == WaitForStopSignal(TimeSpan.FromSeconds(10)))
                    break;

                ProjectRegistry projectRegistry = projectRegistryProvider.GetProjectRegistry();

                foreach (Project project in projectRegistry.ListProjects())
                {
                    if (project.Status == ProjectStatus.Listening)
                    {
                        // skip projects which are already in the queue
                        if (false == checkTriggersQueue.IsInQueue(project.ProjectId))
                            checkTriggersQueue.Enqueue(new ProjectRelatedJob(project.ProjectId));
                    }
                }
            }
        }

        private JobQueue<ProjectRelatedJob> checkTriggersQueue;
        private IProjectRegistryProvider projectRegistryProvider;
    }
}