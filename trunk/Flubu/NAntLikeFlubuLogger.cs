using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using log4net;

namespace Flubu
{
    [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "AntLike")]
    public class NAntLikeFlubuLogger : IFlubuLogger
    {
        public NAntLikeFlubuLogger()
        {
            AddWriter(System.Console.Out);
        }

        public void AddWriter (TextWriter writer)
        {
            this.writers.Add(writer);
        }

        public void Log(string message)
        {
            foreach (TextWriter writer in writers)
                writer.WriteLine(message);
            log.Debug(message);
        }

        public void Log(string format, params object[] args)
        {
            foreach (TextWriter writer in writers)
                writer.WriteLine(format, args);
            log.DebugFormat(CultureInfo.InvariantCulture, format, args);
        }

        public void LogExternalProgramOutput(string output)
        {
            Log("     [exec] {0}", output);
        }

        public void ReportRunnerFinished(bool success, TimeSpan buildDuration)
        {
            Log(String.Empty);
            Log(success ? "BUILD SUCCESSFUL" : "BUILD FAILED");
            Log("Build duration: {0} ({1} seconds)", buildDuration, (int)buildDuration.TotalSeconds);
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
                
        private readonly List<TextWriter> writers = new List<TextWriter>();
        private static readonly ILog log = log4net.LogManager.GetLogger("flubu");
    }
}