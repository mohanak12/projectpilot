using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectPilot.Framework.Projects;

namespace ProjectPilot.Framework.Modules
{
    public interface IProjectModule
    {
        string ModuleId { get; }
        string ModuleName { get; }
        Project Project { get; }
    }
}