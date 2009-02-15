using System.Diagnostics.CodeAnalysis;
using Headless.Configuration;
using log4net;

namespace Headless.Web
{
    public class MainPageProcessor : TemplatedWebRouteProcessorBase
    {
        public MainPageProcessor(
            IService service,
            IProjectRegistry projectRegistry)
        {
            this.service = service;
            this.projectRegistry = projectRegistry;
        }

        protected override ResponseTemplate GetResponseTemplate(RoutedWebRequest route)
        {
            ResponseTemplate template = new ResponseTemplate(@"Web\Templates\Main.vm.htm", "text/html");

            template.TemplateParameters.Add("registry", projectRegistry);

            ServiceInfo serviceInfo = service.GetServiceInfo();
            template.TemplateParameters.Add("service", serviceInfo);

            return template;
        }

        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private static readonly ILog log = LogManager.GetLogger(typeof(MainPageProcessor));
        private IProjectRegistry projectRegistry;
        private readonly IService service;
    }
}