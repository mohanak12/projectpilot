using System;
using System.Globalization;
using System.IO;
using Accipio;
using Accipio.Console;
using MbUnit.Framework;
using Rhino.Mocks;

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
            IConsoleCommand consoleCommand = new HtmlReportGeneratorCommand(null)
                .ParseArguments(new[] { "report", fileName, "Report.html" });
            consoleCommand.AccipioDirectory = string.Empty;
            // process command
            consoleCommand.ProcessCommand();

            // get output file
            return ((HtmlReportGeneratorCommand)consoleCommand).OutputFile;
        }
    }
}
