using System.Collections.Generic;

namespace Flubu.Builds.VSSolutionBrowsing
{
    /// <summary>
    /// Holds information about content items inside of a VisualStudio project.
    /// </summary>
    public class VSProjectContentItem
    {
        public string Content
        {
            get { return content; }
            set { content = value; }
        }

        public IDictionary<string, string> CompileAttributes
        {
            get { return contentAttributes; }
        }

        private string content;
        private Dictionary<string, string> contentAttributes = new Dictionary<string, string>();
    }
}