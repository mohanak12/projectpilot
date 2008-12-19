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
            ILocStats locStats = new CSharpLocStats();

            Stream stream = File.OpenRead(@"..\..\..\Data\Samples\LocSample.cs");
            
            LocStatsData data = locStats.CountLocStream(stream);

            Assert.AreEqual(14, data.Cloc);
            Assert.AreEqual(9, data.Eloc);
            Assert.AreEqual(56, data.Sloc);
        }

        [Test]
        public void SolutionLocMetrics()
        {
            VSSolutionLocMetrics metrics = new VSSolutionLocMetrics("ProjectPilot.sln");
            
            // add known extensions
            metrics.LocStatsMap.AddToMap(".cs", new CSharpLocStats());
            //metrics.LocStatsMap.AddToMap(".aspx", new AspxLocStats());

            metrics.CalculateLocForSolution(
            //    @"c:\Documents and Settings\jureh\My Documents\Visual Studio 2008\Projects\WebApplication1\WebApplication1.sln");
                @"..\..\..\ProjectPilot.sln");

            LocStatsData data = metrics.GetLocStatsData();
        }

        [Test, Pending("Marko: todo")]
        public void TestGeneratingSolutionXmlReport()
        {
            VSSolutionLocMetrics metrics = new VSSolutionLocMetrics("ProjectPilot.sln");
            metrics.CalculateLocForSolution(@"..\..\..\ProjectPilot.sln");
            metrics.GenerateXmlReport(@"XML_report.xml");
        }
    }
}
