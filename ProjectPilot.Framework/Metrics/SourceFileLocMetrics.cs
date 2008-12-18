using System.IO;

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
        /// <param name="sourceFileName">The source file to analyze.</param>
        /// <param name="map">The map of <see cref="ILocStats"/> objects which can calculate LoC metrics for different source file types.</param>
        public void CalcLocStatData(string sourceFileName, LocStatsMap map)
        {
            using (Stream streamOfFile = File.OpenRead(sourceFileName))
            {
                string fileExtension = Path.GetExtension(sourceFileName);

                ILocStats locStats = map.GetLocStatsForExtension(fileExtension);
                if (locStats != null)
                    this.locStatsData = locStats.CountLocStream(streamOfFile);
            }
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
