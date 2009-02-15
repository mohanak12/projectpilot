using System;
using System.Globalization;
using System.Threading;
using Headless.Configuration;
using Headless.Threading;

namespace Headless
{
    public class CheckTriggersQueueFeeder : Worker
    {
        public CheckTriggersQueueFeeder(
            WaitHandle stopSignal,
            JobQueue<ProjectRelatedJob> buildQueue,
            IThreadFactory threadFactory,
            IWorkerMonitor workerMonitor,
            IProjectRegistry projectRegistry)
            : base("CheckTriggersQueueFeeder", stopSignal, threadFactory, workerMonitor)
        {
            this.projectRegistry = projectRegistry;
            this.checkTriggersQueue = new JobQueue<ProjectRelatedJob> ("Check triggers queue", stopSignal);

            for (int i = 0; i < checkTriggersWorkersCount; i++)
            {
                CheckTriggersWorker worker = new CheckTriggersWorker(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "CheckTriggersWorker {0}",
                        i),
                    checkTriggersQueue, 
                    buildQueue,
                    threadFactory,
                    projectRegistry,
                    workerMonitor);
                checkTriggersQueue.AddWorker(worker);
            }

            checkTriggersQueue.StartWorkers();
        }

        public JobQueue<ProjectRelatedJob> CheckTriggersQueue
        {
            get { return checkTriggersQueue; }
        }

        protected override void Run()
        {
            while (true)
            {
                foreach (string projectId in projectRegistry.ListProjects())
                {
                    Project project = projectRegistry.GetProject(projectId);
                    if (project.Status == ProjectStatus.Sleeping)
                    {
                        project.Status = ProjectStatus.CheckingTriggers;
                        checkTriggersQueue.Enqueue(new ProjectRelatedJob(project.ProjectId));
                    }
                }

                if (true == WaitForStopSignal(TimeSpan.FromSeconds(10)))
                    break;
            }
        }

        private JobQueue<ProjectRelatedJob> checkTriggersQueue;
        private int checkTriggersWorkersCount = 3;
        private IProjectRegistry projectRegistry;
    }
}