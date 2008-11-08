using MbUnit.Framework;
using ProjectPilot.TestFramework;
using Rhino.Mocks;

namespace ProjectPilot.Tests.TestFramework
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

            TestSpecs testSpecs = new TestSpecs();

            TestCase testCase = new TestCase("testcase1");
            testCase.AddTestAction(new TestAction("testaction1"));
			TestAction testAction = new TestAction("testaction2");
			testAction.AddActionParameter(new ActionParameters("name", "parOne"));
			testCase.AddTestAction(testAction);
            testSpecs.AddTestCase(testCase);

            testCase = new TestCase("testcase2");
            testCase.AddTestAction(new TestAction("testaction1"));
            testSpecs.AddTestCase(testCase);

            // expectations
            mockCodeWriter.Expect(writer => writer.WriteLine("void testcase1()"));
            mockCodeWriter.Expect(writer => writer.WriteLine("{"));
            mockCodeWriter.Expect(writer => writer.WriteLine("    Tester tester = new Tester();"));
            mockCodeWriter.Expect(writer => writer.WriteLine("    tester.testaction1();"));
			mockCodeWriter.Expect(writer => writer.WriteLine("    tester.testaction2(\"parOne\");"));
            mockCodeWriter.Expect(writer => writer.WriteLine("}"));
            mockCodeWriter.Expect(writer => writer.WriteLine(string.Empty));
            mockCodeWriter.Expect(writer => writer.WriteLine("void testcase2()"));
            mockCodeWriter.Expect(writer => writer.WriteLine("{"));
            mockCodeWriter.Expect(writer => writer.WriteLine("    Tester tester = new Tester();"));
            mockCodeWriter.Expect(writer => writer.WriteLine("    tester.testaction1();"));
            mockCodeWriter.Expect(writer => writer.WriteLine("}"));

            // execution
            generator.Generate(testSpecs);

            // post-conditions
            mockCodeWriter.VerifyAllExpectations();
        }
    }
}