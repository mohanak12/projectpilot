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
            // setup
            ICodeWriter mockCodeWriter = MockRepository.GenerateMock<ICodeWriter>();

            ITestCodeGenerator generator = new CSharpTestCodeGenerator(mockCodeWriter);

            TestSuite testSuite = new TestSuite();

            TestCase testCase = new TestCase("testcase1");
            testCase.AddTestAction(new TestAction("testaction1"));
            TestAction testAction = new TestAction("testaction2");
            testAction.AddActionParameter(new TestActionParameter("name", "parOne"));
            testCase.AddTestAction(testAction);
            testSuite.AddTestCase(testCase);

            testCase = new TestCase("testcase2", "Regression");
            testCase.AddTestAction(new TestAction("testaction1"));
            testSuite.AddTestCase(testCase);

            // expectations
            mockCodeWriter.Expect(writer => writer.WriteLine("[Test]"));
            mockCodeWriter.Expect(writer => writer.WriteLine("[Category(\"Smoke\")]"));
            mockCodeWriter.Expect(writer => writer.WriteLine("public void testcase1()"));
            mockCodeWriter.Expect(writer => writer.WriteLine("{"));
            mockCodeWriter.Expect(writer => writer.WriteLine("    Tester tester = new Tester();"));
            mockCodeWriter.Expect(writer => writer.WriteLine("    tester.testaction1();"));
            mockCodeWriter.Expect(writer => writer.WriteLine("    tester.testaction2(\"parOne\");"));
            mockCodeWriter.Expect(writer => writer.WriteLine("}"));
            mockCodeWriter.Expect(writer => writer.WriteLine(string.Empty));
            mockCodeWriter.Expect(writer => writer.WriteLine("[Test]"));
            mockCodeWriter.Expect(writer => writer.WriteLine("[Category(\"Regression\")]"));
            mockCodeWriter.Expect(writer => writer.WriteLine("public void testcase2()"));
            mockCodeWriter.Expect(writer => writer.WriteLine("{"));
            mockCodeWriter.Expect(writer => writer.WriteLine("    Tester tester = new Tester();"));
            mockCodeWriter.Expect(writer => writer.WriteLine("    tester.testaction1();"));
            mockCodeWriter.Expect(writer => writer.WriteLine("}"));

            // execution
            generator.Generate(testSuite);

            // post-conditions
            mockCodeWriter.VerifyAllExpectations();
        }
    }
}