using System;

namespace SourceServer
{
    public interface IDirectoryRenderer
    {
        string RenderDirectory(string basePath, string directoryPath, DirectoryItem[] directoryItems);
    }
}