using System;
using System.Collections.Generic;
using Gallio.Framework;
using Gallio.Model;
using log4net;

namespace Accipio.TestFramework
{
    /// <summary>
    /// The base class for running tests using Accipio framework.
    /// </summary>
    /// <typeparam name="TRunner">The concrete type of the runner.</typeparam>
    public abstract class AccipioTestRunnerBase<TRunner> : IDisposable 
        where TRunner : AccipioTestRunnerBase<TRunner>
    {
        /// <summary>
        /// Gets the description of the currently running test case.
        /// </summary>
        /// <value>The description of the currently running test case.</value>
        public string Description
        {
            get { return description; }
        }

        /// <summary>
        /// Gets a collection of tags that were used to tag the currently running test case.
        /// </summary>
        /// <value>The collection of tags that were used to tag the currently running test case.</value>
        public ICollection<string> Tags
        {
            get { return tags; }
        }

        /// <summary>
        /// Adds a tags to the test case that will be run.
        /// </summary>
        /// <param name="tag">The tag to tag the test case with.</param>
        /// <returns>
        /// The same instance of the <see cref="TRunner"/> instance.
        /// </returns>
        public TRunner AddTag(string tag)
        {
            this.tags.Add(tag);
            return (TRunner)this;
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
        /// Marks the test as pending. This method should be called by all MiMiTestRunner actions
        /// which have not been implemented fully. 
        /// </summary>
        public void MarkTestAsPending()
        {
            throw new SilentTestException(TestOutcome.Pending, "To be implemented.");
        }

        /// <summary>
        /// Marks the test as pending. This method should be called by all MiMiTestRunner actions
        /// which have not been implemented fully.
        /// </summary>
        /// <param name="pendingMessage">The pending message.</param>
        public void MarkTestAsPending(string pendingMessage)
        {
            throw new SilentTestException(TestOutcome.Pending, pendingMessage);
        }

        /// <summary>
        /// Sets the description of the test case that will be run.
        /// </summary>
        /// <param name="description">The description of the test case.</param>
        /// <returns>The same instance of the <see cref="TRunner"/> instance.</returns>
        public TRunner SetDescription (string description)
        {
            this.description = description;
            return (TRunner)this;
        }

        protected AccipioTestRunnerBase(string testCaseName)
        {
            this.testCaseName = testCaseName;
            log.InfoFormat("Starting test case '{0}'", testCaseName);
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
                    log.InfoFormat("Finishing test case '{0}'", testCaseName);
                }

                disposed = true;
            }
        }

        private string description;
        private bool disposed;
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(AccipioTestRunnerBase<TRunner>));
        private List<string> tags = new List<string>();
        private readonly string testCaseName;
    }
}
