using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectPilot.Framework.Projects;


namespace ProjectPilot.Portal.Views
{
    public partial class ProjectView : ViewPage<Project>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.ProjectModulesList.DataSource = ViewData.Model.Modules;
            this.ProjectModulesList.DataBind();
        }
    }
}
