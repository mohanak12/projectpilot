using System.Collections.Generic;
using System.IO;
using Accipio;
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
            testCodeGeneratorCommand.ParseArguments(args);
            testCodeGeneratorCommand.ProcessCommand();
        }

        public static string[] GetArgs()
        {
            string[] args = new string[4];
            args[0] = "codegen";
            args[1] = "..\\..\\..\\Data\\Samples\\BusinessActions.xml";
            args[2] = "..\\..\\..\\Accipio.Console\\TestSuiteTemplate.xsd";
            args[3] = "..\\..\\..\\Data\\Samples\\TestSuite.xml";
            return args;
        }
    }
}