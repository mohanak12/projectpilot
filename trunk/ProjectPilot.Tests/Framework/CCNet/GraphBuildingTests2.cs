using System.Linq;
using System.Text;
using MbUnit.Framework;
using ProjectPilot.Framework.CCNet;
using ProjectPilot.Framework.Charts;

namespace ProjectPilot.Tests.Framework.CCNet
{
    [TestFixture]
    public class GraphBuildingTests2
    {
        [Test,Ignore("TODO: Gregor")]
        public void Test()
        {
            ProjectStatsGraph graph = new ProjectStatsGraph();

            graph.AddParameter<int>("MbUnit TestFailed", "red");
            graph.AddParameter<int>("MbUnit TestPassed", "green");
            graph.AddParameter<int>("MbUnit TestIgnored", "yellow");

            ProjectStatsGraphData graphData = null; // todo: get this data

            using (FluentChart chart = FluentChart.Create("test", "builds", "value"))
            {
                foreach (ProjectStatsGraphParameter parameter in graph.GraphParameters)
                {
                    chart.AddLineSeries(parameter.ParameterName, parameter.SeriesColor)
                        .AddData(graphData.GetValuesForParameter(parameter.ParameterName));
                }
            }
        }
    }
}
