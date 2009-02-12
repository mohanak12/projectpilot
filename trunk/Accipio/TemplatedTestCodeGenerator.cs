using System.IO;
using System.Text;
using Commons.Collections;
using NVelocity;
using NVelocity.App;
using NVelocity.Runtime;

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
            velocity.SetProperty(RuntimeConstants.RESOURCE_LOADER, "file");
            //velocity.SetProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, settings.TemplatesDirectory);
            velocity.Init();

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