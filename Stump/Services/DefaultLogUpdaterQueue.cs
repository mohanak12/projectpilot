using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Stump.Services
{
    [SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
    public class DefaultLogUpdaterQueue : ILogUpdaterQueue
    {
        public DefaultLogUpdaterQueue(ILogReader logReader)
        {
            this.logReader = logReader;
        }

        public int RequestsInQueue
        {
            get
            {
                lock (this)
                    return this.queuedRequests.Count;
            }
        }

        public void FetchLogContents(string logFileName, LogContentsFetchedCallback logContentsFetchedCallback)
        {
            lock (this)
            {
                // if the log file is already queued, but not yet in processing
                LogUpdateRequest existingRequest = queuedRequests.Find(r => String.Equals(r.LogFileName, logFileName, StringComparison.OrdinalIgnoreCase));

                if (existingRequest != null)
                {
                    // we ignore the new request, since the old one is already being processed
                }
                else
                {
                    LogUpdateRequest logUpdateRequest = new LogUpdateRequest(logFileName, logContentsFetchedCallback);
                    queuedRequests.Add(logUpdateRequest);

                    CreateWorkerThread(logUpdateRequest);
                }
            }
        }

        private void CreateWorkerThread(LogUpdateRequest request)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(Worker_DoWork);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Worker_RunWorkerCompleted);
            worker.WorkerSupportsCancellation = false;
            worker.RunWorkerAsync(request);
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            LogUpdateRequest request = (LogUpdateRequest)e.Argument;

            request.LogContents = logReader.ReadLog(request.LogFileName);

            e.Result = request;
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            LogUpdateRequest request = (LogUpdateRequest) e.Result;
            lock (this)
            {
                int removedRequests = queuedRequests.RemoveAll(
                    r => String.Equals(r.LogFileName, request.LogFileName, StringComparison.OrdinalIgnoreCase));

                Debug.Assert(removedRequests == 1, "The request was not removed from the queue.");
            }

            request.LogContentsFetchedCallback(request);
            ((BackgroundWorker)sender).Dispose();
        }

        private readonly ILogReader logReader;
        private List<LogUpdateRequest> queuedRequests = new List<LogUpdateRequest>();
    }
}