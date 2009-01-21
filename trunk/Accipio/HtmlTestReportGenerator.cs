using System.Collections.Generic;
using System.Globalization;

namespace Accipio
{
    public class HtmlTestReportGenerator : IHtmlTestReportGenerator
    {
        /// <summary>
        /// Initializes a new instance of the HtmlTestReportGenerator class.
        /// </summary>
        /// <param name="writer">Interface of <see cref="ICodeWriter" />.</param>
        public HtmlTestReportGenerator(ICodeWriter writer)
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
            WriteLine("    <title>Acceptance test report</title>");
            WriteLine(CssStyle);
            WriteLine("</head>");
            WriteLine("<body>");
            WriteLine("     <h1>Test report details</h1>");
            WriteLine("     <b>Version:</b> {0}  <b>StartTime:</b> {1:s}  <b>Duration:</b> {2} seconds", reportData.Version, reportData.StartTime, reportData.Duration);
            WriteLine("     <h2>User stories</h2>");

            WriteUserStoriesDetails(reportData);

            WriteLine("     <h2>Suites</h2>");

            foreach (ReportSuite reportSuite in reportData.TestSuites)
            {
                string report = string.Format(
                    CultureInfo.InvariantCulture, 
                    "Summary: Passed {0}, Failed {1}, Skipped {2}", 
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

        /// <summary>
        /// Write all user stories that are presented in test cases.
        /// </summary>
        /// <param name="reportData">Test report data.</param>
        private void WriteUserStoriesDetails(ReportData reportData)
        {
            IList<UserStory> userStories = UserStoryDataMiner.GetUserStoryDetails(reportData);

            string details = "<ul>";

            foreach (UserStory userStory in userStories)
            {
                details += string.Format(CultureInfo.InvariantCulture, "<li>{0}</li>", userStory);
            }

            details += "</ul>";

            WriteLine(details);
        }

        /// <summary>
        /// Write all test cases, its status and details.
        /// </summary>
        /// <param name="reportCases">Test cases.</param>
        private void WriteTestCases(IEnumerable<ReportCase> reportCases)
        {
            // write html table start element
            WriteLine(HtmlTableStartElement);
            
            // write cases and case details
            foreach (ReportCase reportCase in reportCases)
            {
                WriteLine(
                    HtmlTableRow,
                    reportCase.CaseId,
                    reportCase.Status,
                    AddUserStoriesInTestCaseToReport(reportCase.UserStories),
                    reportCase.ReportDetails ?? HtmlSpaceElement,
                    AddColor(reportCase));
            }

            // write html table end element
            WriteLine(HtmlTableEndElement);
        }

        /// <summary>
        /// Adds background color to test case column.
        /// </summary>
        /// <param name="reportCase">See <see cref="ReportCase"/></param>
        /// <returns>
        /// <c>Green</c> for Passed, 
        /// <c>Red</c> for Failed, 
        /// <c>Yellow</c> otherwise
        /// </returns>
        private static string AddColor(ReportCase reportCase)
        {
            return reportCase.Status == ReportCaseStatus.Passed ? "tdColorGreen" : reportCase.Status == ReportCaseStatus.Failed ? "tdColorRed" : "tdColorYellow";
        }

        /// <summary>
        /// Add all user stories for each test case to unordered list (html).
        /// </summary>
        /// <param name="stories">List of user stories.</param>
        /// <returns>Unordered list of user stories.</returns>
        private static string AddUserStoriesInTestCaseToReport(IEnumerable<string> stories)
        {
            string details = "<ul>";

            foreach (string userStory in stories)
            {
                details += string.Format(CultureInfo.InvariantCulture, "<li>{0}</li>", userStory);
            }

            return string.Format(CultureInfo.InvariantCulture, "{0}</ul>", details);
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
            @"<tr class=""trColorWhite""><td class=""{4}"">{0}<br />[<b><i>{1}</i></b>]</td><td class=""tdTextAlign"">{2}</td><td class=""tdTextAlign"">{3}</td></tr>";

        private const string HtmlTableStartElement =
            @"<table border=""1"" width=""80%""><tr><td width=""20%""><b>CaseId</b></td><td width=""30%""><b>User stories</b></td><td><b>Details</b></td></tr>";

        private const string HtmlTableEndElement = "</table>";
        private const string HtmlListElement = "<ul><li><h3><u>{0}</u></h3></li></ul>";
        private const string HtmlSpaceElement = "&nbsp;";
        private const string CssStyle = @"<style type=""text/css"">
                                            .tdTextAlign { vertical-align: top; }
                                            .tdColorGreen { vertical-align: top; background-color: Green; }
                                            .tdColorRed { vertical-align: top; background-color: Red; }
                                            .tdColorYellow { vertical-align: top; background-color: Yellow; }
                                            .trColorWhite { background-color: White; }
                                        </style>";

        private readonly ICodeWriter writer;
    }
}
