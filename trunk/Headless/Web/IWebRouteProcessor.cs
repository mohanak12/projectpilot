namespace Headless.Web
{
    public interface IWebRouteProcessor
    {
        void Process(RoutedWebRequest route, WebResponseData response);
    }
}