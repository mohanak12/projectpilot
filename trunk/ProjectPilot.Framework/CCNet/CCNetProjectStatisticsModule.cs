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
            
            IList<string> chartImageFileNames = new List<string>();

            //Build statistic for graphs
            foreach (CCNetProjectStatisticsGraph graph in graphs)
            {
                //Tukaj se izbere stevilo grafov
                PrepareChartData(graph.GraphParameters, data, chartImageFileNames);
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

        private static SortedList<int, double> AddValuesToSortedList(IList<int> xValues, IList<double> yValues)
        {
            SortedList<int, double> sortedList = new SortedList<int, double>();

            for (int i = 0; i < yValues.Count; i++)
            {
                sortedList.Add(xValues[i], yValues[i]);
            }

            return sortedList;
        }

        private void PrepareChartData(IEnumerable<CCNetProjectStatisticsGraphParameter> graphParameters,
            CCNetProjectStatisticsData data,
             IList<string> chartImageFileNames)
        {
            
            foreach (CCNetProjectStatisticsGraphParameter graphParameter in graphParameters)
            {
                chartImageFileNames.Add(ExtractChartData(data, graphParameter));
            }
        }

        private string ExtractChartData(CCNetProjectStatisticsData data, 
            CCNetProjectStatisticsGraphParameter graphParameter)
        {
            //Project name
            List<string> xLabels = new List<string>();

            //Locate data on X-Axis
            List<int> xScale = new List<int>();

            //Fill X and Y Axis with data
            foreach (CCNetProjectStatisticsBuildEntry build in data.Builds)
            {
                if (xLabels.Count == 10) break;

                CCNetProjectStatisticsBuildEntry entry = build;

                bool newParameter = false;

                //Add build name, increase scale on X-Axis
                if (build.BuildLabel != xLabels.Find(
                                             temp => temp == entry.BuildLabel))
                {
                    //Build name
                    xLabels.Add(build.BuildLabel);

                    //X-Axis value for build
                    if (xScale.Count == 0)
                    {
                        xScale.Add(0);   
                    }
                    else
                    {
                        xScale.Add(xScale[xScale.Count - 1] + 1);    
                    }

                    newParameter = true;
                }

                //Add data to graph parameters
                AddExtractChartData(entry, graphParameter.ParametersList, newParameter);
            }

            //Draw chart
            return DrawChart(graphParameter.ParametersList, graphParameter.GraphName, xLabels, xScale);
        }

        private static void AddExtractChartData(CCNetProjectStatisticsBuildEntry entry, 
            IEnumerable<KeyValuePair<string, List<double>>> parameters, 
            bool newParameter)
        {
            foreach (KeyValuePair<string, List<double>> parameter in parameters)
            {
                if (newParameter)
                {
                    parameter.Value.Add(0);
                }

                parameter.Value[parameter.Value.Count - 1] += Convert.ToDouble(entry.Parameters[parameter.Key], CultureInfo.InvariantCulture);
            }
        }

        private string DrawChart(IEnumerable<KeyValuePair<string, List<double>>> parameters, 
            string graphName,
            IList<string> xLabels,
            IList<int> xScaleValues)
        {
            string[] colors = new string[] { "Green", "Red", "Blue", "Yellow", "Black", "Grey" };

            //Draw chart
            FluentChart chart = FluentChart.Create(graphName, null, null)
                .SetLabelsToXAxis(xLabels);

            int i = 0;
            foreach (KeyValuePair<string, List<double>> parameter in parameters)
            {
                chart
                    .AddLineSeries(parameter.Key, colors[i])
                    .AddData(AddValuesToSortedList(xScaleValues, parameter.Value))
                    .SetSymbol(SymbolType.Circle, colors[i++], 4, true);
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
        
        private ICCNetProjectStatisticsPlugIn ccnetPlugIn;
        private List<CCNetProjectStatisticsGraph> graphs = new List<CCNetProjectStatisticsGraph>();
        private string projectId;
        private readonly IFileManager fileManager;
        private readonly ITemplateEngine templateEngine;
        private ITrigger trigger = new NullTrigger();
    }
}