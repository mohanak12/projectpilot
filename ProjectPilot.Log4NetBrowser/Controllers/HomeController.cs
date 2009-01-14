using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
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

            parserContent = new LogDisplay();
            parserContent.Parsing10MBLogFile();

            return View();
            //return RedirectToAction("Display"); 
        }

        public ActionResult Display(int? Id)
        {
            if (Id != null)
                ViewData["Index"] = Id;
            else 
                ViewData["Index"] = -1;
            
            ViewData["Content"] = parserContent;
            
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
