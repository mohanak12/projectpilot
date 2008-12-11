using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Xml;

namespace Flubu.Builds.VSSolutionBrowsing
{
    public class VSProject
    {
        public IList<VSProjectCompileItem> CompileItems
        {
            get { return compileItems; }
        }

        public IList<VSProjectConfiguration> Configurations
        {
            get { return configurations; }
        }

        public IDictionary<string, string> Properties
        {
            get { return properties; }
        }

        public IList<VSProjectReference> References
        {
            get { return references; }
        }

        public static VSProject Load(Stream stream)
        {
            VSProject data = new VSProject();
            
            XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
            xmlReaderSettings.IgnoreComments = true;
            xmlReaderSettings.IgnoreProcessingInstructions = true;
            xmlReaderSettings.IgnoreWhitespace = true;

            using (XmlReader xmlReader = XmlReader.Create(stream, xmlReaderSettings))
            {
                xmlReader.Read();
                while (false == xmlReader.EOF)
                {
                    switch (xmlReader.NodeType)
                    {
                        case XmlNodeType.XmlDeclaration:
                            xmlReader.Read();
                            break;

                        case XmlNodeType.Element:
                            if (xmlReader.Name == "Project") 
                            {
                                data.ReadProject(xmlReader);
                            }  
                            //  data.properties1.Add(Test, test);
                            break;
                        default:
                            throw new XmlException();
                    }
                }
            }

            return null;
        }

        private void ReadProject(XmlReader xmlReader)
        {
            xmlReader.Read();

            while (xmlReader.NodeType != XmlNodeType.EndElement)
            {
                switch (xmlReader.Name)
                {
                    case "PropertyGroup":
                        ReadPropertyGroup(xmlReader);
                        xmlReader.Read();
                        break;
                    case "ItemGroup":
                        ReadItemGroup(xmlReader);
                         break;
                    default:    
                        throw new NotSupportedException();
                }
            }
        }

        private void ReadPropertyGroup(XmlReader xmlReader)
        {
            xmlReader.Read();
            
            while (xmlReader.NodeType != XmlNodeType.EndElement)
            {
                this.properties.Add(xmlReader.Name, xmlReader.ReadElementContentAsString());
            }
        }

        private VSProjectReference ReadItemGroup(XmlReader xmlReader)
        {
            VSProjectReference reference = new VSProjectReference();
            xmlReader.Read();

            while (xmlReader.NodeType != XmlNodeType.EndElement)
            {
                switch (xmlReader.Name)
                {
                    case "Reference":
                        reference = ReadReference(xmlReader);
                        references.Add(reference);
                        //xmlReader.Read();
                        break;
                    default:
                        throw new NotSupportedException();
                }

                references.Add(reference);
            }

            return reference;
        }

        private VSProjectReference ReadReference(XmlReader xmlReader)
        {
            VSProjectReference reference = new VSProjectReference();
            
            while (xmlReader.NodeType != XmlNodeType.EndElement)
            {
                switch (xmlReader.Name)
                {
                    case "Reference":
                        if (xmlReader["Include"] != null)
                        {
                            reference.Include = xmlReader["Include"];
                        }

                        break;
                    case "HintPath":
                         reference.HintPath = ReadHintPath(xmlReader).HintPath;
                        break;
                    case "SpecificVersion":
                        reference.SpecificVersion = ReadSpecificVersion(xmlReader).SpecificVersion;
                        break;
                    default:
                        throw new NotSupportedException();
                }

                xmlReader.Read();
            }

            return reference;
        }

        private VSProjectReference ReadHintPath(XmlReader xmlReader)
        {
            VSProjectReference reference = new VSProjectReference();

            xmlReader.Read();

            while (xmlReader.NodeType != XmlNodeType.EndElement)
            {
                switch (xmlReader.Name)
                {
                    case "HintPath":
                        reference.HintPath = xmlReader.ReadElementContentAsString();
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }

            return reference;
        }

        private VSProjectReference ReadSpecificVersion(XmlReader xmlReader)
        {
            VSProjectReference reference = new VSProjectReference();

            while (xmlReader.NodeType != XmlNodeType.EndElement)
            {
                switch (xmlReader.Name)
                {
                    case "SpecificVersion":
                        xmlReader.Read();
                        reference.SpecificVersion = bool.Parse(xmlReader.ReadString());
                      //  reference.SpecificVersion = xmlReader.Value;
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }

            return reference;
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        private VSProjectCompileItem ReadCompile(XmlReader xmlReader)
        {
            VSProjectCompileItem compile = new VSProjectCompileItem();

            xmlReader.Read();

            while (xmlReader.NodeType != XmlNodeType.EndElement)
            {
                switch (xmlReader.Name)
                {
                    case "Compile":
                        compile.Compile = xmlReader["Include"];
                        break;
                }
            }

            return compile;
        }

        private Dictionary<string, string> properties = new Dictionary<string, string>();
        private List<VSProjectConfiguration> configurations = new List<VSProjectConfiguration>();
        private List<VSProjectReference> references = new List<VSProjectReference>();
        private List<VSProjectCompileItem> compileItems = new List<VSProjectCompileItem>();
    }
}

