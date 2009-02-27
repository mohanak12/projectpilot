using System;
using Stump.Services;

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

        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        private string fileName;
        private bool isActive = true;
    }
}