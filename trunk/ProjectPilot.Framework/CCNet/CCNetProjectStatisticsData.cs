using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace ProjectPilot.Framework.CCNet
{
    public class CCNetProjectStatisticsData
    {
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private List<CCNetProjectStatisticsBuildEntry> builds = new List<CCNetProjectStatisticsBuildEntry>();
    }
}
