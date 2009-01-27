using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Accipio.Reporting
{
    public class TestRun
    {
        private DateTime endTime;
        private DateTime startTime;
        private Dictionary<string, TestSuiteRun> testSuitesRuns = new Dictionary<string, TestSuiteRun>();
    }
}
