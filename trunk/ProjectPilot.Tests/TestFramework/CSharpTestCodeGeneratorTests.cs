using System.IO;
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
            testCase.AddTestAction(new TestAction("testaction2", "parameter"));
            testSpecs.AddTestCase(testCase);

            testCase = new TestCase("testcase2");
            testCase.AddTestAction(new TestAction("testaction1"));
            testSpecs.AddTestCase(testCase);

            // expectations
            mockCodeWriter.Expect(writer => writer.WriteLine("void testcase1()"));

            // execution
            generator.Generate(testSpecs);

            // post-conditions
            mockCodeWriter.VerifyAllExpectations();

            // now check the file contents
            const string expectedFile = @"void testcase1()
{
    Tester tester = new Tester();
    tester.testaction1();
    tester.testaction2(""parameter"");
}

void testcase2()
{
    Tester tester = new Tester();
    tester.testaction1();
}
";
        }
    }
}