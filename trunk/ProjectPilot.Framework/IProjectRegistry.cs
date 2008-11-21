using System.Collections.Generic;
using ProjectPilot.Framework.Modules;

namespace ProjectPilot.Framework
{
    public interface IProjectRegistry
    {
        int ProjectsCount { get; }

        void AddProject (Project projectToAdd);

        Project GetProject(string projectId);
        
        ICollection<Project> ListAllProjects();
    }
}