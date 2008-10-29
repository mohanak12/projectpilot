using System.Web.Mvc;
using ProjectPilot.Portal.Models;

namespace ProjectPilot.Portal.Controllers
{
    public class BreadcrumbsFilterAttribute : ActionFilterAttribute
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

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            IBreadcrumbsManager breadcrumbsManager = new DefaultBreadcrumbsManager(filterContext.HttpContext);
            ProjectPilotControllerBase controller = (ProjectPilotControllerBase)filterContext.Controller;

            string breadcrumbLinkText = string.Format(breadcrumbLinkTextFormat,
                controller.Session[breadcrumbLinkTextVariable]);

            Breadcrumb[] breadcrumbs = breadcrumbsManager.AddBreadcrumb(breadcrumbLinkText, breadcrumbLevel);
            controller.ViewData.Add("Breadcrumbs", breadcrumbs);
        }

        private readonly int breadcrumbLevel;
        private readonly string breadcrumbLinkTextFormat;
        private readonly string breadcrumbLinkTextVariable;
    }
}