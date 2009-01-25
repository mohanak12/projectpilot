namespace Headless
{
    /// <summary>
    /// Defines the possible values for the outcome of the execution of a build.
    /// </summary>
    public enum BuildOutcome
    {
        /// <summary>
        /// The build is in its initial state.
        /// </summary>
        Initial,

        /// <summary>
        /// The build has not been executed because dependecy stages have not finished successfully.
        /// </summary>
        NotExecuted,

        /// <summary>
        /// The build execution is in progress.
        /// </summary>
        InProgress,

        /// <summary>
        /// The build has been successfully executed.
        /// </summary>
        Successful,

        /// <summary>
        /// The build execution has failed.
        /// </summary>
        Failed,
    }
}