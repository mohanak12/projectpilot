using System.Collections.Generic;

namespace ProjectPilot.Framework.CCNet
{
    /// <summary>
    /// Saves list of <see cref="ProjectStatsBuildEntry" />
    /// </summary>
    public class ProjectStatsData
    {
        /// <summary>
        /// Gets list of all build
        /// </summary>
        public IList<ProjectStatsBuildEntry> Builds
        {
            get { return builds; }
        }

        private readonly List<ProjectStatsBuildEntry> builds = new List<ProjectStatsBuildEntry>();
    }
}
