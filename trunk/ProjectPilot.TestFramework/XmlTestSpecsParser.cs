using System.Xml;

namespace ProjectPilot.TestFramework
{
    public class XmlTestSpecsParser : ITestSpecsParser
    {
        public XmlTestSpecsParser(string xmlSpecs)
        {
            this.xmlSpecs = xmlSpecs;
        }

        public TestSpecs Parse()
        {
            TestSpecs testSpecs = new TestSpecs();

            // parse XML document`
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xmlSpecs);

            if (xmlDocument.DocumentElement != null)
                foreach (XmlNode testCaseNode in xmlDocument.DocumentElement.ChildNodes)
                {
                    string testCaseName = testCaseNode.Name;
                    TestCase testCase = new TestCase {TestCaseName = testCaseName};

                    foreach (XmlNode testActionNode in testCaseNode.ChildNodes)
                    {
                        string testActionName = testActionNode.Name;
                        string testActionParameter = testActionNode.InnerText;
                        TestAction testAction = new TestAction
                                                    {
                                                        ActionName = testActionName,
                                                        Parameter = testActionParameter
                                                    };
                        testCase.AddTestAction(testAction);
                    }
                    testSpecs.AddTestCase(testCase);
                }

            return testSpecs;
        }

        private readonly string xmlSpecs;
    }
}