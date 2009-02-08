namespace Headless.Threading
{
    public enum WorkerActivityType
    {
        /// <summary>
        /// The worker is waiting for the work to be assigned to it.
        /// </summary>
        WaitingForWork,

        /// <summary>
        /// The worker is executing the assigned work.
        /// </summary>
        ExecutingWork,
    }
}