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
        public void DrawBuildReportChartTest()
        {
            IDictionary<string, List<double>> parameters = new Dictionary<string, List<double>>();
            parameters.Add("Success", new List<double>());
            parameters.Add("Failure", new List<double>());
            parameters.Add("Exception", new List<double>());

            parameters["Success"].AddRange(new double[] {0, 0, 1, 1, 1, 0});
            parameters["Failure"].AddRange(new double[] {2, 3, 1, 1, 0, 1});
            parameters["Exception"].AddRange(new double[] {0, 0, 1, 1, 4, 2});

            //Locate data on X-Axis
            List<int> xScaleValues = new List<int> {0, 1, 2, 3, 4, 5};

            //Labels on X-Axis
            List<string> xLabels = new List<string> { "Test0", "Test1", "Test2", "Test3", "Test4", "Test5" };

            //Draw chart
            string[] colors = new string[] { "Green", "Red", "Blue", "Yellow", "Black", "Grey" };

            //Draw chart
            FluentChart chart = FluentChart.Create("Build Report", null, null)
                .SetLabelsToXAxis(xLabels);

            int i = 0;
            foreach (KeyValuePair<string, List<double>> parameter in parameters)
            {
                chart
                    .AddLineSeries(parameter.Key, colors[i])
                    .AddData(AddValuesToSortedList(xScaleValues, parameter.Value))
                    .SetSymbol(SymbolType.Circle, colors[i++], 4, true);
            }

            chart
                .ExportToBitmap("test.png", ImageFormat.Png, 2000, 800);
        }

        [Test,Ignore]
        public void GraphsTest()
        {
            List<ProjectStatsGraph> graphs = new List<ProjectStatsGraph>();

            //ProjectStatsGraph graph = new ProjectStatsGraph();
            //graph.GraphName = "FxCop Info";
            //graph.AddParameter<double>("Blue", "FxCop Warnings");
            //graph.AddParameter<double>("Red", "FxCop Errors");

            //graphs.Add(graph);

            ProjectStatsGraph graph = new ProjectStatsGraph();
            graph.GraphName = "Build report";
            graph.AddParameter<double>("Success", "Green");
            graph.AddParameter<double>("Failure", "Red");
            graph.AddParameter<double>("Exception", "Blue");

            graphs.Add(graph);

            //ProjectStatsGraph graph = new ProjectStatsGraph();
            //graph.GraphName = "Build duration";
            //graph.AddParameter<TimeSpan>("Green", "Duration");

            //graphs.Add(graph);

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

            //Prepare test data
            ProjectStatsData data;
            using (Stream stream = File.OpenRead(@"..\..\..\Data\Samples\ccnet.stats.xml"))
            {
                data = CCNetProjectStatisticsPlugIn.Load(stream);
            }

            ICCNetProjectStatisticsPlugIn plugIn = MockRepository.GenerateStub<ICCNetProjectStatisticsPlugIn>();
            plugIn.Stub(action => action.FetchStatistics()).Return(data);

            CCNetProjectStatisticsModule module = new CCNetProjectStatisticsModule(plugIn, graphs, fileManager, templateEngine);
            module.ProjectId = "CCNetStatistics";
            project.AddModule(module);

            module.ExecuteTask(null);
        }

        [Test]
        public void Test()
        {
            RemoteCruiseManagerFactory factory = new RemoteCruiseManagerFactory();
            Uri url = new Uri(string.Format(CultureInfo.InvariantCulture, "tcp://firefly:21234/CruiseManager.rem"));
            string projectName = "ProjectPilot";
            ICruiseManager mgr = factory.GetCruiseManager(url.ToString());

            string proj = mgr.GetProject(projectName);
            string stat = mgr.GetStatisticsDocument(projectName);
            //File.WriteAllText("ccnet.stats.xml", stat);
        }

        #region Private helpers

        private SortedList<int, double> AddValuesToSortedList(List<int> xValues, List<double> yValues)
        {
            SortedList<int, double> sortedList = new SortedList<int, double>();

            for (int i = 0; i < yValues.Count; i++)
            {
                sortedList.Add(xValues[i], yValues[i]);
            }

            return sortedList;
        }

        #endregion
    }
}
