using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Headless.Web;
using log4net;

namespace Headless.Runner
{
    public class WebServer : IDisposable
    {
        public WebServer(ServiceInfo serviceInfo, IWebRequestProcessor webRequestProcessor)
        {
            this.serviceInfo = serviceInfo;
            this.webRequestProcessor = webRequestProcessor;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or
        /// resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Start()
        {
            httpListener = new HttpListener();
            httpListener.Prefixes.Add(String.Format(CultureInfo.InvariantCulture, "http://+:{0}/", serviceInfo.PortNumber));
            httpListener.Start();

            IAsyncResult result = httpListener.BeginGetContext(new AsyncCallback(WebRequestCallback), httpListener);
        }

        public void Stop()
        {
            httpListener.Stop();
        }

        /// <summary>
        /// Disposes the object.
        /// </summary>
        /// <param name="disposing">If <code>false</code>, cleans up native resources. 
        /// If <code>true</code> cleans up both managed and native resources</param>
        protected virtual void Dispose(bool disposing)
        {
            if (false == disposed)
            {
                if (disposing)
                {
                    httpListener.Close();
                }

                disposed = true;
            }
        }

        private void WebRequestCallback(IAsyncResult result)
        {
            log.DebugFormat("WebRequestCallback (completed={0})", result.IsCompleted);

            if (false == httpListener.IsListening)
                return;

            // Get out the context object
            HttpListenerContext context = httpListener.EndGetContext(result);

            // *** Immediately set up the next context
            httpListener.BeginGetContext(new AsyncCallback(WebRequestCallback), httpListener);

            WebRequestData request = new WebRequestData();
            request.RequestUrl = context.Request.Url;
            request.LocalPath = context.Request.RawUrl.Substring(1);
            WebResponseData response = new WebResponseData();
            response.OutputStream = context.Response.OutputStream;

            webRequestProcessor.ProcessRequest(request, response);

            context.Response.ContentType = response.ContentType;

            context.Response.OutputStream.Flush();
            context.Response.Close();
        }

        private bool disposed;
        private HttpListener httpListener;
        private static readonly ILog log = LogManager.GetLogger(typeof(WebServer));
        private ServiceInfo serviceInfo;
        private readonly IWebRequestProcessor webRequestProcessor;
    }
}
