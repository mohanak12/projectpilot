using System;
using System.Diagnostics.CodeAnalysis;
using MbUnit.Framework;
using SourceServer;

namespace ProjectPilot.Tests.SourceServerTests
{
    [TestFixture]
    public class PathUrlResolverTests
    {
        [Test]
        [Row(@"somedir/somedir/somefile.cs", @"http://localhost/SourceServer/somedir/somedir/somefile.cs")]
        [Row(@"somedir/somedir/", @"http://localhost/SourceServer/somedir/somedir/")]
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#")]
        public void Test(string expectedPath, string url)
        {
            PathUrlResolver resolver = new PathUrlResolver();
            string filePath =
                resolver.ResolveUrl(new Uri(url));

            Assert.AreEqual(expectedPath, filePath);
        }
    }
}