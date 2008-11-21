using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ProjectPilot.Framework.CCNet
{
    public class ProjectStatsGraphData
    {
        public ProjectStatsGraphData (ProjectStatsData statsData)
        {
            // sort builds by their start times
            buildsSortedByStartTime = new SortedList<DateTime, ProjectStatsBuildEntry>();

            foreach (ProjectStatsBuildEntry build in statsData.Builds)
                buildsSortedByStartTime.Add(build.BuildStartTime, build);

            // now find out each build's order id 
            foreach (ProjectStatsBuildEntry build in buildsSortedByStartTime.Values)
                buildsToIntegers.Add(build.BuildLabel, buildsSortedByStartTime.IndexOfValue(build));
        }

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "buildId")]
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "parameterName")]
        public double GetValue(string buildId, string parameterName)
        {
            throw new NotImplementedException();
        }

        public SortedList<int,double> GetValuesForParameter (string parameterName)
        {
            SortedList<int,double> values = new SortedList<int, double>();

            foreach (KeyValuePair<string, Dictionary<string, double>> pair in dataByBuildsByParametersByValue)
            {
                string buildId = pair.Key;
                Dictionary<string, double> dictionaryOfParameters = pair.Value;
                if (dictionaryOfParameters.ContainsKey(parameterName))
                {
                    int buildOrderId = buildsToIntegers[buildId];
                    double parameterValue = dictionaryOfParameters[parameterName];
                    values.Add(buildOrderId, parameterValue);
                }
            }

            return values;
        }

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "value")]
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "buildId")]
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "parameterName")]
        public void SetValue(string buildId, string parameterName, double value)
        {
            throw new NotImplementedException();
        }

        private SortedList<DateTime, ProjectStatsBuildEntry> buildsSortedByStartTime;
        private Dictionary<string, Dictionary<string, double>> dataByBuildsByParametersByValue = new Dictionary<string, Dictionary<string, double>>();
        private Dictionary<string, int> buildsToIntegers = new Dictionary<string, int>();
    }
}