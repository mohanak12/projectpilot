using System;
using Stump.Models;
using Stump.Services;
using Stump.Views;

namespace Stump.Presenters
{
    public class LogPresenter : IDisposable
    {
        public LogPresenter(
            ILogView view, 
            ILogMonitor logMonitor,
            ILogReader logReader,
            MonitoredLogFile logFile)
        {
            this.view = view;
            this.logReader = logReader;
            this.logFile = logFile;
            this.monitor = logMonitor;

            OnMonitoringEnabledToggled();
            
            if (view.IsLogDisplayActive && logFile.IsActive)
                OnLogDisplayActivated();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or
        /// resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void OnLogDisplayActivated()
        {
            string logContents = logReader.FetchLogContents(logFile.FileName);
            view.ShowLogContents(logContents);
        }

        public void OnMonitoringEnabledToggled()
        {
            bool isMonitored = logFile.IsActive;
            if (false == isMonitored)
                ShutdownMonitor();
            else
                StartMonitor();
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
                // clean native resources         

                if (disposing)
                {
                    // clean managed resources   
                    if (monitor != null)
                    {
                        monitor.StopMonitoring();
                        monitor.Dispose();
                    }
                }

                disposed = true;
            }
        }

        private void OnLogFileUpdated(string logFileName)
        {
            if (logFile.FileName != logFileName)
                throw new InvalidOperationException();

            if (view.IsLogDisplayActive)
                OnLogDisplayActivated();
            else
                view.IndicateLogFileUpdated();
        }

        private void OnLogFileDeleted(string logFileName)
        {
            if (logFile.FileName != logFileName)
                throw new InvalidOperationException();

            view.IndicateLogFileDeleted();
        }

        private void ShutdownMonitor()
        {
            monitor.StopMonitoring();
        }

        private void StartMonitor()
        {
            monitor.StartMonitoring(
                logFile.FileName,
                OnLogFileUpdated,
                OnLogFileDeleted);
        }

        private bool disposed;
        private readonly MonitoredLogFile logFile;
        private ILogMonitor monitor;
        private readonly ILogReader logReader;
        private readonly ILogView view;
    }
}