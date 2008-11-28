using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace ProjectPilot.Framework.Metrics
{
    public interface ILocStats
    {
        LocStatsData CountLocString(string code);
        
        LocStatsData CountLocString(Stream stream);

        LocStatsData CountLocFile(string filePath);
    }
}
