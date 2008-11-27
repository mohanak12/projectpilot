#region

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Xml;

#endregion

namespace Accipio
{
    public class XmlTestSpecsParser : ITestSpecsParser
    {
        public XmlTestSpecsParser(string xmlSpecs)
        {
            this.xmlSpecs = xmlSpecs;
        }

        public void FetchXml()
        {
            using (Stream stream = File.OpenRead(@"..\..\..\Data\Samples\ccnet.stats.xml"))
            {
                TestSpecs data = Parse(stream);
            }
        }

        public TestSpecs Parse(Stream stream)
        {
            XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
            xmlReaderSettings.IgnoreComments = true;
            xmlReaderSettings.IgnoreProcessingInstructions = true;
            xmlReaderSettings.IgnoreWhitespace = true;

            TestSpecs testSpecs = new TestSpecs();

            using (XmlReader xmlReader = XmlReader.Create(stream, xmlReaderSettings))
            {
                xmlReader.Read();

                while (false == xmlReader.EOF)
                {
                    switch (xmlReader.NodeType)
                    {
                        case XmlNodeType.XmlDeclaration:
                            xmlReader.Read();
                            continue;

                        case XmlNodeType.Element:
                            if (xmlReader.Name != "TestSpecs")
                                throw new XmlException("<TestSpecs> (root) element expected.");

                            ReadTestSpec(testSpecs, xmlReader);

                            break;
                            //return data;

                        default:
                            throw new XmlException();
                    }
                }
            }
            // parse XML document`
            //XmlDocument xmlDocument = new XmlDocument();
            //xmlDocument.LoadXml(xmlSpecs);

            //if (xmlDocument.DocumentElement != null)
            //{
            //    foreach (XmlNode testCaseNode in xmlDocument.DocumentElement.ChildNodes)
            //    {
            //        //<TestCase id='myFirstTestCase'>
            //        string testCaseName = String.Empty;
            //        string testCaseCategory = String.Empty;
            //        foreach (XmlAttribute attribute in testCaseNode.Attributes)
            //        {
            //            if (attribute.Name.Equals("id"))
            //            {
            //                testCaseName = attribute.InnerText;
            //            }

            //            if (attribute.Name.Equals("category"))
            //            {
            //                testCaseCategory = attribute.InnerText;
            //            }
            //        }

            //        if (testCaseName != null)
            //        {
            //            TestCase testCase = testCaseCategory != null
            //                                    ? new TestCase(testCaseName, testCaseCategory)
            //                                    : new TestCase(testCaseName);

            //            foreach (XmlNode testActionNode in testCaseNode.ChildNodes)
            //            {
            //                string defaultParameterValue = testActionNode.InnerText;
            //                string testActionName = testActionNode.Name;
            //                TestAction testAction = new TestAction(testActionName);
            //                foreach (XmlAttribute testActionAttribute in testActionNode.Attributes)
            //                {
            //                    string parameterKey = testActionAttribute.Name;
            //                    string parameterValue = testActionAttribute.InnerText;
            //                    TestActionParameter testActionParameter = new TestActionParameter(parameterKey, parameterValue);
            //                    testAction.AddActionParameter(testActionParameter);
            //                }

            //                if (!testAction.HasParameters && defaultParameterValue.Length > 0)
            //                {
            //                    testAction.AddActionParameter(new TestActionParameter("default", defaultParameterValue));
            //                }

            //                testCase.AddTestAction(testAction);
            //            }

            //            testSpecs.AddTestCase(testCase);
            //        }
            //        else
            //        {
            //            throw new NotImplementedException("Test case ID not set!");
            //        }
            //    }
            //}

            return testSpecs;
        }

        private void ReadTestSpec(TestSpecs testSpecs, XmlReader xmlReader)
        {
            xmlReader.Read();

            while (xmlReader.NodeType != XmlNodeType.EndElement)
            {
                switch (xmlReader.Name)
                {
                    case "TestCase":

                        TestCase testCase = new TestCase(ReadAttribute(xmlReader, "id"), ReadAttribute(xmlReader, "category"));
                        testSpecs.AddTestCase(testCase);
                        ReadAction(testCase, xmlReader);

                        break;

                    default:
                        throw new NotSupportedException();
                }
            }
        }

        private void ReadAction(TestCase testCase, XmlReader xmlReader)
        {
            xmlReader.Read();

            while (xmlReader.NodeType != XmlNodeType.EndElement)
            {
                TestAction testAction = new TestAction(xmlReader.Name);
          
                while (xmlReader.MoveToNextAttribute())
                {
                    string key = xmlReader.LocalName;
                    string value = xmlReader.Value;

                    testAction.AddActionParameter(new TestActionParameter(key, value));
                }

                // move back to element
                xmlReader.MoveToElement();
                // move to the end of element
                xmlReader.Read();

                if (xmlReader.NodeType == XmlNodeType.Text)
                {
                    // TODO: there is also text node. Where to add this text node
                    xmlReader.Read();
                }

                // add test action parameters
                testCase.AddTestAction(testAction);

                // end of Action element
                xmlReader.Read();
            }

            // end of TestCase element
            xmlReader.Read();
        }

        private static string ReadAttribute(XmlReader xmlReader, string attributeName)
        {
            return xmlReader.GetAttribute(attributeName);
        }

        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private readonly string xmlSpecs;
    }
}