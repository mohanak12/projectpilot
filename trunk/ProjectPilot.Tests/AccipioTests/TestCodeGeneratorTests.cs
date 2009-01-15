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
            TestCodeGeneratorCommand testCodeGeneratorCommand = new TestCodeGeneratorCommand(null);
            testCodeGeneratorCommand.AccipioDirectory = @"D:\Projects\MiMi\source\Hsl.Ganesha.AcceptanceTests";
            testCodeGeneratorCommand.ParseArguments(args);
            testCodeGeneratorCommand.ProcessCommand();
        }

        public static string[] GetArgs()
        {
            string[] args = new string[4];
            args[0] = "codegen";
            args[1] = "..\\..\\..\\Data\\Samples\\BusinessActions.xml";
            args[2] = TestSuiteSchemaGeneratorTests.GenerateXsdValidationSchemaOutputFile();
            args[3] = "..\\..\\..\\Data\\Samples\\TestSuite.xml";
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