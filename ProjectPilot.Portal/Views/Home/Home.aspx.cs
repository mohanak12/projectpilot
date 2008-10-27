using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectPilot.Framework.Projects;


namespace ProjectPilot.Portal.Views.Home
{
    public partial class Home : ViewPage<ICollection<Project>>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.ProjectList.DataSource = ViewData.Model;
            this.ProjectList.DataBind();
        }
    }
}
