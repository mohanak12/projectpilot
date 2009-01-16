using Accipio;
using MbUnit.Framework;

namespace ProjectPilot.Tests.AccipioTests
{
    [TestFixture]
    public class ReportDataParserTest
    {
        [Test]
        public void TestReportDataParse()
        {
            string reportDataFileName = @"..\..\..\Data\Samples\TestResults.xml";

            using (ReportDataParser parser = new ReportDataParser(reportDataFileName))
            {
                ReportData reportData = parser.Parse();

                ICodeWriter writer = new FileCodeWriter("TestReport.html");
                CreateHtmlTestReport report = new CreateHtmlTestReport(writer);
                report.Generate(reportData);
            }
        }
    }
}
