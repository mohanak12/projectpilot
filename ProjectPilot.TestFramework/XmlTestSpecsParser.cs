using System;
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
            {
                foreach (XmlNode testCaseNode in xmlDocument.DocumentElement.ChildNodes)
                {
                    //<TestCase id='myFirstTestCase'>
                    string testCaseName = String.Empty;
                    foreach (XmlAttribute attribute in testCaseNode.Attributes)
                    {
                        if (attribute.Name.Equals("id"))
                        {
                            testCaseName = attribute.InnerText;
                        }
                        TestCase testCase = new TestCase {TestCaseName = testCaseName};

                        foreach (XmlNode testActionNode in testCaseNode.ChildNodes)
                        {
                            string testActionName = testActionNode.Name;
                            string testActionParameter = testActionNode.InnerText;
                            //if node has attribute, first attribute is parameter
                            foreach (XmlAttribute testActionAttribute in testActionNode.Attributes)
                            {
                                testActionParameter = testActionAttribute.InnerText;
                                break;
                            }
                            TestAction testAction = new TestAction
                                                        {
                                                            ActionName = testActionName,
                                                            Parameter = testActionParameter
                                                        };
                            testCase.AddTestAction(testAction);
                        }
                        testSpecs.AddTestCase(testCase);
                    }
                }
            }
            return testSpecs;
        }

        private readonly string xmlSpecs;
    }
}