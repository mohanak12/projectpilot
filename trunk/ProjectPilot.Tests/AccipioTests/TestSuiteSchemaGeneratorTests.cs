using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Accipio;
using Accipio.Console;
using MbUnit.Framework;

namespace ProjectPilot.Tests.AccipioTests
{
    /// <summary>
    /// This is set of tests that checks if data from business action xml file are properly serialized to object <see cref="BusinessActionsRepository" />
    /// Tests also checks if correct exception returns in case of invalid xml file.
    /// </summary>
    [TestFixture]
    public class TestSuiteSchemaGeneratorTests
    {
        /// <summary>
        /// Test checks generation of xsd schema file context for business actions.
        /// </summary>
        [Test]
        public void GenerateXsdValidationSchemaTest()
        {
            string outputFile = GenerateXsdValidationSchemaOutputFile();
            Assert.IsTrue(File.Exists(outputFile));
        }

        /// <summary>
        /// Generates businessaction schema file.
        /// </summary>
        /// <returns>generated filename.</returns>
        public static string GenerateXsdValidationSchemaOutputFile()
        {
            string fileName = "../../AccipioTests/Samples/OnlineBankingBusinessActions.xml";
            TestSuiteSchemaGeneratorCommand consoleCommand = new TestSuiteSchemaGeneratorCommand();

            string[] args = new[] { "-ba=" + fileName, "-ns=http://GenerateXsdValidationSchemaTest" };

            Assert.AreEqual(0, consoleCommand.Execute(args));

            // get output file
            return consoleCommand.TestSuiteSchemaFileName;
        }

        /// <summary>
        /// Test checks correctness of parsed xml document 
        /// </summary>
        [Test]
        public void ParseBusinessActionsTest()
        {
            using (Stream stream = File.OpenRead("../../AccipioTests/Samples/OnlineBankingBusinessActions.xml"))
            {
                IBusinessActionXmlParser parser = new BusinessActionsXmlParser(stream);
                BusinessActionsRepository businessActionsRepository = parser.Parse();

                // test id of actions
                Assert.AreEqual(businessActionsRepository.EnumerateActions().Count, 8);
                IList<BusinessAction> actions = businessActionsRepository.EnumerateActions();

                int i = 0;
                Assert.AreEqual(actions[i++].ActionName, "AssertOperationSuccessful");
                Assert.AreEqual(actions[i++].ActionName, "ClickActionTransfer");
                Assert.AreEqual(actions[i++].ActionName, "ConfirmTransfer");
                Assert.AreEqual(actions[i++].ActionName, "EnterDestinationAccountNumber");
                Assert.AreEqual(actions[i++].ActionName, "EnterTransferAmount");
                Assert.AreEqual(actions[i++].ActionName, "GoToPortal");
                Assert.AreEqual(actions[i++].ActionName, "SignIn");
                Assert.AreEqual(actions[i++].ActionName, "ViewAccount");
            }
        }

        /// <summary>
        /// Test checks that <see cref="BusinessAction"/> is null if invalid action id is passed
        /// </summary>
        [Test, ExpectedException(typeof(KeyNotFoundException))]
        public void MissingBusinessActionDataEntryTest()
        {
            BusinessActionsRepository businessActionsRepository = new BusinessActionsRepository();
            BusinessAction entry = businessActionsRepository.GetAction("NoAction");
        }

        /// <summary>
        /// Test checks that parser throws exception if xml document has invalid element
        /// </summary>
        [Test, ExpectedException(typeof(NotSupportedException))]
        public void NotSupportedActionsElementTest()
        {
            string xml = "<actions><element></element></actions>";

            byte[] bytes = Encoding.ASCII.GetBytes(xml);
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                IBusinessActionXmlParser parser = new BusinessActionsXmlParser(stream);
                parser.Parse();
            }
        }

        /// <summary>
        /// Test checks that parser throws exception if xml document has invalid element
        /// </summary>
        [Test, ExpectedException(typeof(NotSupportedException))]
        public void NotSupportedActionElementTest()
        {
            string xml = "<actions><action><element></element></action></actions>";

            byte[] bytes = Encoding.ASCII.GetBytes(xml);
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                IBusinessActionXmlParser parser = new BusinessActionsXmlParser(stream);
                parser.Parse();
            }
        }

        /// <summary>
        /// Test checks that parser throws exception if xml document has invalid element
        /// </summary>
        [Test, ExpectedException(typeof(NotSupportedException))]
        public void NotSupportedFunctionElementTest()
        {
            string xml = "<actions><functions></functions></actions>";

            byte[] bytes = Encoding.ASCII.GetBytes(xml);
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                IBusinessActionXmlParser parser = new BusinessActionsXmlParser(stream);
                parser.Parse();
            }
        }

        /// <summary>
        /// Test checks that parser throws exception if xml document has invalid element
        /// </summary>
        [Test, ExpectedException(typeof(XmlException))]
        public void ParseBusinessActionsInvalidXmlTest()
        {
            string xml = "<actionss><action></action></actionss>";

            byte[] bytes = Encoding.ASCII.GetBytes(xml);
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                IBusinessActionXmlParser parser = new BusinessActionsXmlParser(stream);
                parser.Parse();
            }
        }
    }
}
