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
            ITemplateEngine templateEngine,
            bool showBuildProjectHistory)
        {
            this.ccnetPlugIn = ccnetPlugIn;
            this.graphs = (List<ProjectStatsGraph>) graphs;
            this.fileManager = fileManager;
            this.templateEngine = templateEngine;
            this.showBuildProjectHistory = showBuildProjectHistory;
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
            // get statistic data
            ProjectStatsData data = ccnetPlugIn.FetchStatistics();
            // sort statistics data by buildId
            ProjectStatsGraphData graphData = new ProjectStatsGraphData(data);

            // labels on X-Axis
            List<string> xLabels = new List<string>();
            IList<string> chartImageFileNames = new List<string>();

            // prepare build statistic data and create graph
            foreach (ProjectStatsGraph graph in graphs)
            {
                // clear list of data
                graphData.ClearListOfData();
                // prepare list of data for graph
                PrepareDataMatrix(data, graph, graphData, xLabels);
                // draw chart
                chartImageFileNames.Add(DrawChart(graph, graphData, xLabels));
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

        private string DrawChart(ProjectStatsGraph graph, ProjectStatsGraphData graphData, IList<string> xLabels)
        {
            // Draw chart
            using (FluentChart chart = FluentChart.Create(graph.GraphName, graph.XAxisTitle, graph.YAxisTitle))
            {
                chart.SetLabelsToXAxis(xLabels);

                int xScaleMaxValue = 1;

                foreach (ProjectStatsGraphParameter parameter in graph.GraphParameters)
                {
                    SortedList<int, double> values = graphData.GetValuesForParameter(parameter.ParameterName);
                    xScaleMaxValue = values.Count;
                    chart
                        .AddLineSeries(parameter.ParameterName, parameter.SeriesColor)
                        .AddData(values)
                        .SetSymbol(SymbolType.Circle, parameter.SeriesColor, 4, true);
                }

                string chartImageFileName = fileManager.GetProjectFullFileName(
                    projectId,
                    ModuleId,
                    string.Format(CultureInfo.InvariantCulture, "CCNet{0}Chart.png", graph.GraphName.Replace(" ", string.Empty)),
                    true);
                
                chart
                    .SetXAxis(1, xScaleMaxValue)
                    .ExportToBitmap(chartImageFileName, ImageFormat.Png, 2000, 800);

                return chartImageFileName;
            }
        }

        /// <summary>
        /// Copy necessary build data to data matrix.
        /// </summary>
        /// <param name="data">The CCNet project statistics data.</param>
        /// <param name="graph">The CCNet project statistics graph.</param>
        /// <param name="graphData">The graph data.</param>
        /// <param name="xLabels">Labels that will be displayed on X-Axis</param>
        private void PrepareDataMatrix(
            ProjectStatsData data, 
            ProjectStatsGraph graph, 
            ProjectStatsGraphData graphData, 
            List<string> xLabels)
        {
            int buildId = 0;
            xLabels.Clear();

            // go through all builds
            for (int i = 0; i < data.Builds.Count; i++)
            {
                // show last 50 builds on graph if parameter is set to false
                if (!showBuildProjectHistory)
                {
                    if (i < data.Builds.Count - BuildNumbers)
                        continue;
                }

                ProjectStatsBuildEntry entry = data.Builds[i];

                // ignore unsuccessful builds
                if (graph.IgnoreFailures && entry.Parameters["Success"] == "0")
                {
                    continue;
                }

                // flag, that marks if parameter value will be added to the list or
                // value will increase existing value (depends on buildId)
                bool addValue = false;
                
                // if the current build label has not already been added to the xLabels
                if (entry.BuildLabel != xLabels.Find(temp => temp == entry.BuildLabel))
                {
                    // add build name to list. Build name will be shown on x-axis
                    xLabels.Add(entry.BuildLabel);

                    addValue = true;
                    buildId = entry.BuildId;
                }

                // go through all graph parameters
                foreach (ProjectStatsGraphParameter parameter in graph.GraphParameters)
                {
                    double value = 0;

                    // if parameter exists in build statistic then get parameter value
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
                        // set value
                        graphData.SetValue(buildId, parameter.ParameterName, value);
                    }
                    else
                    {
                        // increment value
                        graphData.IncValue(buildId, parameter.ParameterName, value);
                    }
                }
            }
        }

        private readonly ICCNetProjectStatisticsPlugIn ccnetPlugIn;
        private readonly List<ProjectStatsGraph> graphs;
        private string projectId;
        private const int BuildNumbers = 50;
        private readonly bool showBuildProjectHistory;
        private readonly IFileManager fileManager;
        private readonly ITemplateEngine templateEngine;
        private ITrigger trigger = new NullTrigger();
    }
}