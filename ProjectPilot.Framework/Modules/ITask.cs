using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace ProjectPilot.Framework.Modules
{
    /// <summary>
    /// Represents a task that needs to be executed when an associated trigger is triggered.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1040:AvoidEmptyInterfaces")]
    public interface ITask
    {
        /// <summary>
        /// Gets the trigger that can cause the task to be executed.
        /// </summary>
        /// <value>The trigger.</value>
        ITrigger Trigger { get; }

        /// <summary>
        /// Executes the task.
        /// </summary>
        void Execute(WaitHandle stopSignal);
    }
}