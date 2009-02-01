namespace Headless.Web
{
    public abstract class TemplatedWebRouteProcessorBase : IWebRouteProcessor
    {
        public void Process(RoutedWebRequest route, WebResponseData response)
        {
            ResponseTemplate template = GetResponseTemplate(route);
            template.Expand(response);
        }

        protected abstract ResponseTemplate GetResponseTemplate(RoutedWebRequest route);
    }
}