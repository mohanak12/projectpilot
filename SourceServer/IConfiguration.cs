using System;

namespace SourceServer
{
    public interface IConfiguration
    {
        string SourceCodeRootDirectory { get; }
    }
}