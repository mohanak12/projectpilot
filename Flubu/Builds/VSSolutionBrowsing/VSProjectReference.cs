using System;
using System.Collections.Generic;
using System.Text;

namespace Flubu.Builds.VSSolutionBrowsing
{
    public class VSProjectReference
    {
        public string HintPath
        {
            get { return hintPath; }
            set { hintPath = value; }
        }

        public string Include
        {
            get { return include; }
            set { include = value; }
        }

        public bool? SpecificVersion
        {
            get { return specificVersion; }
            set { specificVersion = value; }
        }

        private string hintPath;
        private string include;
        private bool? specificVersion;
    }
}
