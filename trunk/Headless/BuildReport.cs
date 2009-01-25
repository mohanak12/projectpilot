using System;
using System.Collections.Generic;

namespace Headless
{
    public class BuildReport
    {
        public BuildOutcome BuildOutcome
        {
            get
            {
                bool allInitial = true;
                bool allSuccessful = true;

                foreach (BuildStageReport stageReport in stageReports.Values)
                {
                    if (stageReport.StageOutcome == BuildOutcome.Failed)
                        return BuildOutcome.Failed;
                    if (stageReport.StageOutcome == BuildOutcome.InProgress
                        || stageReport.StageOutcome == BuildOutcome.NotExecuted)
                        return BuildOutcome.InProgress;
                    if (stageReport.StageOutcome != BuildOutcome.Successful)
                        allSuccessful = false;
                    if (stageReport.StageOutcome != BuildOutcome.Initial)
                        allInitial = false;
                }

                if (allSuccessful)
                    return BuildOutcome.Successful;
                if (allInitial)
                    return BuildOutcome.Initial;

                return BuildOutcome.InProgress;
            }
        }

        public IDictionary<string, BuildStageReport> StageReports
        {
            get { return stageReports; }
        }

        private Dictionary<string, BuildStageReport> stageReports = new Dictionary<string, BuildStageReport>();
    }
}