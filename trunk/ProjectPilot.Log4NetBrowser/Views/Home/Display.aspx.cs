using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectPilot.Extras.LogParser;
using ProjectPilot.Log4NetBrowser.Models;


namespace ProjectPilot.Log4NetBrowser.Views.Home
{
    public partial class Display : ViewPage
    {
        public LogDisplay ParserContent
        {
            get { return parserContent; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            parserContent.Parsing10MBLogFile();
        }

        public string LogEntryToString(LogEntry logEntry, List<string> pattern)
        {
            string lineOutput = String.Empty;
            
            for(int i = 0;i<logEntry.Elements.Count()-1;i++)
            {
                lineOutput += ((ParsedElementBase) logEntry.Elements[i]).Element.ToString() + " ";
            }
            return lineOutput;
        }

        private LogDisplay parserContent = new LogDisplay();
    }
}
