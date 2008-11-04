using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ProjectPilot.Framework;
using ProjectPilot.Framework.Modules;
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

            ProjectPilotConfiguration projectPilotConfiguration = new ProjectPilotConfiguration();
            projectPilotConfiguration.ProjectPilotWebAppRootUrl = this.Request.ApplicationPath;

            IFileManager fileManager = new DefaultFileManager(
                @"D:\MyStuff\projects\ProjectPilot\ProjectPilot.Tests\bin\Debug", projectPilotConfiguration, projectRegistry);

            projectRegistry.FileManager = fileManager;

            Project[] projectsToAdd = new Project[]
                                     {
                                         new Project("ebsy", "EBSy"), 
                                         new Project("mobiinfo", "Mobi-Info"), 
                                         new Project("bhwr", "Mobilkom BHWR"),
                                         new Project("octopus", "Octopus"), 
                                         new Project("projectpilot", "ProjectPilot"), 
                                     };
            projectsToAdd[2].AddModule(
                new StaticHtmlPageModule(projectsToAdd[2], "SVNStats", "SVN Stats", "SvnStats.html", fileManager));
            projectsToAdd[2].AddModule(
                new RevisionControlStatsModule(null, fileManager, null));

            foreach (Project project in projectsToAdd)
                projectRegistry.AddProject(project);

            Session["Facade"] = new DefaultFacade(projectRegistry);
        }
    }
}