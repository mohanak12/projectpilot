using System.Diagnostics.CodeAnalysis;

namespace SourceServer
{
    [SuppressMessage("Microsoft.Naming", "CA1724:TypeNamesShouldNotMatchNamespaces")]
    public class Configuration : IConfiguration
    {
        public Configuration(string sourceCodeRootDirectory)
        {
            this.sourceCodeRootDirectory = sourceCodeRootDirectory;
        }

        public string SourceCodeRootDirectory
        {
            get { return sourceCodeRootDirectory; }
            set { sourceCodeRootDirectory = value; }
        }

        private string sourceCodeRootDirectory;
    }
}