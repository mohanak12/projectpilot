using System.Collections.Generic;

namespace Headless.Configuration
{
    public class Project
    {
        public Project(string projectId)
        {
            this.projectId = projectId;
        }

        public IList<BuildReport> BuildReports
        {
            get { return buildReports; }
        }

        public IList<BuildStage> BuildStages
        {
            get { return buildStages; }
        }

        public BuildReport LastBuild
        {
            get
            {
                if (buildReports.Count > 0)
                    return buildReports[0];
                return null;
            }
        }

        public string ProjectId
        {
            get { return projectId; }
        }

        public IProjectRegistry ProjectRegistry
        {
            get { return projectRegistry; }
            set { projectRegistry = value; }
        }

        public ProjectStatus Status
        {
            get
            {
                return status;
            }

            set
            {
                status = value;
                projectRegistry.ChangeProjectStatus(this.projectId, value);
            }
        }

        public IList<ITrigger> Triggers
        {
            get { return triggers; }
        }

        public IVersionControlSystem VersionControlSystem
        {
            get { return versionControlSystem; }
            set { versionControlSystem = value; }
        }

        private List<BuildStage> buildStages = new List<BuildStage>();
        private List<BuildReport> buildReports = new List<BuildReport>();
        private string projectId;
        private IProjectRegistry projectRegistry;
        private ProjectStatus status;
        private List<ITrigger> triggers = new List<ITrigger>();
        private IVersionControlSystem versionControlSystem;
    }
}