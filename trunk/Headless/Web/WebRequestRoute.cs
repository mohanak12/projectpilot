using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace Headless.Web
{
    public class WebRequestRoute
    {
        public WebRequestRoute(Regex routeRegex, IRouteProcessor routeProcessor)
        {
            this.routeProcessor = routeProcessor;
            this.routeRegex = routeRegex;
        }

        [SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "Headless.Web.RoutedWebRequest")]
        public RoutedWebRequest Match(Uri requestUrl)
        {
            Match match = routeRegex.Match(requestUrl.ToString());
            if (false == match.Success)
                return null;

            RoutedWebRequest routedWebRequest = new RoutedWebRequest();

            //for (int i = 0; i < match.; i++)
            //{
            //}
            foreach (Group group in match.Groups)
            {
                foreach (Capture capture in group.Captures)
                {
                    throw new NotImplementedException();
                }
            }

            throw new NotImplementedException();
        }

        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private IRouteProcessor routeProcessor;
        private Regex routeRegex;
    }
}