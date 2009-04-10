namespace SourceServer
{
    public class DirectoryItem
    {
        public DirectoryItem(string path, bool isDirectory)
        {
            this.path = path;
            this.isDirectory = isDirectory;
        }

        public bool IsDirectory
        {
            get { return isDirectory; }
        }

        public string Path
        {
            get { return path; }
        }

        private bool isDirectory;
        private string path;
    }
}