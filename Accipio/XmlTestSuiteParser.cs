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
        public XmlTestSuiteParser(Stream stream, BusinessActionsRepository businessActionsRepository)
        {
            xmlSpecsStream = stream;
            this.businessActionsRepository = businessActionsRepository;
        }

        public XmlTestSuiteParser(string testSuiteFileName, BusinessActionsRepository businessActionsRepository)
        {
            xmlSpecsStream = File.OpenRead(testSuiteFileName);
            this.businessActionsRepository = businessActionsRepository;
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

                                string pendingValue = xmlReader.GetAttribute("pending");
                                if (false == String.IsNullOrEmpty(pendingValue))
                                    testSuite.PendingMessage = pendingValue;

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

        private void ReadAction(TestCase testCase, XmlReader xmlReader)
        {
            xmlReader.Read();

            while (xmlReader.NodeType != XmlNodeType.EndElement)
            {
                string businessActionName = xmlReader.Name;
                TestCaseStep testCaseStep = new TestCaseStep(businessActionName);

                BusinessAction businessAction = businessActionsRepository.GetAction(businessActionName);

                if (xmlReader.HasAttributes)
                {
                    while (xmlReader.MoveToNextAttribute())
                    {
                        string parameterName = xmlReader.LocalName;
                        string valueString = xmlReader.Value;

                        BusinessActionParameter parameter = businessAction.GetParameter(parameterName);

                        // convert value
                        try
                        {
                            object value = Convert.ChangeType(
                                valueString,
                                parameter.ParameterType,
                                CultureInfo.InvariantCulture);

                            testCaseStep.AddParameter(new TestStepParameter(parameterName, value));
                        }
                        catch (InvalidCastException ex)
                        {
                            throw new InvalidCastException(
                                string.Format(
                                   CultureInfo.InvariantCulture,
                                   "Could not cast parameter value '{0}' to '{1}'. Test case = '{2}', business action = '{3}', parameter = '{4}'",
                                   valueString,
                                   parameter.ParameterType.Name,
                                   testCase.TestCaseName,
                                   businessAction.ActionName,
                                   parameter.ParameterName),
                                ex);
                        }
                    }

                    // move back to element
                    xmlReader.MoveToElement();
                }

                // add test action parameters
                testCase.AddStep(testCaseStep);

                // end of Action element
                xmlReader.Read();
            }

            // end of TestCase element
            xmlReader.Read();
        }

        private void ReadTestCase(TestCase testCase, XmlReader xmlReader)
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

        private void ReadTestSuite(TestSuite testSuite, XmlReader xmlReader)
        {
            xmlReader.Read();

            while (xmlReader.NodeType != XmlNodeType.EndElement)
            {
                switch (xmlReader.Name)
                {
                    case "case":
                        {
                            string testCaseName = xmlReader.GetAttribute("id").Trim();

                            TestCase testCase = new TestCase(testCaseName);

                            string pendingValue = xmlReader.GetAttribute("pending");
                            if (false == String.IsNullOrEmpty(pendingValue))
                                testCase.PendingMessage = pendingValue;

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
        private readonly BusinessActionsRepository businessActionsRepository;
    }
}