using System;

namespace Flubu
{
    public interface IFlubuLogger : IDisposable
    {
        void Log(string message);

        void Log(string format, params object[] args);

        void LogExternalProgramOutput(string output);

        void ReportRunnerFinished(bool success);

        void ReportTaskStarted(ITask task);

        void ReportTaskExecuted(ITask task);

        void ReportTaskFailed(ITask task, Exception ex);

        void ReportTaskFailed(ITask task, string reason);
    }
}