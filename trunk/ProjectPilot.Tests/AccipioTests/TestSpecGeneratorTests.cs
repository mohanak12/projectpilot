using System.IO;
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
            string[] args = new string[4];
            args[0] = "TestSpec";
            args[1] = "..\\..\\..\\Data\\Samples\\AccipioActions.xml";
            args[2] = "..\\..\\..\\Data\\Samples\\AccipioActions.xsd";
            args[3] = "..\\..\\..\\Data\\Samples\\TestSpec.xml";
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
    }
}