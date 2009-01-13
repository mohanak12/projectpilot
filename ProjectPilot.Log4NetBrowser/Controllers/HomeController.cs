using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace ProjectPilot.Log4NetBrowser.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewData["Title"] = "ProjectPilot";
            ViewData["Message"] = "Project Pilot Log4Net browser!";

            //return View();
            return RedirectToAction("Display"); 
        }

        public ActionResult Display()
        {
            return View();
        }
    }
}
