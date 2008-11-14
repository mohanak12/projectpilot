using System.Globalization;
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

        public bool IsTestProject
        {
            get { return isTestProject; }
            set { isTestProject = value; }
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

        public string GetProjectAssemblyFileName (string buildConfiguration)
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                @"{0}\bin\{1}\{0}.dll",
                projectName,
                buildConfiguration);
        }

        private bool isMainProject;
        private bool isTestProject;
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
