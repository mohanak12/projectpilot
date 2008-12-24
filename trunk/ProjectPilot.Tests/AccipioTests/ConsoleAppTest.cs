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
            string[] args = new string[] { "baschema", @"..\..\..\Data\Samples\BusinessActions.xml", "http://projectpilot/AccipioActions.xsd" };

            ConsoleApp consoleApp = new ConsoleApp(args);
            int exitCode = consoleApp.Process();
            Assert.AreEqual(0, exitCode);
        }

        [Test]
        public void ConsoleBusinessActionMissingSchemaNamespaceTest()
        {
            string[] args = new string[] { "baschema", @"..\..\..\Data\Samples\AccipioActions.xml" };

            ConsoleApp consoleApp = new ConsoleApp(args);
            int exitCode = consoleApp.Process();
            Assert.AreEqual(-1, exitCode);
        }

        [Test]
        public void ConsoleBusinessActionInvalidArgsLengthTest()
        {
            string[] args = new string[] { "baschema" };

            ConsoleApp consoleApp = new ConsoleApp(args);
            int exitCode = consoleApp.Process();
            Assert.AreEqual(-1, exitCode);
        }

        [Test]
        public void ConsoleBusinessActionArgsNullTest()
        {
            string[] args = null;

            ConsoleApp consoleApp = new ConsoleApp(args);
            int exitCode = consoleApp.Process();
            Assert.AreEqual(-1, exitCode);
        }
    }
}
