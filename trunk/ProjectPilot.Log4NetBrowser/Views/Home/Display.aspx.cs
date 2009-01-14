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
            this.levelToColor = new Dictionary<string, string>
                                    {
                                        {"TRACE","black"}, 
                                        {"DEBUG","gary"}, 
                                        {"INFO", "green"}, 
                                        {"WARN", "orange"}, 
                                        {"ERROR","red"},
                                        {"FATAL","purple"}
                                    };
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

        public string LogEntryToString(LogEntry logEntry, int index, int? expandIndex)
        {
            string lineOutput = String.Empty;

            for(int i = 0; i < logEntry.Elements.Count(); i++)
            {
                lineOutput += "<td>";
                if(i==0)
                {
                    lineOutput += "<a name=\"";
                    lineOutput += index.ToString();
                    lineOutput += "\"></a>";
                }
                lineOutput += "<a href=\"/Home/Display/";
                lineOutput += index.ToString();
                lineOutput += "#";
                lineOutput += index.ToString();
                lineOutput += "\"><font color=\"";
                lineOutput += levelToColor[((ParsedElementBase) logEntry.Elements[levelIndex]).Element.ToString()];
                lineOutput += "\">";

                if ((((ParsedElementBase)logEntry.Elements[i]).Element.ToString().Length > (TableWidths[i] / pixelVsChar)) &&
                    index != expandIndex)
                {
                    lineOutput += ((ParsedElementBase)logEntry.Elements[i]).Element.ToString().Substring(0, (TableWidths[i] / pixelVsChar));
                    lineOutput += "...";
                }
                else
                {
                    lineOutput += ((ParsedElementBase)logEntry.Elements[i]).Element.ToString();
                }
                lineOutput += "</font></a></td>";
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
                        levelIndex = i;
                        break;
                    case "Message":
                        tableWidths.Add(100);
                        break;
                    case "Ndc":
                        tableWidths.Add(750);
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
        private int levelIndex;
    }
}
