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

            ProjectStatsData data = GetStatisticDataFromFile();

            //Labels on X-Axis
            List<string> xLabels = new List<string> { "Test0_1233345667", "Test1_1233345667", "Test2_1233345667", "Test3_1233345667", "Test4_1233345667", "Test5_1233345667" };

            ProjectStatsGraphData graphData = new ProjectStatsGraphData(data);
            graphData.SetValue(0, "Duration", 12);
            graphData.SetValue(1, "Duration", 52);
            graphData.SetValue(2, "Duration", 2);
            graphData.SetValue(3, "Duration", 60);
            graphData.SetValue(4, "Duration", 10);
            graphData.SetValue(5, "Duration", 61);

            using (FluentChart chart = FluentChart.Create(graph.GraphName, graph.XAxisTitle, graph.YAxisTitle))
            {
                foreach (ProjectStatsGraphParameter parameter in graph.GraphParameters)
                {
                    chart
                        .AddLineSeries(parameter.ParameterName, parameter.SeriesColor)
                        .AddData(graphData.GetValuesForParameter(parameter.ParameterName))
                        .SetSymbol(SymbolType.Circle, parameter.SeriesColor, 4, true);
                }

                chart.SetXAxis(1, 6);
                chart.SetLabelsToXAxis(xLabels);

                chart
                    .ExportToBitmap("test.png", ImageFormat.Png, 2000, 800);
            }
        }

        [Test]
        public void DrawGraphTest()
        {
            List<ProjectStatsGraph> graphs = new List<ProjectStatsGraph>();

            //ProjectStatsGraph graph = new ProjectStatsGraph(); 
            //graph.IgnoreFailures = true;
            //graph.GraphName = "Build Duration";
            //graph.YAxisTitle = "Seconds";
            //graph.AddParameter<TimeSpan>("Duration", "Green");

            ProjectStatsGraph graph = new ProjectStatsGraph();
            graph.IgnoreFailures = false;
            graph.GraphName = "Build Report";
            graph.YAxisTitle = "Build";
            graph.AddParameter<double>("Success", "Green");
            graph.AddParameter<double>("Failure", "Red");
            graph.AddParameter<double>("Exception", "Blue");

            graphs.Add(graph);

            ProjectPilotConfiguration projectPilotConfiguration = new ProjectPilotConfiguration();
            projectPilotConfiguration.ProjectPilotWebAppRootUrl = "http://localhost/projectpilot/";

            ProjectRegistry projectRegistry = new ProjectRegistry();
            Project project = new Project("CCNetStatistics", String.Empty);
            projectRegistry.AddProject(project);

            IFileManager fileManager = new DefaultFileManager(String.Empty, projectPilotConfiguration);
            projectRegistry.FileManager = fileManager;

            IFileManager templateFileManager = MockRepository.GenerateStub<IFileManager>();
            templateFileManager.Stub(action => action.GetFullFileName(null, null)).IgnoreArguments().Return(@"..\..\..\Data\Templates\CCNetReportStatistics.vm");

            ITemplateEngine templateEngine = new DefaultTemplateEngine(templateFileManager);

            // prepare test data
            ProjectStatsData data = GetStatisticDataFromFile();

            ICCNetProjectStatisticsPlugIn plugIn = MockRepository.GenerateStub<ICCNetProjectStatisticsPlugIn>();
            plugIn.Stub(action => action.FetchStatistics()).Return(data);

            // ignore failures only if you want to build build report statistic
            CCNetProjectStatisticsModule module = new CCNetProjectStatisticsModule(
                plugIn, graphs, fileManager, templateEngine, true);

            module.ProjectId = "CCNetStatistics";
            project.AddModule(module);

            module.ExecuteTask(null);
            module.FetchHtmlReport();

            Assert.AreEqual(module.ProjectId, "CCNetStatistics");
            Assert.AreEqual(module.ModuleName, "CCNet Project Statistics");
        }

        [Test]
        public void DrawGraphIgnoreFailuresTest()
        {
            List<ProjectStatsGraph> graphs = new List<ProjectStatsGraph>();

            ProjectStatsGraph graph = new ProjectStatsGraph();
            graph.IgnoreFailures = true;
            graph.GraphName = "Build Duration";
            graph.YAxisTitle = "Seconds";
            graph.AddParameter<TimeSpan>("Duration", "Green");

            graphs.Add(graph);

            ProjectPilotConfiguration projectPilotConfiguration = new ProjectPilotConfiguration();
            projectPilotConfiguration.ProjectPilotWebAppRootUrl = "http://localhost/projectpilot/";

            ProjectRegistry projectRegistry = new ProjectRegistry();
            Project project = new Project("CCNetStatistics", String.Empty);
            projectRegistry.AddProject(project);

            IFileManager fileManager = new DefaultFileManager(String.Empty, projectPilotConfiguration);
            projectRegistry.FileManager = fileManager;

            IFileManager templateFileManager = MockRepository.GenerateStub<IFileManager>();
            templateFileManager.Stub(action => action.GetFullFileName(null, null)).IgnoreArguments().Return(@"..\..\..\Data\Templates\CCNetReportStatistics.vm");

            ITemplateEngine templateEngine = new DefaultTemplateEngine(templateFileManager);

            // prepare test data
            ProjectStatsData data = GetStatisticDataFromFile();

            ICCNetProjectStatisticsPlugIn plugIn = MockRepository.GenerateStub<ICCNetProjectStatisticsPlugIn>();
            plugIn.Stub(action => action.FetchStatistics()).Return(data);

            // ignore failures only if you want to build build report statistic
            CCNetProjectStatisticsModule module = new CCNetProjectStatisticsModule(
                plugIn, graphs, fileManager, templateEngine, false);

            module.ProjectId = "CCNetStatistics";
            project.AddModule(module);

            module.ExecuteTask(null);
            module.FetchHtmlReport();
        }

        [Test, Explicit("This test should not be run automatically"),]
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

        private ProjectStatsData GetStatisticDataFromFile()
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
