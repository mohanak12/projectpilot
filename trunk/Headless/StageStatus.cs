using System;
using Headless.Configuration;

namespace Headless
{
    public class StageStatus : IDisposable
    {
        public StageStatus(BuildStage stage)
        {
            this.stage = stage;
        }

        public BuildOutcome Outcome
        {
            get { return outcome; }
            set { outcome = value; }
        }

        public BuildStage Stage
        {
            get { return stage; }
        }

        public IStageRunner StageRunner
        {
            get { return stageRunner; }
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

        public void MarkAsNotExecuted()
        {
            outcome = BuildOutcome.NotExecuted;
        }

        public void PrepareToStart(IStageRunner stageRunner)
        {
            this.stageRunner = stageRunner;
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
                    if (stageRunner != null)
                        stageRunner.Dispose();
                }

                disposed = true;
            }
        }

        private bool disposed;
        private BuildOutcome outcome = BuildOutcome.Initial;
        private BuildStage stage;
        private IStageRunner stageRunner;
    }
}