using System;
using System.Collections.Generic;
using System.Text;

namespace Flubu.Builds.VSSolutionBrowsing
{
    public class VSProjectReference
    {
        public string Include
        {
            get { return include; }
            set { include = value; }
        }

        public IDictionary<string, string> Reference
        {
            get { return reference; }
        }

        private string include;
        private Dictionary<string, string> reference = new Dictionary<string, string>();
    }
}
