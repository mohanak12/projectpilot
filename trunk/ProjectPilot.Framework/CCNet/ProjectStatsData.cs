using System.Collections.Generic;

namespace ProjectPilot.Framework.CCNet
{
    public class ProjectStatsData
    {
        public IList<ProjectStatsBuildEntry> Builds
        {
            get { return builds; }
        }

        private List<ProjectStatsBuildEntry> builds = new List<ProjectStatsBuildEntry>();
    }
}
