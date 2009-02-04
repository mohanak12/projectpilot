using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using Accipio.Reporting;
using NVelocity;
using NVelocity.App;
using NVelocity.Runtime;

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
            graphDataGenerators.Add(new TestRunsThroughTimeGraphDataGenerator());
            graphDataGenerators.Add(new UserStoriesThroughTimeGraphDataGenerator());
            graphGenerator = new DefaultTestReportGraphGenerator();
        }

        public IList<ITestReportGraphDataGenerator> GraphDataGenerators
        {
            get { return graphDataGenerators; }
        }

        public ITestReportGraphGenerator GraphGenerator
        {
            get { return graphGenerator; }
            set { graphGenerator = value; }
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

            foreach (ITestReportGraphDataGenerator generator in graphDataGenerators)
            {
                TestReportGraphData graphData = generator.GenerateData(testRunDatabase);
                graphGenerator.GenerateGraph(graphData, settings);
            }
        }

        private void GenerateReportFile (
            string outputFileNameFormat,
            string templateFileName, 
            Hashtable context)
        {
            VelocityEngine velocity = new VelocityEngine();
            velocity.SetProperty(RuntimeConstants.RESOURCE_LOADER, "file");
            velocity.SetProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, settings.TemplatesDirectory);
            velocity.Init();
            VelocityContext velocityContext = new VelocityContext(context);
            velocityContext.Put("settings", settings);
            velocityContext.Put("reportTime", DateTime.Now);

            Template template = velocity.GetTemplate(templateFileName, new UTF8Encoding(false).WebName);

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
        private List<ITestReportGraphDataGenerator> graphDataGenerators = new List<ITestReportGraphDataGenerator>();
        private ITestReportGraphGenerator graphGenerator;
    }
}
