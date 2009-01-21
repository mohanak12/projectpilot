using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.SessionState;
using ProjectPilot.Extras.LogParser;
using ProjectPilot.Log4NetBrowser.Models;

namespace ProjectPilot.Log4NetBrowser.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewData["Title"] = "ProjectPilot";
            ViewData["Message"] = "Project Pilot Log4Net browser!";

            return View();
        }

        public ActionResult FileSelect()
        {
            return View();
        }

        public ActionResult Load(
                            string levelSelect, string StartTime,
                            string EndTime, string ThreadId, 
                            string fileSelect, string numberOfItems, 
                            string logPattern, string separator, 
                            string searchType, string Search)
        {
            //file is not selected
            if (string.IsNullOrEmpty(fileSelect) || string.IsNullOrEmpty(logPattern) || string.IsNullOrEmpty(separator))
                return RedirectToAction("Index");

            //save values !!!  Session not supported yet
            fileSelected = fileSelect;
            pattern = logPattern;
            logSeparator = separator;

            LogParserFilter filter = new LogParserFilter();
            parserContent = new LogDisplay();
            bool time = true;

            DateTime startTime = new DateTime();
            DateTime endTime = new DateTime();
            CultureInfo cultureToUse = CultureInfo.InvariantCulture;


            if (string.IsNullOrEmpty(StartTime) && string.IsNullOrEmpty(EndTime))
            {
                StartTime = "";
                EndTime = "";
            }

            try
            {
                startTime = DateTime.ParseExact(StartTime, "dd.MM.yyyy HH:mm:ss,fff", cultureToUse);
            }
            catch (FormatException)
            {
                time = false;
            }

            try
            {
                endTime = DateTime.ParseExact(EndTime, "dd.MM.yyyy HH:mm:ss,fff", cultureToUse);
            }
            catch (FormatException)
            {
                time = false;
            }

            if (time)
            {
                filter.FilterTimestampStart = startTime;
                filter.FilterTimestampEnd = endTime;
            }

            filter.FilterLevel = levelSelect;

            filter.FilterThreadId = ThreadId;

            if (!string.IsNullOrEmpty(numberOfItems))
            {
                filter.FilterNumberOfLogItems = int.Parse(numberOfItems);
            }
            else
            {
                filter.FilterNumberOfLogItems = 255;
            }

            if (!string.IsNullOrEmpty(Search))
            {
                if (searchType == "MatchWholeWord")
                {
                    filter.MatchWholeWordOnly = Search;
                }

                if (searchType == "MatchCase")
                {
                    filter.MatchCase = Search;
                }
            }

            parserContent.Parsing10MBLogFile(filter, fileSelected, pattern, separator);

            Session["parserContent"] = parserContent;
            Session["fileSelected"] = fileSelected;
            Session["logSeparator"] = logSeparator;
            Session["pattern"] = pattern;

            return RedirectToAction("Display"); 
        }


        public ActionResult Reload(
                            string levelSelect, string StartTime, string EndTime, 
                            string ThreadId, string numberOfItems,
                            string searchType, string Search)
        {
            LogParserFilter filter = new LogParserFilter();
            parserContent = new LogDisplay();
            bool time = true;
//            StartTime = "";
//            EndTime = "";
            DateTime startTime = new DateTime();
            DateTime endTime = new DateTime();
            CultureInfo cultureToUse = CultureInfo.InvariantCulture;

            fileSelected = (string)Session["fileSelected"];
            logSeparator =(string)Session["logSeparator"];
            pattern = (string)Session["pattern"];

            if (string.IsNullOrEmpty(StartTime) && string.IsNullOrEmpty(EndTime))
            {
                StartTime = "";
                EndTime = "";
            }

            try
            {
                startTime = DateTime.ParseExact(StartTime, "dd.MM.yyyy HH:mm:ss,fff", cultureToUse);
            }
            catch (FormatException)
            {
                time = false;
            }

            try
            {
                endTime = DateTime.ParseExact(EndTime, "dd.MM.yyyy HH:mm:ss,fff", cultureToUse);
            }
            catch (FormatException)
            {
                time = false;
            } 

            if(time)
            {
                filter.FilterTimestampStart = startTime;
                filter.FilterTimestampEnd = endTime;
            }

            
            filter.FilterLevel = levelSelect;

            filter.FilterThreadId = ThreadId;

            if (!string.IsNullOrEmpty(numberOfItems))
            {
                filter.FilterNumberOfLogItems = int.Parse(numberOfItems);
            }
            else
            {
                filter.FilterNumberOfLogItems = 255;
            }

            parserContent.Parsing10MBLogFile(filter, fileSelected, pattern, logSeparator);
            
            //(Log4NetBrowser - Display) Search Filter /
            bool deleteFlag = false;
            
            if (!string.IsNullOrEmpty(Search))
            {
                for(int i = 0; i < parserContent.LineParse.ElementsLog.Count; i++)
                {
                    if (searchType == "MatchCase")
                    {
                        if (parserContent.LineParse.ElementsPattern.Contains("Ndc"))
                        {
                            string stringTemp = (string)((NdcElement)parserContent.LineParse.ElementsLog[i].Elements[parserContent.LineParse.ElementsPattern.IndexOf("Ndc")]).Element;

                            if (!stringTemp.Contains(Search))
                                deleteFlag = true;
                        }

                        if (parserContent.LineParse.ElementsPattern.Contains("Message"))
                        {
                            string stringTemp = (string)((NdcElement)parserContent.LineParse.ElementsLog[i].Elements[parserContent.LineParse.ElementsPattern.IndexOf("Message")]).Element;

                            if (!stringTemp.Contains(Search))
                                deleteFlag = true;
                        }

                        if (searchType == "MatchWholeWord")
                        {
                            if (parserContent.LineParse.ElementsPattern.Contains("Ndc"))
                            {
                                string[] elementsTemp = ((string)((NdcElement)parserContent.LineParse.ElementsLog[i].Elements[parserContent.LineParse.ElementsPattern.IndexOf("Ndc")]).Element).Split(' ');

                                foreach (string element in elementsTemp)
                                {
                                    if (element == Search)
                                        deleteFlag = true;
                                }
                            }

                            if (parserContent.LineParse.ElementsPattern.Contains("Message"))
                            {
                                string[] elementsTemp = ((string)((NdcElement)parserContent.LineParse.ElementsLog[i].Elements[parserContent.LineParse.ElementsPattern.IndexOf("Message")]).Element).Split(' ');

                                foreach (string element in elementsTemp)
                                {
                                    if (element == Search)
                                        deleteFlag = true;
                                }
                            }
                        }
                    }

                    if (deleteFlag == true)
                        parserContent.LineParse.ElementsLog.RemoveAt(i);

                    deleteFlag = false;
                }
            }
            //(Log4NetBrowser - Display) Search Filter /

            Session["parserContent"] = parserContent;

            return RedirectToAction("Display"); 
        }

        public ActionResult Display(int? Id)
        {
            parserContent = (LogDisplay)Session["parserContent"];

            if (Id != null)
            {
                if (Id < 0)
                {
                    Id *= -1;
                    parserContent.IndexList.Remove((int)Id-1);
                }
                else
                    parserContent.IndexList.Add((int)Id-1);
            }
            else
                Id = 0;

            ViewData["Content"] = parserContent;
            ViewData["Id"] = (int)Id;
            
            return View();
        }

        public ActionResult Ndc(string Id)
        {
                      

            return View();
        }

        private LogDisplay parserContent;
        private string fileSelected;
        private string pattern;
        private string logSeparator;
    }
}
