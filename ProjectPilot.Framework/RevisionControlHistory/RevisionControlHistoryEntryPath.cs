using ProjectPilot.Framework.RevisionControlHistory;

namespace ProjectPilot.Framework.RevisionControlHistory
{
    public class RevisionControlHistoryEntryPath
    {
        public RevisionControlHistoryEntryAction Action
        {
            get { return action; }
            set { action = value; }
        }

        public string Path
        {
            get { return path; }
            set { path = value; }
        }

        private RevisionControlHistoryEntryAction action; 
        private string path;
    }
}