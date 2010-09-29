using System;
using System.Diagnostics.CodeAnalysis;

namespace Flubu
{
    public interface IFlubuLogger : IDisposable
    {
        void LogError(string message);

        void LogError(string format, params object[] args);

        void LogMessage (string message);

        void LogMessage (string format, params object[] args);

        [SuppressMessage ("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "LogRunner")]
        void LogRunnerFinished (IFlubuRunner runner);

        void LogTargetStarted(IFlubuRunnerTarget target);

        void LogTargetFinished(IFlubuRunnerTarget target);

        void LogTaskStarted(string taskDescription);

        void LogTaskFinished();
    }
}