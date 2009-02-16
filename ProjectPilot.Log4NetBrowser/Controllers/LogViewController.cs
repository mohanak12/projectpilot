using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.SessionState;
using ProjectPilot.Extras.LogParser;
using ProjectPilot.Log4NetBrowser.Models;
using System.Xml;

namespace ProjectPilot.Log4NetBrowser.Controllers
{
    public class LogViewController : Controller
    {
        public LogViewController()
        {
            XMLsettingsFilePath = @"\\zarja\share\Marko\LogConfig.xml";

            Dictionary<string, string> settings;
            settings = SettingsFromXMLfile.Read(XMLsettingsFilePath);

            numberOfItemsPerPage = int.Parse(settings["NumberOfItemsPerPage"]);
            searchNumberOfItems = int.Parse(settings["FilterNumberOfLogItems"]);
        }

        public ActionResult LoadFile(
                           string startTime,
                           string endTime,
                           string threadId,
                           string level,
                           string searchContent,
                           string matchWholeWord,
                           int? numberOfItemsPerPage,
                           int? searchNumberOfItems,
                           int? startSearchIndex,
                           int? endSearchIndex,
                           int? startSearchByte,
                           int? endSearchByte,
                           string selectedFile)
        {
            if (string.IsNullOrEmpty(selectedFile))
                return RedirectToAction("DisplayLogFiles", "LogView");

            bool matchWholeWordFilter;

            if (matchWholeWord == "on")
            {
                matchWholeWordFilter = true;
                Session["matchWholeWordFilter"] = true;
            }
            else
            {
                matchWholeWordFilter = false;
                Session["matchWholeWordFilter"] = false;
            }

            if (numberOfItemsPerPage == null)
            {
                numberOfItemsPerPage = this.numberOfItemsPerPage;
            }

            if (searchNumberOfItems == null)
            {
                searchNumberOfItems = this.searchNumberOfItems;
            }

            LogParserFilter filter = Filter.CreateFilter(startTime, endTime, threadId, level, searchContent, matchWholeWordFilter, searchNumberOfItems,
                                                         startSearchIndex, endSearchIndex, startSearchByte,
                                                         endSearchByte, numberOfItemsPerPage);


            parserContent = ParseLogFile.ParseFile(selectedFile, XMLsettingsFilePath);

            parserContent.ParseLogFile(filter);

            Session["Id"] = selectedFile;
            Session["filter"] = filter;
            Session["parserContent"] = parserContent;
            Session["searchContent"] = searchContent;
            
            if (numberOfItemsPerPage != null)
            this.numberOfItemsPerPage = (int)numberOfItemsPerPage;

            return RedirectToAction("DisplayLog", "LogView");
        }
        
        public ActionResult Reload(
                                   string startTime,
                                   string endTime,
                                   string threadId,
                                   string level,
                                   string searchContent,
                                   string matchWholeWord,
                                   int? numberOfItemsPerPage,
                                   int? searchNumberOfItems,
                                   int? startSearchIndex,
                                   int? endSearchIndex,
                                   int? startSearchByte,
                                   int? endSearchByte)
        {
            bool matchWholeWordFilter;

            if (matchWholeWord == "on")
            {
                matchWholeWordFilter = true;
                Session["matchWholeWordFilter"] = true;
            }
            else
            {
                matchWholeWordFilter = false;
                Session["matchWholeWordFilter"] = false;
            }

            if (numberOfItemsPerPage == null)
            {
                numberOfItemsPerPage = this.numberOfItemsPerPage;
            }

            if (searchNumberOfItems == null)
            {
                searchNumberOfItems = this.numberOfItemsPerPage;
            }

            LogParserFilter filter = Filter.CreateFilter(startTime, endTime, threadId, level, searchContent, matchWholeWordFilter, searchNumberOfItems,
                                                         startSearchIndex, endSearchIndex, startSearchByte,
                                                         endSearchByte, numberOfItemsPerPage);
            
            parserContent = (LogDisplay)Session["parserContent"];
            
            parserContent.ParseLogFile(filter);
            
            Session["parserContent"] = parserContent;
            Session["filter"] = filter;
            Session["searchContent"] = searchContent;

            if (numberOfItemsPerPage != null)
                this.numberOfItemsPerPage = (int)numberOfItemsPerPage;

            return RedirectToAction("DisplayLog", "LogView");     
        }

        public ActionResult DisplayLog()
        {
            parserContent = (LogDisplay)Session["parserContent"];

            LogParserFilter filter;
            if (Session["filter"] != null)
            {
                filter = (LogParserFilter)Session["filter"];
            }
            else
            {
                filter = new LogParserFilter();
                filter.FilterNumberOfLogItems = searchNumberOfItems;
            }
            
            if (parserContent == null)
            {
                parserContent = new LogDisplay();
                parserContent.Parsing10MBLogFile(null, null, null, null);
            }

            ViewData["Id"] = Session["Id"];
            ViewData["Content"] = parserContent;
            ViewData["filter"] = filter;
            ViewData["numberOfItemsPerPage"] = numberOfItemsPerPage;
            ViewData["matchWholeWordFilter"] = Session["matchWholeWordFilter"];
            ViewData["searchContent"] = Session["searchContent"];
            Session["parserContent"] = parserContent;

            return View();
        }

        public ActionResult DisplayLogFiles()
        {
            LogParserFilter filter;
            if (Session["filter"] != null)
            {
                filter = (LogParserFilter)Session["filter"];
            }
            else
            {
                filter = new LogParserFilter();
                filter.FilterNumberOfLogItems = searchNumberOfItems;
            }
            ViewData["filter"] = filter;
            ViewData["numberOfItemsPerPage"] = numberOfItemsPerPage;

            return View();
        }

        public ActionResult Log(string Id)
        {
            parserContent = ParseLogFile.ParseFile(Id, XMLsettingsFilePath);

            Session["Id"] = Id;
            Session["parserContent"] = parserContent;
            Session["filter"] = null;
            Session["matchWholeWordFilter"] = null;
            Session["searchContent"] = "";

            return RedirectToAction("DisplayLog", "LogView");
        }

        private LogDisplay parserContent;
        private int numberOfItemsPerPage = 50;
        private int searchNumberOfItems = 255;
        private string XMLsettingsFilePath;
    }
}
