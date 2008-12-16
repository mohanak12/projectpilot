using System.IO;
using Flubu.Builds.VSSolutionBrowsing;
using MbUnit.Framework;
using ProjectPilot.Framework.Metrics;

namespace ProjectPilot.Tests.Framework.Metrics
{
    [TestFixture]
    public class LocStatsTest
    {
        [Test]
        public void TestLocOnSampleFile()
        {
            ILocStats locStats = new LocStats();

            Stream stream = File.OpenRead(@"..\..\..\Data\Samples\LocSample.cs");
            
            LocStatsData data = locStats.CountLocStream(stream);

            Assert.AreEqual(14, data.Cloc);
            Assert.AreEqual(9, data.Eloc);
            Assert.AreEqual(56, data.Sloc);
        }

        [Test]
        public void SolutionLocMetrics()
        {
            VSSolutionLocMetrics metrics = VSSolutionLocMetrics.CalculateLocForSolution(
                @"..\..\..\ProjectPilot.sln");

            LocStatsData data = metrics.GetLocStatsData();
        }
    }
}
