using System.IO;
using MbUnit.Framework;
using ProjectPilot.Framework.CCNet;

namespace ProjectPilot.Tests.Framework.CCNet
{
    [TestFixture]
    public class ProjectStatsLoadingTests
    {
        [Test]
        public void LoadStatistics()
        {
            ProjectStatsData data = LoadStatisticsFromFile();

            Assert.AreEqual(data.Builds.Count, 844);
        }

        private static ProjectStatsData LoadStatisticsFromFile()
        {
            ProjectStatsData data;
            using (Stream stream = File.OpenRead(@"..\..\..\Data\Samples\ccnet.stats.xml"))
            {
                data = CCNetProjectStatisticsPlugIn.Load(stream);
            }
            return data;
        }
    }
}
