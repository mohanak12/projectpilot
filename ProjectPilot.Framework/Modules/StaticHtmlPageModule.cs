using ProjectPilot.Framework.Modules;

namespace ProjectPilot.Framework.Modules
{
    public class StaticHtmlPageModule : IProjectModule, IViewable
    {
        public StaticHtmlPageModule(IFileManager fileManager, string projectId, string moduleId, string moduleName, string pageName)
        {
            this.fileManager = fileManager;
            this.projectId = projectId;
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

        public string ProjectId
        {
            get { return projectId; }
        }

        public string FetchHtmlReport()
        {
            throw new System.NotImplementedException();
        }

        private readonly IFileManager fileManager;
        private string moduleId;
        private string moduleName;
        private string pageName;
        private readonly string projectId;
    }
}