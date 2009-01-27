using Accipio.Console;
using MbUnit.Framework;

namespace ProjectPilot.Tests.AccipioTests
{
    /// <summary>
    /// Unit tests for class <see cref="TestCodeGeneratorCommand"/>
    /// </summary>
    [TestFixture]
    public class TestCodeGeneratorTests
    {
        /// <summary>
        /// Test checks if Console program correctly parses XML with test cases.
        /// </summary>
        [Test]
        public void ParseTestSpec()
        {
            string[] args = GetArgs();
            TestCodeGeneratorCommand testCodeGeneratorCommand = new TestCodeGeneratorCommand();
            Assert.AreEqual(0, testCodeGeneratorCommand.Execute(args));
        }

        public static string[] GetArgs()
        {
            string[] args = new string[3];
            args[0] = "-ba=..\\..\\..\\Data\\Samples\\BusinessActions.xml";
            args[1] = "-tx=" + TestSuiteSchemaGeneratorTests.GenerateXsdValidationSchemaOutputFile();
            args[2] = "-i=..\\..\\..\\Data\\Samples\\TestSuite.xml";
            return args;
        }

        public static string[] GetArgsWithOption()
        {
            string[] args = new string[5];
            args[0] = "-o=..\\..\\..\\Data\\Samples";
            args[1] = "codegen";
            args[2] = "..\\..\\..\\Data\\Samples\\BusinessActions.xml";
            args[3] = TestSuiteSchemaGeneratorTests.GenerateXsdValidationSchemaOutputFile();
            args[4] = "..\\..\\..\\Data\\Samples\\TestSuite.xml";
            return args;
        }
    }
}