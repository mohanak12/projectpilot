using System;
using System.Collections.Generic;
using System.IO;

namespace Flubu.Packaging
{
    public class DirectorySource : IFilesSource
    {
        public DirectorySource(
            ILogger logger,
            IDirectoryFilesLister directoryFilesLister, 
            string id, 
            string directoryName) : this (logger, directoryFilesLister, id, directoryName, true)
        {
        }

        public DirectorySource(
            ILogger logger,
            IDirectoryFilesLister directoryFilesLister,
            string id,
            string directoryName,
            bool recursive)
        {
            this.logger = logger;
            this.directoryFilesLister = directoryFilesLister;
            this.id = id;
            this.recursive = recursive;
            directoryPath = new FullPath(directoryName);
        }

        public string Id
        {
            get { return id; }
        }

        public ICollection<PackagedFileInfo> ListFiles()
        {
            List<PackagedFileInfo> files = new List<PackagedFileInfo>();

            string directoryPathString = directoryPath.ToString();
            int directoryPathStringLength = directoryPathString.Length;
            if (false == directoryPathString.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal))
                directoryPathStringLength++;

            foreach (string fileName in directoryFilesLister.ListFiles(directoryPathString, recursive))
            {
                if (false == fileName.StartsWith(
                    directoryPathString, 
                    StringComparison.OrdinalIgnoreCase))
                    throw new InvalidOperationException();

                if (false == LoggingHelper.LogIfFilteredOut(fileName, filter, logger))
                    continue;

                LocalPath localPath = new LocalPath(
                    fileName.Substring(directoryPathStringLength));
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
        private bool recursive = true;
    }
}