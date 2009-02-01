using System;

namespace Headless.Web
{
    public class WebRequestData
    {
        public string LocalPath
        {
            get { return localPath; }
            set { localPath = value; }
        }

        public Uri RequestUrl
        {
            get { return requestUrl; }
            set { requestUrl = value; }
        }

        private string localPath;
        private Uri requestUrl;
    }
}