using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using ProjectPilot.Framework.Projects;

namespace ProjectPilot.Portal.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Project[] projects = new Project[]
                                     {
                                         new Project("EBSy"), 
                                         new Project("Mobi-Info"), 
                                         new Project("Mobilkom BHWR"),
                                         new Project("Octopus"), 
                                         new Project("ProjectPilot"), 
                                     };

            return View("Home", projects);
        }
    }
}