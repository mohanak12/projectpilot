using System.Collections.Generic;
using ProjectPilot.Framework.Modules;
using ProjectPilot.Framework.Projects;

namespace ProjectPilot.Framework
{
    public interface IProjectRegistry
    {
        Project GetProject(string projectId);
        ICollection<Project> ListAllProjects();        
    }

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
            throw new System.NotImplementedException();
        }

        private IFileManager fileManager;
        private Dictionary<string, Project> projects = new Dictionary<string, Project>();
    }
}