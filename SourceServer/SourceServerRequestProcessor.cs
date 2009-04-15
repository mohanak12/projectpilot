using System;
using System.Diagnostics.CodeAnalysis;

namespace SourceServer
{
    public class SourceServerRequestProcessor
    {
        public SourceServerRequestProcessor(IFileBrowser fileBrowser)
        {
            this.fileBrowser = fileBrowser;
        }

        public IFileTypeRecognizer FileTypeRecognizer
        {
            get { return fileTypeRecognizer; }
            set { fileTypeRecognizer = value; }
        }

        public ISourceCodeRenderer SourceCodeRenderer
        {
            get { return sourceCodeRenderer; }
            set { sourceCodeRenderer = value; }
        }

        public string ProcessRequest(Uri requestUrl, string basePath)
        {
            path = pathUrlResolver.ResolveUrl(requestUrl);
            this.basePath = basePath;

            if (path != null)
            {
                if (false == fileBrowser.Exists(path))
                    return RenderFileDoesNotExist();
                else if (fileBrowser.IsDirectory(path))
                    return RenderDirectory();
                else
                    return RenderFile();
            }

            return null;
        }

        private string RenderDirectory()
        {
            DirectoryItem[] listDirectoryItems = fileBrowser.ListDirectoryItems(path);
            return directoryRenderer.RenderDirectory(basePath, path, listDirectoryItems);
        }

        private string RenderFile()
        {
            string fileContents = fileBrowser.ReadFile(path);
            string fileType = fileTypeRecognizer.RecognizeFileType(path);

            if (fileType != null)
                return RenderSourceCodeFile(fileContents, fileType, path);

            return String.Empty;
        }

        private string RenderFileDoesNotExist()
        {
            return RenderSourceCodeFile(null, null, path);
        }

        private string RenderSourceCodeFile(string fileContents, string fileType, string filePath)
        {
            return sourceCodeRenderer.Render(basePath, fileContents, filePath, fileType);
        }

        private string basePath;
        private IDirectoryRenderer directoryRenderer = new DirectoryRenderer();
        private IFileBrowser fileBrowser;
        private IFileTypeRecognizer fileTypeRecognizer = new FileTypeRecognizer();
        private string path;
        private PathUrlResolver pathUrlResolver = new PathUrlResolver();
        private ISourceCodeRenderer sourceCodeRenderer = new SourceCodeRenderer();
    }
}