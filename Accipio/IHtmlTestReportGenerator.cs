
using Accipio.Reporting;

namespace Accipio
{
    public interface IHtmlTestReportGenerator
    {
        /// <summary>
        /// Generate HTML report files from the test runs database.
        /// </summary>
        /// <param name="testRunDatabase">Test runs database.</param>
        void Generate(TestRunsDatabase testRunDatabase);
    }
}
