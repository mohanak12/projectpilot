using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Text;
using Accipio.Reporting;
using Commons.Collections;
using NVelocity;
using NVelocity.App;
using ProjectPilot.Framework.Charts;
using ZedGraph;

namespace Accipio
{
    public class HtmlTestReportGenerator : IHtmlTestReportGenerator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlTestReportGenerator"/> class.
        /// </summary>
        /// <param name="settings">The settings to use for generating the report.</param>
        public HtmlTestReportGenerator(HtmlTestReportGeneratorSettings settings)
        {
            this.settings = settings;
        }

        /// <summary>
        /// Generate HTML report files from the test runs database.
        /// </summary>
        /// <param name="testRunDatabase">Test runs database.</param>
        public void Generate(TestRunsDatabase testRunDatabase)
        {
            // copy the CSS file to the reports directory
            string cssFileFullPath = settings.CssFileName;

            if (false == Path.IsPathRooted(cssFileFullPath))
                cssFileFullPath = Path.Combine(settings.TemplatesDirectory, cssFileFullPath);

            string cssCopiedPath = Path.Combine(settings.OutputDirectory, Path.GetFileName(cssFileFullPath));

            // make sure the path exists
            AccipioHelper.EnsureDirectoryPathExists(cssCopiedPath, true);

            File.Copy(cssFileFullPath, cssCopiedPath, true);

            FileVersionInfo version = System.Diagnostics.FileVersionInfo.GetVersionInfo (
                System.Reflection.Assembly.GetExecutingAssembly ().Location);
            string accipioVersion = version.FileVersion;
            string testCasesHistoryGraphFileName = "TestCasesHistoyGraph.png";
            string userStoriesHistoryGraphFileName = "UserStoriesHistoyGraph.png";

            IDictionary<string, string> graphs = new Dictionary<string, string>
                                                     {
                                                         { TestCaseGraph, testCasesHistoryGraphFileName },
                                                         { UserStoriesGraph, userStoriesHistoryGraphFileName }
                                                     };

            Hashtable context = new Hashtable();
            context.Add("db", testRunDatabase);
            context.Add("accipioVersion", accipioVersion);
            context.Add("testCasesGraphFileName", testCasesHistoryGraphFileName);
            context.Add("userStoriesGraphFileName", userStoriesHistoryGraphFileName);

            GenerateReportFile(
                "TestRunsHistory.htm",
                "TestRunsHistory.vm.htm",
                context);

            foreach (TestRun testRun in testRunDatabase.TestRuns)
            {
                context["testRun"] = testRun;
                GenerateReportFile(
                    @"TestRuns/$testRun.FileName",
                    "TestRunReport.vm.htm",
                    context);
            }

            CreateTestReportGraphs(graphs, testRunDatabase.TestRuns);
        }

        private void CreateTestReportGraphs(IDictionary<string, string> graphsFileNames, IList<TestRun> testRuns)
        {
            foreach (string graphType in graphsFileNames.Keys)
            {
                DrawTestReportGraph(graphType, graphsFileNames[graphType], testRuns);
            }
        }

        private void DrawTestReportGraph(
            string graphType,
            string graphFileName,
            IList<TestRun> runs)
        {
            using (FluentChart chart = FluentChart.Create(string.Empty, string.Empty, string.Empty))
            {
                chart
                    .SetBarSettings(BarType.Stack, 0)
                    .UseDateAsAxisY(runs[0].EndTime, runs[runs.Count - 1].EndTime);

                IDictionary<string, SortedList<DateTime, double>> fetchHistory = FetchTestCaseHistory(runs, graphType);

                string[] colors = { "green", "red", "yellow" };

                int i = 0;
                foreach (string status in fetchHistory.Keys)
                {
                    chart
                        .AddBarSeries(status, colors[i++])
                        .AddDataByTime(fetchHistory[status], runs[0].EndTime, runs[runs.Count - 1].EndTime);
                }

                chart
                    .ExportToBitmap(graphFileName, ImageFormat.Png, 1024, 800);
            }
        }

        private IDictionary<string, SortedList<DateTime, double>> FetchTestCaseHistory(IList<TestRun> runs, string graphType)
        {
            IDictionary<string, SortedList<DateTime, double>> testCaseshistory 
                = new Dictionary<string, SortedList<DateTime, double>>
                      {
                          { "success", new SortedList<DateTime, double>() },
                          { "failed", new SortedList<DateTime, double>() },
                          { "not implemented", new SortedList<DateTime, double>() }
                      };

            foreach (TestRun testRun in runs)
            {
                DateTime date = testRun.EndTime;

                testCaseshistory["success"].Add(
                    date, 
                    graphType == TestCaseGraph ? testRun.TestCasesSuccess : testRun.UserStoriesSuccess);
                testCaseshistory["failed"].Add(
                    date,
                    graphType == TestCaseGraph ? testRun.TestCasesFail : testRun.UserStoriesFail);
                testCaseshistory["not implemented"].Add(
                    date,
                    graphType == TestCaseGraph ? testRun.TestCasesNotImplemented : testRun.UserStoriesNotImplemented);
            }

            return testCaseshistory;
        }

        private void GenerateReportFile (
            string outputFileNameFormat, 
            string templateFileName, 
            Hashtable context)
        {
            VelocityEngine velocity = new VelocityEngine();
            ExtendedProperties props = new ExtendedProperties();
            velocity.Init(props);
            VelocityContext velocityContext = new VelocityContext(context);
            velocityContext.Put("settings", settings);
            velocityContext.Put("reportTime", DateTime.Now);

            string fullTemplateFileName = Path.Combine(settings.TemplatesDirectory, templateFileName);

            Template template = velocity.GetTemplate(fullTemplateFileName, new UTF8Encoding(false).WebName);

            string outputFileName;

            using (StringWriter writer = new StringWriter(CultureInfo.InvariantCulture))
            {
                velocity.Evaluate(velocityContext, writer, "dummy", outputFileNameFormat);
                writer.Flush();
                outputFileName = writer.ToString();
            }

            string fullOutputFileName = Path.Combine(settings.OutputDirectory, outputFileName);

            // make sure the path exists
            AccipioHelper.EnsureDirectoryPathExists(fullOutputFileName, true);

            using (Stream stream = File.Open(fullOutputFileName, FileMode.Create))
            {
                using (TextWriter writer = new StreamWriter(stream, new UTF8Encoding(false)))
                {
                    template.Merge(velocityContext, writer);
                }
            }
        }

        private readonly HtmlTestReportGeneratorSettings settings;
        private const string TestCaseGraph = "TestCases";
        private const string UserStoriesGraph = "UserStories";
    }
}
