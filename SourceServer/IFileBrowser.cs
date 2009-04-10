namespace SourceServer
{
    public interface IFileBrowser
    {
        bool Exists(string path);

        bool IsDirectory(string path);

        DirectoryItem[] ListDirectoryItems(string path);

        string ReadFile(string path);
    }
}