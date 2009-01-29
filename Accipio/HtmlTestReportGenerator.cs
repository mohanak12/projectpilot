using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using Accipio.Reporting;
using Commons.Collections;
using NVelocity;
using NVelocity.App;

namespace Accipio
{
    public class HtmlTestReportGenerator : IHtmlTestReportGenerator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlTestReportGenerator"/> class.
        /// </summary>
        /// <param name="settings">The settings to use for generating the report.</param>
        public HtmlTestReportGenerator(HtmlTestReportGeneratorSettings settings)
        {
            this.settings = settings;
        }

        /// <summary>
        /// Generate HTML report files from the test runs database.
        /// </summary>
        /// <param name="testRunDatabase">Test runs database.</param>
        public void Generate(TestRunsDatabase testRunDatabase)
        {
            // copy the CSS file to the reports directory
            string cssFileFullPath = settings.CssFileName;

            if (false == Path.IsPathRooted(cssFileFullPath))
                cssFileFullPath = Path.Combine(settings.TemplatesDirectory, cssFileFullPath);

            string cssCopiedPath = Path.Combine(settings.OutputDirectory, Path.GetFileName(cssFileFullPath));

            // make sure the path exists
            AccipioHelper.EnsureDirectoryPathExists(cssCopiedPath, true);

            File.Copy(cssFileFullPath, cssCopiedPath, true);

            FileVersionInfo version = System.Diagnostics.FileVersionInfo.GetVersionInfo (
                System.Reflection.Assembly.GetExecutingAssembly ().Location);
            string accipioVersion = version.FileVersion;
                
            Hashtable context = new Hashtable();
            context.Add("db", testRunDatabase);
            context.Add("accipioVersion", accipioVersion);

            GenerateReportFile(
                "${settings.ProjectName}_TestRunsHistory.htm",
                "TestRunsHistory.vm.htm",
                context);
        }

        private void GenerateReportFile (
            string outputFileNameFormat, 
            string templateFileName, 
            Hashtable context)
        {
            VelocityEngine velocity = new VelocityEngine();
            ExtendedProperties props = new ExtendedProperties();
            velocity.Init(props);
            VelocityContext velocityContext = new VelocityContext(context);
            velocityContext.Put("settings", settings);
            velocityContext.Put("reportTime", DateTime.Now);

            string fullTemplateFileName = Path.Combine(settings.TemplatesDirectory, templateFileName);

            Template template = velocity.GetTemplate(fullTemplateFileName);

            string outputFileName;

            using (StringWriter writer = new StringWriter(CultureInfo.InvariantCulture))
            {
                velocity.Evaluate(velocityContext, writer, "dummy", outputFileNameFormat);
                writer.Flush();
                outputFileName = writer.ToString();
            }

            string fullOutputFileName = Path.Combine(settings.OutputDirectory, outputFileName);

            // make sure the path exists
            AccipioHelper.EnsureDirectoryPathExists(fullOutputFileName, true);

            using (Stream stream = File.Open(fullOutputFileName, FileMode.Create))
            {
                using (TextWriter writer = new StreamWriter(stream))
                {
                    template.Merge(velocityContext, writer);
                }
            }
        }

        private readonly HtmlTestReportGeneratorSettings settings;
    }
}
