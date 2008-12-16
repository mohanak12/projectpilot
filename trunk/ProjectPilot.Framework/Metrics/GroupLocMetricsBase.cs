using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flubu.Builds.VSSolutionBrowsing;

namespace ProjectPilot.Framework.Metrics
{
    /// <summary>
    /// Abstract class derived from LocMetricsBase. 
    /// Used to hold groups (nodes) of loc metrics items
    /// </summary>
    public abstract class GroupLocMetricsBase : LocMetricsBase
    {
        /// <summary>
        /// Gets the loc stats data.
        /// </summary>
        /// <returns>Returns the combined loc stats of the group</returns>
        public override LocStatsData GetLocStatsData()
        {
            LocStatsData groupData = new LocStatsData();

            foreach (LocMetricsBase childLocMetrics in groupLocStatsData)
            {
                LocStatsData childLocStatsData = childLocMetrics.GetLocStatsData();
                groupData.Add(childLocStatsData);
            }

            return groupData;
        }

        /// <summary>
        /// Adds another loc metrics item to teh group.
        /// </summary>
        /// <param name="locMetrics">Loc metrics item.</param>
        protected void AddLocMetrics(LocMetricsBase locMetrics)
        {
            groupLocStatsData.Add(locMetrics);
        }

        /// <summary>
        /// The list of loc metrics items.
        /// </summary>
        private List<LocMetricsBase> groupLocStatsData = new List<LocMetricsBase>();
    }
}
