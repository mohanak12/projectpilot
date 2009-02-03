using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
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

            ViewData["Content"] = parserContent;

            return View();
        }

        public ActionResult Log(string Id)
        {
            parserContent = ParseLogFile.ParseFile(Id);

            Session["parserContent"] = parserContent;
            return RedirectToAction("DisplayLog"); 
        }

        private LogDisplay parserContent;
    }
}
