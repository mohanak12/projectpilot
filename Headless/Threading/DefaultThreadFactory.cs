using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Headless.Threading
{
    public class DefaultThreadFactory : IThreadFactory
    {
        public Thread CreateThread(string threadName, ThreadStart threadStart)
        {
            Thread thread = new Thread(threadStart);
            thread.Name = threadName;

            threads.Add(thread);

            return thread;
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

        public void WaitForAllThreadsToStop(TimeSpan timeout)
        {
            List<Thread> threadsStillRunning = new List<Thread>();

            foreach (Thread thread in threads)
            {
                if (thread.IsAlive)
                    threadsStillRunning.Add(thread);
            }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            while (threadsStillRunning.Count > 0 && stopwatch.Elapsed < timeout)
            {
                for (int i = 0; i < threadsStillRunning.Count;)
                {
                    Thread thread = threadsStillRunning[i];
                    if (false == thread.IsAlive)
                        threadsStillRunning.RemoveAt(i);
                    else
                        i++;
                }

                Thread.Sleep(TimeSpan.FromSeconds(1));
            }

            foreach (Thread thread in threadsStillRunning)
                thread.Abort();

            threads.Clear();
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
                    WaitForAllThreadsToStop(TimeSpan.FromSeconds(10));
                }

                disposed = true;
            }
        }

        private bool disposed;
        private List<Thread> threads = new List<Thread>();
    }
}