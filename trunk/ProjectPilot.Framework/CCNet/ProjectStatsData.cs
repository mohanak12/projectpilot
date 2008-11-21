using System.Collections.Generic;

namespace ProjectPilot.Framework.CCNet
{
    public class ProjectStatsData
    {
        #region Public property

        public IList<ProjectStatsBuildEntry> Builds
        {
            get { return builds; }
        }

        #endregion

        #region Private member

        private List<ProjectStatsBuildEntry> builds = new List<ProjectStatsBuildEntry>();

        #endregion
    }
}
