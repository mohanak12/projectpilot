using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Flubu.Builds;

namespace Flubu.Builds
{
    public class SimpleBuildProduct<TRunner> : BuildProduct<TRunner>
        where TRunner : FlubuRunner<TRunner>
    {
        public SimpleBuildProduct(
            string buildPartId,
            string sourceDirectory, 
            string productDirectoryName,
            string includePattern,
            string excludePattern) : base(buildPartId)
        {
            this.sourceDirectory = sourceDirectory;
            this.productDirectoryName = productDirectoryName;
            this.includePattern = includePattern;
            this.excludePattern = excludePattern;
        }

        public override void CopyProductFiles(
            TRunner buildRunner,
            string packageDirectory)
        {
            ClearCopiedFilesList();

            string actualExcludePattern = @"\.svn";

            if (excludePattern != null)
            {
                actualExcludePattern = string.Format(
                    CultureInfo.InvariantCulture,
                    "{0}|{1}",
                    actualExcludePattern,
                    excludePattern);
            }

            buildRunner.CopyDirectoryStructure(
                sourceDirectory, 
                Path.Combine(packageDirectory, productDirectoryName), 
                true,
                includePattern,
                actualExcludePattern);

            AddFilesToCopiedList(buildRunner.LastCopiedFilesList);
        }

        private string excludePattern;
        private string includePattern;
        private string productDirectoryName;
        private string sourceDirectory;
    }
}