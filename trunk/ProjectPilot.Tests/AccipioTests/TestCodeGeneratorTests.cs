using System.Collections.Generic;
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
            List<string> args = new List<string>();
            args.Add("-ba=..\\..\\..\\Data\\Samples\\BusinessActions.xml");
            args.Add("-tx=" + TestSuiteSchemaGeneratorTests.GenerateXsdValidationSchemaOutputFile());
            args.Add("-i=..\\..\\..\\Data\\Samples\\TestSuite.xml");
            args.Add("-tm=Templates/CSharpMbUnitTestCodeGenerator.vm");
            args.Add("-tmx=.cs");
            TestCodeGeneratorCommand testCodeGeneratorCommand = new TestCodeGeneratorCommand();
            Assert.AreEqual(0, testCodeGeneratorCommand.Execute(args));
        }
    }
}