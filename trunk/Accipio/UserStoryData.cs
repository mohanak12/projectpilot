using System.Collections;
using System.Collections.Generic;

namespace Accipio
{
    public class UserStoryData
    {
        public IList<ReportCase> TestCases
        {
            get { return testCases; }
        }

        public void AddReportCase(ReportCase reportCase)
        {
            testCases.Add(reportCase);
        }

        private readonly List<ReportCase> testCases = new List<ReportCase>();
    }
}