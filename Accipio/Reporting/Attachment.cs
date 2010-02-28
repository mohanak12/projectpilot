namespace Accipio.Reporting
{
    public class Attachment
    {
        public Attachment(
            string name,
            string contentType,
            string path,
            string contentDisposition,
            string content)
        {
            this.name = name;
            this.contentType = contentType;
            this.path = path;
            this.contentDisposition = contentDisposition;
            this.content = content;
        }

        public string Name
        {
            get { return name; }
        }

        public string ContentType
        {
            get { return contentType; }
        }

        public string Path
        {
            get { return path; }
        }

        public string ContentDisposition
        {
            get { return contentDisposition; }
        }

        public string Content
        {
            get { return content; }
        }

        private readonly string name;
        private readonly string contentType;
        private readonly string path;
        private readonly string contentDisposition;
        private readonly string content;
    }
}