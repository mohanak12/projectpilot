using System.Collections.Generic;
using System.IO;
using Flubu.Builds;

namespace Flubu.Builds
{
    public class WebApplicationBuildProduct<TRunner> : BuildProduct<TRunner>
        where TRunner : FlubuRunner<TRunner>
    {
        public WebApplicationBuildProduct(
            string buildPartId,
            string sourceDirectory, 
            string productDirectoryName) : base (buildPartId)
        {
            this.productDirectoryName = productDirectoryName;
            this.sourceDirectory = sourceDirectory;
        }

        public override void CopyProductFiles(
            TRunner buildRunner,
            string packageDirectory)
        {
            ClearCopiedFilesList();

            buildRunner.CopyDirectoryStructure(
                sourceDirectory,
                Path.Combine(packageDirectory, productDirectoryName),
                true,
                null,
                //@"^.*\.(asax|aspx|ascx|asmx|master|css|gif)$",
                @"obj\\|\.svn|\.(cs|snk)$|logs\\");

            AddFilesToCopiedList(buildRunner.LastCopiedFilesList);
        }

        private string productDirectoryName;
        private string sourceDirectory;
    }
}