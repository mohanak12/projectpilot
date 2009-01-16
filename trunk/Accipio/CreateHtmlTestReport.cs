using System.Collections.Generic;
using System.Globalization;

namespace Accipio
{
    public class CreateHtmlTestReport : ITestReportGenerator
    {
        /// <summary>
        /// Initializes a new instance of the CreateHtmlTestReport class.
        /// </summary>
        /// <param name="writer">Interface of <see cref="ICodeWriter" />.</param>
        public CreateHtmlTestReport(ICodeWriter writer)
        {
            this.writer = writer;
        }

        /// <summary>
        /// Generate HTML report for tests.
        /// </summary>
        /// <param name="reportData">Test report data.</param>
        public void Generate(ReportData reportData)
        {
            WriteLine(
                @"<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">");
            WriteLine(@"<html xmlns=""http://www.w3.org/1999/xhtml"" >");
            WriteLine("<head>");
            WriteLine("    <title>MbUnit test report</title>");
            WriteLine("</head>");
            WriteLine("<body>");
            WriteLine("     <h1>Test report details</h1>");
            WriteLine("     <b>Version:</b> {0}  <b>StartTime:</b> {1:s}  <b>Duration:</b> {2} seconds", reportData.Version, reportData.StartTime, reportData.Duration);
            WriteLine("     <h2>Suites</h2>");

            foreach (ReportSuite reportSuite in reportData.TestSuites)
            {
                string report = string.Format(
                    CultureInfo.InvariantCulture, 
                    "(Passed: {0}, Failed {1}, Skipped {2})", 
                    reportSuite.PassedTests, 
                    reportSuite.FailedTests, 
                    reportSuite.SkippedTests);

                WriteLine(HtmlListElement, reportSuite.SuiteId);
                WriteLine(report);
                WriteTestCases(reportSuite.TestCases);
            }

            WriteLine("</body>");
            WriteLine("</html>");

            writer.Close();
        }

        private void WriteTestCases(IList<ReportCase> reportCases)
        {
            // write html table start element
            WriteLine(HtmlTableStartElement);
            
            // write cases and case details
            foreach (ReportCase reportCase in reportCases)
            {
                if (reportCase.Status == ReportCaseStatus.Failed)
                {
                    WriteLine(
                        HtmlTableRow, 
                        reportCase.CaseId, 
                        reportCase.Status,
                        AddUserStoriesToReport(reportCase.UserStories),
                        reportCase.ReportDetails);
                }
                else
                {
                    WriteLine(
                        HtmlTableRow, 
                        reportCase.CaseId, 
                        reportCase.Status, 
                        AddUserStoriesToReport(reportCase.UserStories), 
                        HtmlSpaceElement);
                }
            }

            // write html table end element
            WriteLine(HtmlTableEndElement);
        }

        private static string AddUserStoriesToReport(IEnumerable<string> stories)
        {
            string userStories = "<ul>";

            foreach (string userStory in stories)
            {
                userStories += string.Format(CultureInfo.InvariantCulture, "<li>{0}</li>", userStory);
            }

            return string.Format(CultureInfo.InvariantCulture, "{0}</ul>", userStories);
        }

        private void WriteLine(string line)
        {
            writer.WriteLine(line);
        }

        private void WriteLine(
            string format,
            params object[] args)
        {
            writer.WriteLine(string.Format(CultureInfo.InvariantCulture, format, args));
        }

        private const string HtmlTableRow =
            "<tr style=\"background-color: White;\"><td style=\"vertical-align: top;\">{0}<br />[<b><i>{1}</i></b>]</td><td style=\"vertical-align: top;\">{2}</td><td style=\"vertical-align: top;\">{3}</td></tr>";

        private const string HtmlTableStartElement =
            "<table border=\"1\" width=\"80%\"><tr><td width=\"20%\"><b>CaseId</b></td><td width=\"30%\"><b>User stories</b></td><td><b>Details</b></td></tr>";

        private const string HtmlTableEndElement = "</table>";
        private const string HtmlListElement = "<ul><li><h3><u>{0}</u></h3></li></ul>";
        private const string HtmlSpaceElement = "&nbsp;"; 
        private readonly ICodeWriter writer;
    }
}
