using System;
using System.IO;
using System.Text;
using System.Xml;
using Accipio;
using MbUnit.Framework;
using TestCase=Accipio.TestCase;
using TestSuite=Accipio.TestSuite;

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
            TestCaseStep testCaseStep = new TestCaseStep("TestAction");
            string parameter = testCaseStep.GetParameterKeyValue("key");

            Assert.IsNull(parameter);
        }

        /// <summary>
        /// Test checks that test case does not contain test action with name TestCaseStep.
        /// </summary>
        [Test]
        public void TestActionForTestCaseIsNullTest()
        {
            TestCase testCase = new TestCase("TestCase");
            TestCaseStep testCaseStep = testCase.GetTestAction("TestAction");

            Assert.IsNull(testCaseStep);
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

            Assert.AreEqual("Banking", testSuite.TestSuiteName);
            Assert.AreEqual("OnlineBanking", testSuite.TestRunnerName);
            Assert.AreEqual("OnlineBankingNamespace", testSuite.Namespace);
            Assert.AreEqual(true, testSuite.IsParallelizable);
            Assert.AreEqual(20, testSuite.DegreeOfParallelism);

            Assert.AreEqual(1, testSuite.TestCasesCount);

            TestCase testCase = testSuite.GetTestCase("MoneyTransfer");
            Assert.IsNotNull(testCase);
            Assert.AreEqual("MoneyTransfer", testCase.TestCaseName);
            Assert.AreEqual(8, testCase.TestSteps.Count);

            TestCaseStep testCaseStep = testCase.GetTestAction("GoToPortal");
            Assert.IsNotNull(testCaseStep);
            Assert.AreEqual("GoToPortal", testCaseStep.ActionName);
            Assert.IsFalse(testCaseStep.HasParameters, "Action should NOT have parameters!");

            testCaseStep = testCase.GetTestAction("SignIn");
            Assert.IsNotNull(testCaseStep);
            Assert.AreEqual("SignIn", testCaseStep.ActionName);
            Assert.AreEqual(2, testCaseStep.ActionParametersCount);
            Assert.IsTrue(testCaseStep.HasParameters, "Action should have 2 parameters!");
            Assert.IsNotNull(testCaseStep.GetParameterKeyValue("username"));
            Assert.AreEqual("john", testCaseStep.GetParameterKeyValue("username"));
            Assert.IsNotNull(testCaseStep.GetParameterKeyValue("password"));
            Assert.AreEqual("doe", testCaseStep.GetParameterKeyValue("password"));

            testCaseStep = testCase.GetTestAction("EnterDestinationAccountNumber");
            Assert.IsNotNull(testCaseStep);
            Assert.AreEqual("EnterDestinationAccountNumber", testCaseStep.ActionName);
            Assert.AreEqual(1, testCaseStep.ActionParametersCount);
            Assert.IsNotNull(testCaseStep.GetParameterKeyValue("destAccountId"));
            Assert.AreEqual("23677", testCaseStep.GetParameterKeyValue("destAccountId"));

            testCaseStep = testCase.GetTestAction("EnterTransferAmount");
            Assert.IsNotNull(testCaseStep);
            Assert.AreEqual("EnterTransferAmount", testCaseStep.ActionName);
            Assert.AreEqual(1, testCaseStep.ActionParametersCount);
            Assert.IsNotNull(testCaseStep.GetParameterKeyValue("transferAmount"));
            Assert.AreEqual("644.33", testCaseStep.GetParameterKeyValue("transferAmount"));
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

            TestCaseStep testCaseStep = testCase.GetTestAction("GoToPortal");
            Assert.IsNotNull(testCaseStep);
            Assert.AreEqual("GoToPortal", testCaseStep.ActionName);
            Assert.IsFalse(testCaseStep.HasParameters, "Action should NOT have parameters!");

            testCaseStep = testCase.GetTestAction("SignIn");
            Assert.IsNotNull(testCaseStep);
            Assert.AreEqual("SignIn", testCaseStep.ActionName);
            Assert.AreEqual(2, testCaseStep.ActionParametersCount);
            Assert.IsTrue(testCaseStep.HasParameters, "Action should have 2 parameters!");
            Assert.IsNotNull(testCaseStep.GetParameterKeyValue("username"));
            Assert.AreEqual("john", testCaseStep.GetParameterKeyValue("username"));
            Assert.IsNotNull(testCaseStep.GetParameterKeyValue("password"));
            Assert.AreEqual("doe", testCaseStep.GetParameterKeyValue("password"));

            testCaseStep = testCase.GetTestAction("EnterDestinationAccountNumber");
            Assert.IsNotNull(testCaseStep);
            Assert.AreEqual("EnterDestinationAccountNumber", testCaseStep.ActionName);
            Assert.AreEqual(1, testCaseStep.ActionParametersCount);
            Assert.IsNotNull(testCaseStep.GetParameterKeyValue("destAccountId"));
            Assert.AreEqual("23677", testCaseStep.GetParameterKeyValue("destAccountId"));

            testCaseStep = testCase.GetTestAction("EnterTransferAmount");
            Assert.IsNotNull(testCaseStep);
            Assert.AreEqual("EnterTransferAmount", testCaseStep.ActionName);
            Assert.AreEqual(1, testCaseStep.ActionParametersCount);
            Assert.IsNotNull(testCaseStep.GetParameterKeyValue("transferAmount"));
            Assert.AreEqual("644.33", testCaseStep.GetParameterKeyValue("transferAmount"));
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