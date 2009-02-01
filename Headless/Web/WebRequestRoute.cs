using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace Headless.Web
{
    public class WebRequestRoute
    {
        public WebRequestRoute(Regex routeRegex, IWebRouteProcessor webRouteProcessor)
        {
            this.webRouteProcessor = webRouteProcessor;
            this.routeRegex = routeRegex;
        }

        public RoutedWebRequest Match(WebRequestData request)
        {
            Match match = routeRegex.Match(request.RequestUrl.ToString());
            if (false == match.Success)
                return null;

            RoutedWebRequest routedWebRequest = new RoutedWebRequest(request, this.webRouteProcessor);

            for (int i = 0; i < match.Groups.Count; i++)
            {
                string propertyName = routeRegex.GroupNameFromNumber(i);
                string propertyValue = match.Groups[propertyName].Value;

                routedWebRequest.RouteParameters.Add(propertyName, propertyValue);
            }

            return routedWebRequest;
        }

        private IWebRouteProcessor webRouteProcessor;
        private Regex routeRegex;
    }
}