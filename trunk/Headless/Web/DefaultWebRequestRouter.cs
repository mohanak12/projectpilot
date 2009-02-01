using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using log4net;

namespace Headless.Web
{
    public class DefaultWebRequestRouter : IWebRequestRouter
    {
        public void AddRoute (WebRequestRoute route)
        {
            routes.Add(route);
        }

        public void AddRoute(string routeRegex, IWebRouteProcessor webRouteProcessor)
        {
            routes.Add(new WebRequestRoute(new Regex(routeRegex, RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase), webRouteProcessor));
        }

        public RoutedWebRequest RouteRequest(WebRequestData request)
        {
            if (log.IsDebugEnabled)
                log.DebugFormat("Routing request URL '{0}'", request.RequestUrl);

            foreach (WebRequestRoute route in routes)
            {
                RoutedWebRequest routedWebRequest = route.Match(request);
                if (routedWebRequest != null)
                    return routedWebRequest;
            }

            if (log.IsDebugEnabled)
                log.DebugFormat("No route found for request URL '{0}'", request.RequestUrl);
            return null;
        }

        private static readonly ILog log = LogManager.GetLogger(typeof(DefaultWebRequestRouter));
        private List<WebRequestRoute> routes = new List<WebRequestRoute>();
    }
}