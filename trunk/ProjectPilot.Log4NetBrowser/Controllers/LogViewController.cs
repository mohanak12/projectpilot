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

            //return RedirectToAction("DisplayLog"); 
            return RedirectToAction("DisplayLog", "LogView");
        }

        private LogDisplay parserContent;
    }
}
