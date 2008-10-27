using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ProjectPilot.Framework;
using ProjectPilot.Portal.Models;

namespace ProjectPilot.Portal
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "ProjectOverview",                                              // Route name
                "ProjectView/Overview/{projectId}",
                new { controller = "ProjectView", action = "Overview" }
            );

            routes.MapRoute(
                "ProjectModule",                                              // Route name
                "ProjectView/Module/{projectId}/{moduleId}",
                new { controller = "ProjectView", action = "Module", projectId = "", moduleId = "" }
            );

            routes.MapRoute(
                "Default",                                              // Route name
                "{controller}/{action}/{id}",                           // URL with parameters
                new { controller = "Home", action = "Index", id = "" }  // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            RegisterRoutes(RouteTable.Routes);
        }

        protected void Session_Start()
        {
            ProjectRegistry projectRegistry = new ProjectRegistry();
            IFileManager fileManager = new DefaultFileManager(projectRegistry);
            projectRegistry.FileManager = fileManager;

            Session["Facade"] = new DefaultFacade(projectRegistry);
        }
    }
}