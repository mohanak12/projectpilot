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
    public class ProjectViewController : ProjectPilotControllerBase
    {
        public ActionResult Overview(string projectId, string moduleName)
        {
            Project project = Facade.GetProject (projectId);
            return View("ProjectView", new ProjectViewModel(project, null));
        }

        public ActionResult Module(string projectId, string moduleId)
        {
            Project project = Facade.GetProject(projectId);
            return View("ProjectView", new ProjectViewModel(project, moduleId));
        }
    }
}
