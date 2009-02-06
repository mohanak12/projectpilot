using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using MbUnit.Framework;
using ProjectPilot.Common.Charts;
using ProjectPilot.Framework.RevisionControlHistory;
using ProjectPilot.Framework.Subversion;
using ZedGraph;

namespace ProjectPilot.Tests.Framework.Subversion
{
    [TestFixture]
    public class HistoryTests
    {
        [Test]
        public void LoadHistory()
        {
            RevisionControlHistoryData data = LoadHistoryFromFile();

            Assert.AreEqual(834, data.Entries.Count);
        }

        [Test]
        public void DrawCommitsPerDayPerAuthorChart()
        {
            RevisionControlHistoryData data = LoadHistoryFromFile();

            FluentChart chart = FluentChart.Create("Commits History", null, "commits per day")
                .SetBarSettings(BarType.Stack, 0)
                .UseDateAsAxisY(data.MinTime.Date, data.MaxTime.Date);

            string[] colors = { "blue", "red", "green", "yellow", "orange", "brown" };

            IDictionary<string, SortedList<DateTime, double>> commitsPerAuthorPerDay 
                = RevisionControlHistoryDataMiner.FetchCommitsPerAuthorPerDay(data);

            int i = 0;
            foreach (string author in commitsPerAuthorPerDay.Keys)
            {
                chart
                    .AddBarSeries(author, colors[i++])
                    .AddDataByDate(commitsPerAuthorPerDay[author], data.MinTime.Date, data.MaxTime.Date);
            }

            chart
                .ExportToBitmap("test.png", ImageFormat.Png, 2000, 800);
        }

        [Test]
        public void DrawCommittedFilesPerDayPerActionChart()
        {
            RevisionControlHistoryData data = LoadHistoryFromFile();

            FluentChart chart = FluentChart.Create("Committed Files History", null, "commited files per day")
                .SetBarSettings(BarType.Stack, 0)
                .UseDateAsAxisY(data.MinTime.Date, data.MaxTime.Date);

            string[] colors = { "blue", "red", "green", "yellow", "orange", "brown" };

            IDictionary<RevisionControlHistoryEntryAction, SortedList<DateTime, double>> reportData
                = RevisionControlHistoryDataMiner.FetchCommittedFilesPerActionTypePerDay(data);

            int i = 0;
            foreach (RevisionControlHistoryEntryAction action in reportData.Keys)
            {
                chart
                    .AddBarSeries(action.ToString(), colors[i++])
                    .AddDataByDate(reportData[action], data.MinTime.Date, data.MaxTime.Date);
            }

            chart
                .ExportToBitmap("test.png", ImageFormat.Png, 2000, 800);
        }
    
        private RevisionControlHistoryData LoadHistoryFromFile()
        {
            RevisionControlHistoryData data;
            using (Stream stream = File.OpenRead(@"..\..\..\Data\Samples\svn-log.xml"))
            {
                data = SubversionHistoryFacility.LoadHistory(stream);
            }

            return data;
        }
    }
}