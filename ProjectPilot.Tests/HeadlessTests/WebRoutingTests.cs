﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Headless.Web;
using MbUnit.Framework;
using Rhino.Mocks;

namespace ProjectPilot.Tests.HeadlessTests
{
    [TestFixture]
    public class WebRoutingTests
    {
        [Test, Pending("TODO")]
        public void Test()
        {
            IRouteProcessor routeProcessor1 = MockRepository.GenerateMock<IRouteProcessor>();
            IRouteProcessor routeProcessor2 = MockRepository.GenerateMock<IRouteProcessor>();

            DefaultWebRequestRouter router = new DefaultWebRequestRouter();
            router.AddRoute(
                new WebRequestRoute(
                    new Regex(
                        @"Project/(?<projectid>\w+)$", 
                        RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase), 
                    routeProcessor1));
            router.AddRoute(
                new WebRequestRoute(
                    new Regex(
                        @"Project/(?<projectid>\w+)/Build/(?<buildid>\w+)$",
                        RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase),
                    routeProcessor2));

            RoutedWebRequest routedWebRequest = router.RouteRequest(new Uri(@"http://localhost:9233/headless/Project/Headless"));

            Assert.IsNotNull(routedWebRequest);
            Assert.AreSame(routeProcessor1, routedWebRequest.RouteProcessor);
        }
    }
}
