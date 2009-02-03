using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectPilot.Extras.LogParser;
using ProjectPilot.Log4NetBrowser.Models;


namespace ProjectPilot.Log4NetBrowser.Views.LogView
{
    public partial class DisplayLog : ViewPage
    {
        public LogDisplay ParserContent
        {
            get { return parserContent; }
            set { parserContent = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //parserContent.Parsing10MBLogFile();
        }

        public string LogEntryToString(LogEntry logEntry, int index)
        {
            string lineOutput = String.Empty;

            if (index % 2 == 0)
                lineOutput += "<tr class=\"even ";
            else
                lineOutput += "<tr class=\"odd ";

            if (levelIndex != -1)
            {
                lineOutput += logEntry.Elements[levelIndex].ToString().ToLower();
            }

            /*lineOutput += "\" id=\"";
            lineOutput += index.ToString();*/
            lineOutput += "\" idShow=\"" + index.ToString() + "\">";

            //for each element in table row
            for(int i = 0; i < logEntry.Elements.Count(); i++)
            {
                //table cell HTML tag
                lineOutput += "<td class=\"";
                lineOutput += parserContent.LineParse.ElementsPattern[i].ToString().ToLower();

                if (parserContent.LineParse.ElementsPattern[i].ToString().ToLower() == "message" ||
                    parserContent.LineParse.ElementsPattern[i].ToString().ToLower() == "ndc")
                {
                    lineOutput += "\" id=\"";
                    lineOutput += index.ToString();
                }
                
                lineOutput += "\">";
                lineOutput += "<a href=\"#\">";
                
                lineOutput += logEntry.Elements[i].ToString();

    
                lineOutput += "</a></td>";
            }
            lineOutput += "</tr>\n";
            return lineOutput;
        }

        public void FindLevelIndex(IList<string> pattern)
        {
            for(int i=0; i<pattern.Count();i++)
            {
                switch(pattern[i])
                {
                    case "Time":
                        break;
                    case "ThreadID":
                        break;
                    case "Level":
                        this.levelIndex = i;
                        break;
                    case "Message":
                        break;
                    case "Ndc":
                        break;
                    default:
                        break;
                }
            }
        }

        private LogDisplay parserContent;
        private int levelIndex=-1;
    }
}
