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

        public ISourceCodeRenderer HighlightedRenderer
        {
            get { return highlightedRenderer; }
            set { highlightedRenderer = value; }
        }

        public ISourceCodeRenderer PlainRenderer
        {
            get { return plainRenderer; }
            set { plainRenderer = value; }
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

            //if (fileType != null)
            //    return RenderHighlightedSourceCode(fileContents, filePath, fileType);
            //else
            return RenderPlainSourceCode(fileContents, path);
        }

        private string RenderFileDoesNotExist()
        {
            return RenderPlainSourceCode(null, path);
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        private string RenderHighlightedSourceCode(string fileContents, string filePath, string fileType)
        {
            return highlightedRenderer.Render(basePath, fileContents, filePath, fileType);
        }

        private string RenderPlainSourceCode(string fileContents, string filePath)
        {
            return plainRenderer.Render(basePath, fileContents, filePath, null);
        }

        private string basePath;
        private IDirectoryRenderer directoryRenderer = new DirectoryRenderer();
        private IFileTypeRecognizer fileTypeRecognizer = new FileTypeRecognizer();
        private ISourceCodeRenderer highlightedRenderer = new HighlightedSourceCodeRenderer();
        private ISourceCodeRenderer plainRenderer = new PlainSourceCodeRenderer();
        private IFileBrowser fileBrowser;
        private string path;
        private PathUrlResolver pathUrlResolver = new PathUrlResolver();
    }
}