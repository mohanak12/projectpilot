namespace Accipio.Reporting
{
    public interface ITestReportGraphDataGenerator
    {
        TestReportGraphData GenerateData(TestRunsDatabase testRunsDatabase);
    }
}