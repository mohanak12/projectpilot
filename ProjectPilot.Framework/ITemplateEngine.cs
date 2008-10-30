using System.Collections;

namespace ProjectPilot.Framework
{
    public interface ITemplateEngine
    {
        void ApplyTemplate(string templateName, Hashtable context, string outputFileName);
    }
}