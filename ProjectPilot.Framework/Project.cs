using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public void AddModule(IProjectModule projectModule)
        {
            modules.Add(projectModule.ModuleId, projectModule);
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