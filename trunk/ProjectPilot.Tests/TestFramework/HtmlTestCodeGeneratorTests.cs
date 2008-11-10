#region

using MbUnit.Framework;
using ProjectPilot.TestFramework;
using Rhino.Mocks;

#endregion

namespace ProjectPilot.Tests.TestFramework
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
			TestSpecs testSpecs = new TestSpecs();

			TestCase testCase = new TestCase("Open Page");
			testCase.AddTestAction(new TestAction("NavigateTo", new TestActionParameter("url", "http://test.aspx")));
			testSpecs.AddTestCase(testCase);

			testCase = new TestCase("Select module");
			testCase.AddTestAction(new TestAction("SelectModule", new TestActionParameter("name", "Mobi-Info")));
			testSpecs.AddTestCase(testCase);

			ICodeWriter mockWriter = MockRepository.GenerateMock<ICodeWriter>();
			ITestCodeGenerator generator = new HtmlTestCodeGenerator(mockWriter);

			mockWriter.Expect(writer => writer.WriteLine("<body>"));
			mockWriter.Expect(writer => writer.WriteLine("<h1>Open Page</h1>"));
			mockWriter.Expect(writer => writer.WriteLine("<i>NavigateTo</i>http://test.aspx<br />"));
			mockWriter.Expect(writer => writer.WriteLine("<h1>Select module</h1>"));
			mockWriter.Expect(writer => writer.WriteLine("<i>SelectModule</i>Mobi-Info<br />"));
			mockWriter.Expect(writer => writer.WriteLine("</body>"));

			generator.Generate(testSpecs);

			mockWriter.VerifyAllExpectations();
		}
	}
}