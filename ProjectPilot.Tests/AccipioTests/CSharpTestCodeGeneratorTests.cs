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
                @"Templates\CSharpMbUnitTestCodeGenerator.vm",
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

            BusinessActionData businessActionData = new BusinessActionData();
            BusinessActionEntry businessActionEntry;
            businessActionEntry =
                            new BusinessActionEntry("AssertTopicDescription")
                            {
                                Description =
                                    "Assert each topic has correct description with the content '{0}' in AllServices list."
                            };
            businessActionEntry.ActionParameters.Add(new BusinessActionParameters("topic", "string"));
            businessActionEntry.ActionParameters.Add(new BusinessActionParameters("sequenceNumberInList", "int"));
            businessActionEntry.ActionParameters.Add(new BusinessActionParameters("topicDescription", "string"));
            businessActionEntry.ActionParameters.Add(new BusinessActionParameters("isBool", "bool"));
            businessActionData.Actions.Add(businessActionEntry);

            testSuite.BusinessActionData = businessActionData;

            // execution
            testCodeGenerator.Generate(testSuite);
        }

        [Test]
        public void GenerateTestFromTemplate()
        {
            ITestCodeGenerator testCodeGenerator = new TemplatedTestCodeGenerator(
                @"Templates\CSharpMbUnitTestCodeGenerator.vm",
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

            BusinessActionData businessActionData = new BusinessActionData();
            BusinessActionEntry businessActionEntry =
                new BusinessActionEntry("GoToPortal")
                {
                    Description =
                        "Open the online banking portal web site in the browser."
                };
            businessActionData.Actions.Add(businessActionEntry);
            businessActionEntry =
                new BusinessActionEntry("SignIn")
                {
                    Description = "Sign in user '{0}'."
                };
            businessActionEntry.ActionParameters.Add(new BusinessActionParameters("username", "string"));
            businessActionEntry.ActionParameters.Add(new BusinessActionParameters("password", "string"));
            businessActionData.Actions.Add(businessActionEntry);
            businessActionEntry =
                new BusinessActionEntry("AssertIsUserIdCorrect")
                {
                    Description = "Assert if user id is correct."
                };
            businessActionEntry.ActionParameters.Add(new BusinessActionParameters("userId", "int"));
            businessActionData.Actions.Add(businessActionEntry);
            businessActionEntry =
                new BusinessActionEntry("AssertOperationSuccessful")
                {
                    Description = "Assert the operation was successful."
                };
            businessActionData.Actions.Add(businessActionEntry);

            testSuite.BusinessActionData = businessActionData; 
            testCodeGenerator.Generate(testSuite);
        }

        /// <summary>
        /// Test checks generation of cs file context for Test spec.
        /// </summary>
        [Test]
        public void GenerateTest()
        {
            ITestCodeGenerator testCodeGenerator = new TemplatedTestCodeGenerator(
                @"Templates\CSharpMbUnitTestCodeGenerator.vm",
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

            BusinessActionData businessActionData = new BusinessActionData();
            BusinessActionEntry businessActionEntry =
                new BusinessActionEntry("GoToPortal")
                    {
                        Description =
                            "Open the online banking portal web site in the browser."
                    };
            businessActionData.Actions.Add(businessActionEntry);
            businessActionEntry =
                new BusinessActionEntry("SignIn")
                    {
                        Description = "Sign in user 'john'."
                    };
            businessActionEntry.ActionParameters.Add(new BusinessActionParameters("username", "string"));
            businessActionEntry.ActionParameters.Add(new BusinessActionParameters("password", "string"));
            businessActionData.Actions.Add(businessActionEntry);
            businessActionEntry =
                new BusinessActionEntry("AssertIsUserIdCorrect")
                {
                    Description = "Assert if user id is correct."
                };
            businessActionEntry.ActionParameters.Add(new BusinessActionParameters("userId", "int"));
            businessActionData.Actions.Add(businessActionEntry);
            businessActionEntry =
                new BusinessActionEntry("AssertOperationSuccessful")
                    {
                        Description = "Assert the operation was successful."
                    };
            businessActionData.Actions.Add(businessActionEntry);

            testSuite.BusinessActionData = businessActionData;

            // execution
            testCodeGenerator.Generate(testSuite);
        }
    }
}