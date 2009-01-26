using System;

namespace Headless.Web
{
    public interface IWebRequestRouter
    {
        RoutedWebRequest RouteRequest(Uri requestUrl);
    }
}