using System;
using System.IO;
using System.Text;
using System.Xml;
using Accipio;
using Accipio.Console;
using MbUnit.Framework;

namespace ProjectPilot.Tests.AccipioTests
{
    [TestFixture]
    public class BusinessActionGeneratorTest
    {
        [Test]
        public void GenerateXsdValidationSchemaTest()
        {
            string fileName = @"..\..\..\Data\Samples\AccipioActions.xml";
            IConsoleCommand consoleCommand = new BusinessActionsSchemaGeneratorCommand(null);
            string outputFile = ((BusinessActionsSchemaGeneratorCommand)consoleCommand).OutputFile;
            Assert.AreEqual(outputFile, "businessActionValidationSchema.xsd");

            // set output file name
            ((BusinessActionsSchemaGeneratorCommand) consoleCommand).OutputFile = "OutpuXsdFile.xsd";
            consoleCommand.ParseArguments(new string[] { "baschema", fileName });
            consoleCommand.ProcessCommand();

            // get output file
            outputFile = ((BusinessActionsSchemaGeneratorCommand) consoleCommand).OutputFile;
            Assert.IsTrue(File.Exists(outputFile));
        }

        [Test]
        public void ArgsIsNullTest()
        {
            IConsoleCommand consoleCommand = new BusinessActionsSchemaGeneratorCommand(null);
            Assert.IsNull(consoleCommand.ParseArguments(null));
        }

        [Test]
        public void InvalidArgsLengthTest()
        {
            IConsoleCommand consoleCommand = new BusinessActionsSchemaGeneratorCommand(null);
            Assert.IsNull(consoleCommand.ParseArguments(new string[0]));
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void InvalidArgsLengthFirstArgOkTest()
        {
            IConsoleCommand consoleCommand = new BusinessActionsSchemaGeneratorCommand(null);
            Assert.IsNull(consoleCommand.ParseArguments(new string[] { "baschema" }));
        }

        [Test]
        public void InvalidStartArgument()
        {
            IConsoleCommand consoleCommand = new BusinessActionsSchemaGeneratorCommand(null);
            Assert.IsNull(consoleCommand.ParseArguments(new string[] { "test", string.Empty }));
        }

        [Test]
        public void ParseBusinessActionsTest()
        {
            using (Stream stream = File.OpenRead(@"..\..\..\Data\Samples\AccipioActions.xml"))
            {
                IBusinessActionXmlParser parser = new BusinessActionsXmlParser(stream);
                BusinessActionData data = parser.Parse();

                // test id of actions
                Assert.AreEqual(data.Actions.Count, 8);
                Assert.AreEqual(data.Actions[0].ActionId, "GoToPortal");
                Assert.AreEqual(data.Actions[1].ActionId, "SignIn");
                Assert.AreEqual(data.Actions[2].ActionId, "ViewAccount");
                Assert.AreEqual(data.Actions[3].ActionId, "ClickActionTranfer");
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

        [Test]
        public void MissingBusinessActionDataEntryTest()
        {
            BusinessActionData data = new BusinessActionData();
            BusinessActionEntry entry = data.GetAction("NoAction");
            Assert.IsNull(entry);
        }

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
