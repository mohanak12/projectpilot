using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace Accipio.Reporting
{
    public class TestReportGraphData
    {
        public TestReportGraphData(string graphName, string graphFileName)
        {
            this.graphName = graphName;
            this.graphFileName = graphFileName;
        }

        public string GraphFileName
        {
            get { return graphFileName; }
        }

        public string GraphName
        {
            get { return graphName; }
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public IDictionary<string, List<int>> GraphValues
        {
            get { return graphValues; }
        }

        public IDictionary<string, string> SeriesColors
        {
            get { return seriesColors; }
        }

        public int ValuesCount
        {
            get { return graphValues.Values[0].Count; }
        }

        public void AddDataValue(string series, int value)
        {
            graphValues[series].Add(value);
        }

        public void AddSeries(string seriesName, string seriesColor)
        {
            seriesOrder.Add(seriesName);
            graphValues.Add(seriesName, new List<int>());
            seriesColors.Add(seriesName, seriesColor);
        }

        public IList<string> ListSeriesInOrder()
        {
            return seriesOrder;
        }

        private string graphFileName;
        private string graphName;
        private SortedList<string, List<int>> graphValues = new SortedList<string, List<int>>();
        private Dictionary<string, string> seriesColors = new Dictionary<string, string>();
        private List<string> seriesOrder = new List<string>();
    }
}
