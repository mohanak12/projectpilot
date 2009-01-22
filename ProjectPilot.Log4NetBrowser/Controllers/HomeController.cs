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
                            string numberOfItems, string searchType, 
                            string Search)
        {

            LogParserFilter filter = LoadParameters.CreateFilter(levelSelect,StartTime,
                                                                 EndTime, ThreadId, null,
                                                                 searchType, Search);

            parserContent = new LogDisplay();

            parserContent.Parsing10MBLogFile(filter, fileSelected);

            Session["parserContent"] = parserContent;
            Session["fileSelected"] = fileSelected;

            return RedirectToAction("Display"); 
        }


        public ActionResult Reload(string levelSelect, 
                                   string StartTime, 
                                   string EndTime, 
                                   string ThreadId, 
                                   string numberOfItems,
                                   string searchType,
                                   string Search)
        {
            LogParserFilter filter = LoadParameters.CreateFilter(levelSelect, StartTime,
                                                                 EndTime, ThreadId,
                                                                 numberOfItems, searchType,
                                                                 Search);
            
            parserContent = new LogDisplay();

            fileSelected = (string)Session["fileSelected"];

            parserContent.Parsing10MBLogFile(filter, fileSelected);

            parserContent = LocalSearchFilter.Filter(parserContent, searchType, Search);

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
            ViewData["test"] = test;

            return View();
        }

        public ActionResult Ndc(string Id)
        {
                      

            return View();
        }

        private LogDisplay parserContent;
        private string fileSelected;
        private string test = "Niz123";
    }
}
