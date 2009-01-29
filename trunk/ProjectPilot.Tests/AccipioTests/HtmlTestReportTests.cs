using System.IO;
using Accipio;
using Accipio.Reporting;
using MbUnit.Framework;

namespace ProjectPilot.Tests.AccipioTests
{
    [TestFixture]
    public class HtmlTestReportTests
    {
        /// <summary>
        /// Test checks generation of html report file context for report data.
        /// </summary>
        [Test]
        public void GenerateHtmlReportFileTest()
        {
            TestRunsDatabase db = new TestRunsDatabase();
            db.LoadDatabase(@"..\..\..\Data\Samples\", @"TestResults.xml");

            TestRun originalTestRun = db.TestRuns[0];

            // make 100 copies of the same test run
            for (int i = 0; i < 100; i++)
                db.TestRuns.Add(originalTestRun);
            
            HtmlTestReportGeneratorSettings settings = new HtmlTestReportGeneratorSettings("TestProject");
            settings.OutputDirectory = "reports";
            settings.TemplatesDirectory = @"..\..\..\Accipio.Console\Templates";

            HtmlTestReportGenerator generator = new HtmlTestReportGenerator(settings);
            generator.Generate(db);
        }
    }
}
