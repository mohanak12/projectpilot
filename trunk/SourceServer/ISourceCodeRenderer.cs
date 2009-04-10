using System;

namespace SourceServer
{
    public interface ISourceCodeRenderer
    {
        string Render(string basePath, string sourceCode, string fileName, string fileType);
    }
}