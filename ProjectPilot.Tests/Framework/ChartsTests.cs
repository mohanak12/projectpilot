using System;
using System.Drawing.Imaging;
using MbUnit.Framework;
using ProjectPilot.Common.Charts;
using ZedGraph;

namespace ProjectPilot.Tests.Framework
{
    [TestFixture]
    public class ChartsTests
    {
        [Test]
        public void Test()
        {
            FluentChart chart = FluentChart
                .Create("Test chart", "xaxis", "yaxis")
                .SetXAxis (0, 100)
                .SetYAxis (45, 65)
                .AddBarSeries("Test bar", "red");

            Random rnd = new Random();
            for (int i = 0; i < 100; i++)
            {
                chart
                    .AddDataPair(i, rnd.Next(10) + 50);
            }

            chart
                .AddLineSeries("Test bar 2", "blue")
                .SetLineWidth(2)
                .SetSymbol(SymbolType.Circle, "green", 5, false);

            for (int i = 0; i < 100; i++)
            {
                chart
                    .AddDataPair(i, rnd.Next(10) + 50);
            }

            chart
                .ExportToBitmap("test.png", ImageFormat.Png, 1000, 800);
        }
    }
}
