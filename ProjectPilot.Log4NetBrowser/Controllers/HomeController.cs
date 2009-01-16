using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
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

        public ActionResult Reload(string level, string levelSelect, string StartTime, string EndTime, string ThreadId, string fileSelect)
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

            parserContent.Parsing10MBLogFile(filter, fileSelect);

            return RedirectToAction("Display"); 
        }

        public ActionResult Display(int? Id)
        {
            if (Id != null)
            {
                if(Id ==-1)
                {
                    parserContent.IndexList.Remove(parserContent.IndexList.Last());
                }
                else
                {
                    if (parserContent.IndexList.Contains((int)Id))
                        parserContent.IndexList.Remove((int)Id);
                    else
                        parserContent.IndexList.Add((int)Id);
                }
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
            return View();
        }

        private static LogDisplay parserContent;
    }
}
