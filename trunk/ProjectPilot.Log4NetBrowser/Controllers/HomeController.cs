using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
    [HandleError]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewData["Title"] = "ProjectPilot";
            ViewData["Message"] = "Project Pilot Log4Net browser!";

            XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
            xmlReaderSettings.IgnoreComments = true;
            xmlReaderSettings.IgnoreProcessingInstructions = true;
            xmlReaderSettings.IgnoreWhitespace = true;

            using (XmlReader xmlReader = XmlReader.Create(File.OpenRead(@"\\zarja\share\Marko\LogConfig.xml"), xmlReaderSettings))
            {
                string logKey;

                while (false == xmlReader.EOF)
                {
                    xmlReader.Read();

                    if (xmlReader.Name == "LogURL" && xmlReader.NodeType != XmlNodeType.EndElement)
                        {
                            logKey = xmlReader["logKey"];
                            xmlReader.Read();
                            logFiles.Add(logKey ,xmlReader.Value);
                        }
                }
            }

            Session["fileSelected"] = null;
            Session["logFilesList"] = logFiles;

            return View();
        }

        public ActionResult FileSelect()
        {
            ViewData["logFilesList"] = Session["logFilesList"];
            return View();
        }

        public ActionResult Load(
                            string levelSelect, string StartTime,
                            string EndTime, string ThreadId,
                            string numberOfItems, string numberOfItemsPerPage,
                            string searchType, 
                            string Search)
        {
            if (!string.IsNullOrEmpty((string)Session["fileSelected"]))
                fileSelected = (string)Session["fileSelected"];
            else
                fileSelected = @"\\zarja\share\Marko\SSM+2009-01-08.log.28.small"; // Default file

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
            Session["numberOfItemsPerPage"] = numberOfItemsPerPage;
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
            ViewData["numberOfItemsPerPage"] = Session["numberOfItemsPerPage"];
            ViewData["fileKeySelected"] = Session["fileKeySelected"];
       
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
            XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
            xmlReaderSettings.IgnoreComments = true;
            xmlReaderSettings.IgnoreProcessingInstructions = true;
            xmlReaderSettings.IgnoreWhitespace = true;

            using (XmlReader xmlReader = XmlReader.Create(File.OpenRead(@"\\zarja\share\Marko\LogConfig.xml"), xmlReaderSettings))
            {
                string key;

                while (false == xmlReader.EOF)
                {
                    xmlReader.Read();

                    if (xmlReader.Name == "LogURL" && xmlReader.NodeType != XmlNodeType.EndElement)
                    {

                        key = xmlReader["logKey"];
                        xmlReader.Read();
                        logFiles.Add(key, xmlReader.Value);
                    }
                }
            }

            if (logFiles.ContainsKey(Id))
            {
                ViewData["DisplayURL"] = logFiles[Id];
                Session["fileSelected"] = logFiles[Id];
                Session["fileKeySelected"] = Id;
                return RedirectToAction("Load"); 
            }

            return View();
        }

        private LogDisplay parserContent;
        private int currentLogIndex;
        private int StartIndexOfLogItemsShow;
        private int EndIndexOfLogItemsShow;
        private string fileSelected;
        private string fileKeySelected;
        private int numberOfLogItems;
        private int numberOfItemsPerPage;
        private readonly Dictionary<string, string> logFiles = new Dictionary<string, string>();
    }
}
