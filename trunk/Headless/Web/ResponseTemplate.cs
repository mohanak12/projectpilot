using System;
using System.Collections.Generic;

namespace Headless.Web
{
    public class ResponseTemplate
    {
        public string TemplateFileName
        {
            get { return templateFileName; }
            set { templateFileName = value; }
        }

        public IDictionary<string, object> TemplateParameters
        {
            get { return templateParameters; }
        }

        public string Expand()
        {
            throw new NotImplementedException();
        }

        private string templateFileName;
        private Dictionary<string, object> templateParameters = new Dictionary<string, object>();
    }
}