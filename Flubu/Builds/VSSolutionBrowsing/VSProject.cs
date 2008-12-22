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
        public IList<VSProjectItem> Items
        {
            get { return items; }
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

        /// <summary>
        /// Loads the specified project file name.
        /// </summary>
        /// <param name="projectFileName">Name of the project file.</param>
        /// <returns>VSProject class containing project information.</returns>
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

        /// <summary>
        /// Gets the List of VSProjectItem single type items.
        /// </summary>
        /// <param name="getItemType">Type of the item.</param>
        /// <returns>List of items of specific itemType.</returns>
        public IList<VSProjectItem> GetSingleTypeItems(string getItemType)
        {
            List<VSProjectItem> returnList = new List<VSProjectItem>();
            foreach (VSProjectItem item in this.Items)
            {
                if (item.ItemType == getItemType)
                    returnList.Add(item);
            }

            return returnList;
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
                        VSProjectItem reference = ReadItem(xmlReader, VSProjectItem.Reference);
                        items.Add(reference);
                        if (xmlReader.NodeType == XmlNodeType.EndElement)
                        {
                            xmlReader.Read();
                        }

                        break;
                    case "Compile":
                        VSProjectItem compileItems = ReadItem(xmlReader, VSProjectItem.CompileItem);
                        items.Add(compileItems);

                        if (xmlReader.NodeType == XmlNodeType.EndElement)
                        {
                            xmlReader.Read();
                        }

                        break;
                    case "Content":
                        VSProjectItem contentItem = ReadItem(xmlReader, VSProjectItem.Content);
                        items.Add(contentItem);

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

        private static VSProjectItem ReadItem(XmlReader xmlReader, string itemType)
        {
            VSProjectItem item = new VSProjectItem(itemType);

            while (xmlReader.NodeType != XmlNodeType.EndElement)
            {
                if (xmlReader.HasAttributes && xmlReader.NodeType != XmlNodeType.EndElement)
                {
                    item.Item = xmlReader[0];
                    xmlReader.Read();
                }

                if (xmlReader.HasAttributes == false && xmlReader.NodeType != XmlNodeType.EndElement)
                {
                    item.ItemAttributes.Add(xmlReader.Name, xmlReader.ReadElementContentAsString());
                }
                else
                {
                    return item;
                }
            }

            return item;
        }

        private readonly List<VSProjectItem> items = new List<VSProjectItem>();
        private readonly List<VSProjectConfiguration> configurations = new List<VSProjectConfiguration>();
        private readonly Dictionary<string, string> properties = new Dictionary<string, string>();
        private bool propertiesDictionary;
    }
}

