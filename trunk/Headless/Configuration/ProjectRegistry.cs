using System.Collections.Generic;

namespace Headless.Configuration
{
    public class ProjectRegistry
    {
        public ProjectRegistry()
        {
        }

        public void AddProject (Project project)
        {
            projects.Add(project.ProjectId, project);
        }

        public IList<Project> ListProjects()
        {
            return new List<Project>(projects.Values);
        }

        private Dictionary<string, Project> projects = new Dictionary<string, Project>();
    }
}