using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;
using ProjectPilot.Extras.LogParser;

namespace ProjectPilot.Log4NetBrowser.Models
{
    public class LogDisplay
    {
        public LogCollection LineParse
        {
            get { return lineParse; }
        }

        public List<int> IndexList
        {
            get { return indexList; }
            set { indexList = value; }
        }

        public void Parsing10MBLogFile(LogParserFilter filter, string file, string logPattern, string separator)
        {
            if (string.IsNullOrEmpty(file))

                //file = @"\pilotProject\Data\Samples\Log4Net_sample.log";
                //file = @"..\..\..\Data\Samples\Log4Net_sample.log";
                file = @"C:\SSM+2009-01-08.log.28.small";
                //file = @"http://code.google.com/p/projectpilot/source/browse/trunk/Data/Samples/TestLogParser.log";

           // NetworkStream fileOnNetwork;
                         
            if (string.IsNullOrEmpty(logPattern))
                logPattern = "Time|Level|Ndc";

            if (string.IsNullOrEmpty(separator))
                separator = "|";
            
            using (Stream fileStream = File.OpenRead(file))
            {
                lineParse = new LogCollection(char.Parse(separator), logPattern);
                
                if(filter != null)
                    lineParse.ParseFilter = filter;
                
                lineParse.ParseLogFile(fileStream);
            }
        }

        private LogCollection lineParse;
        private List<int> indexList = new List<int>();
    }
}
