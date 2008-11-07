using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using ProjectPilot.Framework.Modules;

namespace ProjectPilot.Framework.Runners
{
    public class DefaultRunner : IRunner
    {
        public DefaultRunner (IProjectRegistry projectRegistry)
        {
            this.projectRegistry = projectRegistry;
        }

        /// <summary>
        /// Starts the runner.
        /// </summary>
        public void Start()
        {
            stopAllThreadsSignal = new ManualResetEvent(false);

            // create the control thread and start it
            controlThread = new Thread(ControlThreadLoop);
            controlThread.Start();

            // exit the method
        }

        /// <summary>
        /// Stops the runner.
        /// </summary>
        public void Stop(int timeoutInMilliseconds)
        {
            // if the control thread is not running, we have nothing to stop
            if (controlThread  == null)
                return;

            // signal all threads to stop
            stopAllThreadsSignal.Set();

            // join all threads. Abort them if they did not terminate themselves
            if (false == controlThread.Join(timeoutInMilliseconds))
                controlThread.Abort();

            // exit the method
            controlThread = null;
            stopAllThreadsSignal.Close();
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
        [SuppressMessage(
            "Microsoft.Usage", 
            "CA2213:DisposableFieldsShouldBeDisposed", 
            MessageId = "stopAllThreadsSignal",
            Justification = "This field is disposed when calling the Stop method.")]
        protected virtual void Dispose(bool disposing)
        {
            if (false == disposed)
            {
                if (disposing)
                {
                    Stop(10000);
                }

                disposed = true;
            }
        }

        private bool disposed;

        #endregion
                
        private void ControlThreadLoop()
        {
            TimeSpan pollPeriod = TimeSpan.FromMinutes(1);
            TimeSpan waitTime = TimeSpan.Zero;

            queuedTasks = new List<ITask>();

            // while the stop signal is not set
            while (false == stopAllThreadsSignal.WaitOne(waitTime))
            {
                // collect pending tasks
                foreach (Project project in projectRegistry.ListAllProjects())
                {
                    foreach (IProjectModule module in project.ListModules())
                    {
                        if (module is ITask)
                        {
                            ITask task = module as ITask;
                            if (task.Trigger.IsTriggered())
                                this.queuedTasks.Add(task);
                        }
                    }
                }

                // execute scheduled tasks
                // TODO: implent this with multithreading
                while (queuedTasks.Count > 0)
                {
                    ITask task = queuedTasks[0];
                    if (stopAllThreadsSignal.WaitOne(TimeSpan.Zero))
                        break;

                    // execute the task
                    task.ExecuteTask(stopAllThreadsSignal);

                    // mark the task trigger event as handled
                    task.Trigger.MarkEventAsHandled();

                    // remove the task from the queue
                    queuedTasks.RemoveAt(0);
                }

                waitTime = pollPeriod;
            }
        }

        private Thread controlThread;
        private readonly IProjectRegistry projectRegistry;
        private List<ITask> queuedTasks = new List<ITask>();
        private ManualResetEvent stopAllThreadsSignal;
    }
}