using System.IO;
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
        public void ConsoleAppWithOption()
        {
            string outputDir = "test";
            if (Directory.Exists(outputDir))
                Directory.Delete(outputDir, true);

            string[] args = new string[]
                                {
                                    "baschema", 
                                    "-ba=../../AccipioTests/Samples/OnlineBankingBusinessActions.xml", 
                                    "-ns=http://projectpilot/AccipioActions.xsd",
                                    "-o=" + outputDir,
                                };

            RunConsole(args, 0);
        }

        [Test]
        public void ConsoleBusinessActionTest()
        {
            string[] args = new string[]
                                {
                                    "baschema", 
                                    "-ba=../../AccipioTests/Samples/OnlineBankingBusinessActions.xml", 
                                    "-ns=http://projectpilot/AccipioActions.xsd"
                                };

            RunConsole(args, 0);
        }

        [Test]
        public void ConsoleBusinessActionMissingSchemaNamespaceTest()
        {
            string[] args = new string[] { "baschema", @"-ba=..\..\..\Data\Samples\AccipioActions.xml" };

            RunConsole(args, 1);
        }

        [Test]
        public void ConsoleBusinessActionInvalidArgsLengthTest()
        {
            string[] args = new string[] { "baschema" };

            RunConsole(args, 1);
        }

        private void RunConsole (string[] args, int expectedExitCode)
        {
            ConsoleApp consoleApp = new ConsoleApp(args);
            int exitCode = consoleApp.Process();
            Assert.AreEqual(expectedExitCode, exitCode);            
        }
    }
}
