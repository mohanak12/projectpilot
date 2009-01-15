using System;
using System.IO;
using System.Text;
using System.Xml;
using Accipio;
using MbUnit.Framework;

namespace ProjectPilot.Tests.AccipioTests
{
    /// <summary>
    /// Unit tests for class <see cref="XmlTestSuiteParser"/>.
    /// </summary>
    [TestFixture]
    public class XmlTestSuiteParserTests
    {
        /// <summary>
        /// Test checks that test action does not have parameter with value key.
        /// </summary>
        [Test]
        public void KeyParameterForActionIsNullTest()
        {
            TestAction testAction = new TestAction("TestAction");
            string parameter = testAction.GetParameterKeyValue("key");

            Assert.IsNull(parameter);
        }

        /// <summary>
        /// Test checks that test case does not contain test action with name TestAction.
        /// </summary>
        [Test]
        public void TestActionForTestCaseIsNullTest()
        {
            TestCase testCase = new TestCase("TestCase");
            TestAction testAction = testCase.GetTestAction("TestAction");

            Assert.IsNull(testAction);
        }

        /// <summary>
        /// Test checks correctness of parsed xml document. Input to parser is memory stream.
        /// </summary>
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

            Assert.AreEqual(testSuite.TestSuiteName, "Banking");
            Assert.AreEqual(testSuite.TestRunnerName, "OnlineBanking");
            Assert.AreEqual(testSuite.Namespace, "OnlineBankingNamespace");

            Assert.AreEqual(1, testSuite.TestCasesCount);

            TestCase testCase = testSuite.GetTestCase("MoneyTransfer");
            Assert.IsNotNull(testCase);
            Assert.AreEqual("MoneyTransfer", testCase.TestCaseName);
            Assert.AreEqual(8, testCase.TestSteps.Count);

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
            Assert.IsNotNull(testAction.GetParameterKeyValue("destAccountId"));
            Assert.AreEqual("23677", testAction.GetParameterKeyValue("destAccountId"));

            testAction = testCase.GetTestAction("EnterTransferAmount");
            Assert.IsNotNull(testAction);
            Assert.AreEqual("EnterTransferAmount", testAction.ActionName);
            Assert.AreEqual(1, testAction.ActionParametersCount);
            Assert.IsNotNull(testAction.GetParameterKeyValue("transferAmount"));
            Assert.AreEqual("644.33", testAction.GetParameterKeyValue("transferAmount"));
        }

        /// <summary>
        /// Test checks correctness of parsed xml document. Input to parser is file name.
        /// </summary>
        [Test]
        public void ParseTestSuiteFromFileTest()
        {
            TestSuite testSuite;

            using (XmlTestSuiteParser parser = new XmlTestSuiteParser(@"..\..\..\Data\Samples\TestSuite.xml"))
            {
                testSuite = parser.Parse();
            }

            Assert.AreEqual(testSuite.TestSuiteName, "Banking");
            Assert.AreEqual(testSuite.TestRunnerName, "OnlineBanking");
            Assert.AreEqual(testSuite.Namespace, "OnlineBankingNamespace");

            Assert.AreEqual(1, testSuite.TestCasesCount);

            TestCase testCase = testSuite.GetTestCase("MoneyTransfer");
            Assert.IsNotNull(testCase);
            Assert.AreEqual("MoneyTransfer", testCase.TestCaseName);
            Assert.AreEqual(8, testCase.TestSteps.Count);

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
            Assert.IsNotNull(testAction.GetParameterKeyValue("destAccountId"));
            Assert.AreEqual("23677", testAction.GetParameterKeyValue("destAccountId"));

            testAction = testCase.GetTestAction("EnterTransferAmount");
            Assert.IsNotNull(testAction);
            Assert.AreEqual("EnterTransferAmount", testAction.ActionName);
            Assert.AreEqual(1, testAction.ActionParametersCount);
            Assert.IsNotNull(testAction.GetParameterKeyValue("transferAmount"));
            Assert.AreEqual("644.33", testAction.GetParameterKeyValue("transferAmount"));
        }

        /// <summary>
        /// Test checks that parser throws exception if xml document has invalid root element
        /// </summary>
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

        /// <summary>
        /// Test checks that parser throws exception if xml document has invalid suite element
        /// </summary>
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

        /// <summary>
        /// Test checks that parser throws exception if xml document has invalid test case element
        /// </summary>
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