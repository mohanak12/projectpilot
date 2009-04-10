using System;
using System.Collections.Generic;
using System.IO;

namespace SourceServer
{
    public class FileBrowser : IFileBrowser
    {
        public FileBrowser(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public bool Exists(string path)
        {
            string fullPath = GetFullPath(path);

            return Directory.Exists(fullPath) || File.Exists(fullPath);
        }

        public bool IsDirectory(string path)
        {
            return Directory.Exists(GetFullPath(path));
        }

        public DirectoryItem[] ListDirectoryItems(string path)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(GetFullPath(path));

            FileSystemInfo[] stringItems = dirInfo.GetFileSystemInfos();
            List<DirectoryItem> directoryItems = new List<DirectoryItem>();

            string rootFullPath = Path.GetFullPath(configuration.SourceCodeRootDirectory);

            foreach (FileSystemInfo fileSystemInfo in stringItems)
            {
                string itemFullPath = Path.Combine(path, fileSystemInfo.FullName);
                bool isDirectory = IsDirectory(itemFullPath);

                string itemRelativePath = itemFullPath.Substring(rootFullPath.Length);

                DirectoryItem item = new DirectoryItem(itemRelativePath, isDirectory);
                directoryItems.Add(item);
            }

            return directoryItems.ToArray();
        }

        public string ReadFile(string path)
        {
            return File.ReadAllText(GetFullPath(path));
        }

        private string GetFullPath(string path)
        {
            return Path.Combine(configuration.SourceCodeRootDirectory, path);
        }

        private readonly IConfiguration configuration;
    }
}