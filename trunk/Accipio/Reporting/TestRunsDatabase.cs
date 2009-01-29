using System;
using System.Collections.Generic;
using System.IO;

namespace Accipio.Reporting
{
    public class TestRunsDatabase
    {
        public IList<TestRun> TestRuns
        {
            get { return testRuns; }
        }

        /// <summary>
        /// Searches for test run log files and loads them into the database.
        /// </summary>
        /// <param name="logDirectory">The log directory.</param>
        /// <param name="fileFilter">The file filter. Only files matching this filter will be read.</param>
        public void LoadDatabase (string logDirectory, string fileFilter)
        {
            foreach (string logFile in Directory.GetFiles(logDirectory, fileFilter))
            {
                TestRunLogParser parser = new TestRunLogParser(logFile);
                TestRun testRun = parser.Parse();
                testRuns.Add(testRun);
            }
        }

        private List<TestRun> testRuns = new List<TestRun>();
    }
}