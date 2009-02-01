using System.IO;

namespace Headless.Web
{
    public class WebResponseData
    {
        public string ContentType
        {
            get { return contentType; }
            set { contentType = value; }
        }

        public Stream OutputStream
        {
            get { return outputStream; }
            set { outputStream = value; }
        }

        private string contentType;
        private Stream outputStream;
    }
}