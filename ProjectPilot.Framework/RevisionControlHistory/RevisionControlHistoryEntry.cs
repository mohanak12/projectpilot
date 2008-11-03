using System;
using System.Collections.Generic;

namespace ProjectPilot.Framework.RevisionControlHistory
{
    public class RevisionControlHistoryEntry
    {
        public string Author
        {
            get { return author; }
            set { author = value; }
        }

        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        public ICollection<RevisionControlHistoryEntryPath> Paths
        {
            get { return paths; }
        }

        public string Revision
        {
            get { return revision; }
            set { revision = value; }
        }

        public DateTime Time
        {
            get { return time; }
            set { time = value; }
        }

        public void SetPaths(RevisionControlHistoryEntryPath[] pathsCollected)
        {
            paths = pathsCollected;
        }

        private string author;
        private string message;
        private RevisionControlHistoryEntryPath[] paths;
        private string revision;
        private DateTime time;
    }
}