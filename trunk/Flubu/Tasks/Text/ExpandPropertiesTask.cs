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

        public ExpandPropertiesTask (string sourceFileName, string expandedFileName)
        {
            this.sourceFileName = sourceFileName;
            this.expandedFileName = expandedFileName;
        }

        public void AddPropertyToExpand (string propertyText, string value)
        {
            properties.Add (propertyText, value);
        }

        protected override void DoExecute (IScriptExecutionEnvironment environment)
        {
            string fileContents = null;
            Encoding encoding;

            using (FileStream stream = File.Open (sourceFileName, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader reader = new StreamReader (stream))
                {
                    fileContents = reader.ReadToEnd ();
                    encoding = reader.CurrentEncoding;
                }
            }

            foreach (string property in properties.Keys)
                fileContents = fileContents.Replace (property, properties[property]);

            using (FileStream stream = File.Open (expandedFileName, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter writer = new StreamWriter (stream, encoding))
                    writer.Write (fileContents);
            }
        }

        private string sourceFileName;
        private string expandedFileName;

        private IDictionary<string, string> properties = new Dictionary<string, string> ();
    }
}
