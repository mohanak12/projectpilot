using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectPilot.Framework.Projects
{
    public class Project
    {
        public Project(string projectId, string projectName)
        {
            this.projectId = projectId;
            this.projectName = projectName;
        }

        public IList<IProjectModule> Modules
        {
            get { return modules; }
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

        private List<IProjectModule> modules = new List<IProjectModule>();
        private string projectId;
        private string projectName;
    }
}
