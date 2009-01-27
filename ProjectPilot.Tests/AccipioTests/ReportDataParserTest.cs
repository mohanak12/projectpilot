﻿using Accipio.Reporting;
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

                Assert.IsNotNull(reportData);
                //ICodeWriter writer = new FileCodeWriter("TestReport.html");
                //HtmlTestReportGenerator report = new HtmlTestReportGenerator(writer);
                //report.Generate(reportData);
            }
        }
    }
}
