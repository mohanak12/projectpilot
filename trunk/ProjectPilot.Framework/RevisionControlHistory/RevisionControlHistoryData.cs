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

        /// <summary>
        /// Returns a <see cref="String"/> that represents the current object.
        /// </summary>
        /// <returns>
        /// A <see cref="String"/> that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return String.Format(System.Globalization.CultureInfo.InvariantCulture,
                "Entries: {0}", entries.Count);
        }
                
        private readonly SortedList<DateTime, RevisionControlHistoryEntry> entries = new SortedList<DateTime, RevisionControlHistoryEntry>();
    }
}