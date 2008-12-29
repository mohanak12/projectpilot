using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectPilot.Framework.Modules;
using ProjectPilot.Portal.Models;

namespace ProjectPilot.Portal.Views
{
    public partial class ProjectView : ViewPage<ProjectViewModel>
    {
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "viewableModule")]
        protected void Page_Load(object sender, EventArgs e)
        {
            // display a list of modules for the project
            this.ProjectModulesList.DataSource = ViewData.Model.Project.ListModules();
            this.ProjectModulesList.DataBind();

            // display the specified model's page
            if (ViewData.Model.ModuleId != null)
            {
                IProjectModule moduleToShow = ViewData.Model.Module;
                IViewable viewableModule = (IViewable)moduleToShow;
            }
        }
    }
}
