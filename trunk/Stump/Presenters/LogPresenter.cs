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
            ILogUpdaterQueue logUpdaterQueue,
            MonitoredLogFile logFile)
        {
            this.view = view;
            this.logUpdaterQueue = logUpdaterQueue;
            this.logFile = logFile;
            this.monitor = logMonitor;

            view.MonitoringEnabled = logFile.IsActive;

            if (logFile.IsActive)
                StartMonitor();
            
            if (view.IsLogDisplayActive)
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
            if (logFile.IsActive)
            {
                logUpdaterQueue.FetchLogContents(logFile.FileName, OnLogContentsFetchedCallback);
            }
            else
                view.IndicateLogFileNotMonitored();
        }

        public void OnMonitoringEnabledToggled()
        {
            logFile.IsActive = !logFile.IsActive;
            bool isMonitored = logFile.IsActive;
            if (false == isMonitored)
            {
                ShutdownMonitor();
                view.IndicateLogFileNotMonitored();
            }
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

        private void OnLogContentsFetchedCallback(LogUpdateRequest request)
        {
            view.ShowLogContents(request.LogContents);
        }

        private void OnLogFileCreated(string logFileName)
        {
            if (logFile.FileName != logFileName)
                throw new InvalidOperationException();

            throw new NotImplementedException();
        }

        private void OnLogFileDeleted(string logFileName)
        {
            if (logFile.FileName != logFileName)
                throw new InvalidOperationException();

            view.IndicateLogFileDeleted();
        }

        private void OnLogFileMonitorError(string logFileName, Exception ex)
        {
            throw ex;
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

        private void ShutdownMonitor()
        {
            monitor.StopMonitoring();
        }

        private void StartMonitor()
        {
            monitor.StartMonitoring(
                logFile.FileName,
                OnLogFileCreated,
                OnLogFileUpdated,
                OnLogFileDeleted,
                OnLogFileMonitorError);
        }

        private bool disposed;
        private readonly MonitoredLogFile logFile;
        private ILogMonitor monitor;
        private readonly ILogUpdaterQueue logUpdaterQueue;
        private readonly ILogView view;
    }
}