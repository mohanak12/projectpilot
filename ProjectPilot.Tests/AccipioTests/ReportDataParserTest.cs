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
            string reportDataFileName = @"..\..\..\Data\Samples\AccipioTestReportSample.xml";

            using (ReportDataParser parser = new ReportDataParser(reportDataFileName))
            {
                ReportData reportData = parser.Parse();
            }
        }
    }
}
