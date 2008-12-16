using System;
using System.IO;
using Accipio.Console;
using MbUnit.Framework;

namespace ProjectPilot.Tests.AccipioTests
{
    [TestFixture]
    public class TestCodeGeneratorCommandTests
    {
        [Test]
        public void ArgumentIsNullTest()
        {
            IConsoleCommand command = new TestCodeGeneratorCommand(null);

            Assert.IsNull(command.ParseArguments(null));    
        }

        [Test]
        public void CodeGeneratorTest()
        {
            string[] args = TestCodeGeneratorTests.GetArgs();

            IConsoleCommand consoleCommand = new TestCodeGeneratorCommand(null).ParseArguments(args);
            consoleCommand.ProcessCommand();
        }

        /// <summary>
        /// Checks that parser throws file not found if business action file was not found.
        /// </summary>
        [Test, ExpectedException(typeof(IOException))]
        public void MissingBusinessActionFile()
        {
            string[] args = TestCodeGeneratorTests.GetArgs();
            args[1] = "NonExistingFile.xml";
            TestCodeGeneratorCommand testCodeGeneratorCommand = new TestCodeGeneratorCommand(null);
            testCodeGeneratorCommand.ParseArguments(args);
        }

        /// <summary>
        /// Checks that parser throws file not found if XSD shema file for tesdt suite was not found.
        /// </summary>
        [Test, ExpectedException(typeof(IOException))]
        public void MissingXsdSchemaFile()
        {
            string[] args = TestCodeGeneratorTests.GetArgs();
            args[2] = "NonExistingFile.xsd";
            TestCodeGeneratorCommand testCodeGeneratorCommand = new TestCodeGeneratorCommand(null);
            testCodeGeneratorCommand.ParseArguments(args);
        }

        /// <summary>
        /// Passes wrong argument to parser.
        /// </summary>
        [Test]
        public void WrongArgument()
        {
            string[] args = TestCodeGeneratorTests.GetArgs();
            args[0] = "beeeeeeee";
            TestCodeGeneratorCommand testCodeGeneratorCommand = new TestCodeGeneratorCommand(null);
            Assert.IsNull(testCodeGeneratorCommand.ParseArguments(args));
        }

        /// <summary>
        /// Checks that parser throws file not found if business action file was not found.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void WrongNumberOfArguments2()
        {
            string[] args = new string[2];
            args[0] = "codegen";
            args[1] = "..\\..\\..\\Data\\Samples\\BusinessActions.xml"; 
            TestCodeGeneratorCommand testCodeGeneratorCommand = new TestCodeGeneratorCommand(null);
            testCodeGeneratorCommand.ParseArguments(args);
        }

        /// <summary>
        /// Checks that parser throws file not found if business action file was not found.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void WrongNumberOfArguments3()
        {
            string[] args = new string[3];
            args[0] = "codegen";
            args[1] = "..\\..\\..\\Data\\Samples\\BusinessActions.xml";
            args[2] = "..\\..\\..\\Data\\Samples\\TestSuite.xsd";
            TestCodeGeneratorCommand testCodeGeneratorCommand = new TestCodeGeneratorCommand(null);
            testCodeGeneratorCommand.ParseArguments(args);
        }
    }
}
