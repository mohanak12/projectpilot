using System.IO;
using Accipio;
using MbUnit.Framework;

namespace ProjectPilot.Tests.AccipioTests
{
    [TestFixture]
    public class XmlTestSpecsParserTests
    {
        [Test]
        public void Test()
        {
            TestSpecs testSpecs;
            using (Stream stream = File.OpenRead("..\\..\\..\\Data\\Samples\\TestSpec.xml"))
            {
                XmlTestSpecsParser parser = new XmlTestSpecsParser(stream);
                testSpecs = parser.Parse();
            }


            Assert.AreEqual(4, testSpecs.TestCasesCount);

            TestCase testCase = testSpecs.GetTestCase("OpenHomePage");
            Assert.IsNotNull(testCase);
            Assert.AreEqual("OpenHomePage", testCase.TestCaseName);
            Assert.AreEqual(1, testCase.TestActionsCount);

            TestAction testAction = testCase.GetTestAction("OpenPage");
            Assert.IsNotNull(testAction);
            Assert.AreEqual("OpenPage", testAction.ActionName);
            Assert.IsTrue(testAction.HasParameters, "Action should have 1 parameter!");

            testCase = testSpecs.GetTestCase("SelectProjectEbsy");
            Assert.IsNotNull(testCase);
            Assert.AreEqual(4, testCase.TestActionsCount);
            testAction = testCase.GetTestAction("ClickButton");
            Assert.IsNotNull(testAction);
            Assert.AreEqual(1, testAction.ActionParametersCount);
            Assert.AreEqual("ClickButton", testAction.ActionName);
            Assert.IsTrue(testAction.HasParameters, "Action should have 1 parameter!");
            Assert.IsNotNull(testAction.GetParameterKeyValue("id"));
            Assert.AreEqual("Search", testAction.GetParameterKeyValue("id"));
        }
    }
}