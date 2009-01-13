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
        public Display()
        {
            /*this.levelToColor = new Dictionary<string, string>();
            this.levelToColor.Add("","");
            this.levelToColor.Add("", "");
            this.levelToColor.Add("", "");
            this.levelToColor.Add("", "");
            this.levelToColor.Add("", "");*/
        }

        public LogDisplay ParserContent
        {
            get { return parserContent; }
        }

        public List<int> TableWidths
        {
            get { return tableWidths; }
        }
        
        public Dictionary<string, string> LevelToColor
        {
            get { return levelToColor; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            parserContent.Parsing10MBLogFile();
        }

        public string LogEntryToString(LogEntry logEntry, List<string> pattern)
        {
            string lineOutput = String.Empty;
            
            for(int i = 0;i<logEntry.Elements.Count();i++)
            {
                lineOutput += "<td> <font color=\"red\">";

                if (((ParsedElementBase)logEntry.Elements[i]).Element.ToString().Length > (TableWidths[i] / pixelVsChar))
                {
                    lineOutput += ((ParsedElementBase)logEntry.Elements[i]).Element.ToString().Substring(0, (TableWidths[i] / pixelVsChar));
                    lineOutput += "...</font></td>";
                }
                else
                {
                    lineOutput += ((ParsedElementBase)logEntry.Elements[i]).Element.ToString() + "</td>";
                }
            }
            return lineOutput;
        }

        public void CalculateTableWidth(IList<string> pattern)
        {
            tableWidths = new List<int>();

            for(int i=0; i<pattern.Count();i++)
            {
                switch(pattern[i])
                {
                    case "Time":
                        tableWidths.Add(140);
                        break;
                    case "ThreadID":
                        tableWidths.Add(100);
                        break;
                    case "Level":
                        tableWidths.Add(60);
                        break;
                    case "Message":
                        tableWidths.Add(100);
                        break;
                    case "Ndc":
                        tableWidths.Add(600);
                        break;
                    default:
                        tableWidths.Add(100);
                        break;
                }
            }
        }

        private LogDisplay parserContent = new LogDisplay();
        private List<int> tableWidths;
        private const int pixelVsChar = 8;
        private Dictionary<string, string> levelToColor;
    }
}
