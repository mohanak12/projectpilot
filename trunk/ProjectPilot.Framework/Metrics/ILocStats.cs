using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.CodeAnalysis;

namespace ProjectPilot.Framework.Metrics
{
    public interface ILocStats
    {
        LocStatsData CountLoc(string code);
    }
}
