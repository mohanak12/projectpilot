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
    public class CCNetTests
    {
        [Test]
        public void DrawBuildReportChartTest()
        {
            CCNetProjectStatisticsData data;
            using (Stream stream = File.OpenRead(@"..\..\..\Data\Samples\ccnet.stats.xml"))
            {
                data = CCNetProjectStatisticsPlugIn.Load(stream);
            }

            //Project name and version
            List<string> xLabels = new List<string>();

            //Locate data on X-Axis
            List<int> xScale = new List<int> { 0 };

            //Values on Y-Axis
            List<double> ySuccess = new List<double>();
            List<double> yFailure = new List<double>();
            List<double> yException = new List<double>();

            //Fill X and Y Axis with data
            foreach (CCNetProjectStatisticsBuildEntry build in data.Builds)
            {
                CCNetProjectStatisticsBuildEntry tempBuild = build;

                if (build.BuildLabel != xLabels.Find(
                                             temp => temp == tempBuild.BuildLabel))
                {
                    xLabels.Add(build.BuildLabel);
                    //Each build name must increase scale on X-Axis
                    xScale.Add(xScale[xScale.Count - 1] + 1);
                    ySuccess.Add(0);
                    yFailure.Add(0);
                    yException.Add(0);
                }

                //Add build statistics
                switch (build.BuildStatus)
                {
                    case "Success":

                        ySuccess[ySuccess.Count - 1] = ySuccess[ySuccess.Count - 1] + 1;

                        break;

                    case "Failure":

                        yFailure[yFailure.Count - 1] = yFailure[yFailure.Count - 1] + 1;

                        break;

                    case "Exception":

                        yException[yException.Count - 1] = yException[yException.Count - 1] + 1;

                        break;

                    default:
                        continue;
                }
            }

            //Draw chart
            FluentChart chart = FluentChart.Create("Build Report", null, null)
                //Failed builds
                .AddLineSeries("Failed Builds", "Red")
                .AddData(AddValuesToSortedList(xScale, yFailure))
                .SetLabelsToXAxis(xLabels)
                .SetSymbol(SymbolType.Circle, "Red", 4, true)
                //Exceptions in build
                .AddLineSeries("Exceptions", "Blue")
                .AddData(AddValuesToSortedList(xScale, yException))
                .SetSymbol(SymbolType.Circle, "Blue", 4, true)
                //Successful builds
                .AddLineSeries("Successful Builds", "Green")
                .AddData(AddValuesToSortedList(xScale, ySuccess))
                .SetSymbol(SymbolType.Circle, "Green", 4, true);

            chart
                .ExportToBitmap("test.png", ImageFormat.Png, 2000, 800);
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

        [Test,Ignore]
        public void GraphsTest()
        {
            //CCNetProjectStatisticsPlugIn plugIn = new CCNetProjectStatisticsPlugIn();

            List<CCNetProjectStatisticsGraph> graphs = new List<CCNetProjectStatisticsGraph>();

            CCNetProjectStatisticsGraph graph = new CCNetProjectStatisticsGraph();
            graph.AddParameter<TimeSpan>("Build Report");

            graphs.Add(graph);

            ProjectPilotConfiguration projectPilotConfiguration = new ProjectPilotConfiguration();
            projectPilotConfiguration.ProjectPilotWebAppRootUrl = "http://localhost/projectpilot/";

            ProjectRegistry projectRegistry = new ProjectRegistry();
            Project project = new Project("bhwr", "");
            projectRegistry.AddProject(project);

            IFileManager fileManager = new DefaultFileManager("", projectPilotConfiguration);
            projectRegistry.FileManager = fileManager;

            IFileManager templateFileManager = MockRepository.GenerateStub<IFileManager>();
            templateFileManager.Stub(action => action.GetFullFileName(null, null)).IgnoreArguments().Return(@"..\..\..\Data\Templates\CCNetReportStatistics.vm");

            ITemplateEngine templateEngine = new DefaultTemplateEngine(templateFileManager);

            //Prepare test data
            CCNetProjectStatisticsData data;
            using (Stream stream = File.OpenRead(@"..\..\..\Data\Samples\ccnet.stats.xml"))
            {
                data = CCNetProjectStatisticsPlugIn.Load(stream);
            }

            ICCNetProjectStatisticsPlugIn plugIn = MockRepository.GenerateStub<ICCNetProjectStatisticsPlugIn>();
            plugIn.Stub(action => action.FetchStatistics()).Return(data);

            CCNetProjectStatisticsModule module = new CCNetProjectStatisticsModule(plugIn, graphs, fileManager, templateEngine);
            module.ProjectId = "bhwr";
            project.AddModule(module);

            module.ExecuteTask(null);
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
