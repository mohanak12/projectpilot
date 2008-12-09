using Accipio.Console;
using MbUnit.Framework;

namespace ProjectPilot.Tests.AccipioTests
{
    [TestFixture]
    public class TestCodeGeneratorCommandTest
    {
        [Test]
        public void ArgumentIsNullTest()
        {
            IConsoleCommand command = new TestCodeGeneratorCommand(null);

            Assert.IsNull(command.ParseArguments(null));    
        }

        [Test, Ignore]
        public void CodeGeneratorTest()
        {
            string[] args = new string[]
                                {
                                    "codegen",
                                    @"..\..\..\Data\Samples\BusinessActions.xml",
                                    @"..\..\..\Data\Samples\BusinessActions.xsd",
                                    @"..\..\..\Data\Samples\TestSuite.xml"
                                };

            IConsoleCommand consoleCommand = new TestCodeGeneratorCommand(null).ParseArguments(args);
            consoleCommand.ProcessCommand();
        }
    }
}
