using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ProjectPilot.Framework.Metrics
{
    public interface ILocStats
    {
        LocStatsData CountLoc(string code);
    }
}
