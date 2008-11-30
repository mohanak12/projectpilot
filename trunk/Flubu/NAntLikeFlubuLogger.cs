using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Flubu
{
    [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "AntLike")]
    public class NAntLikeFlubuLogger : IFlubuLogger
    {
        public NAntLikeFlubuLogger(TextWriter writer)
        {
            this.writer = writer;
        }

        public void Log(string message)
        {
            writer.WriteLine(message);
        }

        public void Log(string format, params object[] args)
        {
            writer.WriteLine(format, args);
        }

        public void LogExternalProgramOutput(string output)
        {
            Log("     [exec] {0}", output);
        }

        public void ReportRunnerFinished(bool success)
        {
            Log(success ? "BUILD SUCCESS" : "BUILD FAILED");
        }

        public void ReportTaskStarted(ITask task)
        {
            Log("     {0}", task.TaskDescription);
        }

        public void ReportTaskExecuted(ITask task)
        {
            //Log("     DONE");
        }

        public void ReportTaskFailed(ITask task, Exception ex)
        {
            Log("     FAILED");
        }

        public void ReportTaskFailed(ITask task, string reason)
        {
            Log("     FAILED");
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
                
        private readonly TextWriter writer;
    }
}