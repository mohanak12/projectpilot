namespace ProjectPilot.Framework.Modules
{
    /// <summary>
    /// Represents a trigger that causes tasks to be executed.
    /// </summary>
    public interface ITrigger
    {
        /// <summary>
        /// Determines whether the trigger has been triggered.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if the trigger is triggered; otherwise, <c>false</c>.
        /// </returns>
        bool IsTriggered();

        void MarkEventAsHandled();
    }
}