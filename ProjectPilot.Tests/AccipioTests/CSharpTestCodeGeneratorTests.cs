using Accipio;
using MbUnit.Framework;
using Rhino.Mocks;

namespace ProjectPilot.Tests.AccipioTests
{
    [TestFixture]
    public class CSharpTestCodeGeneratorTests
    {
        [Test]
        public void GenerateTest()
        {
            ICodeWriter mockCodeWriter = MockRepository.GenerateMock<ICodeWriter>();

            ITestCodeGenerator testCodeGenerator = new CSharpTestCodeGenerator(mockCodeWriter);

            TestSuite testSuite = new TestSuite
            {
                Description = "Class description.",
                Id = "Banking",
                Runner = "OnlineBanking"
            };

            TestCase testCase = new TestCase("ViewAccountTestCase")
            {
                TestCaseDescription = "Tests case description."
            };
            testCase.AddTestAction(new TestAction("GoToPortal"));
            TestAction testAction = new TestAction("SignIn");
            testAction.AddActionParameter(new TestActionParameter("username", "john"));
            testAction.AddActionParameter(new TestActionParameter("password", "doe"));
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
            businessActionData.Actions.Add(businessActionEntry);
            businessActionEntry =
                new BusinessActionEntry("AssertOperationSuccessful")
                {
                    Description = "Assert the operation was successful."
                };
            businessActionData.Actions.Add(businessActionEntry);
            testSuite.BusinessActionData = businessActionData;

            mockCodeWriter.Expect(writer => writer.WriteLine("/// <summary>Class description.</summary>"));
            mockCodeWriter.Expect(writer => writer.WriteLine("public class BankingTestSuite"));
            mockCodeWriter.Expect(writer => writer.WriteLine("{"));
            mockCodeWriter.Expect(writer => writer.WriteLine("    /// <summary>Tests case description.</summary>"));
            mockCodeWriter.Expect(writer => writer.WriteLine("    [Test]"));
            mockCodeWriter.Expect(writer => writer.WriteLine("    [Category(\"Smoke\")]"));
            mockCodeWriter.Expect(writer => writer.WriteLine("    public void ViewAccountTestCase()"));
            mockCodeWriter.Expect(writer => writer.WriteLine("    {"));
            mockCodeWriter.Expect(writer => writer.WriteLine("        using (OnlineBankingTestRunner runner = new OnlineBankingTestRunner())"));
            mockCodeWriter.Expect(writer => writer.WriteLine("        {"));
            mockCodeWriter.Expect(writer => writer.WriteLine("            runner"));
            mockCodeWriter.Expect(writer => writer.WriteLine(string.Empty));
            //mockCodeWriter.Expect(writer => writer.WriteLine("                /// <summary>Open the online banking portal web site in the browser.</summary>"));
            //mockCodeWriter.Expect(writer => writer.WriteLine("                .AddDescription(\"Tests case description.\");"));
            //mockCodeWriter.Expect(writer => writer.WriteLine("                .AddTag (\"R15\")"));
            //mockCodeWriter.Expect(writer => writer.WriteLine("                .AddTag (\"R21.1\")"));
            //mockCodeWriter.Expect(writer => writer.WriteLine(string.Empty));
            mockCodeWriter.Expect(writer => writer.WriteLine("                // Open the online banking portal web site in the browser."));
            mockCodeWriter.Expect(writer => writer.WriteLine("                .GoToPortal()"));
            //mockCodeWriter.Expect(writer => writer.WriteLine(string.Empty));
            mockCodeWriter.Expect(writer => writer.WriteLine("                // Sign in user 'john'."));
            mockCodeWriter.Expect(writer => writer.WriteLine("                .SignIn(\"john\", \"doe\")"));
            //mockCodeWriter.Expect(writer => writer.WriteLine(string.Empty));
            mockCodeWriter.Expect(writer => writer.WriteLine("                // Assert the operation was successful."));
            mockCodeWriter.Expect(writer => writer.WriteLine("                .AssertOperationSuccessful();"));
            mockCodeWriter.Expect(writer => writer.WriteLine("        }"));
            mockCodeWriter.Expect(writer => writer.WriteLine("    }"));
            mockCodeWriter.Expect(writer => writer.WriteLine(string.Empty));
            mockCodeWriter.Expect(writer => writer.WriteLine("}"));

            // execution
            testCodeGenerator.Generate(testSuite);

            // post-conditions
            mockCodeWriter.VerifyAllExpectations();
        }
    }
}