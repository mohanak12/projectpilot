using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Imaging;
using ProjectPilot.Framework.Charts;
using ProjectPilot.Framework.RevisionControlHistory;
using ZedGraph;

namespace ProjectPilot.Framework.Modules
{
    public class RevisionControlStatsModule : IProjectModule, IGenerator, IViewable
    {
        public RevisionControlStatsModule(
            Project project, 
            IRevisionControlHistoryPlugIn rcHistoryPlugIn,
            IFileManager fileManager,
            ITemplateEngine templateEngine)
        {
            this.project = project;
            this.rcHistoryPlugIn = rcHistoryPlugIn;
            this.fileManager = fileManager;
            this.templateEngine = templateEngine;
        }

        public string ModuleId
        {
            get { return "RevisionControlStats"; }
        }

        public string ModuleName
        {
            get { return "Revision Control Stats"; }
        }

        public Project Project
        {
            get { return project; }
        }

        public string DrawCommitsPerDayPerAuthorChart(RevisionControlHistoryData history)
        {
            FluentChart chart = FluentChart.Create("Commits History", null, "commits per day")
                .SetBarSettings(BarType.Stack, 0)
                .UseDateAsAxisY(history.MinTime.Date, history.MaxTime.Date);

            string[] colors = { "blue", "red", "green", "yellow", "orange", "brown" };

            IDictionary<string, SortedList<DateTime, double>> commitsPerAuthorPerDay
                = RevisionControlHistoryDataMiner.FetchCommitsPerAuthorPerDay(history);

            int i = 0;
            foreach (string author in commitsPerAuthorPerDay.Keys)
            {
                chart
                    .AddBarSeries(author, colors[i++])
                    .AddDataByDate(commitsPerAuthorPerDay[author], history.MinTime.Date, history.MaxTime.Date);
            }

            string chartImageFileName = fileManager.GetProjectFullFileName(
                project.ProjectId, 
                ModuleId, 
                "CommitsPerDayPerAuthorChart.png", 
                true);

            chart
                .ExportToBitmap(chartImageFileName, ImageFormat.Png, 2000, 800, 600);

            return chartImageFileName;
        }

        public string DrawCommittedFilesPerDayPerActionChart(RevisionControlHistoryData history)
        {
            FluentChart chart = FluentChart.Create("Committed Files History", null, "commited files per day")
                .SetBarSettings(BarType.Stack, 0)
                .UseDateAsAxisY(history.MinTime.Date, history.MaxTime.Date);

            string[] colors = { "blue", "red", "green", "yellow", "orange", "brown" };

            IDictionary<RevisionControlHistoryEntryAction, SortedList<DateTime, double>> reportData
                = RevisionControlHistoryDataMiner.FetchCommittedFilesPerActionTypePerDay(history);

            int i = 0;
            foreach (RevisionControlHistoryEntryAction action in reportData.Keys)
            {
                chart
                    .AddBarSeries(action.ToString(), colors[i++])
                    .AddDataByDate(reportData[action], history.MinTime.Date, history.MaxTime.Date);
            }

            string chartImageFileName = fileManager.GetProjectFullFileName(
                project.ProjectId, 
                ModuleId, 
                "CommittedFilesPerDayPerActionChart.png", 
                true);

            chart
                .ExportToBitmap(chartImageFileName, ImageFormat.Png, 2000, 800, 600);

            return chartImageFileName;
        }

        public string FetchHtmlReport()
        {
            return fileManager.FetchProjectFile(project.ProjectId, ModuleId, "RevisionControlHistory.html");
        }

        public void Generate()
        {
            // retrieve the latest history from the revision control
            RevisionControlHistoryData history = rcHistoryPlugIn.FetchHistory();

            List<string> chartImageFileNames = new List<string>();
            
            // generate charts and save their storage locations
            chartImageFileNames.Add(DrawCommitsPerDayPerAuthorChart(history));
            chartImageFileNames.Add(DrawCommittedFilesPerDayPerActionChart(history));

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
            templateContext.Add("project", project);
            templateContext.Add("reportImages", chartImageFileNames);
            templateEngine.ApplyTemplate(
                "RevisionControlHistory.vm", 
                templateContext, 
                fileManager.GetProjectFullFileName(project.ProjectId, ModuleId, "RevisionControlHistory.html", true));
        }

        private readonly IFileManager fileManager;
        private readonly Project project;
        private IRevisionControlHistoryPlugIn rcHistoryPlugIn;
        private readonly ITemplateEngine templateEngine;
    }
}