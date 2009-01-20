namespace Headless
{
    /// <summary>
    /// Defines the possible values for the outcome of the execution of a build stage.
    /// </summary>
    public enum StageOutcome
    {
        /// <summary>
        /// The stage is in its initial state.
        /// </summary>
        Initial,

        /// <summary>
        /// The stage has not been executed because dependecy stages have not finished successfully.
        /// </summary>
        NotExecuted,

        /// <summary>
        /// The stage execution is in progress.
        /// </summary>
        InProgress,

        /// <summary>
        /// The stage has been successfully executed.
        /// </summary>
        Successful,

        /// <summary>
        /// The stage execution has failed.
        /// </summary>
        Failed,
    }
}