using System;

namespace Stump.Models
{
    [Serializable]
    public class MonitoredLogFile
    {
        public MonitoredLogFile(string fileName)
        {
            this.fileName = fileName;
        }

        public string FileName
        {
            get { return fileName; }
        }

        private string fileName;
    }
}