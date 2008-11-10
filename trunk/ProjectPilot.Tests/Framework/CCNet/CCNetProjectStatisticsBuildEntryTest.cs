using System.IO;
using MbUnit.Framework;
using ProjectPilot.Framework.CCNet;

namespace ProjectPilot.Tests.Framework.CCNet
{
    [TestFixture]
    class CCNetProjectStatisticsBuildEntryTest
    {
        [Test]
        public void LoadStatistics()
        {
            CCNetProjectStatisticsData data = LoadStatisticsFromFile();

            Assert.AreEqual(data.Builds.Count, 844);
        }

        private static CCNetProjectStatisticsData LoadStatisticsFromFile()
        {
            CCNetProjectStatisticsData data;
            using (Stream stream = File.OpenRead(@"..\..\..\Data\Samples\ccnet.stats.xml"))
            {
                data = CCNetProjectStatisticsPlugIn.Load(stream);
            }
            return data;
        }
    }
}
