using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;

namespace Accipio
{
    /// <summary>
    /// Implementation of <see cref="IBusinessActionXmlParser"/> interface which parse xml file 
    /// of business actions to object <see cref="BusinessActionsRepository" />
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

            supportedParameterTypes.Add("boolean", typeof(bool));
            supportedParameterTypes.Add("decimal", typeof(decimal));
            supportedParameterTypes.Add("double", typeof(double));
            supportedParameterTypes.Add("float", typeof(float));
            supportedParameterTypes.Add("integer", typeof(int));
            supportedParameterTypes.Add("negativeInteger", typeof(int));
            supportedParameterTypes.Add("nonNegativeInteger", typeof(int));
            supportedParameterTypes.Add("nonPositiveInteger", typeof(int));
            supportedParameterTypes.Add("positiveInteger", typeof(int));
            supportedParameterTypes.Add("string", typeof(string));
        }

        /// <summary>
        /// Parse xml to object <see cref="BusinessActionsRepository" />
        /// </summary>
        /// <returns>
        /// <see cref="BusinessActionsRepository" />
        /// Object that contains all business actions data
        /// </returns>
        public BusinessActionsRepository Parse()
        {
            XmlReaderSettings xmlReaderSettings =
                new XmlReaderSettings
                {
                    IgnoreComments = true,
                    IgnoreProcessingInstructions = true,
                    IgnoreWhitespace = true
                };

            BusinessActionsRepository businessActionsRepository = new BusinessActionsRepository();
                    
            using (XmlReader xmlReader = XmlReader.Create(xmlStream, xmlReaderSettings))
            {
                xmlReader.Read();

                while (false == xmlReader.EOF)
                {
                    switch (xmlReader.NodeType)
                    {
                        case XmlNodeType.Element:
                            {
                                if (xmlReader.Name != "actions")
                                    throw new XmlException("<actions> (root) element expected.");

                                ReadAction(businessActionsRepository, xmlReader);
                                
                                break;
                            }

                        case XmlNodeType.XmlDeclaration:
                            xmlReader.Read();
                            continue;

                        default:
                            {
                                throw new System.NotSupportedException();
                            }
                    }
                }
            }

            return businessActionsRepository;
        }

        /// <summary>
        /// Goes through all action elements in xml file
        /// </summary>
        /// <param name="businessActionsRepository">object that will contain all data about business actions and functions</param>
        /// <param name="xmlReader">Xml reader provides access to xml file</param>
        private void ReadAction(BusinessActionsRepository businessActionsRepository, XmlReader xmlReader)
        {
            xmlReader.Read();

            while (xmlReader.NodeType != XmlNodeType.EndElement)
            {
                switch (xmlReader.Name)
                {
                    case "action":
                        {
                            string testActionId = ReadAttribute("id", xmlReader);

                            BusinessAction businessAction = new BusinessAction(testActionId);

                            // read action parameters
                            ReadActionParameters(businessAction, xmlReader);

                            // add new action to list
                            businessActionsRepository.AddAction(businessAction);

                            break;
                        }

                    case "function":
                        {
                            xmlReader.Skip();
                            break;
                    //        string functionId = ReadAttribute("id", xmlReader);

                    //        BusinessFunctionEntry businessFunctionEntry = new BusinessFunctionEntry(functionId);

                    //        // read function steps
                    //        ReadFunctionsSteps(businessFunctionEntry, xmlReader);

                    //        // add new function to list
                    //        businessActionsRepository.Functions.Add(businessFunctionEntry);

                    //        break;
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
        /// <param name="businessAction">object that will contain all data about business actions</param>
        /// <param name="xmlReader">Xml reader provides access to xml file</param>
        private void ReadActionParameters(BusinessAction businessAction, XmlReader xmlReader)
        {
            xmlReader.Read();

            int parameterOrder = 0;
            while (xmlReader.NodeType != XmlNodeType.EndElement)
            {
                switch (xmlReader.Name)
                {
                    case "description":

                        // set description of business action
                        businessAction.Description = xmlReader.ReadElementContentAsString();

                        break;

                    case "parameter":

                        string parameterName = ReadAttribute("name", xmlReader);
                        string parameterTypeXsd = ReadAttribute("type", xmlReader);

                        Type parameterType = null;

                        if (parameterTypeXsd != null)
                        {
                            if (false == supportedParameterTypes.ContainsKey(parameterTypeXsd))
                                throw new ArgumentException(string.Format(
                                                                CultureInfo.InvariantCulture,
                                                                "Unsupported action parameter type: {0}. Business action = '{1}', parameter = '{2}'",
                                                                parameterTypeXsd,
                                                                businessAction.ActionName,
                                                                parameterName));

                            parameterType = supportedParameterTypes[parameterTypeXsd];
                        }
                        else
                        {
                            // the default parameter type is string
                            parameterType = typeof(string);
                            parameterTypeXsd = "string";
                        }

                        // add action parameter name and type
                        businessAction.AddParameter(
                            new BusinessActionParameter(
                                parameterName, 
                                parameterType,
                                parameterTypeXsd,
                                parameterOrder++));

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

        private Dictionary<string, Type> supportedParameterTypes = new Dictionary<string, Type>();
        private readonly Stream xmlStream;
    }
}
