using System;
using System.Collections.Generic;
using System.Text;

namespace Flubu
{
    /// <summary>
    /// Specifies basic properties and methods for a task.
    /// </summary>
    public interface ITask
    {
        /// <summary>
        /// Gets the task description.
        /// </summary>
        /// <value>The task description.</value>
        string TaskDescription { get; }

        /// <summary>
        /// Executes the task using the specified script execution environment.
        /// </summary>
        /// <param name="environment">The script execution environment.</param>
        void Execute (IScriptExecutionEnvironment environment);
    }
}
