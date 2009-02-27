using System;
using System.IO;

namespace Stump.Services
{
    public class DefaultLogMonitor : ILogMonitor
    {
        public void StartMonitoring(
            string logFileName,
            LogFileDeletedCallback logFileDeletedCallback,
            LogFileUpdatedCallback logFileUpdatedCallback)
        {
            if (watcher == null)
            {
                this.logFileName = logFileName;
                this.logFileDeletedCallback = logFileDeletedCallback;
                this.logFileUpdatedCallback = logFileUpdatedCallback;

                watcher = new FileSystemWatcher();
                watcher.Path = Path.GetDirectoryName(logFileName);
                watcher.Filter = Path.GetFileName(logFileName);
                watcher.NotifyFilter = NotifyFilters.LastWrite;
                watcher.Changed += new FileSystemEventHandler(WatcherChanged);
                watcher.Deleted += new FileSystemEventHandler(WatcherDeleted);
                watcher.EnableRaisingEvents = true;
            }
        }

        public void StopMonitoring()
        {
            if (watcher != null)
            {
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
            logFileUpdatedCallback(logFileName);
        }

        private void WatcherDeleted(object sender, FileSystemEventArgs e)
        {
            logFileDeletedCallback(logFileName);
        }

        private bool disposed;
        private LogFileDeletedCallback logFileDeletedCallback;
        private string logFileName;
        private LogFileUpdatedCallback logFileUpdatedCallback;
        private FileSystemWatcher watcher;
    }
}