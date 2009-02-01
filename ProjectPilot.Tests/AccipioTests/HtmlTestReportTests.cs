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
        public void GenerateHtmlReportFile()
        {
            TestRunsDatabase db = new TestRunsDatabase();
            db.LoadDatabase(@"..\..\..\Data\Samples\", @"TestResults.xml");

            HtmlTestReportGeneratorSettings settings = new HtmlTestReportGeneratorSettings("TestProject");
            settings.OutputDirectory = "reports";
            settings.TemplatesDirectory = @"..\..\..\Accipio.Console\Templates";

            HtmlTestReportGenerator generator = new HtmlTestReportGenerator(settings);
            generator.Generate(db);
        }
    }
}
