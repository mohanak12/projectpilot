using System;
using System.Text.RegularExpressions;
using Headless.Web;
using MbUnit.Framework;
using Rhino.Mocks;

namespace ProjectPilot.Tests.HeadlessTests
{
    [TestFixture]
    public class WebRoutingTests
    {
        [Test]
        public void RouteRequests()
        {
            IWebRouteProcessor webRouteProcessor1 = MockRepository.GenerateMock<IWebRouteProcessor>();
            IWebRouteProcessor webRouteProcessor2 = MockRepository.GenerateMock<IWebRouteProcessor>();

            DefaultWebRequestRouter router = new DefaultWebRequestRouter();
            router.AddRoute(
                new WebRequestRoute(
                    new Regex(
                        @"Project/(?<projectid>\w+)$", 
                        RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase), 
                    webRouteProcessor1));
            router.AddRoute(
                new WebRequestRoute(
                    new Regex(
                        @"Project/(?<projectid>\w+)/Build/(?<buildid>\w+)$",
                        RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase),
                    webRouteProcessor2));

            WebRequestData requestData = new WebRequestData();
            requestData.RequestUrl = new Uri(@"http://localhost:9233/headless/Project/Headless");
            RoutedWebRequest routedWebRequest = router.RouteRequest(requestData);

            Assert.IsNotNull(routedWebRequest);
            Assert.AreSame(webRouteProcessor1, routedWebRequest.WebRouteProcessor);
            Assert.AreEqual("Headless", routedWebRequest.RouteParameters["projectid"]);

            requestData.RequestUrl = new Uri(@"http://localhost:9233/headless/Project/Headless/Build/blabla");
            routedWebRequest = router.RouteRequest(requestData);

            Assert.IsNotNull(routedWebRequest);
            Assert.AreSame(webRouteProcessor2, routedWebRequest.WebRouteProcessor);
            Assert.AreEqual("Headless", routedWebRequest.RouteParameters["projectid"]);
            Assert.AreEqual("blabla", routedWebRequest.RouteParameters["buildid"]);
        }
    }
}
