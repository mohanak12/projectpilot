using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public void Parsing10MBLogFile(LogParserFilter filter, string file)
        {
            //if (string.IsNullOrEmpty(file))
            //    //file = @"..\..\..\Data\Samples\Log4Net_sample.log";
            //    file = @"\\zarja\share\Marko\SSM+2009-01-08.log.28.small";
                        
            using (Stream fileStream = File.OpenRead(file))
            {
                lineParse = new LogCollection('|', "Time|Level|Ndc");
                
                if(filter != null)
                    lineParse.ParseFilter = filter;
                
                lineParse.ParseLogFile(fileStream);
            }
        }

        private LogCollection lineParse;
        private List<int> indexList = new List<int>();
    }
}
