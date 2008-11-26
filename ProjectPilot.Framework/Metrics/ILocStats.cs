using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ProjectPilot.Framework.Metrics
{
    public interface ILocStats
    {
        LocStatsData CountLocString(string code);

        LocStatsData CountLocFile(string filePath);
    }
}
