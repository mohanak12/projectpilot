using Accipio.Console;
using MbUnit.Framework;

namespace ProjectPilot.Tests.AccipioTests
{
    /// <summary>
    /// Unit tests for class <see cref="ConsoleApp"/>
    /// </summary>
    [TestFixture]
    public class ConsoleAppTest
    {
        [Test]
        public void ConsoleBusinessActionTest()
        {
            string[] args = new string[] { "baschema", @"..\..\..\Data\Samples\AccipioActions.xml" };

            ConsoleApp consoleApp = new ConsoleApp(args);
            consoleApp.Process();
        }

        [Test]
        public void ConsoleBusinessActionInvalidArgsLengthTest()
        {
            string[] args = new string[] { "baschema" };

            ConsoleApp consoleApp = new ConsoleApp(args);
            consoleApp.Process();
        }

        [Test]
        public void ConsoleBusinessActionArgsNullTest()
        {
            string[] args = null;

            ConsoleApp consoleApp = new ConsoleApp(args);
            consoleApp.Process();
        }

        [Test]
        public void ConsoleCodeGeneratorCommandTest()
        {            
        }
    }
}
