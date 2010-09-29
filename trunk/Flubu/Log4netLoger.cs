using System;
using System.Globalization;
using log4net;

namespace Flubu
{
    public class Log4NetLogger: IFlubuLogger
    {
        public Log4NetLogger(string name)
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
            log.ErrorFormat(CultureInfo.InvariantCulture, format, args);
        }

        public void LogMessage(string message)
        {
            log.Info(message);
        }

        public void LogMessage(string format, params object[] args)
        {
            log.InfoFormat(CultureInfo.InvariantCulture, format, args);
        }

        public void LogRunnerFinished(IFlubuRunner runner)
        {
            // reset the depth counter to make the build report non-indented
            executionDepthCounter = 0;

            if (runner.HasFailed)
                log.Error("BUILD FAILED");
            else
                log.Info("BUILD SUCCESSFUL");

            TimeSpan buildDuration = runner.BuildStopwatch.Elapsed;
            log.InfoFormat(CultureInfo.InvariantCulture, "Build finish time: {0:g}", DateTime.Now);
            log.InfoFormat(
                CultureInfo.InvariantCulture, 
                "Build duration: {0:D2}:{1:D2}:{2:D2} ({3:d} seconds)",
                buildDuration.Hours,
                buildDuration.Minutes,
                buildDuration.Seconds,
                (int)buildDuration.TotalSeconds);
        }

        public void LogTargetFinished(IFlubuRunnerTarget target)
        {
            log.InfoFormat(
                CultureInfo.InvariantCulture,
                "{0} finished (took {1} seconds)",
                target.TargetName,
                (int)target.TargetStopwatch.Elapsed.TotalSeconds);
            executionDepthCounter--;
        }

        public void LogTargetStarted(IFlubuRunnerTarget target)
        {
            log.InfoFormat(CultureInfo.InvariantCulture, "{0}:", target.TargetName);
            executionDepthCounter++;
        }

        public void LogTaskFinished()
        {
            executionDepthCounter--;
        }

        public void LogTaskStarted(string taskDescription)
        {
            log.InfoFormat(CultureInfo.InvariantCulture, "{0}", taskDescription);
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
