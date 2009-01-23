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
                            string numberOfItems, string numberOfItemsPerPage,
                            string searchType, 
                            string Search)
        {

            //Get number of log entries in log file
            LogParserFilter filter = LoadParameters.CreateFilter(null, null,
                                                                 null, null, 
                                                                 null, null,
                                                                 null, true);
            parserContent = new LogDisplay();

            parserContent.Parsing10MBLogFile(filter, fileSelected);

            numberOfLogItems = parserContent.LineParse.NumberOfLogItems;

            filter = LoadParameters.CreateFilter(levelSelect,StartTime,
                                                 EndTime, ThreadId, numberOfItems,
                                                 searchType, Search, false);

            parserContent.Parsing10MBLogFile(filter, fileSelected);

            Session["parserContent"] = parserContent;
            Session["fileSelected"] = fileSelected;
            Session["numberOfLogItems"] = parserContent.LineParse.ElementsLog.Count;
            Session["numberOfLogItemsInLogFile"] = numberOfLogItems;
            Session["currentLogIndex"] = 1;

            //Default numberOfItemsPerPage
            if (string.IsNullOrEmpty(numberOfItemsPerPage))
                numberOfItemsPerPage = "51";

            Session["numberOfItemsPerPage"] = int.Parse(numberOfItemsPerPage) - 1;

            return RedirectToAction("ShowNextLogEntries"); 
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
                                                                 Search, false);
            
            parserContent = new LogDisplay();

            fileSelected = (string)Session["fileSelected"];

            parserContent.Parsing10MBLogFile(filter, fileSelected);

            parserContent = LocalSearchFilter.Filter(parserContent, null, null, searchType, Search);

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
      
            ViewData["StartIndexOfLogItemsShow"] = (int)Session["StartIndexOfLogItemsShow"];
            ViewData["EndIndexOfLogItemsShow"] = (int)Session["EndIndexOfLogItemsShow"];
            ViewData["Content"] = parserContent;
            ViewData["Id"] = (int)Id;
            ViewData["numberOfLogItemsInLogFile"] = Session["numberOfLogItemsInLogFile"];
            ViewData["showPreviousControl"] = Session["showPreviousControl"];
            ViewData["showNextControl"] = Session["showNextControl"];
            
            ViewData["test"] = test;

            return View();
        }

        public ActionResult ShowNextLogEntries()
        {
            numberOfItemsPerPage = (int)Session["numberOfItemsPerPage"];
            numberOfLogItems = (int)Session["numberOfLogItems"];
            
            Session["StartIndexOfLogItemsShow"] = (int)Session["currentLogIndex"];
            Session["currentLogIndex"] = (int)Session["currentLogIndex"] + numberOfItemsPerPage;

            if ((int)Session["currentLogIndex"] > numberOfLogItems)
                Session["currentLogIndex"] = numberOfLogItems;

            Session["EndIndexOfLogItemsShow"] = (int)Session["currentLogIndex"];

            showControls((int)Session["currentLogIndex"]);

            //parserContent = LocalSearchFilter.Filter(parserContent, (int)Session["StartIndexOfLogItemsShow"], (int)Session["EndIndexOfLogItemsShow"], null, null);
            //Session["parserContent"] = parserContent;

            return RedirectToAction("Display"); 
        }

        public ActionResult ShowPreviousLogEntries()
        {
            numberOfItemsPerPage = (int)Session["numberOfItemsPerPage"];
            numberOfLogItems = (int)Session["numberOfLogItems"];

            if ((int)Session["StartIndexOfLogItemsShow"] != 1)
            {
                Session["EndIndexOfLogItemsShow"] = (int)Session["StartIndexOfLogItemsShow"];
                Session["currentLogIndex"] = (int)Session["StartIndexOfLogItemsShow"] - numberOfItemsPerPage;
                Session["StartIndexOfLogItemsShow"] = (int)Session["currentLogIndex"];                
            }

            showControls((int)Session["currentLogIndex"]);

            if ((bool)Session["showPreviousControl"] == false)
                return RedirectToAction("ShowNextLogEntries");

            //parserContent = LocalSearchFilter.Filter(parserContent, (int)Session["StartIndexOfLogItemsShow"], (int)Session["EndIndexOfLogItemsShow"], null, null);
            //Session["parserContent"] = parserContent;

            return RedirectToAction("Display");
        }

        public void showControls(int currentIndex)
        {
            //NextControl visible (on/off)
            if ((int)Session["currentLogIndex"] < numberOfLogItems)
                Session["showNextControl"] = true;
            else
                Session["showNextControl"] = false;

            //PreviousControl visible (on/off)
            if ((int)Session["StartIndexOfLogItemsShow"] == 1)
                Session["showPreviousControl"] = false;
            else
                Session["showPreviousControl"] = true;
        }

        public ActionResult Ndc(string Id)
        {
            return View();
        }

        private LogDisplay parserContent;
        private int currentLogIndex;
        private int StartIndexOfLogItemsShow;
        private int EndIndexOfLogItemsShow;
        private string fileSelected;
        private int numberOfLogItems;
        private int numberOfItemsPerPage;
        private string test = "Niz123";
    }
}
