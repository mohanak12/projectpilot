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
            
            BusinessActionsRepository businessActionsRepository = new BusinessActionsRepository();
            BusinessAction businessAction = new BusinessAction("NavigateTo");
            businessAction.Description = ActionNavigateToDescription;
            businessActionsRepository.AddAction(businessAction);
            businessAction = new BusinessAction("SelectModule");
            businessAction.Description = ActionSelectModuleDescription;
            businessActionsRepository.AddAction(businessAction);
            businessAction = new BusinessAction("Details");
            businessAction.Description = ActionDetailsDescription;
            businessActionsRepository.AddAction(businessAction);
            testSuite.BusinessActionsRepository = businessActionsRepository;
            
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