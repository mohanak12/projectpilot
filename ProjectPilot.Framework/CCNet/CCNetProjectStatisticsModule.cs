using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Threading;
using ProjectPilot.Framework.Charts;
using ProjectPilot.Framework.Modules;
using ZedGraph;

namespace ProjectPilot.Framework.CCNet
{
    public class CCNetProjectStatisticsModule : IProjectModule, IViewable, ITask
    {
        public CCNetProjectStatisticsModule(
            ICCNetProjectStatisticsPlugIn ccnetPlugIn,
            IEnumerable<CCNetProjectStatisticsGraph> graphs,
            IFileManager fileManager,
            ITemplateEngine templateEngine)
        {
            this.ccnetPlugIn = ccnetPlugIn;
            this.graphs = (List<CCNetProjectStatisticsGraph>) graphs;
            this.fileManager = fileManager;
            this.templateEngine = templateEngine;
        }

        public string ModuleId
        {
            get { return "CCNetProjectStatistics";  }
        }

        public string ModuleName
        {
            get { return "CCNet Project Statistics"; }
        }

        public string ProjectId
        {
            get { return projectId; }
            set { projectId = value; }
        }

        public ITrigger Trigger
        {
            get { return trigger; }
            set { trigger = value; }
        }

        public string FetchHtmlReport()
        {
            return fileManager.FetchProjectFile(projectId, ModuleId, "CCNetReportStatistics.html");
        }

        public void ExecuteTask(WaitHandle stopSignal)
        {
            //Get statistic data
            CCNetProjectStatisticsData data = ccnetPlugIn.FetchStatistics();
            
            List<string> chartImageFileNames = new List<string>();

            //Build statistic
            foreach (CCNetProjectStatisticsGraph graph in graphs)
            {
                if (graph == null)
                    continue;

                foreach (CCNetProjectStatisticsGraphParameter parameter in graph.GraphParameters)
                {
                    switch (parameter.ParameterName)
                    {
                        case "Build Report":

                            chartImageFileNames.Add(DrawBuildReportChart(data));

                            break;

                        case "Compiler":

                            break;

                        case "Test Coverage":

                            break;

                        case "Tests":

                            break;

                        case "Build Duration":

                            break;

                        case "Build Duration Report":

                            break;

                        default:
                            continue;
                    }
                }
            }

            // translate storage locations to URLs
            for (int i = 0; i < chartImageFileNames.Count; i++)
            {
                string chartImageFileName = chartImageFileNames[i];
                Uri url = fileManager.TranslateToUrl(chartImageFileName);
                chartImageFileNames[i] = url.ToString();
            }

            // generate wrapper HTML document
            // and save it to the project's storage location
            Hashtable templateContext = new Hashtable();
            templateContext.Add("project", projectId);
            templateContext.Add("reportImages", chartImageFileNames);
            templateEngine.ApplyTemplate(
                "CCNetReportStatistics.vm",
                templateContext,
                fileManager.GetProjectFullFileName(projectId, ModuleId, "CCNetReportStatistics.html", true));
        }

        private static SortedList<int, double> AddValuesToSortedList(List<int> xValues, List<double> yValues)
        {
            SortedList<int, double> sortedList = new SortedList<int, double>();

            for (int i = 0; i < yValues.Count; i++)
            {
                sortedList.Add(xValues[i], yValues[i]);
            }

            return sortedList;
        }

        private string DrawBuildReportChart(CCNetProjectStatisticsData data)
        {
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
                //TODO: remove this
                //Minimize to 50 records
                if (xLabels.Count == 50)
                {
                    break;
                }

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

                //Count build statistics
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

            string chartImageFileName = fileManager.GetProjectFullFileName(
                projectId,
                ModuleId,
                "CCNetBuildReportChart.png",
                true);

            chart
                .ExportToBitmap(chartImageFileName, ImageFormat.Png, 2000, 800);

            return chartImageFileName;
        }
        
        private ICCNetProjectStatisticsPlugIn ccnetPlugIn;
        private List<CCNetProjectStatisticsGraph> graphs = new List<CCNetProjectStatisticsGraph>();
        private string projectId;
        private readonly IFileManager fileManager;
        private readonly ITemplateEngine templateEngine;
        private ITrigger trigger = new NullTrigger();
    }
}