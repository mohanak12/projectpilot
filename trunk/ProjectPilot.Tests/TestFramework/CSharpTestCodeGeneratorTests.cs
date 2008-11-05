using System.IO;
using MbUnit.Framework;
using ProjectPilot.TestFramework;

namespace ProjectPilot.Tests.TestFramework
{
    [TestFixture]
    public class CSharpTestCodeGeneratorTests
    {
        [Test]
        public void Test()
        {
            const string outputFileName = "generatedcode.cs";

            ITestCodeGenerator generator = new CSharpTestCodeGenerator(outputFileName);

            TestSpecs testSpecs = new TestSpecs();

            TestCase testCase = new TestCase("testcase1");
            testCase.AddTestAction(new TestAction("testaction1"));
            testCase.AddTestAction(new TestAction("testaction2", "parameter"));
            testSpecs.AddTestCase(testCase);

            testCase = new TestCase("testcase2");
            testCase.AddTestAction(new TestAction("testaction1"));
            testSpecs.AddTestCase(testCase);

            generator.Generate(testSpecs);

            // now check the file contents
            const string expectedFile = @"
void testcase1()
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
            string fileText;
            using (Stream stream = File.OpenRead(outputFileName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    fileText = reader.ReadToEnd();
                }
            }
            Assert.AreEqual(fileText, expectedFile);
        }
    }
}