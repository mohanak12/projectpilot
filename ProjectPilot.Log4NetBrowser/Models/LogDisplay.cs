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

        public void Parsing10MBLogFile(LogParserFilter filter, string file, string pattern, char? separator)
        {
            //Default settings  TEMP!!
            if (string.IsNullOrEmpty(file))
                file = @"\\zarja\share\Marko\SSM+2009-01-08.log.28.small";

            if (string.IsNullOrEmpty(pattern))
                pattern = "Time|Level|Message";

            //if (separator != null)  !!!
                char separator1 = '|';

            this.separator = separator1;  //!!!
            this.pattern = pattern;
            this.file = file;

            using (Stream fileStream = File.OpenRead(file))
            {
                lineParse = new LogCollection(separator1, pattern);
                
                if(filter != null)
                    lineParse.ParseFilter = filter;
                
                lineParse.ParseLogFile(fileStream);
            }
        }

        public void ParseLogFile(LogParserFilter filter)
        {

            using (Stream fileStream = File.OpenRead(file))
            {
                lineParse = new LogCollection(separator, pattern);

                if (filter != null)
                    lineParse.ParseFilter = filter;

                lineParse.ParseLogFile(fileStream);
            }
        }

        private string file;
        private string pattern;
        private char separator;
        private LogCollection lineParse;
        private List<int> indexList = new List<int>();
    }
}
