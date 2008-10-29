using System;
using System.Collections.Generic;
using System.Web;
using ProjectPilot.Framework;
using ProjectPilot.Framework.Modules;
using ProjectPilot.Framework.Projects;

namespace ProjectPilot.Portal.Models
{
    public class DefaultFacade : IProjectPilotFacade
    {
        public DefaultFacade(IProjectRegistry projectRegistry)
        {
            this.projectRegistry = projectRegistry;
        }

        public Project GetProject(string projectId)
        {
            return projectRegistry.GetProject(projectId);
        }

        public ICollection<Project> ListAllProjects()
        {
            return projectRegistry.ListAllProjects();
        }

        private readonly IProjectRegistry projectRegistry;
    }
}