using System.IO;

namespace Headless.Web
{
    public class FileWebRouteProcessor : TemplatedWebRouteProcessorBase
    {
        protected override ResponseTemplate GetResponseTemplate(RoutedWebRequest route)
        {
            return new ResponseTemplate(Path.Combine(@"Web\Templates", route.Request.LocalPath), "text/css");
        }
    }
}