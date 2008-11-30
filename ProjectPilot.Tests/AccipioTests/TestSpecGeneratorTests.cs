using System.Collections.Generic;
using System.IO;
using Accipio;
using Accipio.Console;
using MbUnit.Framework;

namespace ProjectPilot.Tests.AccipioTests
{
    /// <summary>
    /// Unit tests for class <see cref="TestSpecGenerator"/>
    /// </summary>
    [TestFixture]
    public class TestSpecGeneratorTests
    {
        /// <summary>
        /// Validates that input arguments are existing files.
        /// </summary>
        [Test]
        public void ValidateInputArguments()
        {
            string[] args = GetArgs();
            AccipioHelper.CheckForValidInputArguments(args);
        }

        /// <summary>
        /// Validates that invalid input argument throws <see cref="FileNotFoundException"/>.
        /// </summary>
        [Test]
        [ExpectedException(typeof (FileNotFoundException), "blalba\\AccipioActions.xml")]
        public void InvalidArgument()
        {
            string[] args = new string[2];
            args[0] = "TestSpec";
            args[1] = "blalba\\AccipioActions.xml";
            AccipioHelper.CheckForValidInputArguments(args);
        }

        /// <summary>
        /// Test checks if Console program correct parses XML with test cases.
        /// </summary>
        [Test, Pending]
        public void ParseTestSpec()
        {
            string[] args = GetArgs();
            TestSpecGenerator testSpecGenerator = new TestSpecGenerator();
            testSpecGenerator.Parse(args);
            Dictionary<string, TestSpecs> specs = testSpecGenerator.GetTestSpecs;
            Assert.IsNotNull(specs);
            TestSpecs testspSpecs = specs["TestSpec.xml"];
            Assert.AreEqual(4, testspSpecs.TestCasesCount);
            TestCase testCase = testspSpecs.GetTestCase("OpenHomePage");
            Assert.IsNotNull(testCase);
            Assert.AreEqual(1, testCase.TestActionsCount);
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