using System.Collections;
using System.IO;
using Commons.Collections;
using NVelocity;
using NVelocity.App;

namespace ProjectPilot.Framework
{
    public interface ITemplateEngine
    {
        void ApplyTemplate(string templateName, Hashtable context, string outputFileName);
    }

    public class DefaultTemplateEngine : ITemplateEngine
    {
        public DefaultTemplateEngine(IFileManager fileManager)
        {
            this.fileManager = fileManager;
        }

        public void ApplyTemplate(string templateName, Hashtable context, string outputFileName)
        {
            VelocityEngine velocity = new VelocityEngine();
            ExtendedProperties props = new ExtendedProperties();
            velocity.Init(props);

            string templateFileName = fileManager.GetFullFileName("Templates", templateName);

            Template template = velocity.GetTemplate(templateFileName);

            using (Stream stream = File.Open(outputFileName, FileMode.Create))
            {
                using (TextWriter writer = new StreamWriter(stream))
                {
                    VelocityContext velocityContext = new VelocityContext(context);
                    template.Merge(velocityContext, writer);
                }
            }
        }

        private readonly IFileManager fileManager;
    }
}