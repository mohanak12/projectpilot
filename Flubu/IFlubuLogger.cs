using System;
using System.Diagnostics.CodeAnalysis;

namespace Flubu
{
    public interface IFlubuLogger : IDisposable
    {
        void LogMessage (string message);

        void LogMessage (string format, params object[] args);

        [SuppressMessage ("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "LogRunner")]
        void LogRunnerFinished (bool success, TimeSpan buildDuration);
        
        void LogTargetFinished();
        
        void LogTargetStarted (string targetName);
        
        void LogTaskFinished();
        
        void LogTaskStarted(string taskDescription);
    }
}