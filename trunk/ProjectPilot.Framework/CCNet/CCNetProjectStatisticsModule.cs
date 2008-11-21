using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Globalization;
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
            IEnumerable<ProjectStatsGraph> graphs,
            IFileManager fileManager,
            ITemplateEngine templateEngine)
        {
            this.ccnetPlugIn = ccnetPlugIn;
            this.graphs = (List<ProjectStatsGraph>) graphs;
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
            // Get statistic data
            ProjectStatsData data = ccnetPlugIn.FetchStatistics();
            
            IList<string> chartImageFileNames = new List<string>();

            // Build statistic for graphs
            foreach (ProjectStatsGraph graph in graphs)
            {
                chartImageFileNames.Add(ExtractChartData(data, graph));
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

        private static void AddDataToParamterList(ProjectStatsBuildEntry entry,
            IEnumerable<ProjectStatsGraphParameter> graphParameter, 
            bool newBuildLabel)
        {
            foreach (ProjectStatsGraphParameter parameter in graphParameter)
            {
                if (newBuildLabel)
                {
                    parameter.ValueList.Add(Convert.ToDouble(entry.Parameters[parameter.ParameterName],
                        CultureInfo.InvariantCulture));
                }
                else
                {
                    parameter.ValueList[parameter.ValueList.Count - 1] +=
                        Convert.ToDouble(entry.Parameters[parameter.ParameterName], CultureInfo.InvariantCulture);
                }
            }
        }

        private static SortedList<int, double> AddValuesToSortedList(IList<int> xValues, IList<double> yValues)
        {
            SortedList<int, double> sortedList = new SortedList<int, double>();

            for (int i = 0; i < yValues.Count; i++)
            {
                sortedList.Add(xValues[i], yValues[i]);
            }

            return sortedList;
        }

        private string DrawChart(ProjectStatsGraph graph,
            IList<string> xLabels,
            IList<int> xScaleValues)
        {
            // Draw chart
            FluentChart chart = FluentChart.Create(graph.GraphName, null, null)
                .SetLabelsToXAxis(xLabels);

            foreach (ProjectStatsGraphParameter parameter in graph.GraphParameters)
            {
                chart
                    .AddLineSeries(parameter.ParameterName, parameter.SeriesColor)
                    .AddData(AddValuesToSortedList(xScaleValues, parameter.ValueList))
                    .SetSymbol(SymbolType.Circle, parameter.SeriesColor, 4, true);
            }

            string chartImageFileName = fileManager.GetProjectFullFileName(
                projectId,
                ModuleId,
                "CCNetBuildReportChart.png",
                true);

            chart
                .ExportToBitmap(chartImageFileName, ImageFormat.Png, 2000, 800);

            return chartImageFileName;
        }

        private string ExtractChartData(ProjectStatsData data,
            ProjectStatsGraph graph)
        {
            // Project name
            List<string> xLabels = new List<string>();

            // Locate data on X-Axis
            List<int> xScale = new List<int>();

            // Fill X and Y Axis with data
            foreach (ProjectStatsBuildEntry build in data.Builds)
            {
                if (xLabels.Count == 50) break;

                ProjectStatsBuildEntry entry = build;

                bool newBuildLabel = false;

                // Add build name, increase scale on X-Axis
                // Graphs for build report
                // if the current build label has not already been added to the xLabels
                if (build.BuildLabel != xLabels.Find(
                                             temp => temp == entry.BuildLabel))
                {
                    // Build name
                    xLabels.Add(build.BuildLabel);

                    // X-Axis value for build
                    if (xScale.Count == 0)
                    {
                        xScale.Add(0);
                    }
                    else
                    {
                        xScale.Add(xScale[xScale.Count - 1] + 1);
                    }

                    newBuildLabel = true;
                }
                
                // Add data to graph parameters
                AddDataToParamterList(entry, graph.GraphParameters, newBuildLabel);
            }

            // Draw chart with filled parameters
            return DrawChart(graph, xLabels, xScale);
        }

        private readonly ICCNetProjectStatisticsPlugIn ccnetPlugIn;
        private readonly List<ProjectStatsGraph> graphs;
        private string projectId;
        private readonly IFileManager fileManager;
        private readonly ITemplateEngine templateEngine;
        private ITrigger trigger = new NullTrigger();
    }
}