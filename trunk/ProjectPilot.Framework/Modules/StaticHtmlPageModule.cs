using ProjectPilot.Framework.Modules;
using ProjectPilot.Framework.Projects;

namespace ProjectPilot.Framework.Modules
{
    public class StaticHtmlPageModule : IProjectModule, IViewable
    {
        public StaticHtmlPageModule(
            Project project, 
            string moduleId, 
            string moduleName, 
            string pageName, 
            IFileManager fileManager)
        {
            this.fileManager = fileManager;
            this.project = project;
            this.moduleId = moduleId;
            this.moduleName = moduleName;
            this.pageName = pageName;
        }

        public string ModuleId
        {
            get { return moduleId; }
        }

        public string ModuleName
        {
            get { return moduleName; }
        }

        public Project Project
        {
            get { return project; }
        }

        public string FetchHtmlReport()
        {
            throw new System.NotImplementedException();
        }

        private readonly IFileManager fileManager;
        private string moduleId;
        private string moduleName;
        private string pageName;
        private readonly Project project;
    }
}