using System.Collections.Generic;
using ProjectPilot.Framework.Projects;

namespace ProjectPilot.Portal.Models
{
    public interface IProjectPilotFacade
    {
        Project GetProject(string projectId);
        ICollection<Project> ListAllProjects();
    }
}