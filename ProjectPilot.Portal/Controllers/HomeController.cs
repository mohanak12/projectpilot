using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using ProjectPilot.Framework;
using ProjectPilot.Framework.Projects;

namespace ProjectPilot.Portal.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Project[] projects = new Project[]
                                     {
                                         new Project("ebsy", "EBSy"), 
                                         new Project("mobiinfo", "Mobi-Info"), 
                                         new Project("bhwr", "Mobilkom BHWR"),
                                         new Project("octopus", "Octopus"), 
                                         new Project("projectpilot", "ProjectPilot"), 
                                     };

            projects[2].Modules.Add(new StaticHtmlPageModule("SVN Stats", "SvnStats.html"));

            return View("Home", projects);
        }
    }
}