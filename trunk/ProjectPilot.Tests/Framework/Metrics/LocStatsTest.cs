using System.IO;
using System.Xml;
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
            metrics.LocStatsMap.AddToMap(".cs", new CSharpLocStats());
            metrics.CalculateLocForSolution(@"..\..\..\ProjectPilot.sln");

            const string ReportFileName = @"XML_report.xml";

            metrics.GenerateXmlReport(ReportFileName);

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(ReportFileName);

            XmlNode xmlNode = xmlDocument.SelectSingleNode(
                "Root/Item/Subitem/Item[contains(@FileName,'ProjectPilot.Framework.csproj')]");
            Assert.IsNotNull(xmlNode);

            Assert.AreEqual(".csproj", xmlNode.Attributes["FileType"].Value);
        }
    }
}
