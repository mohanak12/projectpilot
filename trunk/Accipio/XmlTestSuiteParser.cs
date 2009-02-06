#region

using System;
using System.Globalization;
using System.IO;
using System.Xml;

#endregion

namespace Accipio
{
    public class XmlTestSuiteParser : ITestSuiteParser, IDisposable
    {
        public XmlTestSuiteParser(Stream xmlSpecs)
        {
            xmlSpecsStream = xmlSpecs;
        }

        public XmlTestSuiteParser (string testSuiteFileName)
        {
            xmlSpecsStream = File.OpenRead(testSuiteFileName);
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

            using (XmlReader xmlReader = XmlReader.Create(xmlSpecsStream, xmlReaderSettings))
            {
                xmlReader.Read();

                while (false == xmlReader.EOF)
                {
                    switch (xmlReader.NodeType)
                    {
                        case XmlNodeType.Element:
                            {
                                if (xmlReader.Name != "suite")
                                    throw new XmlException("<suite> (root) element expected.");

                                TestSuite testSuite = new TestSuite(xmlReader.GetAttribute("id"));
                                testSuite.TestRunnerName = xmlReader.GetAttribute("runner");
                                testSuite.Namespace = xmlReader.GetAttribute("namespace");

                                string isParallelizableValue = xmlReader.GetAttribute("isParallelizable");
                                if (false == String.IsNullOrEmpty(isParallelizableValue))
                                    testSuite.IsParallelizable = bool.Parse(isParallelizableValue);

                                string degreeOfParallelismValue = xmlReader.GetAttribute("degreeOfParallelism");
                                if (false == String.IsNullOrEmpty(degreeOfParallelismValue))
                                    testSuite.DegreeOfParallelism = int.Parse(
                                        degreeOfParallelismValue,
                                        CultureInfo.InvariantCulture);

                                ReadTestSuite(testSuite, xmlReader);

                                return testSuite;
                            }
                        
                        case XmlNodeType.XmlDeclaration:
                            xmlReader.Read();
                            continue;

                        default:
                            {
                                throw new NotSupportedException(
                                    string.Format(
                                    CultureInfo.InvariantCulture,
                                    "Not supported xml node type. Node type = {0}",
                                    xmlReader.NodeType));
                            }
                    }
                }
            }

            return null;
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

        private static void ReadAction(TestCase testCase, XmlReader xmlReader)
        {
            xmlReader.Read();

            while (xmlReader.NodeType != XmlNodeType.EndElement)
            {
                TestCaseStep testCaseStep = new TestCaseStep(xmlReader.Name);

                if (xmlReader.HasAttributes)
                {
                    while (xmlReader.MoveToNextAttribute())
                    {
                        string key = xmlReader.LocalName;
                        string value = xmlReader.Value;

                        testCaseStep.AddActionParameter(new TestActionParameter(key, value));
                    }

                    // move back to element
                    xmlReader.MoveToElement();
                }

                if (!xmlReader.IsEmptyElement)
                {
                    xmlReader.Read();
                    if (xmlReader.NodeType == XmlNodeType.Text)
                    {
                        string content = xmlReader.ReadContentAsString();
                        testCaseStep.AddActionParameter(new TestActionParameter("value", content));
                    }
                }

                // add test action parameters
                testCase.AddStep(testCaseStep);

                // end of Action element
                xmlReader.Read();
            }

            // end of TestCase element
            xmlReader.Read();
        }

        private static void ReadTestCase(TestCase testCase, XmlReader xmlReader)
        {
            xmlReader.Read();

            while (xmlReader.NodeType != XmlNodeType.EndElement)
            {
                switch (xmlReader.Name)
                {
                    case "description":
                        testCase.TestCaseDescription = xmlReader.ReadElementContentAsString();
                        break;
                        
                    case "tags":
                        testCase.AddTestCaseTag(xmlReader.ReadElementContentAsString());
                        break;

                    case "steps":
                        ReadAction(testCase, xmlReader);
                        break;
                
                    default:
                        throw new NotSupportedException("ReadTestCase -> " + xmlReader.Name);
                }
            }

            xmlReader.Read();
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
                            //TODO: check for white spaces in Id
                            string testCaseName = xmlReader.GetAttribute("id");
                            TestCase testCase = new TestCase(testCaseName);
                            testSuite.AddTestCase(testCase);
                            ReadTestCase(testCase, xmlReader);
                            break;
                        }

                    case "description":
                        {
                            testSuite.Description = xmlReader.ReadElementContentAsString();
                            break;
                        }

                    default:
                        {
                            throw new NotSupportedException("ReadTestSuite -> " + xmlReader.Name);
                        }
                }
            }

            xmlReader.Read();
        }

        private bool disposed;
        private readonly Stream xmlSpecsStream;
    }
}