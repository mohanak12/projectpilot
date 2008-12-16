using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Flubu.Builds.VSSolutionBrowsing;

namespace ProjectPilot.Framework.Metrics
{
    public class SourceFileLocMetrics : LocMetricsBase
    {
        public SourceFileLocMetrics(string fileName)
        {
            this.fileName = fileName;
        }

        public string FileName
        {
            get { return fileName; }
        }

        public void CalcLocStatData(Stream fileStream)
        {
            ILocStats locStats = new LocStats();
            this.locStatsData = locStats.CountLocString(fileStream);
        }

        public override LocStatsData GetLocStatsData()
        {
            return locStatsData;
        }

        private string fileName;
        private LocStatsData locStatsData;
    }
}
