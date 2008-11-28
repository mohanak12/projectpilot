#region

using System;
using System.IO;
using System.Xml;

#endregion

namespace Accipio
{
    public class XmlTestSpecsParser : ITestSpecsParser
    {
        public XmlTestSpecsParser(Stream xmlSpecs)
        {
            this.xmlSpecs = xmlSpecs;
        }

        public TestSpecs Parse()
        {
            XmlReaderSettings xmlReaderSettings =
                new XmlReaderSettings
                    {
                        IgnoreComments = true,
                        IgnoreProcessingInstructions = true,
                        IgnoreWhitespace = true
                    };

            TestSpecs testSpecs = new TestSpecs();

            using (XmlReader xmlReader = XmlReader.Create(xmlSpecs, xmlReaderSettings))
            {
                xmlReader.Read();

                while (false == xmlReader.EOF && xmlReader.NodeType != XmlNodeType.EndElement)
                {
                    switch (xmlReader.NodeType)
                    {
                        case XmlNodeType.XmlDeclaration:
                            {
                                xmlReader.Read();
                                continue;
                            }

                        case XmlNodeType.Element:
                            {
                                if (xmlReader.Name != "TestSpecs")
                                    throw new XmlException("<TestSpecs> (root) element expected.");

                                ReadTestSpec(testSpecs, xmlReader);

                                break;
                            }

                        default:
                            {
                                throw new XmlException();
                            }
                    }
                }
            }

            return testSpecs;
        }

        private static void ReadTestSpec(TestSpecs testSpecs, XmlReader xmlReader)
        {
            xmlReader.Read();

            while (xmlReader.NodeType != XmlNodeType.EndElement)
            {
                switch (xmlReader.Name)
                {
                    case "TestCase":
                        {
                            string testCaseName = ReadAttribute(xmlReader, "id");
                            string testCaseCategory = ReadAttribute(xmlReader, "category");
                            TestCase testCase = new TestCase(testCaseName, testCaseCategory);
                            testSpecs.AddTestCase(testCase);
                            ReadAction(testCase, xmlReader);
                            break;
                        }

                    default:
                        {
                            throw new NotSupportedException();
                        }
                }
            }
        }

        private static void ReadAction(TestCase testCase, XmlReader xmlReader)
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

        private readonly Stream xmlSpecs;
    }
}