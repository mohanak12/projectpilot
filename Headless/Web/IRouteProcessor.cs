namespace Headless.Web
{
    public interface IRouteProcessor
    {
        ResponseTemplate Process(RoutedWebRequest route);
    }
}