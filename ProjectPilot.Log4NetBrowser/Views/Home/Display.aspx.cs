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
            set { parserContent = value; }
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
            //parserContent.Parsing10MBLogFile();
        }

        public string LogEntryToString(LogEntry logEntry, int index, List<int> expandIndex, int currentIndex)
        {
            string lineOutput = String.Empty;

            //for each element in table row
            for(int i = 0; i < logEntry.Elements.Count(); i++)
            {
                //table cell HTML tag
                lineOutput += "<td>";
                
                //On the first cell of the table row, we write an HTML anchor element.
                if(i==0)
                {
                    lineOutput += "<a name=\"";
                    lineOutput += (index + 1).ToString();
                    lineOutput += "\"></a>";
                }
                
                //Each element is a link to its expanded view.
                lineOutput += "<a href=\"/Home/Display/";
                
                //Workaround for opening and closing current element, otherwise the HTML page wont refresh.
                if (expandIndex.Contains(index))
                    lineOutput += "-";

                lineOutput += (index + 1).ToString();
                
                //Continue with the link - link to anchor of the selected row.
                lineOutput += "#";
                lineOutput += (index + 1).ToString();
                lineOutput += "\">";

                //Color of the row according to LEVEL settings.
                if (levelIndex != -1 &&
                    levelToColor.ContainsKey(logEntry.Elements[levelIndex].ToString()))
                {
                    lineOutput += "<font color=\"";
                    lineOutput += levelToColor[logEntry.Elements[levelIndex].ToString()];
                    lineOutput += "\">";
                }

                /*Longer elements of the line will be split. "..." is added at the end,
                 *unless the line is in the expandList - the list of elements we want to se complete.*/
                if ((logEntry.Elements[i]).ToString().Length > (TableWidths[i] / pixelVsChar))
                {
                    if (!expandIndex.Contains(index))
                    {
                        lineOutput += logEntry.Elements[i].ToString().Substring(0, (TableWidths[i] / pixelVsChar));
                        lineOutput += "...";
                    }
                    else
                    {
                        List<string> tmpString = SplitString(logEntry.Elements[i].ToString(), i);
                        for (int j = 0; j < tmpString.Count; j++)
                        {
                            lineOutput += tmpString[j] + "<br />";
                        }
                    }
                }
                else
                {
                    lineOutput += logEntry.Elements[i].ToString();
                }

                //End of the link and cell HTML elements
                if (levelIndex != -1 &&
                    levelToColor.ContainsKey(logEntry.Elements[levelIndex].ToString()))
                    lineOutput += "</font>";
                lineOutput += "</a></td>";
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
                        tableWidths.Add(200);
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

        private List<string> SplitString(string inputString, int idx)
        {
            List<string> stringList = new List<string>();
            int wide = TableWidths[idx]/pixelVsChar;
            if (inputString.Length > wide)
            {
                int i;
                for (i = 0; (i+wide) < inputString.Length; i += wide)
                {
                    stringList.Add(inputString.Substring(i,wide));
                }
                stringList.Add(inputString.Substring(i));
            }
            return stringList;
        }

        private LogDisplay parserContent;
        private List<int> tableWidths;
        private const int pixelVsChar = 8;
        private Dictionary<string, string> levelToColor;
        private int levelIndex=-1;
    }
}
