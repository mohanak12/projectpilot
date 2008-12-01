using Accipio;
using MbUnit.Framework;
using Rhino.Mocks;

namespace ProjectPilot.Tests.AccipioTests
{
    [TestFixture]
    public class HtmlTestCodeGeneratorTests
    {
        /// <summary>
        /// Test checks generation of html file context for Test spec.
        /// </summary>
        /// 
        [Test]
        public void GenerateTestSpec()
        {
            TestSuite testSuite = new TestSuite();

            TestCase testCase = new TestCase("Open Page");
            testCase.AddTestAction(new TestAction("NavigateTo", new TestActionParameter("url", "http://test.aspx")));
            testSuite.AddTestCase(testCase);

            testCase = new TestCase("Select module", "Functional");
            testCase.AddTestAction(new TestAction("SelectModule", new TestActionParameter("name", "Mobi-Info")));
            testSuite.AddTestCase(testCase);

            ICodeWriter mockWriter = MockRepository.GenerateMock<ICodeWriter>();
            ITestCodeGenerator generator = new HtmlTestCodeGenerator(mockWriter);

            mockWriter.Expect(writer => writer.WriteLine("<body>"));
            mockWriter.Expect(writer => writer.WriteLine("<h1>Open Page <i>Smoke</i></h1>"));
            mockWriter.Expect(writer => writer.WriteLine("<i>NavigateTo</i>http://test.aspx<br />"));
            mockWriter.Expect(writer => writer.WriteLine("<h1>Select module <i>Functional</i></h1>"));
            mockWriter.Expect(writer => writer.WriteLine("<i>SelectModule</i>Mobi-Info<br />"));
            mockWriter.Expect(writer => writer.WriteLine("</body>"));

            generator.Generate(testSuite);

            mockWriter.VerifyAllExpectations();
        }
    }
}