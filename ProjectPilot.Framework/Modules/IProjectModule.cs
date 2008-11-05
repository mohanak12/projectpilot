using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectPilot.Framework.Modules
{
    public interface IProjectModule
    {
        string ModuleId { get; }
        string ModuleName { get; }
        string ProjectId { get; set; }
    }
}