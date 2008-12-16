using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Flubu.Builds.VSSolutionBrowsing;

namespace ProjectPilot.Framework.Metrics
{
    /// <summary>
    /// Implementation of LocMetricsBase. Represents a single source file item
    /// and its loc metrics.
    /// </summary>
    public class SourceFileLocMetrics : LocMetricsBase
    {
        public SourceFileLocMetrics(string fileName)
        {
            this.fileName = fileName;
        }

        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        /// <value>The name of the file.</value>
        public string FileName
        {
            get { return fileName; }
        }

        /// <summary>
        /// Calculates the loc stat data of the file.
        /// </summary>
        /// <param name="fileStream">The file stream.</param>
        public void CalcLocStatData(Stream fileStream)
        {
            ILocStats locStats = new LocStats();
            this.locStatsData = locStats.CountLocString(fileStream);
        }

        /// <summary>
        /// Gets the loc stats data.
        /// </summary>
        /// <returns>Returns the loc stat metrics of the file.</returns>
        public override LocStatsData GetLocStatsData()
        {
            return locStatsData;
        }

        private string fileName;
        private LocStatsData locStatsData;
    }
}
