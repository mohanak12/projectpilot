using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Commons.Collections;
using log4net;
using NVelocity;
using NVelocity.App;

namespace Headless.Web
{
    public class ResponseTemplate
    {
        public ResponseTemplate(string templateFileName, string contentType)
        {
            this.templateFileName = templateFileName;
            this.contentType = contentType;
        }

        public IDictionary<string, object> TemplateParameters
        {
            get { return templateParameters; }
        }

        public void Expand(WebResponseData response)
        {
            if (log.IsDebugEnabled)
                log.DebugFormat("Expanding template '{0}'", templateFileName);

            VelocityEngine velocity = new VelocityEngine();
            ExtendedProperties props = new ExtendedProperties();
            velocity.Init(props);

            Hashtable contextHashtable = new Hashtable();
            foreach (KeyValuePair<string, object> pair in templateParameters)
                contextHashtable.Add(pair.Key, pair.Value);

            VelocityContext velocityContext = new VelocityContext(contextHashtable);

            Template template = velocity.GetTemplate(templateFileName, new UTF8Encoding(false).WebName);

            response.ContentType = contentType;

            using (StreamWriter writer = new StreamWriter(response.OutputStream))
            {
                template.Merge(velocityContext, writer);
            }
        }

        private static readonly ILog log = LogManager.GetLogger(typeof(ResponseTemplate));
        private string templateFileName;
        private readonly string contentType;
        private Dictionary<string, object> templateParameters = new Dictionary<string, object>();
    }
}