using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using ProjectPilot.Framework;
using ProjectPilot.Framework.Modules;
using ProjectPilot.Portal.Models;

namespace ProjectPilot.Portal.Controllers
{
    public class ProjectViewController : ProjectPilotControllerBase
    {
        [BreadcrumbsFilter("Project {0}", "CurrentProjectName", 1)]
        public ActionResult Overview(string projectId, string moduleName)
        {
            Project project = Facade.GetProject (projectId);
            this.WebContext.CurrentProjectName = project.ProjectName;
            return View("ProjectView", new ProjectViewModel(project, null));
        }

        [BreadcrumbsFilter("{0}", "CurrentModuleName", 2)]
        public ActionResult Module(string projectId, string moduleId)
        {
            Project project = Facade.GetProject(projectId);
            IProjectModule module = project.GetModule(moduleId);
            this.WebContext.CurrentModuleName = module.ModuleName;
            return View("ProjectView", new ProjectViewModel(project, moduleId));
        }
    }
}
