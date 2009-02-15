using System;
using System.Threading;

namespace Headless.Threading
{
    public abstract class Worker : IDisposable
    {
        protected Worker(string workerName, WaitHandle stopSignal, IThreadFactory threadFactory, IWorkerMonitor workerMonitor)
        {
            this.threadFactory = threadFactory;
            this.workerName = workerName;
            this.stopSignal = stopSignal;
            this.workerMonitor = workerMonitor;
        }

        public Thread Thread
        {
            get { return thread; }
        }

        public void Start()
        {
            thread = threadFactory.CreateThread(workerName, Run);
            thread.Start();
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

        public IWorkerMonitor WorkerMonitor
        {
            get { return workerMonitor; }
        }

        public string WorkerName
        {
            get { return workerName; }
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
                }

                disposed = true;
            }
        }

        protected abstract void Run();

        protected bool WaitForStopSignal (TimeSpan timeout)
        {
            return stopSignal.WaitOne(timeout);           
        }

        private bool disposed;
        private WaitHandle stopSignal;
        private Thread thread;
        private readonly IThreadFactory threadFactory;
        private readonly IWorkerMonitor workerMonitor;
        private readonly string workerName;
    }
}