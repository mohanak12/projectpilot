using System;

namespace ProjectPilot.Portal.Models
{
    public class Breadcrumb
    {
        public Breadcrumb(string text, string url)
        {
            this.linkText = text;
            this.url = url;
        }

        public string LinkText
        {
            get { return linkText; }
        }

        public string Url
        {
            get { return url; }
        }

        private string linkText;
        private string url;
    }
}