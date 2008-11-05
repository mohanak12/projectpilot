using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using ProjectPilot.TestFramework;

namespace ProjectPilot.Tests.TestFramework
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
    <myFirstTestCase>
        <goToHomePage/>
        <selectProject a=""attrA"">Mobi-Info</selectProject>
    </myFirstTestCase>
</TestSpecs>
");

            TestSpecs testSpecs = parser.Parse();

            Assert.AreEqual(1, testSpecs.TestCasesCount);

            TestCase testCase = testSpecs.GetTestCase("myFirstTestCase");
            Assert.IsNotNull(testCase);

            Assert.AreEqual(2, testCase.TestActionsCount);

            Assert.AreEqual("myFirstTestCase", testCase.TestCaseName);

            TestAction testAction = testCase.GetTestAction("goToHomePage");
            Assert.IsNotNull(testAction);
            Assert.AreEqual("goToHomePage", testAction.ActionName);

            testAction = testCase.GetTestAction("selectProject");
            Assert.AreEqual("selectProject", testAction.ActionName);
            Assert.AreEqual("Mobi-Info", testAction.Parameter);
        }
    }
}
