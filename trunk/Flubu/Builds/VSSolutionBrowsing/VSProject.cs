using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Flubu.Builds.VSSolutionBrowsing
{
    /// <summary>
    /// Represents a VisualStudio project.
    /// </summary>
    public class VSProject
    {
        /// <summary>
        /// Gets a read-only collection of all .cs files in the solution.
        /// </summary>
        /// <value>A read-only collection of all the .cs files in the solution.</value>
        public IList<VSProjectCompileItem> CompileItems
        {
            get { return compileItems; }
        }

        /// <summary>
        /// Gets a read-only collection of project configurations.
        /// </summary>
        /// <value>A read-only collection of project configurations.</value>
        public IList<VSProjectConfiguration> Configurations
        {
            get { return configurations; }
        }

        /// <summary>
        /// Gets a read-only collection of project properties.
        /// </summary>
        /// <value>A read-only collection of project properties.</value>
        public IDictionary<string, string> Properties
        {
            get { return properties; }
        }

        /// <summary>
        /// Gets a read-only collection of project references.
        /// </summary>
        /// <value>A read-only collection of project references.</value>
        public IList<VSProjectReference> References
        {
            get { return references; }
        }

        /// <summary>
        /// Finds the VisualStudio project configuration specified by a condition.
        /// </summary>
        /// <param name="condition">The condition which identifies the configuration 
        /// (example: " '$(Configuration)|$(Platform)' == 'Release|AnyCPU' ").</param>
        /// <returns><see cref="VSProjectConfiguration"/> object if found; <c>null</c> if no configuration was found that meets the
        /// specified condition.</returns>
        public VSProjectConfiguration FindConfiguration (string condition)
        {
            foreach (VSProjectConfiguration configuration in configurations)
            {
                if (0 == string.Compare (configuration.Condition, condition, StringComparison.Ordinal))
                    return configuration;
            }

            return null;
        }

        public static VSProject Load(string projectFileName)
        {
            using (Stream stream = File.OpenRead (projectFileName))
            {
                VSProject data = new VSProject();
                data.propertiesDictionary = true;

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

                                xmlReader.Read();
                                break;
                            default:
                                xmlReader.Read();
                                continue;
                        }
                    }
                }

                return data;
            }
        }

        private void ReadProject(XmlReader xmlReader)
        {
            xmlReader.Read();

            while (xmlReader.NodeType != XmlNodeType.EndElement)
            {
                switch (xmlReader.Name)
                {
                    case "PropertyGroup":
                        if (this.propertiesDictionary == true)
                        {
                            ReadPropertyGroup(xmlReader);
                            this.propertiesDictionary = false;
                        }
                        else
                        {
                            configurations.Add(ReadPropertyGroup(xmlReader));
                        }

                        xmlReader.Read();
                        break;
                    case "ItemGroup":
                        ReadItemGroup(xmlReader);
                        xmlReader.Read();
                         break;
                    default:
                         xmlReader.Read();
                         continue;
                }
            }
        }

        private VSProjectConfiguration ReadPropertyGroup(XmlReader xmlReader)
        {
            VSProjectConfiguration configuration = new VSProjectConfiguration();

            if (xmlReader["Condition"] != null && this.propertiesDictionary == false)
            {
                configuration.Condition = xmlReader["Condition"];
            }

            xmlReader.Read();
            
            while (xmlReader.NodeType != XmlNodeType.EndElement)
            {
                if (this.propertiesDictionary == true)
                {
                    this.properties.Add(xmlReader.Name, xmlReader.ReadElementContentAsString());
                }
                else
                {
                    configuration.Properties.Add(xmlReader.Name, xmlReader.ReadElementContentAsString());
                }
            }

            return configuration;
        }

        private void ReadItemGroup(XmlReader xmlReader)
        {
            xmlReader.Read();

            while (xmlReader.NodeType != XmlNodeType.EndElement)
            {
                switch (xmlReader.Name)
                {
                    case "Reference":
                        VSProjectReference reference = ReadReference(xmlReader);
                        references.Add(reference);
                        if (xmlReader.NodeType == XmlNodeType.EndElement)
                        {
                            xmlReader.Read();
                        }

                        break;
                    case "Compile":
                        VSProjectCompileItem compileitems = ReadCompile(xmlReader);
                        compileItems.Add(compileitems);

                        if (xmlReader.NodeType == XmlNodeType.EndElement)
                        {
                            xmlReader.Read();
                        }

                        break;                
                    default:
                        xmlReader.Read();
                        continue;
                }
            }
        }

        private static VSProjectReference ReadReference(XmlReader xmlReader)
        {
            VSProjectReference reference = new VSProjectReference();

            while (xmlReader.NodeType != XmlNodeType.EndElement)
            {
                if (xmlReader.HasAttributes && xmlReader.NodeType != XmlNodeType.EndElement)
                {
                    reference.Include = xmlReader[0];
                    xmlReader.Read();
                }

                if (xmlReader.HasAttributes == false && xmlReader.NodeType != XmlNodeType.EndElement)
                {
                    reference.ReferenceAttributes.Add(xmlReader.Name, xmlReader.ReadElementContentAsString());
                }
                else
                {
                    return reference;
                }
            }

            return reference;
        }

        private static VSProjectCompileItem ReadCompile(XmlReader xmlReader)
        {
            VSProjectCompileItem compile = new VSProjectCompileItem();

            while (xmlReader.NodeType != XmlNodeType.EndElement)
            {
                if (xmlReader.HasAttributes && xmlReader.NodeType != XmlNodeType.EndElement)
                {
                    compile.Compile = xmlReader[0];
                    xmlReader.Read();
                }

                if (xmlReader.HasAttributes == false && xmlReader.NodeType != XmlNodeType.EndElement)
                {
                    compile.CompileAttributes.Add(xmlReader.Name, xmlReader.ReadElementContentAsString());
                }
                else
                {
                    return compile;
                }
            }

            return compile;
        }

        private readonly List<VSProjectCompileItem> compileItems = new List<VSProjectCompileItem>();
        private readonly List<VSProjectConfiguration> configurations = new List<VSProjectConfiguration>();
        private readonly Dictionary<string, string> properties = new Dictionary<string, string>();
        private bool propertiesDictionary;
        private readonly List<VSProjectReference> references = new List<VSProjectReference>();
    }
}

