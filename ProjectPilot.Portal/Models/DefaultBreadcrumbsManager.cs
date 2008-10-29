using System.Collections.Generic;
using System.Web;

namespace ProjectPilot.Portal.Models
{
    public class DefaultBreadcrumbsManager : IBreadcrumbsManager
    {
        public DefaultBreadcrumbsManager(HttpContextBase httpContext)
        {
            this.httpContext = httpContext;
            breadcrumbs = (List<Breadcrumb>)(httpContext.Session["Breadcrumbs"]);
            if (breadcrumbs == null)
                breadcrumbs = new List<Breadcrumb>();
        }

        public Breadcrumb[] AddBreadcrumb(string linkText, int level)
        {
            if (breadcrumbs.Count > level)
                breadcrumbs.RemoveRange(level, breadcrumbs.Count - level);

            breadcrumbs.Add(new Breadcrumb(linkText, httpContext.Request.Url.ToString()));
            httpContext.Session["Breadcrumbs"] = breadcrumbs;
            return breadcrumbs.ToArray();
        }

        private readonly HttpContextBase httpContext;
        private List<Breadcrumb> breadcrumbs;
    }
}