using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using ZedGraph;

namespace ProjectPilot.Framework.Charts
{
    public class FluentChart : IDisposable
    {
        public static FluentChart Create(string chartTitle, string xAxisTitle, string yAxisTitle)
        {
            return new FluentChart(chartTitle, xAxisTitle, yAxisTitle);
        }

        public FluentChart AddBarSeries(string label, string color)
        {
            BarItem barItem = new BarItem(label);
            barItem.Color = Color.FromName(color);
            barItem.Bar.Border = new Border(false, Color.Black, 0);
            barItem.Bar.Fill = new Fill(barItem.Color);
            zedGraph.GraphPane.CurveList.Add(barItem);
            currentCurveItem = barItem;
            return this;
        }

        public FluentChart AddData(SortedList<int, double> dataValues)
        {
            int minValue = 0;
            int maxValue = dataValues.Keys[dataValues.Count - 1];

            for (int i = minValue; i <= maxValue; i++)
            {
                if (dataValues.ContainsKey(i))
                    AddDataPair(i, dataValues[i]);
                else
                    AddDataPair(i, 0);
            }

            return this;
        }

        public FluentChart AddDataByDate(SortedList<DateTime, double> dataValues, DateTime minDate, DateTime maxDate)
        {
            for (DateTime date = minDate; date <= maxDate; date = date.AddDays(1))
            {
                if (dataValues.ContainsKey(date))
                    AddDataPair((double)new XDate(date), dataValues[date]);
                else
                    AddDataPair((double)new XDate(date), 0);
            }

            return this;
        }

        public FluentChart AddLineSeries(string label, string color)
        {
            LineItem lineItem = new LineItem(label);
            lineItem.Color = Color.FromName(color);
            lineItem.Symbol = new Symbol(SymbolType.None, Color.Black);
            zedGraph.GraphPane.CurveList.Add(lineItem);
            currentCurveItem = lineItem;
            return this;
        }

        public FluentChart AddDataPair(double xValue, double yValue)
        {
            currentCurveItem.AddPoint(xValue, yValue);
            return this;
        }

        public FluentChart ExportToBitmap (string fileName, 
            ImageFormat imageFormat,
            int width, 
            int height)
        {
            zedGraph.AxisChange();
            zedGraph.Update();

            using (Image image = zedGraph.GraphPane.GetImage(width, height, 10, true))
            {
                image.Save(fileName, imageFormat);
            }

            return this;
        }

        [CLSCompliant(false)]
        public FluentChart SetBarSettings (BarType barType, float minClusterGap)
        {
            zedGraph.GraphPane.BarSettings.Type = barType;
            zedGraph.GraphPane.BarSettings.MinClusterGap = minClusterGap;
            return this;
        }

        public FluentChart SetLabelsToXAxis(IList<string> labels)
        {
            zedGraph.GraphPane.XAxis.Type = AxisType.Text;
            zedGraph.GraphPane.XAxis.Scale.TextLabels = labels.ToArray();
            return this;
        }

        public FluentChart SetLineWidth(float lineWidth)
        {
            LineItem lineItem = (LineItem)currentCurveItem;
            lineItem.Line.Width = lineWidth;
            return this;
        }

        [CLSCompliant(false)]
        public FluentChart SetSymbol(SymbolType symbolType, string symbolColor, float symbolSize, bool fillSymbol)
        {
            LineItem lineItem = (LineItem)currentCurveItem;
            lineItem.Symbol = new Symbol(symbolType, Color.FromName(symbolColor));
            lineItem.Symbol.Size = symbolSize;
            
            if (fillSymbol)
            {
                lineItem.Symbol.Fill = new Fill(Color.FromName(symbolColor));
            }

            return this;
        }

        public FluentChart SetXAxis(double minValue, double maxValue)
        {
            zedGraph.GraphPane.XAxis.Scale.Min = minValue;
            zedGraph.GraphPane.XAxis.Scale.Max = maxValue;
            return this;
        }

        public FluentChart SetYAxis(double minValue, double maxValue)
        {
            zedGraph.GraphPane.YAxis.Scale.Min = minValue;
            zedGraph.GraphPane.YAxis.Scale.Max = maxValue;
            return this;
        }

        public FluentChart UseDateAsAxisY(DateTime startDate, DateTime endDate)
        {
            zedGraph.GraphPane.XAxis.MajorTic.IsBetweenLabels = true;
            zedGraph.GraphPane.XAxis.Scale.Min = (double)new XDate (startDate);
            zedGraph.GraphPane.XAxis.Scale.Max = (double)new XDate (endDate);
            zedGraph.GraphPane.XAxis.Type = AxisType.Date;
            return this;
        }

        private FluentChart(string chartTitle, string xAxisTitle, string yAxisTitle)
        {
            zedGraph = new ZedGraphControl();
            zedGraph.GraphPane = new GraphPane(new RectangleF(0, 0, 1000, 1000), chartTitle, xAxisTitle, yAxisTitle);
            zedGraph.GraphPane.Legend.Border.IsVisible = false;
            zedGraph.GraphPane.Legend.Position = LegendPos.Bottom;
            zedGraph.GraphPane.XAxis.MajorGrid.IsVisible = true;
            zedGraph.GraphPane.YAxis.MajorGrid.IsVisible = true;
            zedGraph.GraphPane.Chart.Fill = new Fill(Color.FromArgb(unchecked((int)0xffD1B2FF)), Color.White, 90);

            if (String.IsNullOrEmpty(xAxisTitle))
            {
                zedGraph.GraphPane.XAxis.Title.Text = "";
                zedGraph.GraphPane.XAxis.Title.IsVisible = false;
            }

            if (String.IsNullOrEmpty(yAxisTitle))
            {
                zedGraph.GraphPane.YAxis.Title.Text = "";
                zedGraph.GraphPane.YAxis.Title.IsVisible = false;
            }
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or
        /// resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the object.
        /// </summary>
        /// <param name="disposing">If <code>false</code>, cleans up native resources. 
        /// If <code>true</code> cleans up both managed and native resources</param>
        protected virtual void Dispose(bool disposing)
        {
            if (false == disposed)
            {
                if (disposing)
                {
                    if (zedGraph != null)
                        zedGraph.Dispose();
                }

                disposed = true;
            }
        }

        private bool disposed;

        #endregion
                
        private ZedGraphControl zedGraph;
        private CurveItem currentCurveItem;
    }
}
