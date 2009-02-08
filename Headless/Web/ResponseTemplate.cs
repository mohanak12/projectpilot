using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
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
                try
                {
                    template.Merge(velocityContext, writer);
                }
                catch (Exception ex)
                {
                    writer.WriteLine();
                    writer.WriteLine("Error in template '{0}': '{1}'", templateFileName, ex);
                }
            }
        }

        private static readonly ILog log = LogManager.GetLogger(typeof(ResponseTemplate));
        private string templateFileName;
        private readonly string contentType;
        private Dictionary<string, object> templateParameters = new Dictionary<string, object>();
    }
}