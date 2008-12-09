using System.IO;
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

            Stream stream = File.OpenRead(@"\PilotProject\Data\Samples\LocSample.cs");
            
            LocStatsData data = locStats.CountLocString(stream);

            Assert.AreEqual(8, data.Cloc);
            Assert.AreEqual(9, data.Eloc);
            Assert.AreEqual(56, data.Sloc);
        }
    }
}
