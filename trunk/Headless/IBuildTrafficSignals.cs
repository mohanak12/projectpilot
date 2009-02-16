using System;
using System.Threading;

namespace Headless
{
    public interface IBuildTrafficSignals : IDisposable
    {
        BuildTrafficCopSignal WaitForControlSignal(TimeSpan waitPeriod);
    }

    public class DefaultBuildTrafficSignals : IBuildTrafficSignals
    {
        public DefaultBuildTrafficSignals()
        {
            stopSignal = new ManualResetEvent(false);
        }

        public BuildTrafficCopSignal WaitForControlSignal(TimeSpan waitPeriod)
        {
            bool shouldStop = stopSignal.WaitOne(waitPeriod);
            if (shouldStop)
                return BuildTrafficCopSignal.Stop;

            return BuildTrafficCopSignal.NoSignal;
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
                    stopSignal.Close();
                }

                disposed = true;
            }
        }

        private bool disposed;
        private EventWaitHandle stopSignal;
    }
}