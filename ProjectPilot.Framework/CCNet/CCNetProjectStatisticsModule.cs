using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
            bool ignoreFailures,
            bool showBuildProjectHistory)
        {
            this.ccnetPlugIn = ccnetPlugIn;
            this.graphs = (List<ProjectStatsGraph>) graphs;
            this.fileManager = fileManager;
            this.templateEngine = templateEngine;
            this.ignoreFailures = ignoreFailures;
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

            IList<string> chartImageFileNames = new List<string>();

            // prepare build statistic data and create graph
            foreach (ProjectStatsGraph graph in graphs)
            {
                // clear list of data
                graphData.ClearDictionary();
                // prepare list of data for graph
                PrepareDataMatrix(data, graph, graphData);
                // draw chart
                chartImageFileNames.Add(DrawChart(graph, graphData));
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

        private string DrawChart(ProjectStatsGraph graph, ProjectStatsGraphData graphData)
        {
            // Draw chart
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

                string chartImageFileName = fileManager.GetProjectFullFileName(
                    projectId,
                    ModuleId,
                    "CCNetBuildReportChart.png",
                    true);

                chart
                    .ExportToBitmap(chartImageFileName, ImageFormat.Png, 2000, 800);

                return chartImageFileName;
            }
        }

        /// <summary>
        /// Copy necessary build data to data matrix
        /// </summary>
        private void PrepareDataMatrix(ProjectStatsData data, ProjectStatsGraph graph, ProjectStatsGraphData graphData)
        {
            int buildId = 0;

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
                            value = Convert.ToDouble(entry.Parameters[parameter.ParameterName],
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
            }
        }

        private List<string> xLabels = new List<string>();
        private readonly ICCNetProjectStatisticsPlugIn ccnetPlugIn;
        private readonly List<ProjectStatsGraph> graphs;
        private bool ignoreFailures;
        private string projectId;
        private const int buildNumbers = 10;
        private bool showBuildProjectHistory;
        private readonly IFileManager fileManager;
        private readonly ITemplateEngine templateEngine;
        private ITrigger trigger = new NullTrigger();
    }
}