using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Flubu.Packaging
{
    public class CopyProcessor : IPackageProcessor
    {
        public CopyProcessor(ILogger logger, ICopier copier, string destinationRootDir)
        {
            this.logger = logger;
            this.copier = copier;
            this.destinationRootDir = destinationRootDir;
        }

        public CopyProcessor AddTransformation(string sourceId, LocalPath destinationDir)
        {
            transformations.Add(sourceId, destinationDir);
            return this;
        }

        public IPackageDef Process(IPackageDef packageDef)
        {
            return (IPackageDef)ProcessPrivate(packageDef, true);
        }

        public void SetFilter(IFileFilter filter)
        {
            this.filter = filter;
        }

        private ICompositeFilesSource ProcessPrivate(
            ICompositeFilesSource compositeFilesSource, 
            bool isRoot)
        {
            CompositeFilesSource transformedCompositeSource;

            if (isRoot)
                transformedCompositeSource = new StandardPackageDef(compositeFilesSource.Id);
            else
                transformedCompositeSource = new CompositeFilesSource(compositeFilesSource.Id);

            foreach (IFilesSource filesSource in compositeFilesSource.ListChildSources())
            {
                if (filesSource is ICompositeFilesSource)
                    throw new NotImplementedException("Child composites are currently not supported");

                FilesList filesList = new FilesList(filesSource.Id);

                LocalPath destinationPath = FindDestinationPathForSource(filesSource.Id);

                foreach (PackagedFileInfo sourceFile in filesSource.ListFiles())
                {
                    if (false == LoggingHelper.LogIfFilteredOut(sourceFile.FullPath.ToString(), filter, logger))
                        continue;

                    FullPath destinationFileName = new FullPath(destinationRootDir);
                    destinationFileName = destinationFileName.CombineWith(destinationPath);

                    if (sourceFile.LocalPath != null)
                        destinationFileName = destinationFileName.CombineWith(sourceFile.LocalPath);
                    else
                    {
                        destinationFileName =
                            destinationFileName.CombineWith(new LocalPath(Path.GetFileName(
                                sourceFile.FullPath.ToString())));
                    }

                    filesList.AddFile(new PackagedFileInfo(destinationFileName));

                    copier.Copy(sourceFile.FullPath.ToString(), destinationFileName.ToString());
                }

                transformedCompositeSource.AddFilesSource(filesList);
            }

            return transformedCompositeSource;
        }

        private LocalPath FindDestinationPathForSource(string sourceId)
        {
            if (false == transformations.ContainsKey(sourceId))
            {
                string message = string.Format(
                    CultureInfo.InvariantCulture,
                    "Source '{0}' is not registered for the transformation",
                    sourceId);
                throw new KeyNotFoundException(message);
            }

            return transformations[sourceId];
        }

        private readonly ILogger logger;
        private readonly ICopier copier;
        private readonly string destinationRootDir;
        private Dictionary<string, LocalPath> transformations = new Dictionary<string, LocalPath>();
        private IFileFilter filter;
    }
}