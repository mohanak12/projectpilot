using Accipio;
using MbUnit.Framework;
using TestCase=Accipio.TestCase;
using TestSuite=Accipio.TestSuite;

namespace ProjectPilot.Tests.AccipioTests
{
    [TestFixture]
    public class HtmlTestCodeGeneratorTests
    {
        /// <summary>
        /// Test checks generation of html file context for Test spec.
        /// </summary>
        [Test, Pending("Jeza: TODO")]
        public void GenerateTestSpec()
        {
            TestSuite testSuite = new TestSuite("TestSuiteId")
            {
                Description = "Test Sute Description."
            };
            
            // Business Action Data
            const string ActionNavigateToDescription = "Navigate to url '{0}'";
            const string ActionSelectModuleDescription = "Select module name '{0}'";
            const string ActionDetailsDescription = "Select details";
            
            BusinessActionData businessActionData = new BusinessActionData();
            BusinessActionEntry businessActionEntry = new BusinessActionEntry("NavigateTo");
            businessActionEntry.Description = ActionNavigateToDescription;
            businessActionData.Actions.Add(businessActionEntry);
            businessActionEntry = new BusinessActionEntry("SelectModule");
            businessActionEntry.Description = ActionSelectModuleDescription;
            businessActionData.Actions.Add(businessActionEntry);
            businessActionEntry = new BusinessActionEntry("Details");
            businessActionEntry.Description = ActionDetailsDescription;
            businessActionData.Actions.Add(businessActionEntry);
            testSuite.BusinessActionData = businessActionData;
            
            // Test case
            TestCase testCase = new TestCase("Open Page")
            {
                TestCaseDescription = "Open page in web browser"
            };
            TestCaseStep testCaseStep = new TestCaseStep("NavigateTo");
            TestStepParameter testStepParameter = 
                new TestStepParameter("url", "http://test.aspx");
            testCaseStep.AddParameter(testStepParameter);
            testCase.AddStep(testCaseStep);
            testCaseStep = new TestCaseStep("SelectModule");
            testStepParameter = new TestStepParameter("name", "Mobi-Info");
            testCaseStep.AddParameter(testStepParameter);
            testCase.AddStep(testCaseStep);
            testCase.AddStep(new TestCaseStep("Details"));
            testSuite.AddTestCase(testCase);

            ITestCodeGenerator generator = new TemplatedTestCodeGenerator(
                @"Templates/TestRunners/HtmlTestCodeGenerator.vm",
                "TestSuiteId.html");
            generator.Generate(testSuite);
        }
    }
}