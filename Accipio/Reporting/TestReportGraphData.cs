using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

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

        public int ValuesCount
        {
            get { return graphValues.Values[0].Count; }
        }

        public void AddDataValue(string series, int value)
        {
            graphValues[series].Add(value);
        }

        public void AddSeries(params string[] seriesNames)
        {
            foreach (string seriesName in seriesNames)
                graphValues.Add(seriesName, new List<int>());
        }

        private string graphFileName;
        private string graphName;
        private SortedList<string, List<int>> graphValues = new SortedList<string, List<int>>();
    }
}
