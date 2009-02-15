using System;
using System.Globalization;
using System.Threading;
using Headless.Threading;

namespace Headless
{
    public class HeadlessService : IDisposable
    {
        public HeadlessService(IProjectRegistry projectRegistry, IThreadFactory threadFactory, IWorkerMonitor workerMonitor)
        {
            this.projectRegistry = projectRegistry;
            this.threadFactory = threadFactory;
            this.workerMonitor = workerMonitor;
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

        public void Start()
        {
            stopSignal = new ManualResetEvent(false);

            buildQueue = new JobQueue<ProjectRelatedJob>("Build queue", stopSignal);

            for (int i = 0; i < buildWorkersCount; i++)
            {
                BuildWorker worker = new BuildWorker(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "BuildWorker {0}",
                        i),
                    buildQueue,
                    threadFactory,
                    projectRegistry,
                    workerMonitor);
                buildQueue.AddWorker(worker);
            }

            buildQueue.StartWorkers();

            checkTriggersQueueFeeder = new CheckTriggersQueueFeeder(
                stopSignal,
                buildQueue,
                threadFactory,
                workerMonitor,
                projectRegistry);
            checkTriggersQueueFeeder.Start();
        }

        public void Stop(TimeSpan timeout)
        {
            stopSignal.Set();
            threadFactory.WaitForAllThreadsToStop(timeout);

            stopSignal.Close();
            stopSignal = null;
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
                // clean native resources         

                if (disposing)
                {
                    // clean managed resources
                    if (checkTriggersQueueFeeder != null)
                        checkTriggersQueueFeeder.Dispose();
                    if (buildQueue != null)
                        buildQueue.Dispose();
                    if (stopSignal != null)
                        stopSignal.Close();
                    if (threadFactory != null)
                        threadFactory.Dispose();
                }

                disposed = true;
            }
        }

        private JobQueue<ProjectRelatedJob> buildQueue;
        private int buildWorkersCount = 3;
        private CheckTriggersQueueFeeder checkTriggersQueueFeeder;
        private bool disposed;
        private IProjectRegistry projectRegistry;
        private EventWaitHandle stopSignal;
        private readonly IThreadFactory threadFactory;
        private IWorkerMonitor workerMonitor;
    }
}
