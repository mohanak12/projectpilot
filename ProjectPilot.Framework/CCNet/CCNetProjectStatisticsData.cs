using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ProjectPilot.Framework.CCNet
{
    public class CCNetProjectStatisticsData
    {
        #region Private member

        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private List<CCNetProjectStatisticsBuildEntry> builds = new List<CCNetProjectStatisticsBuildEntry>();

        #endregion

        #region Public property

        public List<CCNetProjectStatisticsBuildEntry> Builds
        {
            get { return builds; }
            set { builds = value; }
        }

        #endregion
    }
}
