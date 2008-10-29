using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using ProjectPilot.Framework;
using ProjectPilot.Framework.Projects;
using ProjectPilot.Portal.Models;

namespace ProjectPilot.Portal.Controllers
{
    public class HomeController : ProjectPilotControllerBase
    {
        [BreadcrumbsFilter("Home", "", 0)]
        public ActionResult Index()
        {
            return View("Home", Facade.ListAllProjects());
        }
    }
}