using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Accipio.Reporting
{
    public class TestRun
    {
        public TimeSpan Duration
        {
            get { return endTime - startTime; }
        }

        public string DurationString
        {
            get { return String.Format(CultureInfo.InvariantCulture, "{0:00}:{1:00}", Duration.Minutes, Duration.Seconds); }
        }

        public DateTime EndTime
        {
            get { return endTime; }
            set { endTime = value; }
        }

        public string FileName
        {
            get
            {
                return String.Format(
                    CultureInfo.InvariantCulture,
                    "TestRun_{0:yyyy}{0:MM}{0:dd}_{0:HH}{0:mm}{0:ss}.htm",
                    EndTime);
            }
        }

        public DateTime StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }

        public int TestCasesTotal
        {
            get
            {
                return testSuitesRuns.Sum(n => n.Value.TestCasesTotal);
            }
        }

        public int TestCasesSuccess
        {
            get
            {
                return testSuitesRuns.Sum(n => n.Value.TestCasesSuccess);
            }
        }

        public int TestCasesFail
        {
            get
            {
                return testSuitesRuns.Sum(n => n.Value.TestCasesFail);
            }
        }

        public int TestCasesNotImplemented
        {
            get
            {
                return testSuitesRuns.Sum(n => n.Value.TestCasesNotImplemented);
            }
        }

        public Version TestedSoftwareVersion
        {
            get { return testedSoftwareVersion; }
            set { testedSoftwareVersion = value; }
        }

        public int UserStoriesTotal
        {
            get
            {
                return userStoryRuns.Count;
            }
        }

        public int UserStoriesSuccess
        {
            get
            {
                return userStoryRuns.Count(n => n.Value.Status == TestExecutionStatus.Successful);
            }
        }

        public int UserStoriesFail
        {
            get
            {
                return userStoryRuns.Count(n => n.Value.Status == TestExecutionStatus.Failed);
            }
        }

        public int UserStoriesNotImplemented
        {
            get
            {
                return userStoryRuns.Count(n => n.Value.Status == TestExecutionStatus.NotImplemented);
            }
        }

        public void AddTestSuiteRun (TestSuiteRun testSuiteRun)
        {
            testSuitesRuns.Add(testSuiteRun.TestSuiteId, testSuiteRun);
        }

        private DateTime endTime;
        private DateTime startTime;
        private Dictionary<string, TestSuiteRun> testSuitesRuns = new Dictionary<string, TestSuiteRun>();
        private Version testedSoftwareVersion = new Version(0, 0);
        private Dictionary<string, UserStoryRun> userStoryRuns = new Dictionary<string, UserStoryRun>();
    }
}
