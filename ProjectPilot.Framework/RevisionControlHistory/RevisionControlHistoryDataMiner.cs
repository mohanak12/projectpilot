using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectPilot.Framework.RevisionControlHistory
{
    public class RevisionControlHistoryDataMiner
    {
        public static IDictionary<string, SortedList<DateTime, double>> FetchCommitsPerAuthorPerDay(RevisionControlHistoryData data)
        {
            Dictionary<string, SortedList<DateTime, double>> commitsPerDay = new Dictionary<string, SortedList<DateTime, double>>();

            foreach (RevisionControlHistoryEntry entry in data.Entries)
            {
                if (false == commitsPerDay.ContainsKey(entry.Author))
                    commitsPerDay.Add(entry.Author, new SortedList<DateTime, double>());

                DateTime date = entry.Time.Date;

                if (false == commitsPerDay[entry.Author].ContainsKey(date))
                    commitsPerDay[entry.Author].Add(date, 0);

                commitsPerDay[entry.Author][date] = commitsPerDay[entry.Author][date] + 1;
            }

            return commitsPerDay;
        }

        public static IDictionary<RevisionControlHistoryEntryAction, SortedList<DateTime, double>> 
            FetchCommittedFilesPerActionTypePerDay(RevisionControlHistoryData data)
        {
            Dictionary<RevisionControlHistoryEntryAction, SortedList<DateTime, double>> commitsPerDay 
                = new Dictionary<RevisionControlHistoryEntryAction, SortedList<DateTime, double>>();

            foreach (RevisionControlHistoryEntry entry in data.Entries)
            {
                foreach (RevisionControlHistoryEntryPath path in entry.Paths)
                {
                    if (false == commitsPerDay.ContainsKey(path.Action))
                        commitsPerDay.Add(path.Action, new SortedList<DateTime, double>());

                    DateTime date = entry.Time.Date;

                    if (false == commitsPerDay[path.Action].ContainsKey(date))
                        commitsPerDay[path.Action].Add(date, 0);

                    commitsPerDay[path.Action][date] = commitsPerDay[path.Action][date] + 1;
                }
            }

            return commitsPerDay;
        }
    }
}
