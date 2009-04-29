using System;
using System.Collections.Generic;
using System.Text;
using log4net;

namespace Flubu
{
    public class Log4netLoger: IFlubuLogger
    {
        public Log4netLoger(string name)
        {
           log = LogManager.GetLogger(name);
        }
        
        /// <summary>
        ///                     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void LogError(string message)
        {
            log.Error(message);
        }

        public void LogError(string format, params object[] args)
        {
            log.ErrorFormat(format, args);
        }

        public void LogMessage(string message)
        {
            log.Info(message);
        }

        public void LogMessage(string format, params object[] args)
        {
            log.InfoFormat(format, args);
        }

        public void LogRunnerFinished(bool success, TimeSpan buildDuration)
        {
            // reset the depth counter to make the build report non-indented
            executionDepthCounter = 0;

            if (success)
                log.Info("BUILD SUCCESSFUL");
            else
                log.Error("BUILD FAILED");

            log.InfoFormat("Build finish time: {0:g}", DateTime.Now);
            log.InfoFormat(
                "Build duration: {0:D2}:{1:D2}:{2:D2} ({3:d} seconds)",
                buildDuration.Hours,
                buildDuration.Minutes,
                buildDuration.Seconds,
                (int)buildDuration.TotalSeconds);
        }

        public void LogTargetFinished()
        {
            executionDepthCounter--;
        }

        public void LogTargetStarted(string targetName)
        {
            log.InfoFormat("{0}:", targetName);
            executionDepthCounter++;
        }

        public void LogTaskFinished()
        {
            executionDepthCounter--;
        }

        public void LogTaskStarted(string taskDescription)
        {
            log.InfoFormat("TASK: {0}", taskDescription);
            executionDepthCounter++;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (false == disposed)
            {
                if (disposing)
                {
                   // add some disposing code if needed
                }

                disposed = true;
            }
        }

        private bool disposed;
        private int executionDepthCounter;
        private static ILog log;
    }
}
