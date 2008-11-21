using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ProjectPilot.Framework.CCNet
{
    public class ProjectStatsGraphData
    {
        public ProjectStatsGraphData(ProjectStatsData statsData)
        {
            // sort builds by their start times
            buildsSortedByStartTime = new SortedList<DateTime, ProjectStatsBuildEntry>();

            foreach (ProjectStatsBuildEntry build in statsData.Builds)
                buildsSortedByStartTime.Add(build.BuildStartTime, build);

            // now find out each build's order id 
            foreach (ProjectStatsBuildEntry build in buildsSortedByStartTime.Values)
                buildsToIntegers.Add(build.BuildId, buildsSortedByStartTime.IndexOfValue(build));
        }

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "buildId")]
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "parameterName")]
        public double GetValue(string buildId, string parameterName)
        {
            throw new NotImplementedException();
        }
        public SortedList<int, double> GetValuesForParameter (string parameterName)
        {
            SortedList<int, double> values = new SortedList<int, double>();

            foreach (KeyValuePair<int, Dictionary<string, double>> pair in dataByBuildsByParametersByValue)
            {
                int buildId = pair.Key;
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

        public void SetValue(int buildId, string parameterName, double value)
        {
            Dictionary<string, double> entry = new Dictionary<string, double>();
            entry.Add(parameterName, value);

            if (!dataByBuildsByParametersByValue.ContainsKey(buildId))
            {
                dataByBuildsByParametersByValue.Add(buildId, entry);
            }
            else
            {
                dataByBuildsByParametersByValue[buildId].Add(parameterName, value);
            }
        }

        private SortedList<DateTime, ProjectStatsBuildEntry> buildsSortedByStartTime;
        private Dictionary<int, Dictionary<string, double>> dataByBuildsByParametersByValue = new Dictionary<int, Dictionary<string, double>>();
        private Dictionary<int, int> buildsToIntegers = new Dictionary<int, int>();
    }
}