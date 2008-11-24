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

        public void IncValue(int buildId, string parameterName, double value)
        {
            Dictionary<string, double> entry = dataByBuildsByParametersByValue[buildId];
            entry[parameterName] += value;
        }

        public void ClearDictionary()
        {
            dataByBuildsByParametersByValue.Clear();
        }

        public double GetValue(int buildId, string parameterName)
        {
            Dictionary<string, double> value = dataByBuildsByParametersByValue[buildId];

            return value[parameterName];
        }

        public SortedList<int, double> GetValuesForParameter (string parameterName)
        {
            SortedList<int, double> values = new SortedList<int, double>();

            int i = 0;
            foreach (KeyValuePair<int, Dictionary<string, double>> pair in dataByBuildsByParametersByValue)
            {
                Dictionary<string, double> dictionaryOfParameters = pair.Value;
                if (dictionaryOfParameters.ContainsKey(parameterName))
                {
                    double parameterValue = dictionaryOfParameters[parameterName];
                    values.Add(i++, parameterValue);
                }
            }
        
            return values;
        }

        public void SetValue(int buildId, string parameterName, double value)
        {
            if (!dataByBuildsByParametersByValue.ContainsKey(buildId))
            {
                dataByBuildsByParametersByValue.Add(buildId, new Dictionary<string, double> { { parameterName, value } });
            }
            else
            {
                dataByBuildsByParametersByValue[buildId].Add(parameterName, value);
            }
        }

        private SortedList<DateTime, ProjectStatsBuildEntry> buildsSortedByStartTime;
        private Dictionary<int, Dictionary<string, double>> 
            dataByBuildsByParametersByValue = new Dictionary<int, Dictionary<string, double>>();

        private Dictionary<int, int> buildsToIntegers = new Dictionary<int, int>();
    }
}