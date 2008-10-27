using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProjectPilot.Framework.Modules;
using ProjectPilot.Framework.Projects;

namespace ProjectPilot.Portal.Models
{
    public class ProjectViewModel
    {
        public ProjectViewModel(Project project, string moduleId)
        {
            this.project = project;
            this.moduleId = moduleId;
        }

        public Project Project
        {
            get { return project; }
        }

        public string ModuleId
        {
            get { return moduleId; }
        }

        public IProjectModule Module
        {
            get { return project.GetModule(moduleId); }
        }

        private Project project;
        private string moduleId;
    }
}
