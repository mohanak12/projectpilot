using System;
using System.Collections.Generic;
using System.Text;

namespace Flubu.Builds.VSSolutionBrowsing
{
    /// <summary>
    /// Contains information about references inside of VisualStudio project.
    /// </summary>
    public class VSProjectReference
    {
        public string Include
        {
            get { return include; }
            set { include = value; }
        }

        public IDictionary<string, string> ReferenceAttributes
        {
            get { return referenceAttributes; }
        }

        private string include;
        private Dictionary<string, string> referenceAttributes = new Dictionary<string, string>();
    }
}
