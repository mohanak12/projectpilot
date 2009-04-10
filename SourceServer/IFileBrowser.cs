namespace SourceServer
{
    public interface IFileBrowser
    {
        bool IsDirectory(string path);

        DirectoryItem[] ListDirectoryItems(string path);
        
        string ReadFile(string path);
    }
}