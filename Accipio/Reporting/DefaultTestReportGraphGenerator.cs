using System.Drawing.Imaging;
using System.IO;
using ProjectPilot.Framework.Charts;
using ZedGraph;

namespace Accipio.Reporting
{
    public class DefaultTestReportGraphGenerator : ITestReportGraphGenerator
    {
        public void GenerateGraph(TestReportGraphData graphData, HtmlTestReportGeneratorSettings settings)
        {
            const int GraphWidth = 1000;
            const int GraphHeight = 500;

            using (FluentChart chart = FluentChart.Create(string.Empty, string.Empty, graphData.GraphName))
            {
                chart
                    .SetGraphSize(GraphWidth, GraphHeight)
                    .SetBarSettings(BarType.Stack, 0)
                    .SetXAxis(0, graphData.ValuesCount- 1);

                string[] colors = { "green", "red", "yellow" };

                int i = 0;
                foreach (string status in graphData.GraphValues.Keys)
                {
                    chart
                        .AddBarSeries(status, colors[i++])
                        .AddData(graphData.GraphValues[status]);
                }

                chart
                    .ExportToBitmap(
                    Path.GetFullPath(Path.Combine(settings.OutputDirectory, graphData.GraphFileName)),
                    ImageFormat.Png,
                    GraphWidth,
                    GraphHeight);
            }
        }
    }
}