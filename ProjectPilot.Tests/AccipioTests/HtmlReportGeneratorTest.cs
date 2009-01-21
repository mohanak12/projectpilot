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
        [Test]
        public void GenerateHtmlReportFileTest()
        {
            string outputFile = GenerateHtmlTestReportOutputFile();
            Assert.IsTrue(File.Exists(outputFile));
        }

        [Test]
        public void GenerateHtmlReportTest()
        {
            ReportData reportData = new ReportData();
            reportData.Duration = "14.3";
            reportData.StartTime = Convert.ToDateTime("2009-1-20 10:00", CultureInfo.InvariantCulture);
            reportData.Version = "1.00.00";
            ReportSuite reportSuite = new ReportSuite("TestSuiteId");
            reportSuite.FailedTests = 0;
            reportSuite.SkippedTests = 0;
            reportSuite.PassedTests = 1;
            ReportCase reportCase = new ReportCase("CaseId", Convert.ToDateTime("2009-1-20 10:00", CultureInfo.InvariantCulture), "14.3", ReportCaseStatus.Passed);
            reportCase.UserStories.Add("UserStory1");
            reportCase.UserStories.Add("UserStory2");
            reportSuite.AddTestCase(reportCase);
            reportData.TestSuites.Add(reportSuite);

            ICodeWriter mockWriter = MockRepository.GenerateMock<ICodeWriter>();
            IHtmlTestReportGenerator generator = new HtmlTestReportGenerator(mockWriter);

            mockWriter.Expect(writer => writer.WriteLine(@"<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">"));
            mockWriter.Expect(writer => writer.WriteLine(@"<html xmlns=""http://www.w3.org/1999/xhtml"" >"));
            mockWriter.Expect(writer => writer.WriteLine("<head>"));
            mockWriter.Expect(writer => writer.WriteLine("    <title>Acceptance test report</title>"));
            mockWriter.Expect(writer => writer.WriteLine("</head>"));
            mockWriter.Expect(writer => writer.WriteLine("<body>"));
            mockWriter.Expect(writer => writer.WriteLine("     <h1>Test report details</h1>"));
            mockWriter.Expect(writer => writer.WriteLine("     <b>Version:</b> 1.00.00  <b>StartTime:</b> 2009-01-20T10:00:00  <b>Duration:</b> 14.3 seconds"));
            mockWriter.Expect(writer => writer.WriteLine("     <h2>User stories</h2>"));
            mockWriter.Expect(writer => writer.WriteLine("<ul><li>UserStory1 (1/1)</li><li>UserStory2 (1/1)</li></ul>"));
            mockWriter.Expect(writer => writer.WriteLine("     <h2>Suites</h2>"));
            mockWriter.Expect(writer => writer.WriteLine("<ul><li><h3><u>TestSuiteId</u></h3></li></ul>"));
            mockWriter.Expect(writer => writer.WriteLine("Summary: Passed 1, Failed 0, Skipped 0"));
            mockWriter.Expect(writer => writer.WriteLine("<table border=\"1\" width=\"80%\"><tr><td width=\"20%\"><b>CaseId</b></td><td width=\"30%\"><b>User stories</b></td><td><b>Details</b></td></tr>"));
            mockWriter.Expect(writer => writer.WriteLine("<tr style=\"background-color: White;\"><td style=\"vertical-align: top; background-color: Green;\">CaseId<br />[<b><i>Passed</i></b>]</td><td style=\"vertical-align: top;\"><ul><li>UserStory1</li><li>UserStory2</li></ul></td><td style=\"vertical-align: top;\">&nbsp;</td></tr>"));
            mockWriter.Expect(writer => writer.WriteLine("</table>"));
            mockWriter.Expect(writer => writer.WriteLine("</body>"));
            mockWriter.Expect(writer => writer.WriteLine("</html>"));

            generator.Generate(reportData);

            mockWriter.VerifyAllExpectations();
        }

        /// <summary>
        /// Generates html report file.
        /// </summary>
        /// <returns>generated filename.</returns>
        public static string GenerateHtmlTestReportOutputFile()
        {
            string fileName = @"..\..\..\Data\Samples\TestResults.xml";
            IConsoleCommand consoleCommand = new HtmlReportGeneratorCommand(null)
                .ParseArguments(new[] { "report", fileName });
            consoleCommand.AccipioDirectory = string.Empty;
            // process command
            consoleCommand.ProcessCommand();

            // get output file
            return ((HtmlReportGeneratorCommand)consoleCommand).OutputFile;
        }
    }
}
