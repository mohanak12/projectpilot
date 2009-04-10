using System;
using System.IO;
using MbUnit.Framework;
using Rhino.Mocks;
using SourceServer;

namespace ProjectPilot.Tests.SourceServerTests
{
    [TestFixture]
    public class RequestProcessingTests
    {
        [Test, Pending]
        public void Test()
        {
            const string FileContents =
                @"using MbUnit.Framework;
using Rhino.Mocks;
using SourceServer;

namespace ProjectPilot.Tests.SourceServerTests
{
    [TestFixture]
    public class RequestProcessingTests
    {
";

            const string ExpectedOutput =
                @"<html><body><pre>    1  using MbUnit.Framework;
    2  using Rhino.Mocks;
    3  using SourceServer;

    5  namespace ProjectPilot.Tests.SourceServerTests
    6  {
    7      [TestFixture]
    8      public class RequestProcessingTests
    9      {
</pre></body></html>";

            IFileBrowser fileBrowser = MockRepository.GenerateStub<IFileBrowser>();
            fileBrowser.Expect(p => p.ReadFile("file.cs")).Return(FileContents);

            SourceServerRequestProcessor processor = new SourceServerRequestProcessor(fileBrowser);

            Uri requestUrl = new Uri("file.cs");
            string reponseHtml = processor.ProcessRequest(requestUrl, "/VDF");

            File.WriteAllText("code.html", reponseHtml);

            Assert.AreEqual(ExpectedOutput, reponseHtml);
        }

        [Test]
        public void RenderLongerFile()
        {
            Configuration configuration = new Configuration("../../..");
            FileBrowser fileBrowser = new FileBrowser(configuration);

            SourceServerRequestProcessor processor = new SourceServerRequestProcessor(fileBrowser);

            Uri requestUrl = new Uri(@"http://localhost/VDF/Accipio/BusinessActionsXmlParser.cs");
            string reponseHtml = processor.ProcessRequest(
                requestUrl, 
                "/VDF");

            File.WriteAllText("code.html", reponseHtml);            
        }

        [Test]
        public void RenderDirectory()
        {
            Configuration configuration = new Configuration("../../..");
            FileBrowser fileBrowser = new FileBrowser(configuration);

            SourceServerRequestProcessor processor = new SourceServerRequestProcessor(fileBrowser);

            Uri requestUrl = new Uri(@"http://localhost/VDF/Accipio/");
            string reponseHtml = processor.ProcessRequest(
                requestUrl,
                "/VDF");

            File.WriteAllText("code.html", reponseHtml);
        }
    }
}