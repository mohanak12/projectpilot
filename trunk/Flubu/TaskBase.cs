using System;
using System.Collections.Generic;
using System.Text;

namespace Flubu
{
    /// <summary>
    /// A base abstract class from which tasks can be implemented.
    /// </summary>
    public abstract class TaskBase : ITask
    {
        /// <summary>
        /// Gets the task description.
        /// </summary>
        /// <value>The task description.</value>
        public abstract string TaskDescription { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is safe to execute in dry run mode.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is safe to execute in dry run mode; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsSafeToExecuteInDryRun
        {
            get { return false; }
        }

        /// <summary>
        /// Executes the task using the specified script execution environment.
        /// </summary>
        /// <remarks>This method implements the basic reporting and error handling for
        /// classes which inherit the <see cref="TaskBase"/> class.</remarks>
        /// <param name="environment">The script execution environment.</param>
        public void Execute (IScriptExecutionEnvironment environment)
        {
            if (environment  == null)
                throw new ArgumentNullException ("environment");
            
            try
            {
                environment.Logger.ReportTaskStarted (this);

                // when in dry run do not execute the task (unless it itself indicates that it is safe)
                if (false == environment.DryRun || IsSafeToExecuteInDryRun)
                    DoExecute (environment);

                environment.Logger.ReportTaskExecuted (this);
            }
            catch (RunnerFailedException ex)
            {
                environment.Logger.ReportTaskFailed (this, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                environment.Logger.ReportTaskFailed (this, ex);
                throw;
            }
        }

        /// <summary>
        /// Abstract method defining the actual work for a task.
        /// </summary>
        /// <remarks>This method has to be implemented by the inheriting task.</remarks>
        /// <param name="environment">The script execution environment.</param>
        protected abstract void DoExecute (IScriptExecutionEnvironment environment);
    }
}
