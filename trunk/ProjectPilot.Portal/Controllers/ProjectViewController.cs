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
    public class ProjectViewController : Controller
    {
        public ActionResult Details(string projectId)
        {
            Project project = new Project("bhwr", "Mobilkom BHWR");
            project.Modules.Add(new StaticHtmlPageModule("SVN Stats", "SvnStats.html"));

            return View("ProjectView", project);
        }
    }
}
