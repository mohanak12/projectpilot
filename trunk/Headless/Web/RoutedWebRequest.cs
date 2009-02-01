using System.Collections.Generic;
using log4net;

namespace Headless.Web
{
    public class RoutedWebRequest
    {
        public RoutedWebRequest(WebRequestData request, IWebRouteProcessor webRouteProcessor)
        {
            this.request = request;
            this.webRouteProcessor = webRouteProcessor;
        }

        public WebRequestData Request
        {
            get { return request; }
        }

        public IDictionary<string, object> RouteParameters
        {
            get { return routeParameters; }
        }

        public IWebRouteProcessor WebRouteProcessor
        {
            get { return webRouteProcessor; }
            set { webRouteProcessor = value; }
        }

        public void Process(WebResponseData response)
        {
            webRouteProcessor.Process(this, response);
        }

        private WebRequestData request;
        private Dictionary<string, object> routeParameters = new Dictionary<string, object>();
        private IWebRouteProcessor webRouteProcessor;
    }
}