using System;
using System.Collections.Generic;

namespace Headless.Web
{
    public class DefaultWebRequestRouter : IWebRequestRouter
    {
        public void AddRoute (WebRequestRoute route)
        {
            routes.Add(route);
        }

        public RoutedWebRequest RouteRequest(Uri requestUrl)
        {
            foreach (WebRequestRoute route in routes)
            {
                RoutedWebRequest routedWebRequest = route.Match(requestUrl);
                if (routedWebRequest != null)
                    return routedWebRequest;
            }

            return null;
        }

        private List<WebRequestRoute> routes = new List<WebRequestRoute>();
    }
}