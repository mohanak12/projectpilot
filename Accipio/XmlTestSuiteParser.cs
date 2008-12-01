#region

using System;
using System.IO;
using System.Xml;

#endregion

namespace Accipio
{
    public class XmlTestSuiteParser : ITestSpecsParser, IDisposable
    {
        public XmlTestSuiteParser(Stream xmlSpecs)
        {
            this.xmlSpecsStream = xmlSpecs;
        }

        public XmlTestSuiteParser (string testSuiteFileName)
        {
            this.xmlSpecsStream = File.OpenRead(testSuiteFileName);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or
        /// resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public TestSuite Parse()
        {
            XmlReaderSettings xmlReaderSettings =
                new XmlReaderSettings
                    {
                        IgnoreComments = true,
                        IgnoreProcessingInstructions = true,
                        IgnoreWhitespace = true
                    };

            TestSuite testSuite = new TestSuite();

            using (XmlReader xmlReader = XmlReader.Create(xmlSpecsStream, xmlReaderSettings))
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
                                if (xmlReader.Name != "suite")
                                    throw new XmlException("<suite> (root) element expected.");

                                ReadTestSuite(testSuite, xmlReader);

                                break;
                            }

                        default:
                            {
                                throw new XmlException();
                            }
                    }
                }
            }

            return testSuite;
        }

        /// <summary>
        /// Disposes the object.
        /// </summary>
        /// <param name="disposing">If <code>false</code>, cleans up native resources. 
        /// If <code>true</code> cleans up both managed and native resources</param>
        protected virtual void Dispose(bool disposing)
        {
            if (false == disposed)
            {
                if (disposing)
                {
                    xmlSpecsStream.Dispose();
                }

                disposed = true;
            }
        }

        private static void ReadTestSuite(TestSuite testSuite, XmlReader xmlReader)
        {
            xmlReader.Read();

            while (xmlReader.NodeType != XmlNodeType.EndElement)
            {
                switch (xmlReader.Name)
                {
                    case "case":
                        {
                            string testCaseName = ReadAttribute(xmlReader, "id");
                            string testCaseCategory = ReadAttribute(xmlReader, "category");
                            TestCase testCase = new TestCase(testCaseName, testCaseCategory);
                            testSuite.AddTestCase(testCase);
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

        private bool disposed;
        private readonly Stream xmlSpecsStream;
    }
}