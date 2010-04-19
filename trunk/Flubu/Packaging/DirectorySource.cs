using System;
using System.Collections.Generic;

namespace Flubu.Packaging
{
    public class DirectorySource : IFilesSource
    {
        public DirectorySource(
            ILogger logger,
            IDirectoryFilesLister directoryFilesLister, 
            string id, 
            string directoryName)
        {
            this.logger = logger;
            this.directoryFilesLister = directoryFilesLister;
            this.id = id;
            this.directoryPath = new FullPath(directoryName);
        }

        public string Id
        {
            get { return id; }
        }

        public ICollection<PackagedFileInfo> ListFiles()
        {
            List<PackagedFileInfo> files = new List<PackagedFileInfo>();

            foreach (string fileName in directoryFilesLister.ListFiles(directoryPath.ToString()))
            {
                if (false == fileName.StartsWith(
                    directoryPath.ToString(), 
                    StringComparison.OrdinalIgnoreCase))
                    throw new InvalidOperationException();

                if (false == LoggingHelper.LogIfFilteredOut(fileName, filter, logger))
                    continue;

                LocalPath localPath = new LocalPath(
                    fileName.Substring(directoryPath.ToString().Length + 1));
                PackagedFileInfo packagedFileInfo = new PackagedFileInfo(new FullPath(fileName), localPath);
                files.Add(packagedFileInfo);
            }

            return files;
        }

        public void SetFilter(IFileFilter filter)
        {
            this.filter = filter;
        }

        private readonly ILogger logger;
        private readonly IDirectoryFilesLister directoryFilesLister;
        private readonly string id;
        private readonly FullPath directoryPath;
        private IFileFilter filter;
    }
}