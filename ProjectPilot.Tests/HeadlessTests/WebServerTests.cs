using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using Headless;
using Headless.Configuration;
using Headless.Runner;
using Headless.Web;
using MbUnit.Framework;
using Rhino.Mocks;

namespace ProjectPilot.Tests.HeadlessTests
{
    [TestFixture]
    public class WebServerTests
    {
        [Test, Explicit]
        public void Test()
        {
            HeadlessMother mother = new HeadlessMother();

            IService mockService = MockRepository.GenerateStub<IService>();

            ServiceInfo serviceInfo = new ServiceInfo();
            serviceInfo.ComputerName = "computer";
            serviceInfo.PortNumber = 3434;

            mockService.Expect(s => s.GetServiceInfo()).Return(serviceInfo).Repeat.Any();

            IWebRouteProcessor mainPageProcessor = new MainPageProcessor(mockService, mother.ProjectRegistry);
            IWebRouteProcessor fileWebRouteProcessor = new FileWebRouteProcessor();

            DefaultWebRequestRouter webRequestRouter = new DefaultWebRequestRouter();
            webRequestRouter.AddRoute(@".*\.css$", fileWebRouteProcessor);
            webRequestRouter.AddRoute(@".*", mainPageProcessor);

            IWebRequestProcessor webRequestProcessor = new DefaultWebRequestProcessor(webRequestRouter);

            //string response = webRequestProcessor.ProcessRequest(new Uri("http://localhost:23232/"));

            using (WebServer webServer = new WebServer(serviceInfo, webRequestProcessor))
            {
                webServer.Start();
                Thread.Sleep(TimeSpan.FromSeconds(20));
                webServer.Stop();
            }
        }

        [FixtureSetUp]
        public void FixtureSetup()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
    }
}
