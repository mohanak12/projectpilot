using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectPilot.Framework.Modules
{
    public interface IProjectModule
    {
        string ProjectId { get; }
        string ModuleId { get; }
        string ModuleName { get; }
    }
}