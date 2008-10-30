using System.Collections.Generic;

namespace ProjectPilot.Framework
{
    public class ProjectRegistry : IProjectRegistry
    {
        public ProjectRegistry()
        {
        }

        public IFileManager FileManager
        {
            get { return fileManager; }
            set { fileManager = value; }
        }

        public Dictionary<string, Project> Projects
        {
            get { return projects; }
            set { projects = value; }
        }

        public void AddProject (Project projectToAdd)
        {
            projects.Add(projectToAdd.ProjectId, projectToAdd);
        }

        public Project GetProject(string projectId)
        {
            return projects[projectId];
        }

        public ICollection<Project> ListAllProjects()
        {
            return projects.Values;
        }

        private IFileManager fileManager;
        private Dictionary<string, Project> projects = new Dictionary<string, Project>();
    }
}