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
            XmlTestSpecsParser parser = new XmlTestSpecsParser(
                @"
<TestSpecs>
    <TestCase id='myFirstTestCase'>
        <goToHomePage/>
        <selectProject>Mobi-Info</selectProject>
        <selectModule name='SVN' description='some fancy module'>ignored description</selectModule>
    </TestCase>
    <TestCase id='myFirstTestCase2' category='Smoke'>
        <goToHomePage/>
    </TestCase>
</TestSpecs>
");

            TestSpecs testSpecs = parser.Parse();

            Assert.AreEqual(2, testSpecs.TestCasesCount);

            TestCase testCase = testSpecs.GetTestCase("myFirstTestCase");
            Assert.IsNotNull(testCase);

            Assert.AreEqual(3, testCase.TestActionsCount);

            Assert.AreEqual("myFirstTestCase", testCase.TestCaseName);

            TestAction testAction = testCase.GetTestAction("goToHomePage");
            Assert.IsNotNull(testAction);
            Assert.AreEqual("goToHomePage", testAction.ActionName);
            Assert.IsFalse(testAction.HasParameters, "Action should not contain any parameters!");

            testAction = testCase.GetTestAction("selectProject");
            Assert.AreEqual("selectProject", testAction.ActionName);
            Assert.IsTrue(testAction.HasParameters, "Action should have 1 parameter!");
            Assert.AreEqual(1, testAction.ActionParametersCount);
            Assert.IsNotNull(testAction.GetParameterKeyValue("default"));
            Assert.AreEqual("Mobi-Info", testAction.GetParameterKeyValue("default"));
            Assert.IsNull(testAction.GetParameterKeyValue("name"));

            testAction = testCase.GetTestAction("selectModule");
            Assert.AreEqual("selectModule", testAction.ActionName);
            Assert.AreEqual(2, testAction.ActionParametersCount);
            Assert.IsNotNull(testAction.GetParameterKeyValue("name"));
            Assert.AreEqual("SVN", testAction.GetParameterKeyValue("name"));
            Assert.IsNotNull(testAction.GetParameterKeyValue("description"));
            Assert.AreEqual("some fancy module", testAction.GetParameterKeyValue("description"));
        }
    }
}