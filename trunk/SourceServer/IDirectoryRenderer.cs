namespace SourceServer
{
    public interface IDirectoryRenderer
    {
        string RenderDirectory(string directoryPath, DirectoryItem[] directoryItems);
    }
}