using System;
using System.Collections.Generic;
using System.Text;

namespace Flubu.Builds.VSSolutionBrowsing
{
    /// <summary>
    /// Holds information about C# files inside of VisualStudio project.
    /// </summary>
    public class VSProjectCompileItem
    {   
        public string Compile
        {
            get { return compile; }
            set { compile = value; }
        }

        public IDictionary<string, string> CompileAttributes
        {
            get { return compileAttributes; }
        }

        private string compile;
        private Dictionary<string, string> compileAttributes = new Dictionary<string, string>();
    }
}
