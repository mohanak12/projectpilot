using System;
using System.Globalization;
using System.IO;
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

            graph.AddParameter<int>("MbUnit TestFailed", "Red");
            graph.AddParameter<int>("MbUnit TestPassed", "Green");
            graph.AddParameter<int>("MbUnit TestIgnored", "Yellow");
            graph.AddParameter<TimeSpan>("Duration", "Blue");

            //Prepare test data
            ProjectStatsData data;
            using (Stream stream = File.OpenRead(@"..\..\..\Data\Samples\ccnet.stats.xml"))
            {
                data = CCNetProjectStatisticsPlugIn.Load(stream);
            }

            ProjectStatsGraphData graphData = new ProjectStatsGraphData(data);

            foreach (ProjectStatsBuildEntry entry in data.Builds)
            {
                foreach (ProjectStatsGraphParameter parameter in graph.GraphParameters)
                {
                    double value = 0;

                    if (entry.Parameters.ContainsKey(parameter.ParameterName))
                    {
                        if (parameter.ParameterType == typeof(TimeSpan))
                        {
                            value = TimeSpan.Parse(entry.Parameters[parameter.ParameterName]).TotalSeconds;
                        }
                        else
                        {
                            value = Convert.ToDouble(entry.Parameters[parameter.ParameterName], 
                                CultureInfo.InvariantCulture);
                        }
                    }
                    
                    graphData.SetValue(entry.BuildId, parameter.ParameterName, value);
                }
            }





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
