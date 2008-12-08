using Accipio.Console;
using MbUnit.Framework;

namespace ProjectPilot.Tests.AccipioTests
{
    [TestFixture]
    public class TestCodeGeneratorCommandTest
    {
        [Test]
        public void GenerateNullArgumentTest()
        {
            IConsoleCommand command = new TestCodeGeneratorCommand(null);

            Assert.IsNull(command.ParseArguments(null));    
        }
    }
}
