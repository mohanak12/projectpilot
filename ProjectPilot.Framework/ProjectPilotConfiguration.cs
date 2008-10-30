namespace ProjectPilot.Framework
{
    public class ProjectPilotConfiguration : IProjectPilotConfiguration
    {
        public string ProjectPilotWebAppRootUrl
        {
            get { return projectPilotWebAppRootUrl; }
            set { projectPilotWebAppRootUrl = value; }
        }

        private string projectPilotWebAppRootUrl;
    }
}