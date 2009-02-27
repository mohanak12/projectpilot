using System;

namespace Stump.Services
{
    public delegate void LogFileDeletedCallback(string logFileName);

    public delegate void LogFileUpdatedCallback(string logFileName);

    public interface ILogMonitor : IDisposable
    {
        void StartMonitoring(
            string logFileName,
            LogFileDeletedCallback logFileDeletedCallback,
            LogFileUpdatedCallback logFileUpdatedCallback);

        void StopMonitoring();
    }
}