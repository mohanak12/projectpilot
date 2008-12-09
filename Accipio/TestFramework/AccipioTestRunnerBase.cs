using System;
using System.Collections.Generic;

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
        /// Sets the description of the test case that will be run.
        /// </summary>
        /// <param name="description">The description of the test case.</param>
        /// <returns>The same instance of the <see cref="TRunner"/> instance.</returns>
        public TRunner SetDescription (string description)
        {
            this.description = description;
            return (TRunner)this;
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

        #region IDisposable Members

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
                if (disposing)
                {
                }

                disposed = true;
            }
        }

        private bool disposed;

        #endregion

        private string description;
        private List<string> tags = new List<string>();
    }
}
