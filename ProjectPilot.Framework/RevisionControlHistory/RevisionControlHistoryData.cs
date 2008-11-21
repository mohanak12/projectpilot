using System;
using System.Collections.Generic;

namespace ProjectPilot.Framework.RevisionControlHistory
{
    public class RevisionControlHistoryData
    {
        public IList<RevisionControlHistoryEntry> Entries
        {
            get { return entries.Values; }
        }

        /// <summary>
        /// Gets the last revision stored in history.
        /// </summary>
        /// <value>The last revision stored in history; <c>null</c> if no entries are in the history.</value>
        public string LastRevision
        {
            get
            {
                if (entries.Count > 0)
                    return entries.Values[entries.Count - 1].Revision;

                return null;
            }
        }

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

        /// <summary>
        /// Adds the specified <see cref="RevisionControlHistoryEntry"/> object to the history.
        /// </summary>
        /// <param name="entry">The entry to add.</param>
        public void AddEntry(RevisionControlHistoryEntry entry)
        {
            entries.Add(entry.Time, entry);
        }

        /// <summary>
        /// Tries to find the specified revision in the history.
        /// </summary>
        /// <param name="revision">The revision to find.</param>
        /// <returns>The <see cref="RevisionControlHistoryEntry"/> if find; otherwise <c>null</c>.</returns>
        public RevisionControlHistoryEntry FindRevision(string revision)
        {
            foreach (RevisionControlHistoryEntry entry in entries.Values)
            {
                if (entry.Revision == revision)
                    return entry;
            }

            return null;
        }

        /// <summary>
        /// Merges two <see cref="RevisionControlHistoryData"/> objects.
        /// </summary>
        /// <param name="historyToMergeWith">The history to merge with.</param>
        public void Merge(RevisionControlHistoryData historyToMergeWith)
        {
            foreach (RevisionControlHistoryEntry entry in historyToMergeWith.Entries)
            {
                // merge only if the revision does not already exist in this history
                if (null == FindRevision (entry.Revision))
                    this.AddEntry(entry);
            }
        }

        /// <summary>
        /// Returns a <see cref="String"/> that represents the current object.
        /// </summary>
        /// <returns>
        /// A <see cref="String"/> that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return String.Format(
                System.Globalization.CultureInfo.InvariantCulture,
                "Entries: {0}", 
                entries.Count);
        }
                
        private readonly SortedList<DateTime, RevisionControlHistoryEntry> entries = new SortedList<DateTime, RevisionControlHistoryEntry>();
    }
}