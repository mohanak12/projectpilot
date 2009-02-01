using System;

namespace Headless.Web
{
    public class DefaultWebRequestProcessor : IWebRequestProcessor
    {
        public DefaultWebRequestProcessor(IWebRequestRouter router)
        {
            this.router = router;
        }

        public void ProcessRequest(WebRequestData request, WebResponseData response)
        {
            RoutedWebRequest routedRequest = router.RouteRequest(request);
            routedRequest.Process(response);
        }

        private IWebRequestRouter router;
    }
}