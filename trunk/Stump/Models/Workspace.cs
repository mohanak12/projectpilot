using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stump.Models
{
    [Serializable]
    public class Workspace
    {
        public void MonitorLogFile(MonitoredLogFile logFile)
        {
            logFiles.Add(logFile);
        }

        public IList<MonitoredLogFile> LogFiles
        {
            get { return logFiles; }
        }

        private List<MonitoredLogFile> logFiles = new List<MonitoredLogFile>();
    }
}
