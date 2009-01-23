using System;
using System.Globalization;

namespace Accipio
{
    public class UserStoryGenerator : IHtmlTestReportGenerator
    {
        /// <summary>
        /// Initializes a new instance of the UserStoryGenerator class.
        /// </summary>
        /// <param name="writer">Interface of <see cref="ICodeWriter" />.</param>
        public UserStoryGenerator(ICodeWriter writer)
        {
            this.writer = writer;
        }
        
        public void Generate(ReportData reportData)
        {
            throw new NotImplementedException();
        }

        public void Generate(TestReports testReports)
        {
            WriteLine("<html>");
            WriteLine("    {0}", testReports.UserStories[0]);
            WriteLine("</html>");
        }

        private void WriteLine(string line)
        {
            writer.WriteLine(line);
        }

        private void WriteLine(
            string format,
            params object[] args)
        {
            writer.WriteLine(string.Format(CultureInfo.InvariantCulture, format, args));
        }

        private readonly ICodeWriter writer;
    }
}