using System.Collections.Generic;
using ProjectPilot.Framework.Modules;

namespace ProjectPilot.Framework
{
    public class Project
    {
        public Project(string projectId, string projectName)
        {
            this.projectId = projectId;
            this.projectName = projectName;
        }

        public Project(string projectId, string projectName, IProjectRegistry projectRegistry)
            : this (projectId, projectName)
        {
            projectRegistry.AddProject(this);
        }

        public Project(
            string projectId, 
            string projectName, 
            IProjectRegistry projectRegistry,
            IEnumerable<IProjectModule> modules)
            : this (projectId, projectName, projectRegistry)
        {
            foreach (IProjectModule module in modules)
                AddModule(module);
        }

        public string ProjectId
        {
            get { return projectId; }
            set { projectId = value; }
        }

        public string ProjectName
        {
            get { return projectName; }
            set { projectName = value; }
        }

        public int ModulesCount
        {
            get { return modules.Count; }
        }

        public void AddModule(IProjectModule projectModule)
        {
            modules.Add(projectModule.ModuleId, projectModule);
            projectModule.ProjectId = projectId;
        }

        public IProjectModule GetModule (string moduleId)
        {
            return modules[moduleId];
        }

        public ICollection<IProjectModule> ListModules()
        {
            return modules.Values;
        }

        private Dictionary<string, IProjectModule> modules = new Dictionary<string, IProjectModule>();
        private string projectId;
        private string projectName;
    }
}