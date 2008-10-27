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
            Project[] projectsToAdd = new Project[]
                                     {
                                         new Project("ebsy", "EBSy"), 
                                         new Project("mobiinfo", "Mobi-Info"), 
                                         new Project("bhwr", "Mobilkom BHWR"),
                                         new Project("octopus", "Octopus"), 
                                         new Project("projectpilot", "ProjectPilot"), 
                                     };
            projectsToAdd[2].AddModule(
                new StaticHtmlPageModule(fileManager, projectsToAdd[2].ProjectId, "SVNStats", "SVN Stats", "SvnStats.html"));

            foreach (Project project in projectsToAdd)
                projects.Add(project.ProjectId, project);
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

        public Project GetProject(string projectId)
        {
            throw new System.NotImplementedException();
        }

        public ICollection<Project> ListAllProjects()
        {
            throw new System.NotImplementedException();
        }

        private IFileManager fileManager;
        private Dictionary<string, Project> projects = new Dictionary<string, Project>();
    }
}