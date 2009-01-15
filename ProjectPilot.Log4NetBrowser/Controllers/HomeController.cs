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
            {
                if(Id ==-1)
                {
                    parserContent.IndexList.Remove(parserContent.IndexList.Last());
                }
                else
                {
                    if (parserContent.IndexList.Contains((int)Id))
                        parserContent.IndexList.Remove((int)Id);
                    else
                        parserContent.IndexList.Add((int)Id);
                }
            }
            else
                Id = 0;

            ViewData["Content"] = parserContent;
            ViewData["Id"] = (int)Id;
            
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
