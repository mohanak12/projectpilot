using Accipio;
using Accipio.Reporting;
using MbUnit.Framework;

namespace ProjectPilot.Tests.AccipioTests.Reporting
{
    [TestFixture]
    public class HtmlTestReportTests
    {
        /// <summary>
        /// Test checks generation of html report file context for report data.
        /// </summary>
        [Test]
        public void GenerateHtmlReportFile()
        {
            TestRunsDatabase db = new TestRunsDatabase();
            db.LoadDatabase(@"..\..\..\Data\Samples\", @"TestResults.xml");

            for (int i = 0; i < 20; i++)
                db.TestRuns.Add(db.TestRuns[0]);

            HtmlTestReportGeneratorSettings settings = new HtmlTestReportGeneratorSettings("TestProject");
            settings.OutputDirectory = "reports";
            settings.TemplatesDirectory = @"../../../Accipio/Templates/TestReports";

            HtmlTestReportGenerator generator = new HtmlTestReportGenerator(settings);
            generator.Generate(db);
        }
    }
}