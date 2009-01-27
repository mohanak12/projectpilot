using System;
using System.IO;
using Accipio.Console;
using MbUnit.Framework;

namespace ProjectPilot.Tests.AccipioTests
{
    [TestFixture]
    public class HtmlReportGeneratorTest
    {
        /// <summary>
        /// Test checks generation of html report file context for report data.
        /// </summary>
        [Test, Pending("Igor: Test failes because Template folder is missing in bin\\Release folder")]
        public void GenerateHtmlReportFileTest()
        {
            string outputFile = GenerateHtmlTestReportOutputFile();
            Assert.IsTrue(File.Exists(outputFile));
        }

        /// <summary>
        /// Generates html report file.
        /// </summary>
        /// <returns>generated filename.</returns>
        public static string GenerateHtmlTestReportOutputFile()
        {
            string fileName = @"..\..\..\Data\Samples\TestResults.xml";
            HtmlTestReportGeneratorCommand consoleCommand = new HtmlTestReportGeneratorCommand();

            string[] args = new[] { "report", fileName, "Report.html" };
            Assert.AreEqual(0, consoleCommand.Execute(args));

            // get output file
            throw new NotImplementedException();
        }
    }
}
