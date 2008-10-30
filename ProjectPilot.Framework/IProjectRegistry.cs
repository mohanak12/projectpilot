using System.Collections.Generic;
using ProjectPilot.Framework.Modules;

namespace ProjectPilot.Framework
{
    public interface IProjectRegistry
    {
        Project GetProject(string projectId);
        ICollection<Project> ListAllProjects();        
    }
}