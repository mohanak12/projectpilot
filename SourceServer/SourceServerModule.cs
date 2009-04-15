using System;
using System.Diagnostics.CodeAnalysis;
using System.Web;
using Castle.Core.Resource;
using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;
using log4net;

namespace SourceServer
{
    [SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
    public class SourceServerModule : IHttpModule
    {
        public SourceServerModule()
        {
            log4net.Config.XmlConfigurator.Configure();
            this.windsorContainer = new WindsorContainer(new XmlInterpreter(new ConfigResource("castle")));
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(Application_BeginRequest);
        }

        public void Dispose()
        {
            if (windsorContainer != null)
            {
                windsorContainer.Dispose();
                windsorContainer = null;
            }
        }

        private void Application_BeginRequest(object sender, EventArgs e)
        {
            requestProcessor = windsorContainer.Resolve<SourceServerRequestProcessor>();
            try
            {
                HttpContext context = ((HttpApplication)sender).Context;

                string responseText = requestProcessor.ProcessRequest(
                    context.Request.Url, 
                    context.Request.ApplicationPath);

                if (responseText != null)
                {
                    context.Response.Write(responseText);
                    context.Response.End();
                }
            }
            finally
            {
                windsorContainer.Release(requestProcessor);                
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private static readonly ILog log = LogManager.GetLogger(typeof(SourceServerModule));
        private SourceServerRequestProcessor requestProcessor;
        private IWindsorContainer windsorContainer;
    }
}