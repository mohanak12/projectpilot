using System.IO;
using Accipio.Console;
using MbUnit.Framework;

namespace ProjectPilot.Tests.AccipioTests
{
    [TestFixture]
    public class BusinessActionGeneratorTest
    {
        [Test]
        public void ParseTest()
        {
            string fileName = @"..\..\..\Data\Samples\AccipioActions.xml";

            IConsoleCommand consoleCommand = new BusinessActionsSchemaGeneratorCommand(null);
            consoleCommand.ParseArguments(new string[] { fileName });
        }
    }
}
