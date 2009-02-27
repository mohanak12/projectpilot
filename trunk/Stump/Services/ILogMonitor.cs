using System;

namespace Stump.Services
{
    public delegate void LogFileCreatedCallback(string logFileName);

    public delegate void LogFileDeletedCallback(string logFileName);

    public delegate void LogFileMonitorErrorCallback(string logFileName, Exception ex);

    public delegate void LogFileUpdatedCallback(string logFileName);

    public interface ILogMonitor : IDisposable
    {
        void StartMonitoring(
            string logFileName,
            LogFileCreatedCallback logFileCreatedCallback,
            LogFileDeletedCallback logFileDeletedCallback,
            LogFileUpdatedCallback logFileUpdatedCallback,
            LogFileMonitorErrorCallback logFileMonitorErrorCallback);

        void StopMonitoring();
    }
}