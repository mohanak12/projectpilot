using System.Diagnostics.CodeAnalysis;
using ProjectPilot.Framework.Modules;

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

        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private readonly IFileManager fileManager;
        private string moduleId;
        private string moduleName;
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private string pageName;
        private readonly Project project;
    }
}