using Accipio;
using MbUnit.Framework;
using Rhino.Mocks;
using TestCase=Accipio.TestCase;
using TestSuite=Accipio.TestSuite;

namespace ProjectPilot.Tests.AccipioTests
{
    /// <summary>
    /// Unit test for class <see cref="TestCodeGenerator"/>.
    /// </summary>
    [TestFixture]
    public class CSharpTestCodeGeneratorTests
    {
        [Test]
        public void MoreParametersInTestStep()
        {
            ICodeWriter mockCodeWriter = MockRepository.GenerateMock<ICodeWriter>();

            ITestCodeGenerator testCodeGenerator = new TestCodeGenerator(mockCodeWriter);

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
            testCaseStep.AddActionParameter(new TestActionParameter("topic", "SALA"));
            testCaseStep.AddActionParameter(new TestActionParameter("sequenceNumberInList", "3"));
            testCaseStep.AddActionParameter(new TestActionParameter("topicDescription", "šala dneva"));
            testCaseStep.AddActionParameter(new TestActionParameter("isBool", "true"));
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

            mockCodeWriter.Expect(writer => writer.WriteLine("using MbUnit.Framework;"));
            mockCodeWriter.Expect(writer => writer.WriteLine(string.Empty));
            mockCodeWriter.Expect(writer => writer.WriteLine("namespace Hsl.Ganesha.AcceptanceTests"));
            mockCodeWriter.Expect(writer => writer.WriteLine("{"));
            mockCodeWriter.Expect(writer => writer.WriteLine("    /// <summary>"));
            mockCodeWriter.Expect(writer => writer.WriteLine("    /// Following test suite defines acceptance test cases for Iteration3 (Content Delivery) in MiMi project."));
            mockCodeWriter.Expect(writer => writer.WriteLine("    /// </summary>"));
            mockCodeWriter.Expect(writer => writer.WriteLine("    [TestFixture]"));
            mockCodeWriter.Expect(writer => writer.WriteLine("    public class IT3SuiteTestSuite"));
            mockCodeWriter.Expect(writer => writer.WriteLine("    {"));
            mockCodeWriter.Expect(writer => writer.WriteLine("        /// <summary>"));
            mockCodeWriter.Expect(writer => writer.WriteLine("        /// Subscribe onDemand news to category SALA through SP Gui, where particular topic news does not exists. Check received SMS on phone."));
            mockCodeWriter.Expect(writer => writer.WriteLine("        /// </summary>"));
            mockCodeWriter.Expect(writer => writer.WriteLine("        [Test]"));
            mockCodeWriter.Expect(writer => writer.WriteLine("        [Metadata(\"UserStory\", \"CD.SMS.ImmediateRequest\")]"));
            mockCodeWriter.Expect(writer => writer.WriteLine("        [Parallelizable]"));
            mockCodeWriter.Expect(writer => writer.WriteLine("        public void TopicWithoutContent()"));
            mockCodeWriter.Expect(writer => writer.WriteLine("        {"));
            mockCodeWriter.Expect(
                writer =>
                writer.WriteLine("            using (MiMiTestRunner runner = new MiMiTestRunner())"));
            mockCodeWriter.Expect(writer => writer.WriteLine("            {"));
            mockCodeWriter.Expect(writer => writer.WriteLine("                runner"));
            mockCodeWriter.Expect(
                writer => writer.WriteLine("                    .SetDescription(\"Subscribe onDemand news to category SALA through SP Gui, where particular topic news does not exists. Check received SMS on phone.\")"));
            mockCodeWriter.Expect(writer => writer.WriteLine("                    .AddTag(\"CD.SMS.ImmediateRequest\");"));
            mockCodeWriter.Expect(writer => writer.WriteLine(string.Empty));
            mockCodeWriter.Expect(writer => writer.WriteLine("                runner"));
            mockCodeWriter.Expect(
                writer =>
                writer.WriteLine("                    // Assert each topic has correct description with the content 'SALA' in AllServices list."));
            mockCodeWriter.Expect(writer => writer.WriteLine("                    .AssertTopicDescription(\"SALA\", 3, \"šala dneva\", true);"));
            mockCodeWriter.Expect(writer => writer.WriteLine("            }"));
            mockCodeWriter.Expect(writer => writer.WriteLine("        }"));
            mockCodeWriter.Expect(writer => writer.WriteLine(string.Empty));

            mockCodeWriter.Expect(writer => writer.WriteLine("        /// <summary>"));
            mockCodeWriter.Expect(writer => writer.WriteLine("        /// Test fixture setup code."));
            mockCodeWriter.Expect(writer => writer.WriteLine("        /// </summary>"));
            mockCodeWriter.Expect(writer => writer.WriteLine("        [FixtureSetUp]"));
            mockCodeWriter.Expect(writer => writer.WriteLine("        public void FixtureSetup()"));
            mockCodeWriter.Expect(writer => writer.WriteLine("        {"));
            mockCodeWriter.Expect(
                writer =>
                writer.WriteLine("            Gallio.Framework.Pattern.PatternTestGlobals.DegreeOfParallelism = 10;"));
            mockCodeWriter.Expect(writer => writer.WriteLine("        }"));
            mockCodeWriter.Expect(writer => writer.WriteLine("    }"));
            mockCodeWriter.Expect(writer => writer.WriteLine("}"));

            // execution
            testCodeGenerator.Generate(testSuite);

            // post-conditions
            mockCodeWriter.VerifyAllExpectations();
        }

        [Test, Pending("Working...")]
        public void GenerateTestFromTemplate()
        {
            ICodeWriter mockCodeWriter = MockRepository.GenerateMock<ICodeWriter>();
            TestCodeGenerator csharpTestCodeGenerator = new TestCodeGenerator(mockCodeWriter);
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
            testCaseStep.AddActionParameter(new TestActionParameter("username", "john"));
            testCaseStep.AddActionParameter(new TestActionParameter("password", "doe"));
            testCase.AddStep(testCaseStep);
            testCaseStep = new TestCaseStep("AssertIsUserIdCorrect");
            testCaseStep.AddActionParameter(new TestActionParameter("userId", "1"));
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
            csharpTestCodeGenerator.GenerateFromTemplate(testSuite);
            mockCodeWriter.VerifyAllExpectations();
        }

        /// <summary>
        /// Test checks generation of cs file context for Test spec.
        /// </summary>
        [Test]
        public void GenerateTest()
        {
            ICodeWriter mockCodeWriter = MockRepository.GenerateMock<ICodeWriter>();

            ITestCodeGenerator testCodeGenerator = new TestCodeGenerator(mockCodeWriter);

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
            testCaseStep.AddActionParameter(new TestActionParameter("username", "john"));
            testCaseStep.AddActionParameter(new TestActionParameter("password", "doe"));
            testCase.AddStep(testCaseStep);
            testCaseStep = new TestCaseStep("AssertIsUserIdCorrect");
            testCaseStep.AddActionParameter(new TestActionParameter("userId", "1"));
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

            mockCodeWriter.Expect(writer => writer.WriteLine("using MbUnit.Framework;"));
            mockCodeWriter.Expect(writer => writer.WriteLine(string.Empty));
            mockCodeWriter.Expect(writer => writer.WriteLine("namespace OnlineBankingNamespace"));
            mockCodeWriter.Expect(writer => writer.WriteLine("{"));
            mockCodeWriter.Expect(writer => writer.WriteLine("    /// <summary>"));
            mockCodeWriter.Expect(writer => writer.WriteLine("    /// Class description."));
            mockCodeWriter.Expect(writer => writer.WriteLine("    /// </summary>"));
            mockCodeWriter.Expect(writer => writer.WriteLine("    [TestFixture]"));
            mockCodeWriter.Expect(writer => writer.WriteLine("    public class BankingTestSuite"));
            mockCodeWriter.Expect(writer => writer.WriteLine("    {"));
            mockCodeWriter.Expect(writer => writer.WriteLine("        /// <summary>"));
            mockCodeWriter.Expect(writer => writer.WriteLine("        /// Tests case description."));
            mockCodeWriter.Expect(writer => writer.WriteLine("        /// </summary>"));
            mockCodeWriter.Expect(writer => writer.WriteLine("        [Test]"));
            mockCodeWriter.Expect(writer => writer.WriteLine("        [Metadata(\"UserStory\", \"R15\")]"));
            mockCodeWriter.Expect(writer => writer.WriteLine("        [Metadata(\"UserStory\", \"R21.1\")]"));
            mockCodeWriter.Expect(writer => writer.WriteLine("        [Parallelizable]"));
            mockCodeWriter.Expect(writer => writer.WriteLine("        public void ViewAccountTestCase()"));
            mockCodeWriter.Expect(writer => writer.WriteLine("        {"));
            mockCodeWriter.Expect(
                writer =>
                writer.WriteLine("            using (OnlineBankingTestRunner runner = new OnlineBankingTestRunner())"));
            mockCodeWriter.Expect(writer => writer.WriteLine("            {"));
            mockCodeWriter.Expect(writer => writer.WriteLine("                runner"));
            mockCodeWriter.Expect(
                writer => writer.WriteLine("                    .SetDescription(\"Tests case description.\")"));
            mockCodeWriter.Expect(writer => writer.WriteLine("                    .AddTag(\"R15\")"));
            mockCodeWriter.Expect(writer => writer.WriteLine("                    .AddTag(\"R21.1\");"));
            mockCodeWriter.Expect(writer => writer.WriteLine(string.Empty));
            mockCodeWriter.Expect(writer => writer.WriteLine("                runner"));
            mockCodeWriter.Expect(
                writer =>
                writer.WriteLine("                    // Open the online banking portal web site in the browser."));
            mockCodeWriter.Expect(writer => writer.WriteLine("                    .GoToPortal()"));
            mockCodeWriter.Expect(writer => writer.WriteLine("                    // Sign in user 'john'."));
            mockCodeWriter.Expect(writer => writer.WriteLine("                    .SignIn(\"john\", \"doe\")"));
            mockCodeWriter.Expect(
                writer => writer.WriteLine("                    // Assert if user id is correct."));
            mockCodeWriter.Expect(writer => writer.WriteLine("                    .AssertIsUserIdCorrect(1)"));
            mockCodeWriter.Expect(
                writer => writer.WriteLine("                    // Assert the operation was successful."));
            mockCodeWriter.Expect(writer => writer.WriteLine("                    .AssertOperationSuccessful();"));
            mockCodeWriter.Expect(writer => writer.WriteLine("            }"));
            mockCodeWriter.Expect(writer => writer.WriteLine("        }"));
            mockCodeWriter.Expect(writer => writer.WriteLine(string.Empty));

            mockCodeWriter.Expect(writer => writer.WriteLine("        /// <summary>"));
            mockCodeWriter.Expect(writer => writer.WriteLine("        /// Test fixture setup code."));
            mockCodeWriter.Expect(writer => writer.WriteLine("        /// </summary>"));
            mockCodeWriter.Expect(writer => writer.WriteLine("        [FixtureSetUp]"));
            mockCodeWriter.Expect(writer => writer.WriteLine("        public void FixtureSetup()"));
            mockCodeWriter.Expect(writer => writer.WriteLine("        {"));
            mockCodeWriter.Expect(
                writer =>
                writer.WriteLine("            Gallio.Framework.Pattern.PatternTestGlobals.DegreeOfParallelism = 10;"));
            mockCodeWriter.Expect(writer => writer.WriteLine("        }"));
            mockCodeWriter.Expect(writer => writer.WriteLine("    }"));
            mockCodeWriter.Expect(writer => writer.WriteLine("}"));

            // execution
            testCodeGenerator.Generate(testSuite);

            // post-conditions
            mockCodeWriter.VerifyAllExpectations();
        }
    }
}