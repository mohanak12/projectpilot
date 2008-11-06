using System;
using System.Diagnostics.CodeAnalysis;

namespace ProjectPilot.Framework.Runners
{
    public interface IRunner : IDisposable
    {
        /// <summary>
        /// Starts the runner.
        /// </summary>
        void Start();
        /// <summary>
        /// Stops the runner.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Stop")]
        void Stop(int timeoutInMilliseconds);
    }
}