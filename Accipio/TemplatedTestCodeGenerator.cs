using System.IO;
using System.Text;
using Commons.Collections;
using NVelocity;
using NVelocity.App;

namespace Accipio
{
    public class TemplatedTestCodeGenerator : ITestCodeGenerator
    {
        public TemplatedTestCodeGenerator (string templateFileName, string outputFileName)
        {
            this.templateFileName = templateFileName;
            this.outputFileName = outputFileName;
        }

        public void Generate(TestSuite testSuite)
        {
            VelocityEngine velocity = new VelocityEngine();
            ExtendedProperties props = new ExtendedProperties();
            velocity.Init(props);

            VelocityContext velocityContext = new VelocityContext();
            velocityContext.Put("testSuite", testSuite);

            Template template = velocity.GetTemplate(templateFileName, new UTF8Encoding(false).WebName);

            using (StreamWriter writer = new StreamWriter(outputFileName, false, Encoding.UTF8))
            {
                template.Merge(velocityContext, writer);
            }
        }

        private readonly string templateFileName;
        private readonly string outputFileName;
    }
}