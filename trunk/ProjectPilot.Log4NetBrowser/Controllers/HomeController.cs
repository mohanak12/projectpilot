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
                            string levelSelect, string StartTime, string EndTime,
                            string ThreadId, string fileSelect, string numberOfItems,
                            string logPattern, string separator)
        {
            //file is not selected
            if (string.IsNullOrEmpty(fileSelect))
                return RedirectToAction("Index"); 


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

            parserContent.Parsing10MBLogFile(filter, fileSelected, pattern, separator);

            return RedirectToAction("Display"); 
        }


        public ActionResult Reload(
                            string levelSelect, string StartTime, string EndTime, 
                            string ThreadId, string numberOfItems)
        {
            LogParserFilter filter = new LogParserFilter();
            parserContent = new LogDisplay();
            bool time = true;
//            StartTime = "";
//            EndTime = "";
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

            return RedirectToAction("Display"); 
        }

        public ActionResult Display(int? Id)
        {
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
            ViewData["Data"] = Id;

            SessionStateItemCollection items = new SessionStateItemCollection();

            items["LastName"] = "Wilson";
            items["FirstName"] = "Dan";

            foreach (string s in items.Keys)
                Response.Write("items[\"" + s + "\"] = " + items[s].ToString() + "<br />");

            return View();
        }

        private static LogDisplay parserContent;
        private static string fileSelected;
        private static string pattern;
        private static string logSeparator;
    }
}
