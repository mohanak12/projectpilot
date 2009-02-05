using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Accipio;
using Accipio.Reporting;
using MbUnit.Framework;

namespace ProjectPilot.Tests.AccipioTests.Reporting
{
    /// <summary>
    /// Tests generating Accipio test report graphs using <see cref="DefaultTestReportGraphGenerator"/> class.
    /// </summary>
    [TestFixture]
    public class GraphGenerationTests
    {
        [Test]
        public void Test()
        {
            HtmlTestReportGeneratorSettings settings = new HtmlTestReportGeneratorSettings("Test project");
            TestReportGraphData graphData = new TestReportGraphData("test graph", "TestGraph2.png");
            graphData.AddSeries("successful", "#75FF47");
            graphData.AddSeries("failed", "#FF6B90");
            graphData.AddSeries("not implemented", "#FFFCA8");

            Random rnd = new Random();
            for (int i = 0; i < 100; i++)
            {
                graphData.AddDataValue("successful", rnd.Next(100));
                graphData.AddDataValue("failed", rnd.Next(20));
                graphData.AddDataValue("not implemented", rnd.Next(5));
            }

            ITestReportGraphGenerator graphGenerator = new DefaultTestReportGraphGenerator();
            graphGenerator.GenerateGraph(graphData, settings);
        }
    }
}
