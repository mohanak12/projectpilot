using System;
using System.Xml;

namespace Flubu.Tasks.Text
{
    /// <summary>
    /// Retrieves a value from an XML file.
    /// </summary>
    public class PeekXmlTask : TaskBase
    {
        /// <summary>
        /// Gets the task description.
        /// </summary>
        /// <value>The task description.</value>
        public override string TaskDescription
        {
            get
            {
                return String.Format(
                    System.Globalization.CultureInfo.InvariantCulture,
                    "Read xpath '{0}' from file '{1}' and store it into '{2}' setting.",
                    xpath, 
                    xmlFilename, 
                    configurationSettingName);
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is safe to execute in dry run mode.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is safe to execute in dry run mode; otherwise, <c>false</c>.
        /// </value>
        public override bool IsSafeToExecuteInDryRun
        {
            get
            {
                return true;
            }
        }

        public PeekXmlTask(
            string xmlFilename,
            string xpath,
            string configurationSettingName)
        {
            this.xmlFilename = xmlFilename;
            this.xpath = xpath;
            this.configurationSettingName = configurationSettingName;
        }

        /// <summary>
        /// Reads a specified value from an XML file and stores it as a specified configuration setting.
        /// </summary>
        /// <param name="environment">The script execution environment.</param>
        /// <param name="xmlFilename">The name of the configuration file.</param>
        /// <param name="xpath">The xpath of the value to read.</param>
        /// <param name="configurationSettingName">Name of the configuration setting into which the XML value will be stored.</param>
        public static void Execute(
            IScriptExecutionEnvironment environment,
            string xmlFilename,
            string xpath,
            string configurationSettingName)
        {
            if (environment == null)
                throw new ArgumentNullException("environment");

            if (xmlFilename == null)
                throw new ArgumentNullException("xmlFilename");

            if (xpath == null)
                throw new ArgumentNullException("xpath");

            if (configurationSettingName == null)
                throw new ArgumentNullException("configurationSettingName");

            PeekXmlTask task = new PeekXmlTask(xmlFilename, xpath, configurationSettingName);
            task.Execute(environment);
        }

        /// <summary>
        /// Internal task execution code.
        /// </summary>
        /// <param name="environment">The script execution environment.</param>
        protected override void DoExecute(IScriptExecutionEnvironment environment)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlFilename);

            XmlNode node = xmlDoc.SelectSingleNode("Configuration");

            if (node != null)
                environment.SetConfigurationSettingValue(configurationSettingName, node.InnerText);
        }

        private string xmlFilename;
        private string xpath;
        private string configurationSettingName;
    }
}