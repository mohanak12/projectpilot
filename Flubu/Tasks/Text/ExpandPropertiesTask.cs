using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Commons.Collections;
using NVelocity;
using NVelocity.App;

namespace Flubu.Tasks.Text
{
    public class ExpandPropertiesTask : TaskBase
    {
        /// <summary>
        /// Gets the task description.
        /// </summary>
        /// <value>The task description.</value>
        public override string TaskDescription
        {
            get
            {
                return String.Format (
                    System.Globalization.CultureInfo.InvariantCulture,
                    "Expanding properties in file '{0}' to file '{1}'",
                    sourceFileName, 
                    expandedFileName);
            }
        }

        public ExpandPropertiesTask (
            string sourceFileName, 
            string expandedFileName,
            Encoding sourceFileEncoding,
            Encoding expandedFileEncoding)
        {
            this.sourceFileName = sourceFileName;
            this.expandedFileName = expandedFileName;
            this.sourceFileEncoding = sourceFileEncoding;
            this.expandedFileEncoding = expandedFileEncoding;
        }

        public void AddPropertyToExpand (string propertyText, string value)
        {
            properties.Add (propertyText, value);
        }

        protected override void DoExecute (IScriptExecutionEnvironment environment)
        {
            VelocityEngine velocity = new VelocityEngine();
            ExtendedProperties props = new ExtendedProperties();
            velocity.Init(props);

            Template template = velocity.GetTemplate(sourceFileName, this.sourceFileEncoding.ToString());

            using (Stream stream = File.Open(expandedFileName, FileMode.Create))
            {
                using (TextWriter writer = new StreamWriter(stream, this.expandedFileEncoding))
                {
                    VelocityContext velocityContext = new VelocityContext(properties);
                    template.Merge(velocityContext, writer);
                }
            }
        }

        private Encoding expandedFileEncoding;
        private string expandedFileName;
        private Hashtable properties = new Hashtable();
        private Encoding sourceFileEncoding;
        private string sourceFileName;
    }
}
