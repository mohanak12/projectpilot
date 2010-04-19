using System.Collections.Generic;
using System.IO;
using Flubu.Packaging;
using MbUnit.Framework;
using Rhino.Mocks;

namespace ProjectPilot.Tests.FlubuTests
{
    public class PackagingTests
    {
        [Test]
        public void TestCopyProcessor()
        {
            CopyProcessor copyProcessor = new CopyProcessor(logger, copier, "dest");

            copyProcessor
                .AddTransformation("console", new LocalPath("Console"))
                .AddTransformation("win.service", new LocalPath("WinService"));

            StandardPackageDef transformedPackage = (StandardPackageDef)copyProcessor.Process(
                package);

            ICollection<PackagedFileInfo> transformedFiles = transformedPackage.ListFiles();
            Assert.AreEqual(6, transformedFiles.Count);
            Assert.Contains(transformedFiles, PackagedFileInfo.FromLocalPath(@"dest\Console\file1.txt"));
            Assert.Contains(transformedFiles, PackagedFileInfo.FromLocalPath(@"dest\WinService\subdir\filex.txt"));

            IList<object[]> calls = copier.GetArgumentsForCallsMadeOn(x => x.Copy(null, null));
            Assert.AreEqual(6, calls.Count);

            Assert.AreEqual(Path.GetFullPath(@"somedir1\file1.txt"), calls[0][0]);
            Assert.AreEqual(Path.GetFullPath(@"dest\Console\file1.txt"), calls[0][1]);
        }

        [Test]
        public void TestZipProcessor()
        {
            IZipper zipper = MockRepository.GenerateMock<IZipper>();
            ZipProcessor zipProcessor = new ZipProcessor(
                logger,
                zipper,
                "some.zip",
                "test",
                null,
                "console", 
                "win.service");
            IPackageDef zippedPackageDef = zipProcessor.Process(package);
            Assert.AreEqual(1, zippedPackageDef.ListChildSources().Count);
        }

        [SetUp]
        private void Setup()
        {
            logger = MockRepository.GenerateStub<ILogger>();

            filesList1 = new[] { @"somedir1\file1.txt", @"somedir1\file2.txt", @"somedir1\file3.txt" };
            filesList2 = new[] { @"somedir2\file1.txt", @"somedir2\file2.txt", @"somedir2\file3.txt", @"somedir2\subdir\filex.txt" };

            ConvertToFullPath(filesList1);
            ConvertToFullPath(filesList2);

            package = new StandardPackageDef();

            directoryFilesLister = MockRepository.GenerateMock<IDirectoryFilesLister>();
            directoryFilesLister.Expect(x => x.ListFiles(
                Path.GetFullPath("somedir1"), true)).Return(filesList1);
            directoryFilesLister.Expect(x => x.ListFiles(
                Path.GetFullPath("somedir2"), true)).Return(filesList2);

            DirectorySource dir1 = new DirectorySource(
                logger,
                directoryFilesLister,
                "console",
                "somedir1");
            dir1.SetFilter(new RegexFileFilter("File3"));
            package.AddFilesSource(dir1);

            DirectorySource dir2 = new DirectorySource(
                logger,
                directoryFilesLister,
                "win.service",
                "somedir2");
            package.AddFilesSource(dir2);

            copier = MockRepository.GenerateMock<ICopier>();
        }

        private static void ConvertToFullPath(string[] paths)
        {
            for (int i = 0; i < paths.Length; i++)
                paths[i] = Path.GetFullPath(paths[i]);
        }

        private string[] filesList1;
        private string[] filesList2;
        private ICopier copier;
        private IDirectoryFilesLister directoryFilesLister;
        private StandardPackageDef package;
        private ILogger logger;        
    }
}