#region

using System;
using System.Xml;

#endregion

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
                    string testCaseCategory = String.Empty;
                    foreach (XmlAttribute attribute in testCaseNode.Attributes)
                    {
                        if (attribute.Name.Equals("id"))
                        {
                            testCaseName = attribute.InnerText;
                        }

                        if (attribute.Name.Equals("category"))
                        {
                            testCaseCategory = attribute.InnerText;
                        }
                    }

                    if (testCaseName != null)
                    {
                        TestCase testCase = testCaseCategory != null
                                                ? new TestCase(testCaseName, testCaseCategory)
                                                : new TestCase(testCaseName);

                        foreach (XmlNode testActionNode in testCaseNode.ChildNodes)
                        {
                            string defaultParameterValue = testActionNode.InnerText;
                            string testActionName = testActionNode.Name;
                            TestAction testAction = new TestAction(testActionName);
                            foreach (XmlAttribute testActionAttribute in testActionNode.Attributes)
                            {
                                string parameterKey = testActionAttribute.Name;
                                string parameterValue = testActionAttribute.InnerText;
                                TestActionParameter testActionParameter = new TestActionParameter(parameterKey, parameterValue);
                                testAction.AddActionParameter(testActionParameter);
                            }

                            if (!testAction.HasParameters && defaultParameterValue.Length > 0)
                            {
                                testAction.AddActionParameter(new TestActionParameter("default", defaultParameterValue));
                            }

                            testCase.AddTestAction(testAction);
                        }

                        testSpecs.AddTestCase(testCase);
                    }
                    else
                    {
                        throw new NotImplementedException("Test case ID not set!");
                    }
                }
            }

            return testSpecs;
        }

        private readonly string xmlSpecs;
    }
}