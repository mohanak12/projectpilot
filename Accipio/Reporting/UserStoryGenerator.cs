using System.Globalization;
using Accipio.Reporting;

namespace Accipio.Reporting
{
    public class UserStoryGenerator
    {
        /// <summary>
        /// Initializes a new instance of the UserStoryGenerator class.
        /// </summary>
        /// <param name="writer">Interface of <see cref="ICodeWriter" />.</param>
        public UserStoryGenerator(ICodeWriter writer)
        {
            this.writer = writer;
        }

        public void Generate(UserStoryData testCases)
        {
            WriteLine("<html>");
            foreach (TestCaseExecutionReport testCase in testCases.TestCases)
            {
                WriteLine("    {0} | {1} | {2} | {3}", testCase.TestCaseId, testCase.StartTime, testCase.ExecutionStatus, testCase.ReportDetails);
            }

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