namespace Headless
{
    /// <summary>
    /// Control signals emmited by the traffic cop.
    /// </summary>
    public enum TrafficCopControlSignal
    {
        /// <summary>
        /// The traffic cop has not emitted any control signal.
        /// </summary>
        NoSignal,

        /// <summary>
        /// The traffic cop has signalled the build runner to stop.
        /// </summary>
        Stop,
    }
}