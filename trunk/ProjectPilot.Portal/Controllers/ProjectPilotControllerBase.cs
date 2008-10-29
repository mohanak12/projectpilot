using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectPilot.Portal.Models;

namespace ProjectPilot.Portal.Controllers
{
    public abstract class ProjectPilotControllerBase : Controller
    {
        protected override void ExecuteCore()
        {
            this.webContext = new DefaultProjectPilotWebContext(this.Session);
            base.ExecuteCore();
        }

        protected IProjectPilotFacade Facade
        {
            get
            {
                return (IProjectPilotFacade)Session["Facade"];
            }
        }

        protected IProjectPilotWebContext WebContext
        {
            get { return webContext; }
        }

        private IProjectPilotWebContext webContext;
    }
}
