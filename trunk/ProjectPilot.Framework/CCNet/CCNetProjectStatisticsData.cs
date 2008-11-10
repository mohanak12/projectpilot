using System.Collections.Generic;

namespace ProjectPilot.Framework.CCNet
{
    public class CCNetProjectStatisticsData
    {
        #region Private member

        private List<CCNetProjectStatisticsBuildEntry> builds = new List<CCNetProjectStatisticsBuildEntry>();

        #endregion

        #region Public property

        public IList<CCNetProjectStatisticsBuildEntry> Builds
        {
            get { return builds; }
        }

        #endregion
    }
}
