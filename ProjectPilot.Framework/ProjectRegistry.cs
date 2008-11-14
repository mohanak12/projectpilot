using System;
using System.Collections.Generic;

namespace ProjectPilot.Framework
{
    public class ProjectRegistry : IProjectRegistry
    {
        public ProjectRegistry()
        {
        }

        public ProjectRegistry(IEnumerable<Project> projects)
        {
            foreach (Project project in projects)
                AddProject(project);
        }

        public IFileManager FileManager
        {
            get { return fileManager; }
            set { fileManager = value; }
        }

        public IDictionary<string, Project> Projects
        {
            get { return projects; }
        }

        public int ProjectsCount
        {
            get { return projects.Count; }
        }

        public void AddProject (Project projectToAdd)
        {
            projects.Add(projectToAdd.ProjectId, projectToAdd);
        }

        public Project GetProject(string projectId)
        {
            if (projectId == null)
                throw new ArgumentNullException ("projectId");                
            
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