using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Accipio
{
    /// <summary>
    /// Implementation of <see cref="IBusinessActionXmlParser"/> interface which parse xml file 
    /// of business actions to object <see cref="BusinessActionData" />
    /// </summary>
    public class BusinessActionsXmlParser : IBusinessActionXmlParser
    {
        /// <summary>
        /// Initializes a new instance of the BusinessActionsXmlParser class 
        /// using the specified <see cref="IBusinessActionXmlParser" />.
        /// </summary>
        /// <param name="xmlStream">stream of binary data</param>
        public BusinessActionsXmlParser(Stream xmlStream)
        {
            this.xmlStream = xmlStream;
        }

        /// <summary>
        /// Parse xml to object <see cref="BusinessActionData" />
        /// </summary>
        /// <returns>
        /// <see cref="BusinessActionData" />
        /// Object that contains all business actions data
        /// </returns>
        public BusinessActionData Parse()
        {
            XmlReaderSettings xmlReaderSettings =
                new XmlReaderSettings
                {
                    IgnoreComments = true,
                    IgnoreProcessingInstructions = true,
                    IgnoreWhitespace = true
                };

            BusinessActionData businessActionData = new BusinessActionData();
                    
            using (XmlReader xmlReader = XmlReader.Create(xmlStream, xmlReaderSettings))
            {
                xmlReader.Read();

                while (false == xmlReader.EOF)
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
                                if (xmlReader.Name != "actions")
                                    throw new XmlException("<actions> (root) element expected.");

                                ReadAction(businessActionData, xmlReader);
                                
                                break;
                            }

                        default:
                            {
                                throw new System.NotSupportedException();
                            }
                    }
                }
            }

            if (businessActionData.Actions.Count == 0)
            {
                throw new XmlException("Missing action element in xml.");
            }

            return businessActionData;
        }

        /// <summary>
        /// Goes through all action elements in xml file
        /// </summary>
        /// <param name="businessActionData">object that will contain all data about business actions and functions</param>
        /// <param name="xmlReader">Xml reader provides access to xml file</param>
        private static void ReadAction(BusinessActionData businessActionData, XmlReader xmlReader)
        {
            xmlReader.Read();

            while (xmlReader.NodeType != XmlNodeType.EndElement)
            {
                switch (xmlReader.Name)
                {
                    case "action":
                        {
                            string testActionId = ReadAttribute("id", xmlReader);

                            BusinessActionEntry businessActionEntry = new BusinessActionEntry(testActionId);

                            // read action parameters
                            ReadActionParameters(businessActionEntry, xmlReader);

                            // add new action to list
                            businessActionData.Actions.Add(businessActionEntry);

                            break;
                        }

                    case "function":
                        {
                            string functionId = ReadAttribute("id", xmlReader);

                            BusinessFunctionEntry businessFunctionEntry = new BusinessFunctionEntry(functionId);

                            // read function steps
                            ReadFunctionsSteps(businessFunctionEntry, xmlReader);

                            // add new function to list
                            businessActionData.Functions.Add(businessFunctionEntry);

                            break;
                        }

                    default:
                        {
                            throw new System.NotSupportedException();
                        }
                }
            }

            xmlReader.Read();
        }

        /// <summary>
        /// Goes through all child elements of element action in xml document
        /// </summary>
        /// <param name="businessActionEntry">object that will contain all data about business actions</param>
        /// <param name="xmlReader">Xml reader provides access to xml file</param>
        private static void ReadActionParameters(BusinessActionEntry businessActionEntry, XmlReader xmlReader)
        {
            xmlReader.Read();

            while (xmlReader.NodeType != XmlNodeType.EndElement)
            {
                switch (xmlReader.Name)
                {
                    case "description":

                        // set description of business action
                        businessActionEntry.Description = xmlReader.ReadElementContentAsString();

                        break;

                    case "parameter":

                        string parameterName = ReadAttribute("name", xmlReader);
                        string parameterType = ReadAttribute("type", xmlReader);

                        // add action parameter name and type
                        businessActionEntry.AddParameter(parameterName, parameterType);

                        xmlReader.Read();

                        break;

                        default:
                            throw new System.NotSupportedException();
                }
            }

            xmlReader.Read();
        }

        /// <summary>
        /// Read attribute of current xml element in xml file
        /// </summary>
        /// <param name="attributeName">Name of attribute to be readed</param>
        /// <param name="xmlReader">Xml reader provides access to xml file</param>
        /// <returns>Attribute value</returns>
        private static string ReadAttribute(string attributeName, XmlReader xmlReader)
        {
            return xmlReader.GetAttribute(attributeName);
        }

        /// <summary>
        /// Goes through all steps elements in xml file
        /// </summary>
        /// <param name="businessFunctionEntry">object that will contain all data about business functions</param>
        /// <param name="xmlReader">Xml reader provides access to xml file</param>
        private static void ReadFunctionsSteps(BusinessFunctionEntry businessFunctionEntry, XmlReader xmlReader)
        {
            xmlReader.Read();

            while (xmlReader.NodeType != XmlNodeType.EndElement)
            {
                switch (xmlReader.Name)
                {
                    case "steps":

                        // add new steps of the function
                        businessFunctionEntry.AddFunctionStep(ReadRunParameters(xmlReader));

                        break;

                    default:
                        throw new System.NotSupportedException();
                }
            }

            xmlReader.Read();
        }

        /// <summary>
        /// Goes through all run elements in xml file
        /// </summary>
        /// <param name="xmlReader">Xml reader provides access to xml file</param>
        /// <returns>List of run elements</returns>
        private static List<string> ReadRunParameters(XmlReader xmlReader)
        {
            xmlReader.Read();

            List<string> runActions = new List<string>();

            while (xmlReader.NodeType != XmlNodeType.EndElement)
            {
                switch (xmlReader.Name)
                {
                    case "run":

                        // add run actions to list
                        runActions.Add(ReadAttribute("action", xmlReader));
                        xmlReader.Read();

                        break;

                    default:
                        throw new System.NotSupportedException();
                }
            }

            xmlReader.Read();

            return runActions;
        }

        private readonly Stream xmlStream;
    }
}
