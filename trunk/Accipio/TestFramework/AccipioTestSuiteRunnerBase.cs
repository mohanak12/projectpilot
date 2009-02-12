using System;
using log4net;

namespace Accipio.TestFramework
{
    public abstract class AccipioTestSuiteRunnerBase<TTestCaseRunner> : IDisposable
        where TTestCaseRunner : AccipioTestRunnerBase<TTestCaseRunner> 
    {
        public abstract TTestCaseRunner CreateTestRunner(string testCaseName);

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or
        /// resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected AccipioTestSuiteRunnerBase(string testSuiteName)
        {
            this.testSuiteName = testSuiteName;
            log.InfoFormat("Starting test suite '{0}'", testSuiteName);
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
                    log.InfoFormat("Finishing test suite '{0}'", testSuiteName);
                }

                disposed = true;
            }
        }

        private bool disposed;
        private static readonly ILog log = LogManager.GetLogger(typeof(AccipioTestSuiteRunnerBase<TTestCaseRunner>));
        private readonly string testSuiteName;
    }
}
