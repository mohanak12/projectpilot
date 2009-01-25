using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Headless.Configuration;
using log4net.Core;

namespace Headless
{
    public class LocalStageRunner : IStageRunner
    {
        public LocalStageRunner(IHeadlessLogger logger)
        {
            this.logger = logger;
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

        public void SetBuildStage(BuildStage buildStage)
        {
            this.buildStage = buildStage;
        }

        public void StartStage()
        {
            lock (this)
            {
                buildOutcome = BuildOutcome.InProgress;
            }

            runnerThread = new Thread(ThreadMethod);
            runnerThread.Start();
        }

        public void UpdateStatus(StageStatus status)
        {
            lock (this)
            {
                status.Outcome = buildOutcome;
            }
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

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private void ThreadMethod()
        {
            try
            {
                buildStage.Task.Execute();

                lock (this)
                {
                    buildOutcome = BuildOutcome.Successful;
                }
            }
            catch (Exception ex)
            {
                lock (this)
                {
                    buildOutcome = BuildOutcome.Failed;
                }

                logger.Log(LogEvent.ForBuildStage(buildStage, Level.Error, "Build task failed: {0}", ex));
            }
        }

        private BuildStage buildStage;
        private bool disposed;
        private IHeadlessLogger logger;
        private Thread runnerThread;
        private BuildOutcome buildOutcome;
    }
}