using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Accipio.Reporting;
using ProjectPilot.Framework;

namespace Accipio
{
    public class HtmlTestReportGenerator : IHtmlTestReportGenerator
    {
        /// <summary>
        /// Generate HTML report for tests.
        /// </summary>
        /// <param name="testRunDatabase">Test report data.</param>
        public void Generate(TestRunsDatabase testRunDatabase)
        {
            IFileManager fileManager = new DefaultFileManager(string.Empty, null);
            ITemplateEngine templateEngine = new DefaultTemplateEngine(fileManager);
            //IList<UserStory> userStories = UserStoryDataMiner.GetUserStoryDetails(testRunDatabase);

            //Hashtable templateContext = new Hashtable();
            //templateContext.Add("startTime", testRunDatabase.StartTime);
            //templateContext.Add("endTime", testRunDatabase.StartTime.AddSeconds(Convert.ToDouble(testRunDatabase.Duration, CultureInfo.InvariantCulture)));
            //templateContext.Add("duration", testRunDatabase.Duration);
            //templateContext.Add("userStories", userStories);
            //templateContext.Add("reportSuites", testRunDatabase.TestSuites);

            //string reportFileName = string.Format(CultureInfo.InvariantCulture, "TestReport_{0:yyyyMMddhhmm}.htm", DateTime.Now);

            //// generate html report
            //templateEngine.ApplyTemplate(
            //"ReportTemplate.vm",
            //templateContext,
            //fileManager.CreateNewFile("Reports", reportFileName));
        }
    }
}
