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
            TestSuite testSuite = new TestSuite
            {
                Id = "TestSuiteId",
                Description = "Test Sute Description."
            };
            
            // Business Action Data
            const string actionNavigateToDescription = "Navigate to url 'http://test.aspx'";
            const string actionSelectModuleDescription = "Select module name 'Mobi-Info'";
            const string actionDetailsDescription = "Select details";
            
            BusinessActionData businessActionData = new BusinessActionData();
            BusinessActionEntry businessActionEntry = new BusinessActionEntry("NavigateTo");
            businessActionEntry.Description = actionNavigateToDescription;
            businessActionData.Actions.Add(businessActionEntry);
            businessActionEntry = new BusinessActionEntry("SelectModule");
            businessActionEntry.Description = actionSelectModuleDescription;
            businessActionData.Actions.Add(businessActionEntry);
            businessActionEntry = new BusinessActionEntry("Details");
            businessActionEntry.Description = actionDetailsDescription;
            businessActionData.Actions.Add(businessActionEntry);
            testSuite.BusinessActionData = businessActionData;
            
            // Test case
            TestCase testCase = new TestCase("Open Page")
            {
                TestCaseDescription = "Open page in web browser"
            };
            TestAction testAction = new TestAction("NavigateTo");
            TestActionParameter testActionParameter = 
                new TestActionParameter("url", "http://test.aspx");
            testAction.AddActionParameter(testActionParameter);
            testCase.AddTestAction(testAction);
            testCase.AddTestAction(new TestAction("SelectModule"));
            testActionParameter = new TestActionParameter("name", "Mobi-Info");
            testAction.AddActionParameter(testActionParameter);
            testCase.AddTestAction(new TestAction("Details"));
            testSuite.AddTestCase(testCase);

            ICodeWriter mockWriter = MockRepository.GenerateMock<ICodeWriter>();
            ITestCodeGenerator generator = new HtmlTestCodeGenerator(mockWriter);

            mockWriter.Expect(writer => writer.WriteLine(@"<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">"));
            mockWriter.Expect(writer => writer.WriteLine(@"<html xmlns=""http://www.w3.org/1999/xhtml"" >"));
            mockWriter.Expect(writer => writer.WriteLine("<head>"));
            mockWriter.Expect(writer => writer.WriteLine("    <title>Test plan</title>"));
            mockWriter.Expect(writer => writer.WriteLine("</head>"));
            mockWriter.Expect(writer => writer.WriteLine("<body>"));
            mockWriter.Expect(writer => writer.WriteLine("    <h1>TestSuiteId</h1>"));
            mockWriter.Expect(writer => writer.WriteLine("    <p>Description : <i>Test Sute Description.</i></p>"));
            mockWriter.Expect(writer => writer.WriteLine("    <h2>Open Page</h2>"));
            mockWriter.Expect(writer => writer.WriteLine("    <p>Category : <i>Smoke</i></p>"));
            mockWriter.Expect(writer => writer.WriteLine("    <p>Description : <i>Open page in web browser</i></p>"));
            mockWriter.Expect(writer => writer.WriteLine("    <ol>"));
            mockWriter.Expect(writer => writer.WriteLine("        <li>Navigate to url 'http://test.aspx'</li>"));
            mockWriter.Expect(writer => writer.WriteLine("        <li>Select module name 'Mobi-Info'</li>"));
            mockWriter.Expect(writer => writer.WriteLine("        <li>Select details</li>"));
            mockWriter.Expect(writer => writer.WriteLine("    </ol>"));
            mockWriter.Expect(writer => writer.WriteLine("</body>"));
            mockWriter.Expect(writer => writer.WriteLine("</html>"));

            generator.Generate(testSuite);

            mockWriter.VerifyAllExpectations();
        }
    }
}