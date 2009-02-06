using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using ZedGraph;

namespace ProjectPilot.Common.Charts
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
            barItem.Color = ColorTranslator.FromHtml(color);
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

        public FluentChart AddData(IList<int> dataValues)
        {
            int minValue = 0;
            int maxValue = dataValues.Count - 1;

            for (int i = minValue; i <= maxValue; i++)
                AddDataPair(i, dataValues[i]);

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

        public FluentChart AddDataByTime(SortedList<DateTime, double> dataValues, DateTime minDate, DateTime maxDate)
        {
            DateTime min = new DateTime(minDate.Year, minDate.Month, minDate.Day);
            DateTime max = new DateTime(maxDate.Year, maxDate.Month, maxDate.Day);

            for (DateTime date = min; date <= max; date = date.AddDays(1))
            {
                bool addEmptyRow = true;

                foreach (DateTime temp in dataValues.Keys)
                {
                    if (temp.ToShortDateString() == date.ToShortDateString())
                    {
                        AddDataPair((double)new XDate(temp), dataValues[temp]);
                        addEmptyRow = false;
                    }
                }

                if (addEmptyRow)
                    AddDataPair((double)new XDate(date), 0);
            }

            return this;
        }

        public FluentChart AddDataPair(double xValue, double yValue)
        {
            currentCurveItem.AddPoint(xValue, yValue);
            return this;
        }

        public FluentChart AddLineSeries(string label, string color)
        {
            LineItem lineItem = new LineItem(label);
            lineItem.Color = ColorTranslator.FromHtml(color);
            lineItem.Symbol = new Symbol(SymbolType.None, Color.Black);
            zedGraph.GraphPane.CurveList.Add(lineItem);
            currentCurveItem = lineItem;
            return this;
        }

        public FluentChart AddStackedData(IList<int> dataValues)
        {
            int minValue = 0;
            int maxValue = dataValues.Count - 1;

            for (int i = minValue; i <= maxValue; i++)
                AddStackedDataPair(i, dataValues[i]);

            return this;
        }

        public FluentChart AddStackedData(SortedList<int, double> dataValues)
        {
            int minValue = 0;
            int maxValue = dataValues.Keys[dataValues.Count - 1];

            for (int i = minValue; i <= maxValue; i++)
            {
                if (dataValues.ContainsKey(i))
                    AddStackedDataPair(i, dataValues[i]);
                else
                    AddStackedDataPair(i, 0);
            }

            return this;
        }

        public FluentChart AddStackedDataPair(double xValue, double yValue)
        {
            double stackedValue = 0;
            if (stackedValues.ContainsKey(xValue))
                stackedValue = stackedValues[xValue];

            currentCurveItem.AddPoint(xValue, yValue + stackedValue);
            stackedValues[xValue] = yValue + stackedValue;

            return this;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or
        /// resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public FluentChart ExportToBitmap (
            string fileName, 
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
            zedGraph.GraphPane.XAxis.Scale.MinorStepAuto = true;
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

        public FluentChart SetGraphSize (int width, int height)
        {
            zedGraph.GraphPane.Rect = new RectangleF(0, 0, width, height);
            return this;
        }

        /// <summary>
        /// Sets the color filling for the current line item.
        /// </summary>
        /// <param name="color">The color to use for filling.</param>
        /// <returns>This same instance of the <see cref="FluentChart"/> object.</returns>
        public FluentChart SetFilling(string color)
        {
            ((LineItem)currentCurveItem).Line.Fill = new Fill(ColorTranslator.FromHtml(color));
            return this;
        }

        /// <summary>
        /// Sets the global font for the chart.
        /// </summary>
        /// <param name="familyName">Name of the font family.</param>
        /// <param name="emSize">Font size.</param>
        /// <param name="isBold">if set to <c>true</c> the font will be bold.</param>
        /// <returns>
        /// This same instance of the <see cref="FluentChart"/> object.
        /// </returns>
        public FluentChart SetFont(string familyName, float emSize, bool isBold)
        {
            SetFontSpec(zedGraph.GraphPane.Title.FontSpec, familyName, emSize, isBold);
            SetFontSpec(zedGraph.GraphPane.YAxis.Title.FontSpec, familyName, emSize, isBold);
            SetFontSpec(zedGraph.GraphPane.YAxis.Scale.FontSpec, familyName, emSize, isBold);
            SetFontSpec(zedGraph.GraphPane.XAxis.Scale.FontSpec, familyName, emSize, isBold);
            SetFontSpec(zedGraph.GraphPane.XAxis.Title.FontSpec, familyName, emSize, isBold);
            SetFontSpec(zedGraph.GraphPane.Legend.FontSpec, familyName, emSize, isBold);
            return this;
        }

        [CLSCompliant(false)]
        public FluentChart SetSymbol(SymbolType symbolType, string symbolColor, float symbolSize, bool fillSymbol)
        {
            LineItem lineItem = (LineItem)currentCurveItem;
            lineItem.Symbol = new Symbol(symbolType, ColorTranslator.FromHtml(symbolColor));
            lineItem.Symbol.Size = symbolSize;
            
            if (fillSymbol)
            {
                lineItem.Symbol.Fill = new Fill(ColorTranslator.FromHtml(symbolColor));
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
                zedGraph.GraphPane.XAxis.Title.Text = String.Empty;
                zedGraph.GraphPane.XAxis.Title.IsVisible = false;
            }

            if (String.IsNullOrEmpty(yAxisTitle))
            {
                zedGraph.GraphPane.YAxis.Title.Text = String.Empty;
                zedGraph.GraphPane.YAxis.Title.IsVisible = false;
            }
        }

        private void SetFontSpec (FontSpec fontSpec, string familyName, float emSize, bool isBold)
        {
            fontSpec.Family = familyName;
            fontSpec.Size = emSize;
            fontSpec.IsBold = isBold;
        }

        private CurveItem currentCurveItem;
        private bool disposed;
        private Dictionary<double, double> stackedValues = new Dictionary<double, double>();
        private ZedGraphControl zedGraph;
    }
}