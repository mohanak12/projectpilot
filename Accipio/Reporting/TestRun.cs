using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Accipio.Reporting
{
    public class TestRun
    {
        public void AddTestSuiteRun (TestSuiteRun testSuiteRun)
        {
            testSuitesRuns.Add(testSuiteRun.TestSuiteId, testSuiteRun);
        }

        public DateTime EndTime
        {
            get { return endTime; }
            set { endTime = value; }
        }

        public DateTime StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }

        public Version Version
        {
            get { return version; }
            set { version = value; }
        }

        private DateTime endTime;
        private DateTime startTime;
        private Dictionary<string, TestSuiteRun> testSuitesRuns = new Dictionary<string, TestSuiteRun>();
        private Version version;
    }
}
