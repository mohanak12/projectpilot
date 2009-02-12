using Accipio;
using MbUnit.Framework;
using TestCase=Accipio.TestCase;
using TestSuite=Accipio.TestSuite;

namespace ProjectPilot.Tests.AccipioTests
{
    /// <summary>
    /// Unit test for class <see cref="TemplatedTestCodeGenerator"/>.
    /// </summary>
    [TestFixture]
    public class CSharpTestCodeGeneratorTests
    {
        [Test]
        public void MoreParametersInTestStep()
        {
            ITestCodeGenerator testCodeGenerator = new TemplatedTestCodeGenerator(
                @"Templates/TestRunners/CSharpMbUnitTestRunner.vm",
                "IT3Suite.cs");

            TestSuite testSuite = new TestSuite("IT3Suite")
            {
                Description = "Following test suite defines acceptance test cases for Iteration3 (Content Delivery) in MiMi project.",
                TestRunnerName = "MiMi",
                Namespace = "Hsl.Ganesha.AcceptanceTests",
                IsParallelizable = true,
            };

            TestCase testCase = new TestCase("TopicWithoutContent")
            {
                TestCaseDescription = "Subscribe onDemand news to category SALA through SP Gui, where particular topic news does not exists. Check received SMS on phone."
            };
            testCase.AddTestCaseTag("CD.SMS.ImmediateRequest");

            TestCaseStep testCaseStep;
            testCaseStep = new TestCaseStep("AssertTopicDescription");
            testCaseStep.AddParameter(new TestStepParameter("topic", "SALA"));
            testCaseStep.AddParameter(new TestStepParameter("sequenceNumberInList", "3"));
            testCaseStep.AddParameter(new TestStepParameter("topicDescription", "šala dneva"));
            testCaseStep.AddParameter(new TestStepParameter("isBool", "true"));
            testCase.AddStep(testCaseStep);
            testSuite.AddTestCase(testCase);

            BusinessActionsRepository businessActionsRepository = new BusinessActionsRepository();
            BusinessAction businessAction;
            businessAction =
                            new BusinessAction("AssertTopicDescription")
                            {
                                Description =
                                    "Assert each topic has correct description with the content '{0}' in AllServices list."
                            };
            businessAction.AddParameter(new BusinessActionParameter("topic", typeof(string), "string", 0));
            businessAction.AddParameter(new BusinessActionParameter("sequenceNumberInList", typeof(int), "integer", 1));
            businessAction.AddParameter(new BusinessActionParameter("topicDescription", typeof(string), "string", 2));
            businessAction.AddParameter(new BusinessActionParameter("isBool", typeof(bool), "boolean", 3));
            businessActionsRepository.AddAction(businessAction);

            testSuite.BusinessActionsRepository = businessActionsRepository;

            // execution
            testCodeGenerator.Generate(testSuite);
        }

        [Test]
        public void GenerateTestFromTemplate()
        {
            ITestCodeGenerator testCodeGenerator = new TemplatedTestCodeGenerator(
                @"Templates/TestRunners/CSharpMbUnitTestRunner.vm",
                "Banking.cs");

            TestSuite testSuite = new TestSuite("Banking")
            {
                Description = "Class description.",
                TestRunnerName = "OnlineBanking",
                Namespace = "OnlineBankingNamespace",
                IsParallelizable = true,
            };

            TestCase testCase = new TestCase("ViewAccountTestCase")
            {
                TestCaseDescription = "Tests case description."
            };

            testCase.AddTestCaseTag("R15");
            testCase.AddTestCaseTag("R21.1");
            testCase.AddStep(new TestCaseStep("GoToPortal"));
            
            TestCaseStep testCaseStep = new TestCaseStep("SignIn");
            testCaseStep.AddParameter(new TestStepParameter("username", "john"));
            testCaseStep.AddParameter(new TestStepParameter("password", "doe"));
            testCase.AddStep(testCaseStep);
            
            testCaseStep = new TestCaseStep("AssertIsUserIdCorrect");
            testCaseStep.AddParameter(new TestStepParameter("userId", "1"));
            testCase.AddStep(testCaseStep);
            
            testCase.AddStep(new TestCaseStep("AssertOperationSuccessful"));
            testSuite.AddTestCase(testCase);

            BusinessActionsRepository businessActionsRepository = new BusinessActionsRepository();
            BusinessAction businessAction =
                new BusinessAction("GoToPortal")
                {
                    Description =
                        "Open the online banking portal web site in the browser."
                };
            businessActionsRepository.AddAction(businessAction);
            businessAction =
                new BusinessAction("SignIn")
                {
                    Description = "Sign in user '{0}'."
                };
            businessAction.AddParameter(new BusinessActionParameter("username", typeof(string), "string", 0));
            businessAction.AddParameter(new BusinessActionParameter("password", typeof(string), "string", 1));
            businessActionsRepository.AddAction(businessAction);
            businessAction =
                new BusinessAction("AssertIsUserIdCorrect")
                {
                    Description = "Assert if user id is correct."
                };
            businessAction.AddParameter(new BusinessActionParameter("userId", typeof(int), "integer", 0));
            businessActionsRepository.AddAction(businessAction);
            businessAction =
                new BusinessAction("AssertOperationSuccessful")
                {
                    Description = "Assert the operation was successful."
                };
            businessActionsRepository.AddAction(businessAction);

            testSuite.BusinessActionsRepository = businessActionsRepository; 
            testCodeGenerator.Generate(testSuite);
        }

        /// <summary>
        /// Test checks generation of cs file context for Test spec.
        /// </summary>
        [Test]
        public void GenerateTest()
        {
            ITestCodeGenerator testCodeGenerator = new TemplatedTestCodeGenerator(
                @"Templates/TestRunners/CSharpMbUnitTestRunner.vm",
                "Banking.cs");

            TestSuite testSuite = new TestSuite("Banking")
                                      {
                                          Description = "Class description.",
                                          TestRunnerName = "OnlineBanking",
                                          Namespace = "OnlineBankingNamespace",
                                          IsParallelizable = true,
                                      };

            TestCase testCase = new TestCase("ViewAccountTestCase")
                                    {
                                        TestCaseDescription = "Tests case description."
                                    };
            testCase.AddTestCaseTag("R15");
            testCase.AddTestCaseTag("R21.1");
            testCase.AddStep(new TestCaseStep("GoToPortal"));
            TestCaseStep testCaseStep = new TestCaseStep("SignIn");
            testCaseStep.AddParameter(new TestStepParameter("username", "john"));
            testCaseStep.AddParameter(new TestStepParameter("password", "doe"));
            testCase.AddStep(testCaseStep);
            testCaseStep = new TestCaseStep("AssertIsUserIdCorrect");
            testCaseStep.AddParameter(new TestStepParameter("userId", "1"));
            testCase.AddStep(testCaseStep);
            testCase.AddStep(new TestCaseStep("AssertOperationSuccessful"));
            testSuite.AddTestCase(testCase);

            BusinessActionsRepository businessActionsRepository = new BusinessActionsRepository();
            BusinessAction businessAction =
                new BusinessAction("GoToPortal")
                    {
                        Description =
                            "Open the online banking portal web site in the browser."
                    };
            businessActionsRepository.AddAction(businessAction);
            businessAction =
                new BusinessAction("SignIn")
                    {
                        Description = "Sign in user 'john'."
                    };
            businessAction.AddParameter(new BusinessActionParameter("username", typeof(string), "string", 0));
            businessAction.AddParameter(new BusinessActionParameter("password", typeof(string), "string", 1));
            businessActionsRepository.AddAction(businessAction);
            businessAction =
                new BusinessAction("AssertIsUserIdCorrect")
                {
                    Description = "Assert if user id is correct."
                };
            businessAction.AddParameter(new BusinessActionParameter("userId", typeof(int), "integer", 0));
            businessActionsRepository.AddAction(businessAction);
            businessAction =
                new BusinessAction("AssertOperationSuccessful")
                    {
                        Description = "Assert the operation was successful."
                    };
            businessActionsRepository.AddAction(businessAction);

            testSuite.BusinessActionsRepository = businessActionsRepository;

            // execution
            testCodeGenerator.Generate(testSuite);
        }

        /// <summary>
        /// Tests loading of templates from location different from the default one.
        /// </summary>
        [Test]
        public void LoadTemplateFromDifferentLocation()
        {
            ITestCodeGenerator testCodeGenerator = new TemplatedTestCodeGenerator(
                @"../../../Accipio/Templates/TestRunners/CSharpMbUnitTestRunner.vm",
                "Banking.cs");      
            testCodeGenerator.Generate(new TestSuite("test"));
        }
    }
}