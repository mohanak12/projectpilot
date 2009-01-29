using System.IO;

namespace Accipio
{
    public class HtmlTestReportGeneratorSettings
    {
        public HtmlTestReportGeneratorSettings(string projectFileName)
        {
            this.projectName = projectFileName;
        }

        public string CssFileName
        {
            get { return cssFileName; }
            set { cssFileName = value; }
        }
        
        public string CssShortFileName
        {
            get { return Path.GetFileName(CssFileName); }
        }

        public string OutputDirectory
        {
            get { return outputDirectory; }
            set { outputDirectory = value; }
        }

        public string ProjectName
        {
            get { return projectName; }
            set { projectName = value; }
        }

        public string TemplatesDirectory
        {
            get { return templatesDirectory; }
            set { templatesDirectory = value; }
        }

        private string cssFileName = "TestReport.css";
        private string outputDirectory = "Reports";
        private string projectName;
        private string templatesDirectory = "Templates";
    }
}