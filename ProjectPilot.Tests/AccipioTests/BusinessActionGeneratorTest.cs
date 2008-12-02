using System.IO;
using Accipio;
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
            consoleCommand.ParseArguments(new string[] { "baschema", fileName });
            consoleCommand.ProcessCommand();
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
    }
}
