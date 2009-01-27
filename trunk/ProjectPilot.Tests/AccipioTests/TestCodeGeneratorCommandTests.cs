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
        public void CodeGeneratorTest()
        {
            string[] args = TestCodeGeneratorTests.GetArgs();
            IConsoleCommand consoleCommand = new TestCodeGeneratorCommand();
            Assert.AreEqual(0, consoleCommand.Execute(args));
        }
    }
}
