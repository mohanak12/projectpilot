using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using MbUnit.Framework;
using ProjectPilot.Framework;
using ProjectPilot.Framework.CCNet;
using ProjectPilot.Framework.Charts;
using Rhino.Mocks;
using ThoughtWorks.CruiseControl.Remote;
using ZedGraph;

namespace ProjectPilot.Tests.Framework.CCNet
{
    [TestFixture]
    public class GraphBuildingTests
    {
        [Test]
        public void DrawChartTest()
        {
            ProjectStatsGraph graph = new ProjectStatsGraph();
            graph.GraphName = "Build duration";
            graph.YAxisTitle = "Seconds";
            graph.AddParameter<TimeSpan>("Duration", "Green");

            ProjectStatsData data = GetStatisticData();

            //Labels on X-Axis
            List<string> xLabels = new List<string> { "Test0", "Test1", "Test2", "Test3", "Test4", "Test5" };

            ProjectStatsGraphData graphData = new ProjectStatsGraphData(data);
            graphData.SetValue(0, "Duration", 12);
            graphData.SetValue(1, "Duration", 52);
            graphData.SetValue(2, "Duration", 2);
            graphData.SetValue(3, "Duration", 60);
            graphData.SetValue(4, "Duration", 10);
            graphData.SetValue(5, "Duration", 61);

            using (FluentChart chart = FluentChart.Create(graph.GraphName, graph.XAxisTitle, graph.YAxisTitle))
            {
                chart.SetLabelsToXAxis(xLabels);

                foreach (ProjectStatsGraphParameter parameter in graph.GraphParameters)
                {
                    chart
                        .AddLineSeries(parameter.ParameterName, parameter.SeriesColor)
                        .AddData(graphData.GetValuesForParameter(parameter.ParameterName))
                        .SetSymbol(SymbolType.Circle, parameter.SeriesColor, 4, true);
                }

                chart
                    .ExportToBitmap("test.png", ImageFormat.Png, 2000, 800);
            }
        }

        [Test]
        public void PrepareDataMatrixTest()
        {
            ProjectStatsData data = GetStatisticData();

            ProjectStatsGraph graph = new ProjectStatsGraph();
            graph.GraphName = "Build duration";
            graph.YAxisTitle = "Seconds";
            graph.AddParameter<TimeSpan>("Duration", "Green");

            ProjectStatsGraphData graphData = new ProjectStatsGraphData(data);

            int buildId = 0;

            int buildNumbers = 50;
            bool showBuildProjectHistory = false;
            bool ignoreFailures = true;
            List<string> xLabels = new List<string>();


            // go through all builds
            for (int i = 0; i < data.Builds.Count; i++)
            {
                // show last 50 builds on graph if parameter is set to false
                if (!showBuildProjectHistory)
                {
                    if (i < data.Builds.Count - buildNumbers)
                        continue;
                }

                ProjectStatsBuildEntry entry = data.Builds[i];

                // only successful builds are allowed
                if (ignoreFailures && entry.Parameters["Success"] == "0")
                {
                    continue;
                }

                bool addValue = false;

                // if the current build label has not already been added to the xLabels
                // group builds by build name
                if (entry.BuildLabel != xLabels.Find(temp => temp == entry.BuildLabel))
                {
                    // add build name to list. Build name will be shown on x-axis
                    xLabels.Add(entry.BuildLabel);

                    // this two values are used only for grouping builds with same name
                    // grouping is used only for create build report statistic
                    addValue = true;
                    buildId = entry.BuildId;
                }

                // go through all parameters
                foreach (ProjectStatsGraphParameter parameter in graph.GraphParameters)
                {
                    double value = 0;

                    if (entry.Parameters.ContainsKey(parameter.ParameterName))
                    {
                        if (parameter.ParameterType == typeof(TimeSpan))
                        {
                            value = TimeSpan.Parse(entry.Parameters[parameter.ParameterName]).TotalSeconds;
                        }
                        else if (parameter.ParameterType == typeof(double))
                        {
                            value = Convert.ToDouble(
                                entry.Parameters[parameter.ParameterName],
                                CultureInfo.InvariantCulture);
                        }
                    }

                    if (addValue)
                    {
                        // set value for parameter name
                        graphData.SetValue(buildId, parameter.ParameterName, value);
                    }
                    else
                    {
                        // increment value
                        graphData.IncValue(buildId, parameter.ParameterName, value);
                    }
                }

                Assert.AreEqual(
                    TimeSpan.Parse(entry.Parameters["Duration"]).TotalSeconds,
                    graphData.GetValue(entry.BuildId, "Duration"));
            }

            Assert.AreEqual(buildNumbers, graphData.GetValuesForParameter("Duration").Count);
        }

        [Test,Ignore]
        public void GraphsTest()
        {
            List<ProjectStatsGraph> graphs = new List<ProjectStatsGraph>();

            ProjectStatsGraph graph = new ProjectStatsGraph();
            graph.IgnoreFailures = true;
            graph.GraphName = "FxCop Info";
            graph.AddParameter<double>("Blue", "FxCop Warnings");
            graph.AddParameter<double>("Red", "FxCop Errors");

            graphs.Add(graph);

            graph = new ProjectStatsGraph();
            graph.IgnoreFailures = false;
            graph.GraphName = "Build Report";
            graph.AddParameter<double>("Success", "Green");
            graph.AddParameter<double>("Failure", "Red");
            graph.AddParameter<double>("Exception", "Blue");

            graphs.Add(graph);

            graph = new ProjectStatsGraph();
            graph.IgnoreFailures = true;
            graph.GraphName = "MbUnit Tests";
            graph.AddParameter<double>("MbUnit TestCount", "Red");
            graph.AddParameter<double>("MbUnit TestPassed", "Green");
            graphs.Add(graph);

            graph = new ProjectStatsGraph(); 
            graph.IgnoreFailures = true;
            graph.GraphName = "Build Duration";
            graph.YAxisTitle = "Seconds";
            graph.AddParameter<TimeSpan>("Duration", "Green");

            graphs.Add(graph);

            ProjectPilotConfiguration projectPilotConfiguration = new ProjectPilotConfiguration();
            projectPilotConfiguration.ProjectPilotWebAppRootUrl = "http://localhost/projectpilot/";

            ProjectRegistry projectRegistry = new ProjectRegistry();
            Project project = new Project("CCNetStatistics", "");
            projectRegistry.AddProject(project);

            IFileManager fileManager = new DefaultFileManager("", projectPilotConfiguration);
            projectRegistry.FileManager = fileManager;

            IFileManager templateFileManager = MockRepository.GenerateStub<IFileManager>();
            templateFileManager.Stub(action => action.GetFullFileName(null, null)).IgnoreArguments().Return(@"..\..\..\Data\Templates\CCNetReportStatistics.vm");

            ITemplateEngine templateEngine = new DefaultTemplateEngine(templateFileManager);

            // prepare test data
            ProjectStatsData data = GetStatisticData();

            ICCNetProjectStatisticsPlugIn plugIn = MockRepository.GenerateStub<ICCNetProjectStatisticsPlugIn>();
            plugIn.Stub(action => action.FetchStatistics()).Return(data);

            // ignore failures only if you want to build build report statistic
            CCNetProjectStatisticsModule module = new CCNetProjectStatisticsModule(
                plugIn, graphs, fileManager, templateEngine, false);

            module.ProjectId = "CCNetStatistics";
            project.AddModule(module);

            module.ExecuteTask(null);
        }

        [Test,Explicit("This test should not be run automatically"),]
        public void ReadCCNetStatisticsTest()
        {
            RemoteCruiseManagerFactory factory = new RemoteCruiseManagerFactory();
            Uri url = new Uri(string.Format(CultureInfo.InvariantCulture, "tcp://firefly:21234/CruiseManager.rem"));
            string projectName = "ProjectPilot";
            ICruiseManager mgr = factory.GetCruiseManager(url.ToString());

            string proj = mgr.GetProject(projectName);
            string stat = mgr.GetStatisticsDocument(projectName);
            //File.WriteAllText("ccnet.stats.xml", stat);
        }

        private ProjectStatsData GetStatisticData()
        {
            ProjectStatsData data;
            using (Stream stream = File.OpenRead(@"..\..\..\Data\Samples\ccnet.stats.xml"))
            {
                data = CCNetProjectStatisticsPlugIn.Load(stream);
            }

            return data;
        }
    }
}
