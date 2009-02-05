using System.Collections.Generic;
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
                    .SetFont("Palatino Linotype", 11, true)
                    .SetXAxis(0, graphData.ValuesCount- 1);

                foreach (string seriesName in graphData.ListSeriesInOrder())
                {
                    chart
                        .AddLineSeries(seriesName, graphData.SeriesColors[seriesName])
                        .SetFilling (graphData.SeriesColors[seriesName])
                        .AddStackedData(graphData.GraphValues[seriesName]);
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