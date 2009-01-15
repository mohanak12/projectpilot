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

        public void Parsing10MBLogFile()
        {
            //using (Stream fileStream = File.OpenRead(@"C:\share\Marko\SSM+2009-01-08.log.28"))
            using (Stream fileStream = File.OpenRead(@"SSM+2009-01-08.log.28"))
            {
                lineParse = new LogCollection('|', "Time|Level|Ndc");

                /*LogParserFilter filter = new LogParserFilter();
                filter.FilterLevel = "WARN";
                lineParse.ParseFilter = filter;*/
                lineParse.ParseLogFile(fileStream);
            }
        }

        private LogCollection lineParse;
        private List<int> indexList = new List<int>();
    }
}
