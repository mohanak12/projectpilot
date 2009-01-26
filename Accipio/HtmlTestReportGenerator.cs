using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using ProjectPilot.Framework;

namespace Accipio
{
    public class HtmlTestReportGenerator : IHtmlTestReportGenerator
    {
        /// <summary>
        /// Generate HTML report for tests.
        /// </summary>
        /// <param name="reportData">Test report data.</param>
        public void Generate(ReportData reportData)
        {
            IFileManager fileManager = new DefaultFileManager(string.Empty, null);
            ITemplateEngine templateEngine = new DefaultTemplateEngine(fileManager);
            IList<UserStory> userStories = UserStoryDataMiner.GetUserStoryDetails(reportData);

            Hashtable templateContext = new Hashtable();
            templateContext.Add("startTime", reportData.StartTime);
            templateContext.Add("endTime", reportData.StartTime.AddSeconds(Convert.ToDouble(reportData.Duration, CultureInfo.InvariantCulture)));
            templateContext.Add("duration", reportData.Duration);
            templateContext.Add("userStories", userStories);
            templateContext.Add("reportSuites", reportData.TestSuites);

            string reportFileName = string.Format(CultureInfo.InvariantCulture, "TestReport_{0:yyyyMMddhhmm}.htm", DateTime.Now);

            // generate html report
            templateEngine.ApplyTemplate(
            "ReportTemplate.vm",
            templateContext,
            fileManager.CreateNewFile("Reports", reportFileName));
        }
    }
}
