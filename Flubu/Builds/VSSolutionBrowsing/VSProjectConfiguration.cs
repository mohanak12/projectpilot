using System;
using System.Collections.Generic;
using System.Text;

namespace Flubu.Builds.VSSolutionBrowsing
{
    public class VSProjectConfiguration
    {
        public string Condition
        {
            get { return condition; }
            set { condition = value; }
        }
        
        public IDictionary<string, string> Properties
        {
            get { return properties; }
        }

        private Dictionary<string, string> properties = new Dictionary<string, string>();
        private string condition;
    }
}
