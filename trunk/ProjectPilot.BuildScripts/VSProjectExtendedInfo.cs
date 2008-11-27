using ProjectPilot.BuildScripts.VSSolutionBrowsing;

namespace ProjectPilot.BuildScripts
{
    public class VSProjectExtendedInfo
    {
        public VSProjectExtendedInfo (VSProjectInfo projectInfo)
        {
            this.projectInfo = projectInfo;
        }

        public bool IsWebProject
        {
            get { return isWebProject; }
            set { isWebProject = value; }
        }

        public VSProjectInfo ProjectInfo
        {
            get { return projectInfo; }
        }

        private readonly VSProjectInfo projectInfo;
        private bool isWebProject;
    }
}