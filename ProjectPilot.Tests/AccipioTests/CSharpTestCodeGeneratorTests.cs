using Accipio;
using MbUnit.Framework;
using Rhino.Mocks;
using TestCase=Accipio.TestCase;
using TestSuite=Accipio.TestSuite;

namespace ProjectPilot.Tests.AccipioTests
{
    /// <summary>
    /// Unit test for class <see cref="CSharpTestCodeGenerator"/>.
    /// </summary>
    [TestFixture]
    public class CSharpTestCodeGeneratorTests
    {
        /// <summary>
        /// Test checks generation of cs file context for Test spec.
        /// </summary>
        [Test]
        public void GenerateTest()
        {
            ICodeWriter mockCodeWriter = MockRepository.GenerateMock<ICodeWriter>();

            ITestCodeGenerator testCodeGenerator = new CSharpTestCodeGenerator(mockCodeWriter);

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
            testCase.AddTestAction(new TestAction("GoToPortal"));
            TestAction testAction = new TestAction("SignIn");
            testAction.AddActionParameter(new TestActionParameter("username", "john"));
            testAction.AddActionParameter(new TestActionParameter("password", "doe"));
            testCase.AddTestAction(testAction);
            testAction = new TestAction("AssertIsUserIdCorrect");
            testAction.AddActionParameter(new TestActionParameter("userId", "1"));
            testCase.AddTestAction(testAction);
            testCase.AddTestAction(new TestAction("AssertOperationSuccessful"));
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