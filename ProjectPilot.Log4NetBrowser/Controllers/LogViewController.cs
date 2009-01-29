using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using ProjectPilot.Extras.LogParser;
using ProjectPilot.Log4NetBrowser.Models;

namespace ProjectPilot.Log4NetBrowser.Controllers
{
    public class LogViewController : Controller
    {
        public ActionResult DisplayLog()
        {
            // Add action logic here
            parserContent = new LogDisplay();
            parserContent.Parsing10MBLogFile(null, null);

            Session["parserContent"] = parserContent;
            ViewData["Content"] = parserContent;

            return View();
        }

        private LogDisplay parserContent;
    }
}
