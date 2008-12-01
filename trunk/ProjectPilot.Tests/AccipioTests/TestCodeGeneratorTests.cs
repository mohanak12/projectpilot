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
        [Test, Pending ("The test code has to be refactored")]
        public void ParseTestSpec()
        {
            string[] args = GetArgs();
            TestCodeGeneratorCommand testCodeGeneratorCommand = new TestCodeGeneratorCommand(null);
            testCodeGeneratorCommand.ParseArguments(args);

            //testCodeGeneratorCommand.ProcessCommand();
            //Dictionary<string, TestSuite> specs = testCodeGeneratorCommand.GetTestSpecs;
            //Assert.IsNotNull(specs);
            //TestSuite testSuite = specs["TestSpec.xml"];
            //Assert.AreEqual(4, testSuite.TestCasesCount);
            //TestCase testCase = testSuite.GetTestCase("OpenHomePage");
            //Assert.IsNotNull(testCase);
            //Assert.AreEqual(1, testCase.TestActionsCount);
        }

        private static string[] GetArgs()
        {
            string[] args = new string[4];
            args[0] = "TestSpec";
            args[1] = "..\\..\\..\\Data\\Samples\\AccipioActions.xml";
            args[2] = "..\\..\\..\\Data\\Samples\\AccipioActions.xsd";
            args[3] = "..\\..\\..\\Data\\Samples\\TestSpec.xml";
            return args;
        }
    }
}