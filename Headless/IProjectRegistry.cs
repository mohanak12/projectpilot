using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Headless.Configuration;

namespace Headless
{
    public interface IProjectRegistry
    {
        void ChangeProjectStatus(string projectId, ProjectStatus status);

        Project GetProject(string projectId);

        IList<string> ListProjects();
    }
}