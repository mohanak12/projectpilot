using System.Linq;
using System.Text;

namespace ProjectPilot.BuildScripts
{
    public class ProjectToBuild
    {
        public ProjectToBuild(string projectName)
        {
            this.projectName = projectName;
        }

        public bool IsMainProject
        {
            get { return isMainProject; }
            set { isMainProject = value; }
        }

        public bool IsWebProject
        {
            get { return isWebProject; }
            set { isWebProject = value; }
        }

        public string ProjectName
        {
            get { return projectName; }
        }

        private bool isMainProject;
        private bool isWebProject;
        private string projectName;
    }

    // clean.output
    // clean.build
    // compile
    // fxcop
    // copy.deliverables
    // package.build
}
