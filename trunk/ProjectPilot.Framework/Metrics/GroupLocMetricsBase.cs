using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flubu.Builds.VSSolutionBrowsing;

namespace ProjectPilot.Framework.Metrics
{
    public abstract class GroupLocMetricsBase : LocMetricsBase
    {
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

        protected void AddLocMetrics(LocMetricsBase locMetrics)
        {
            groupLocStatsData.Add(locMetrics);
        }

        private List<LocMetricsBase> groupLocStatsData = new List<LocMetricsBase>();
    }
}
