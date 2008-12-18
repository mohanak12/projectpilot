using System.Diagnostics.CodeAnalysis;

namespace ProjectPilot.Framework.Metrics
{
    /// <summary>
    /// Abstract class for holding loc metrics objects.
    /// </summary>
    public abstract class LocMetricsBase
    {
        /// <summary>
        /// Gets the loc stats data.
        /// </summary>
        /// <returns>Returns loc metrics.</returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public abstract LocStatsData GetLocStatsData();
    }
}
