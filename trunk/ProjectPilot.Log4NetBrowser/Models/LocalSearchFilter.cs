using System;
using System.Collections.Generic;
using ProjectPilot.Extras.LogParser;

namespace ProjectPilot.Log4NetBrowser.Models
{
    public class LocalSearchFilter
    {
        public static LogDisplay Filter(LogDisplay logObject, int? startIndex, int? endIndex, string searchType, string searchContent)
        {
            bool deleteFlag = true;
            List<int> indexToDelete = new List<int>();

            if (!string.IsNullOrEmpty(searchContent))
            {
                for(int i = 0; i < logObject.LineParse.ElementsLog.Count; i++)
                {
                    if (i < startIndex && i > endIndex)
                    {
                        indexToDelete.Add(i); 
                    }

                    if (searchType != "MatchWholeWord")
                    {
                        if (logObject.LineParse.ElementsPattern.Contains("Ndc"))
                        {
                            string stringTemp = (string)((NdcElement)logObject.LineParse.ElementsLog[i].Elements[logObject.LineParse.ElementsPattern.IndexOf("Ndc")]).Element;

                            if (stringTemp.Contains(searchContent))
                                deleteFlag = false;
                        }

                        if (logObject.LineParse.ElementsPattern.Contains("Message"))
                        {
                            string stringTemp = (string)((NdcElement)logObject.LineParse.ElementsLog[i].Elements[logObject.LineParse.ElementsPattern.IndexOf("Message")]).Element;

                            if (stringTemp.Contains(searchContent))
                                deleteFlag = false;
                        }
                    }

                    if (searchType == "MatchWholeWord")
                    {
                        if (logObject.LineParse.ElementsPattern.Contains("Ndc"))
                        {
                            string[] elementsTemp = ((string)((NdcElement)logObject.LineParse.ElementsLog[i].Elements[logObject.LineParse.ElementsPattern.IndexOf("Ndc")]).Element).Split(' ');

                            foreach (string element in elementsTemp)
                            {
                                if (element.ToLower() == searchContent.ToLower())
                                    deleteFlag = false;
                            }
                        }

                        if (logObject.LineParse.ElementsPattern.Contains("Message"))
                        {
                            string[] elementsTemp = ((string)((NdcElement)logObject.LineParse.ElementsLog[i].Elements[logObject.LineParse.ElementsPattern.IndexOf("Message")]).Element).Split(' ');

                            foreach (string element in elementsTemp)
                            {
                                if (element.ToLower() == searchContent.ToLower())
                                    deleteFlag = false;
                            }
                        }
                    }

                    if (deleteFlag == true)
                        indexToDelete.Add(i);
                    
                    deleteFlag = true;
                }

                for (int i = indexToDelete.Count - 1; i >= 0; i--)
                {
                    logObject.LineParse.ElementsLog.RemoveAt(indexToDelete[i]);
                }
            }

            return logObject;
        }
    }
}