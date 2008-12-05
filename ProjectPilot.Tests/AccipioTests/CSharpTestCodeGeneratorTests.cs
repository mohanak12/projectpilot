using Accipio;
using MbUnit.Framework;
using Rhino.Mocks;

namespace ProjectPilot.Tests.AccipioTests
{
    [TestFixture]
    public class CSharpTestCodeGeneratorTests
    {
        [Test, Pending("FIx test code generator")]
        public void GenerateTest()
        {
            // setup
            ICodeWriter mockCodeWriter = MockRepository.GenerateMock<ICodeWriter>();

            ITestCodeGenerator generator = new CSharpTestCodeGenerator(mockCodeWriter);

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
            testAction = new TestAction("ViewAccount");
            testAction.AddActionParameter(new TestActionParameter("accountId", "123"));
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
                new BusinessActionEntry("ViewAccount")
                {
                    Description = "Click on the \"View\" button for the account '123'."
                };
            businessActionData.Actions.Add(businessActionEntry);
            businessActionEntry =
                new BusinessActionEntry("AssertOperationSuccessful")
                {
                    Description = "Assert the operation was successful."
                };
            businessActionData.Actions.Add(businessActionEntry);
            testSuite.BusinessActionData = businessActionData;

            //testCase = new TestCase("testcase2", "Regression");
            //testCase.AddTestAction(new TestAction("testaction1"));
            //testSuite.AddTestCase(testCase);

            // expectations
            mockCodeWriter.Expect(writer => writer.WriteLine("/// <summary>Class description.</summary>"));
            mockCodeWriter.Expect(writer => writer.WriteLine("public class BankingTestSuite"));
            mockCodeWriter.Expect(writer => writer.WriteLine("{"));
            mockCodeWriter.Expect(writer => writer.WriteLine("    /// <summary>Tests case description.</summary>"));
            mockCodeWriter.Expect(writer => writer.WriteLine("    [Test]"));
            mockCodeWriter.Expect(writer => writer.WriteLine("    [Category(\"Smoke\")]"));
            mockCodeWriter.Expect(writer => writer.WriteLine("    public void ViewAccountTestCase"));
            mockCodeWriter.Expect(writer => writer.WriteLine("    {"));
            mockCodeWriter.Expect(writer => writer.WriteLine("        using (OnlineBankingTestRunner runner = new OnlineBankingTestRunner())"));
            mockCodeWriter.Expect(writer => writer.WriteLine("        {"));
            mockCodeWriter.Expect(writer => writer.WriteLine("            runner"));
            //mockCodeWriter.Expect(writer => writer.WriteLine("                .AddDescription(\"Tests case description.\");"));
            //mockCodeWriter.Expect(writer => writer.WriteLine("                .AddTag (\"R15\")"));
            //mockCodeWriter.Expect(writer => writer.WriteLine("                .AddTag (\"R21.1\")"));
            mockCodeWriter.Expect(writer => writer.WriteLine(string.Empty));
            mockCodeWriter.Expect(writer => writer.WriteLine("                // Open the online banking portal web site in the browser."));
            mockCodeWriter.Expect(writer => writer.WriteLine("                .GoToPortal()"));
            mockCodeWriter.Expect(writer => writer.WriteLine(string.Empty));
            mockCodeWriter.Expect(writer => writer.WriteLine("                // Sign in user 'john'."));
            mockCodeWriter.Expect(writer => writer.WriteLine("                .SignIn(\"john\", \"doe\")"));
            mockCodeWriter.Expect(writer => writer.WriteLine(string.Empty));
            mockCodeWriter.Expect(writer => writer.WriteLine("                // Click on the \"View\" button for the account '123'."));
            mockCodeWriter.Expect(writer => writer.WriteLine("                .ViewAccount(123)"));
            mockCodeWriter.Expect(writer => writer.WriteLine(string.Empty));
            mockCodeWriter.Expect(writer => writer.WriteLine("                // Assert the operation was successful."));
            mockCodeWriter.Expect(writer => writer.WriteLine("                .AssertOperationSuccessful()"));
            mockCodeWriter.Expect(writer => writer.WriteLine("        }"));
            mockCodeWriter.Expect(writer => writer.WriteLine("    }"));
            mockCodeWriter.Expect(writer => writer.WriteLine(string.Empty));
            //mockCodeWriter.Expect(writer => writer.WriteLine("    /// <summary>Tests case 2 description.</summary>"));
            //mockCodeWriter.Expect(writer => writer.WriteLine("    [Test]"));
            //mockCodeWriter.Expect(writer => writer.WriteLine("    [Category(\"Regression\")]"));
            //mockCodeWriter.Expect(writer => writer.WriteLine("    public void MoneyTransferTestCase()"));
            //mockCodeWriter.Expect(writer => writer.WriteLine("    {"));
            //mockCodeWriter.Expect(writer => writer.WriteLine("        using (OnlineBankingTestRunner runner = new OnlineBankingTestRunner())"));
            //mockCodeWriter.Expect(writer => writer.WriteLine("        {"));
            //mockCodeWriter.Expect(writer => writer.WriteLine("            runner"));
            //mockCodeWriter.Expect(writer => writer.WriteLine("                .AddDescription(\"Tests case 2 description.\");"));
            //mockCodeWriter.Expect(writer => writer.WriteLine("                .AddTag (\"R10\")"));
            //mockCodeWriter.Expect(writer => writer.WriteLine(string.Empty));
            //mockCodeWriter.Expect(writer => writer.WriteLine("                // Open the online banking portal web site in the browser."));
            //mockCodeWriter.Expect(writer => writer.WriteLine("                .GoToPortal()"));
            //mockCodeWriter.Expect(writer => writer.WriteLine(string.Empty));
            //mockCodeWriter.Expect(writer => writer.WriteLine("                // Sign in user 'joe'."));
            //mockCodeWriter.Expect(writer => writer.WriteLine("                .SignIn(\"joe\", \"mock\")"));
            //mockCodeWriter.Expect(writer => writer.WriteLine(string.Empty));
            //mockCodeWriter.Expect(writer => writer.WriteLine("                // Click on the \"Transfer\" button in the account actions list."));
            //mockCodeWriter.Expect(writer => writer.WriteLine("                .ClickActionTranfer()"));
            //mockCodeWriter.Expect(writer => writer.WriteLine(string.Empty));
            //mockCodeWriter.Expect(writer => writer.WriteLine("                // Assert the operation was successful."));
            //mockCodeWriter.Expect(writer => writer.WriteLine("                .AssertOperationSuccessful()"));
            //mockCodeWriter.Expect(writer => writer.WriteLine("        }"));
            //mockCodeWriter.Expect(writer => writer.WriteLine("    }"));
            mockCodeWriter.Expect(writer => writer.WriteLine("}"));

            // execution
            generator.Generate(testSuite);

            // post-conditions
            mockCodeWriter.VerifyAllExpectations();
        }
    }
}