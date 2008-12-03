using System;
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

        public Uri WebApplicationUrl
        {
            get { return webApplicationUrl; }
            set { webApplicationUrl = value; }
        }

        public VSProjectInfo ProjectInfo
        {
            get { return projectInfo; }
        }

        private bool isWebProject;
        private readonly VSProjectInfo projectInfo;
        private Uri webApplicationUrl;
    }
}