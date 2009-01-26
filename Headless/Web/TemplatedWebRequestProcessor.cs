using System;

namespace Headless.Web
{
    public class TemplatedWebRequestProcessor : IWebRequestProcessor
    {
        public TemplatedWebRequestProcessor(IWebRequestRouter router)
        {
            this.router = router;
        }

        public string ProcessRequest(Uri requestUrl)
        {
            RoutedWebRequest request = router.RouteRequest(requestUrl);
            ResponseTemplate responseTemplate = request.Process();
            return responseTemplate.Expand();
        }

        private IWebRequestRouter router;
    }
}