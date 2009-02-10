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
            }
            else
            {
                matchWholeWordFilter = false;
            }


            LogParserFilter filter = Filter.CreateFilter(startTime, endTime, threadId, level, searchContent, matchWholeWordFilter, searchNumberOfItems,
                                                         startSearchIndex, endSearchIndex, startSearchByte,
                                                         endSearchByte);


            parserContent = ParseLogFile.ParseFile(selectedFile);

            parserContent.ParseLogFile(filter);

            Session["parserContent"] = parserContent;

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
            }
            else
            {
                matchWholeWordFilter = false;
            }


            LogParserFilter filter = Filter.CreateFilter(startTime, endTime, threadId, level, searchContent, matchWholeWordFilter, searchNumberOfItems,
                                                         startSearchIndex, endSearchIndex, startSearchByte,
                                                         endSearchByte);
            
            parserContent = (LogDisplay)Session["parserContent"];
            
            parserContent.ParseLogFile(filter);
            
            Session["parserContent"] = parserContent;

            return RedirectToAction("DisplayLog", "LogView");     
        }

        public ActionResult DisplayLog()
        {
            parserContent = (LogDisplay)Session["parserContent"];

            if (parserContent == null)
            {
                parserContent = new LogDisplay();
                parserContent.Parsing10MBLogFile(null, null, null, null);
            }
            ViewData["Id"] = Session["Id"];
            ViewData["Content"] = parserContent;

            return View();
        }

        public ActionResult DisplayLogFiles()
        {
            return View();
        }

        public ActionResult Log(string Id)
        {
            parserContent = ParseLogFile.ParseFile(Id);

            Session["Id"] = Id;
            Session["parserContent"] = parserContent;

            return RedirectToAction("DisplayLog", "LogView");
        }

        private LogDisplay parserContent;
    }
}
