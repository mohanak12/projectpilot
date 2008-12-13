using System;
using System.IO;
using System.Text;
using System.Xml;
using Accipio;
using MbUnit.Framework;

namespace ProjectPilot.Tests.AccipioTests
{
    [TestFixture]
    public class XmlTestSuiteParserTests
    {
        [Test]
        public void KeyParameterForActionIsNullTest()
        {
            TestAction testAction = new TestAction("TestAction");
            string parameter = testAction.GetParameterKeyValue("key");

            Assert.IsNull(parameter);
        }

        [Test]
        public void TestActionForTestCaseIsNullTest()
        {
            TestCase testCase = new TestCase("TestCase");
            TestAction testAction = testCase.GetTestAction("TestAction");

            Assert.IsNull(testAction);
        }

        [Test]
        public void ParseTestSuite()
        {
            TestSuite testSuite;
            using (Stream stream = File.OpenRead(@"..\..\..\Data\Samples\TestSuite.xml"))
            {
                using (XmlTestSuiteParser parser = new XmlTestSuiteParser(stream))
                {
                    testSuite = parser.Parse();
                }
            }

            Assert.AreEqual(1, testSuite.TestCasesCount);

            TestCase testCase = testSuite.GetTestCase("MoneyTransfer");
            Assert.IsNotNull(testCase);
            Assert.AreEqual("MoneyTransfer", testCase.TestCaseName);
            Assert.AreEqual(8, testCase.TestActionsCount);

            TestAction testAction = testCase.GetTestAction("GoToPortal");
            Assert.IsNotNull(testAction);
            Assert.AreEqual("GoToPortal", testAction.ActionName);
            Assert.IsFalse(testAction.HasParameters, "Action should NOT have parameters!");

            testAction = testCase.GetTestAction("SignIn");
            Assert.IsNotNull(testAction);
            Assert.AreEqual("SignIn", testAction.ActionName);
            Assert.AreEqual(2, testAction.ActionParametersCount);
            Assert.IsTrue(testAction.HasParameters, "Action should have 2 parameters!");
            Assert.IsNotNull(testAction.GetParameterKeyValue("username"));
            Assert.AreEqual("john", testAction.GetParameterKeyValue("username"));
            Assert.IsNotNull(testAction.GetParameterKeyValue("password"));
            Assert.AreEqual("doe", testAction.GetParameterKeyValue("password"));

            testAction = testCase.GetTestAction("EnterDestinationAccountNumber");
            Assert.IsNotNull(testAction);
            Assert.AreEqual("EnterDestinationAccountNumber", testAction.ActionName);
            Assert.AreEqual(1, testAction.ActionParametersCount);
            Assert.IsNotNull(testAction.GetParameterKeyValue("value"));
            Assert.AreEqual("23677", testAction.GetParameterKeyValue("value"));

            testAction = testCase.GetTestAction("EnterTransferAmount");
            Assert.IsNotNull(testAction);
            Assert.AreEqual("EnterTransferAmount", testAction.ActionName);
            Assert.AreEqual(1, testAction.ActionParametersCount);
            Assert.IsNotNull(testAction.GetParameterKeyValue("value"));
            Assert.AreEqual("644.33", testAction.GetParameterKeyValue("value"));
        }

        [Test]
        public void ParseTestSuiteFromFileTest()
        {
            TestSuite testSuite;

            using (XmlTestSuiteParser parser = new XmlTestSuiteParser(@"..\..\..\Data\Samples\TestSuite.xml"))
            {
                testSuite = parser.Parse();
            }
            
            Assert.AreEqual(1, testSuite.TestCasesCount);

            TestCase testCase = testSuite.GetTestCase("MoneyTransfer");
            Assert.IsNotNull(testCase);
            Assert.AreEqual("MoneyTransfer", testCase.TestCaseName);
            Assert.AreEqual(8, testCase.TestActionsCount);

            TestAction testAction = testCase.GetTestAction("GoToPortal");
            Assert.IsNotNull(testAction);
            Assert.AreEqual("GoToPortal", testAction.ActionName);
            Assert.IsFalse(testAction.HasParameters, "Action should NOT have parameters!");

            testAction = testCase.GetTestAction("SignIn");
            Assert.IsNotNull(testAction);
            Assert.AreEqual("SignIn", testAction.ActionName);
            Assert.AreEqual(2, testAction.ActionParametersCount);
            Assert.IsTrue(testAction.HasParameters, "Action should have 2 parameters!");
            Assert.IsNotNull(testAction.GetParameterKeyValue("username"));
            Assert.AreEqual("john", testAction.GetParameterKeyValue("username"));
            Assert.IsNotNull(testAction.GetParameterKeyValue("password"));
            Assert.AreEqual("doe", testAction.GetParameterKeyValue("password"));

            testAction = testCase.GetTestAction("EnterDestinationAccountNumber");
            Assert.IsNotNull(testAction);
            Assert.AreEqual("EnterDestinationAccountNumber", testAction.ActionName);
            Assert.AreEqual(1, testAction.ActionParametersCount);
            Assert.IsNotNull(testAction.GetParameterKeyValue("value"));
            Assert.AreEqual("23677", testAction.GetParameterKeyValue("value"));

            testAction = testCase.GetTestAction("EnterTransferAmount");
            Assert.IsNotNull(testAction);
            Assert.AreEqual("EnterTransferAmount", testAction.ActionName);
            Assert.AreEqual(1, testAction.ActionParametersCount);
            Assert.IsNotNull(testAction.GetParameterKeyValue("value"));
            Assert.AreEqual("644.33", testAction.GetParameterKeyValue("value"));
        }

        [Test, ExpectedException(typeof(XmlException))]
        public void InvalidRootElementTest()
        {
            const string Xml = "<suites></suites>";

            byte[] bytes = Encoding.ASCII.GetBytes(Xml);
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                using (XmlTestSuiteParser parser = new XmlTestSuiteParser(stream))
                {
                    parser.Parse();
                }
            }
        }

        [Test, ExpectedException(typeof(NotSupportedException))]
        public void InvalidSuiteElementTest()
        {
            const string Xml = "<suite><element /></suite>";

            byte[] bytes = Encoding.ASCII.GetBytes(Xml);
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                using (XmlTestSuiteParser parser = new XmlTestSuiteParser(stream))
                {
                    parser.Parse();
                }
            }
        }

        [Test, ExpectedException(typeof(NotSupportedException))]
        public void InvalidTestCaseElementTest()
        {
            const string Xml = "<suite><case id='SearchForProjects' category='Smoke'><element /></case></suite>";

            byte[] bytes = Encoding.ASCII.GetBytes(Xml);
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                using (XmlTestSuiteParser parser = new XmlTestSuiteParser(stream))
                {
                    parser.Parse();
                }
            }
        }
    }
}