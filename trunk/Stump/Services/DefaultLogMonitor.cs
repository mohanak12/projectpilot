using System;
using System.IO;

namespace Stump.Services
{
    public class DefaultLogMonitor : ILogMonitor
    {
        public void StartMonitoring(
            string logFileName,
            LogFileCreatedCallback logFileCreatedCallback,
            LogFileDeletedCallback logFileDeletedCallback,
            LogFileUpdatedCallback logFileUpdatedCallback,
            LogFileMonitorErrorCallback logFileMonitorErrorCallback)
        {
            if (watcher == null)
            {
                this.logFileName = logFileName;
                this.logFileCreatedCallback = logFileCreatedCallback;
                this.logFileDeletedCallback = logFileDeletedCallback;
                this.logFileUpdatedCallback = logFileUpdatedCallback;
                this.logFileMonitorErrorCallback = logFileMonitorErrorCallback;

                watcher = new FileSystemWatcher();
                watcher.Path = Path.GetDirectoryName(Path.GetFullPath(logFileName));
                //watcher.Filter = "*";
                //watcher.Filter = Path.GetFileName(logFileName);
                //watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.CreationTime;
                watcher.Changed += new FileSystemEventHandler(WatcherChanged);
                watcher.Created += new FileSystemEventHandler(WatcherCreated);
                watcher.Deleted += new FileSystemEventHandler(WatcherDeleted);
                watcher.Error += new ErrorEventHandler(WatcherError);
                watcher.EnableRaisingEvents = true;
            }
        }

        public void StopMonitoring()
        {
            if (watcher != null)
            {
                watcher.EnableRaisingEvents = false;
                watcher.Dispose();
                watcher = null;
            }
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
                    StopMonitoring();
                }

                disposed = true;
            }
        }

        private void WatcherChanged(object sender, FileSystemEventArgs e)
        {
            if (e.Name == Path.GetFileName(logFileName))
                logFileUpdatedCallback(logFileName);
        }

        private void WatcherCreated(object sender, FileSystemEventArgs e)
        {
            if (e.Name == Path.GetFileName(logFileName))
                logFileCreatedCallback(logFileName);
        }

        private void WatcherDeleted(object sender, FileSystemEventArgs e)
        {
            if (e.Name == Path.GetFileName(logFileName))
                logFileDeletedCallback(logFileName);
        }

        private void WatcherError(object sender, ErrorEventArgs e)
        {
            logFileMonitorErrorCallback(logFileName, e.GetException());
        }

        private bool disposed;
        private LogFileCreatedCallback logFileCreatedCallback;
        private LogFileDeletedCallback logFileDeletedCallback;
        private string logFileName;
        private LogFileMonitorErrorCallback logFileMonitorErrorCallback;
        private LogFileUpdatedCallback logFileUpdatedCallback;
        private FileSystemWatcher watcher;
    }
}