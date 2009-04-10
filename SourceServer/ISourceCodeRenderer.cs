namespace SourceServer
{
    public interface ISourceCodeRenderer
    {
        string Render(string sourceCode, string fileName, string fileType);
    }
}