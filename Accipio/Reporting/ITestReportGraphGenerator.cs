namespace Accipio.Reporting
{
    public interface ITestReportGraphGenerator
    {
        void GenerateGraph(TestReportGraphData graphData, HtmlTestReportGeneratorSettings settings);
    }
}