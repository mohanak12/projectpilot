using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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
            string fileContents = null;

            using (FileStream stream = File.Open (sourceFileName, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader reader = new StreamReader (stream, sourceFileEncoding))
                {
                    fileContents = reader.ReadToEnd ();
                }
            }

            foreach (string property in properties.Keys)
                fileContents = fileContents.Replace (property, properties[property]);

            using (FileStream stream = File.Open (expandedFileName, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter writer = new StreamWriter (stream, expandedFileEncoding))
                    writer.Write (fileContents);
            }
        }

        private Encoding expandedFileEncoding;
        private string expandedFileName;
        private IDictionary<string, string> properties = new Dictionary<string, string> ();
        private Encoding sourceFileEncoding;
        private string sourceFileName;
    }
}
