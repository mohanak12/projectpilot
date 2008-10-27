using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using ProjectPilot.Framework.Charts;
using ProjectPilot.Framework.Modules;
using ProjectPilot.Framework.RevisionControlHistory;
using ZedGraph;

namespace ProjectPilot.Framework.Modules
{
    public class RevisionControlStatsModule : IProjectModule, IGenerator, IViewable
    {
        public RevisionControlStatsModule(
            string projectId, 
            IRevisionControlHistoryPlugIn rcHistoryPlugIn,
            IFileManager fileManager)
        {
            this.projectId = projectId;
            this.rcHistoryPlugIn = rcHistoryPlugIn;
            this.fileManager = fileManager;
        }

        public string ModuleId
        {
            get { return "RevisionControlStats"; }
        }

        public string ModuleName
        {
            get { return "Revision Control Stats"; }
        }

        public string ProjectId
        {
            get { return projectId; }
        }

        public string FetchHtmlReport()
        {
            throw new System.NotImplementedException();
        }

        public void Generate()
        {
            // retrieve the latest history from the revision control
            RevisionControlHistoryData history = rcHistoryPlugIn.FetchHistory();

            // generate charts and save them to the project's storage location
            DrawCommitsPerDayPerAuthorChart(history);

            // generate wrapper HTML document

            // save it to the project's storage location
        }

        public void DrawCommitsPerDayPerAuthorChart(RevisionControlHistoryData history)
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

            string chartImageFileName = fileManager.GetFullFileName(projectId, "CommitsPerDayPerAuthorChart.png");

            chart
                .ExportToBitmap(chartImageFileName, ImageFormat.Png, 2000, 800, 600);
        }

        private readonly string projectId;
        private IRevisionControlHistoryPlugIn rcHistoryPlugIn;
        private readonly IFileManager fileManager;
    }
}