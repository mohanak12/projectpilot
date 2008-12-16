using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Flubu.Builds.VSSolutionBrowsing;

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
