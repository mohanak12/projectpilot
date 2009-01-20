using System.Collections.Generic;

namespace Headless.Configuration
{
    public class Project
    {
        public IList<BuildStage> BuildStages
        {
            get { return buildStages; }
        }

        public string ProjectId
        {
            get { return projectId; }
            set { projectId = value; }
        }

        public IVersionControlSystem VersionControlSystem
        {
            get { return versionControlSystem; }
            set { versionControlSystem = value; }
        }

        private List<BuildStage> buildStages = new List<BuildStage>();
        private string projectId;
        private IVersionControlSystem versionControlSystem;
    }
}