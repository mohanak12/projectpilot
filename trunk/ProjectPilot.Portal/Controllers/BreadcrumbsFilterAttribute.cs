using System;
using System.Globalization;
using System.Web.Mvc;
using ProjectPilot.Portal.Models;

namespace ProjectPilot.Portal.Controllers
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class BreadcrumbsFilterAttribute : ActionFilterAttribute
    {
        public BreadcrumbsFilterAttribute(
            string breadcrumbLinkTextFormat, 
            string breadcrumbLinkTextVariable, 
            int breadcrumbLevel)
        {
            this.breadcrumbLinkTextFormat = breadcrumbLinkTextFormat;
            this.breadcrumbLinkTextVariable = breadcrumbLinkTextVariable;
            this.breadcrumbLevel = breadcrumbLevel;
        }

        public int BreadcrumbLevel
        {
            get { return breadcrumbLevel; }
        }

        public string BreadcrumbLinkTextFormat
        {
            get { return breadcrumbLinkTextFormat; }
        }

        public string BreadcrumbLinkTextVariable
        {
            get { return breadcrumbLinkTextVariable; }
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            IBreadcrumbsManager breadcrumbsManager = new DefaultBreadcrumbsManager(filterContext.HttpContext);
            ProjectPilotControllerBase controller = (ProjectPilotControllerBase)filterContext.Controller;

            string breadcrumbLinkText = string.Format(
                CultureInfo.InvariantCulture,
                breadcrumbLinkTextFormat,
                controller.Session[breadcrumbLinkTextVariable]);

            Breadcrumb[] breadcrumbs = breadcrumbsManager.AddBreadcrumb(breadcrumbLinkText, breadcrumbLevel);
            controller.ViewData.Add("Breadcrumbs", breadcrumbs);
        }

        private readonly int breadcrumbLevel;
        private readonly string breadcrumbLinkTextFormat;
        private readonly string breadcrumbLinkTextVariable;
    }
}