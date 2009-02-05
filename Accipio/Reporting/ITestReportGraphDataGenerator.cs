namespace Accipio.Reporting
{
    /// <summary>
    /// Generates data structure used for generating Accipio report graphs.
    /// </summary>
    public interface ITestReportGraphDataGenerator
    {
        /// <summary>
        /// Generates data structure used for generating Accipio report graphs.
        /// </summary>
        /// <param name="testRunsDatabase">The test runs database.</param>
        /// <returns><see cref="TestReportGraphData"/> object containing the report graph data.</returns>
        TestReportGraphData GenerateData(TestRunsDatabase testRunsDatabase);
    }
}