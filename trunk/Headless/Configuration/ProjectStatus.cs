namespace Headless.Configuration
{
    public enum ProjectStatus
    {
        /// <summary>
        /// The project is actively listening for new events.
        /// </summary>
        Listening,

        /// <summary>
        /// The project is currently executing the build. 
        /// </summary>
        Building,

        /// <summary>
        /// The project has been stopped and is not listening to new events.
        /// </summary>
        Stopped,

        /// <summary>
        /// There is an error in the project which prevents it from running normally.
        /// </summary>
        Error,
    }
}