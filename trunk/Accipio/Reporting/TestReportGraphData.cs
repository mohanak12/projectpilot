using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Accipio.Reporting
{
    [SuppressMessage("Microsoft.Design", "CA1053:StaticHolderTypesShouldNotHaveConstructors")]
    public class TestReportGraphData
    {
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public static IDictionary<string, SortedList<DateTime, double>> FetchTestCasesRunHistory(IEnumerable<TestRun> runs)
        {
            IDictionary<string, SortedList<DateTime, double>> testRunHistory
                = new Dictionary<string, SortedList<DateTime, double>>
                      {
                          { "success", new SortedList<DateTime, double>() },
                          { "failed", new SortedList<DateTime, double>() },
                          { "pending", new SortedList<DateTime, double>() }
                      };

            foreach (TestRun testRun in runs)
            {
                DateTime date = testRun.EndTime;

                // add number of success test cases
                if (false == testRunHistory["success"].ContainsKey(date))
                    testRunHistory["success"].Add(date, testRun.TestCasesSuccess);

                // add number of failed test cases
                if (false == testRunHistory["failed"].ContainsKey(date))
                    testRunHistory["failed"].Add(date, testRun.TestCasesFail);

                // add number of not implemented test cases
                if (false == testRunHistory["pending"].ContainsKey(date))
                    testRunHistory["pending"].Add(date, testRun.TestCasesNotImplemented);
            }

            return testRunHistory;
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public static IDictionary<string, SortedList<DateTime, double>> FetchUserStoriesRunHistory(IEnumerable<TestRun> runs)
        {
            IDictionary<string, SortedList<DateTime, double>> testRunHistory
                = new Dictionary<string, SortedList<DateTime, double>>
                      {
                          { "success", new SortedList<DateTime, double>() },
                          { "failed", new SortedList<DateTime, double>() },
                          { "pending", new SortedList<DateTime, double>() }
                      };

            foreach (TestRun testRun in runs)
            {
                DateTime date = testRun.EndTime;

                // add number of success user stories
                if (false == testRunHistory["success"].ContainsKey(date))
                    testRunHistory["success"].Add(date, testRun.UserStoriesSuccess);

                // add number of failed user stories
                if (false == testRunHistory["failed"].ContainsKey(date))
                    testRunHistory["failed"].Add(date, testRun.UserStoriesFail);

                // add number of not implemented user stories
                if (false == testRunHistory["pending"].ContainsKey(date))
                    testRunHistory["pending"].Add(date, testRun.UserStoriesNotImplemented);
            }

            return testRunHistory;
        }
    }
}
