using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Accipio.TestFramework
{
    public abstract class AccipioTestSuiteRunnerBase : IDisposable
    {
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
                // TODO: clean native resources         

                if (disposing)
                {
                    // TODO: clean managed resources
                }

                disposed = true;
            }
        }

        private bool disposed;
    }
}
