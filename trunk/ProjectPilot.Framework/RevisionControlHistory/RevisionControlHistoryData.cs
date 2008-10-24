using System;
using System.Collections.Generic;

namespace ProjectPilot.Framework.RevisionControlHistory
{
    public class RevisionControlHistoryData
    {
        public DateTime MaxTime
        {
            get
            {
                if (entries.Count == 0)
                    return DateTime.MinValue;

                return entries.Keys[entries.Count-1];
            }
        }

        public DateTime MinTime
        {
            get
            {
                if (entries.Count == 0)
                    return DateTime.MaxValue;

                return entries.Keys[0];
            }
        }

        public IList<RevisionControlHistoryEntry> Entries
        {
            get { return entries.Values; }
        }

        public void AddEntry(RevisionControlHistoryEntry entry)
        {
            entries.Add(entry.Time, entry);
        }

        private readonly SortedList<DateTime, RevisionControlHistoryEntry> entries = new SortedList<DateTime, RevisionControlHistoryEntry>();
    }
}