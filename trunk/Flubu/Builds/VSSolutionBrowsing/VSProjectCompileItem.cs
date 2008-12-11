using System;
using System.Collections.Generic;
using System.Text;

namespace Flubu.Builds.VSSolutionBrowsing
{
    public class VSProjectCompileItem
    {
        public string Compile
        {
            get { return compile; }
            set { compile = value; }
        }

        private string compile;
    }
}
