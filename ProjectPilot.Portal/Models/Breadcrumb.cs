using System;
using System.Diagnostics.CodeAnalysis;

namespace ProjectPilot.Portal.Models
{
    public class Breadcrumb
    {
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#")]
        public Breadcrumb(string text, string url)
        {
            this.linkText = text;
            this.url = url;
        }

        public string LinkText
        {
            get { return linkText; }
        }

        [SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
        public string Url
        {
            get { return url; }
        }

        private string linkText;
        private string url;
    }
}