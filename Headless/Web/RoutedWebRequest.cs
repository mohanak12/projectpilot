using System.Collections.Generic;

namespace Headless.Web
{
    public class RoutedWebRequest
    {
        public IDictionary<string, object> RouteParameters
        {
            get { return routeParameters; }
        }

        public IRouteProcessor RouteProcessor
        {
            get { return routeProcessor; }
            set { routeProcessor = value; }
        }

        public ResponseTemplate Process()
        {
            return routeProcessor.Process(this);
        }

        private Dictionary<string, object> routeParameters = new Dictionary<string, object>();
        private IRouteProcessor routeProcessor;
    }
}