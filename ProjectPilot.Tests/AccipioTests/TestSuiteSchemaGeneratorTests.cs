using System;
using System.IO;
using System.Text;
using System.Xml;
using Accipio;
using Accipio.Console;
using MbUnit.Framework;

namespace ProjectPilot.Tests.AccipioTests
{
    /// <summary>
    /// This is set of tests that checks if data from business action xml file are properly serialized to object <see cref="BusinessActionData" />
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
            string fileName = @"..\..\..\Data\Samples\BusinessActions.xml";
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
            using (Stream stream = File.OpenRead(@"..\..\..\Data\Samples\BusinessActions.xml"))
            {
                IBusinessActionXmlParser parser = new BusinessActionsXmlParser(stream);
                BusinessActionData data = parser.Parse();

                // test id of actions
                Assert.AreEqual(data.Actions.Count, 8);
                Assert.AreEqual(data.Actions[0].ActionId, "GoToPortal");
                Assert.AreEqual(data.Actions[1].ActionId, "SignIn");
                Assert.AreEqual(data.Actions[2].ActionId, "ViewAccount");
                Assert.AreEqual(data.Actions[3].ActionId, "ClickActionTransfer");
                Assert.AreEqual(data.Actions[4].ActionId, "EnterTransferAmount");
                Assert.AreEqual(data.Actions[5].ActionId, "EnterDestinationAccountNumber");
                Assert.AreEqual(data.Actions[6].ActionId, "ConfirmTransfer");
                Assert.AreEqual(data.Actions[7].ActionId, "AssertOperationSuccessful");

                // test functions & steps
                Assert.AreEqual(data.Functions.Count, 1);
                Assert.AreEqual(data.Functions[0].FunctionId, "TransferMoney");
                Assert.AreEqual(data.Functions[0].Steps.Count, 1);

                // test run actions
                Assert.AreEqual(data.Functions[0].Steps[0].RunActions.Count, 5);
                Assert.AreEqual(data.Functions[0].Steps[0].RunActions[0], "ViewAccount");
                Assert.AreEqual(data.Functions[0].Steps[0].RunActions[1], "ClickActionTranfer");
                Assert.AreEqual(data.Functions[0].Steps[0].RunActions[2], "EnterTransferAmount");
                Assert.AreEqual(data.Functions[0].Steps[0].RunActions[3], "EnterDestinationAccountNumber");
                Assert.AreEqual(data.Functions[0].Steps[0].RunActions[4], "ConfirmTransfer");
            }
        }

        /// <summary>
        /// Test checks that <see cref="BusinessActionEntry"/> is null if invalid action id is passed
        /// </summary>
        [Test]
        public void MissingBusinessActionDataEntryTest()
        {
            BusinessActionData data = new BusinessActionData();
            BusinessActionEntry entry = data.GetAction("NoAction");
            Assert.IsNull(entry);
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
            string xml = "<actions><function><element></element></function></actions>";

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
        public void NotSupportedStepsElementTest()
        {
            string xml = "<actions><function><steps><element></element></steps></function></actions>";

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
