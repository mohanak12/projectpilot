using System.Collections;
using System.Linq;

namespace ProjectPilot.Portal.Models
{
    public interface IBreadcrumbsManager
    {
        Breadcrumb[] AddBreadcrumb(string linkText, int level);
    }
}
