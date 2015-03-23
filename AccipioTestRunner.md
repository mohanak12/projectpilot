## Requirements ##
  * the runner should provide a command-line interface for running tests
  * ideally it should be possible to run tests inside VisualStudio (with debugging)
  * the runner should reference MbUnit.Framework assembly in order to use its assertion methods
  * after each test run, the runner should provide an XML log of acceptance test results. The log should contain:
    * version of the binaries that were tested
    * version of the test cases that were used (SVN revision or just date-time?)
    * a list of all test cases that were run
    * for each test case, the log should contain the result, duration, tags and the time the test was started
  * each test case can have three different outcomes:
    * successful - the test case has met the acceptance requirements criteria
    * failed - the test case did not meet the acceptance requirements criteria
    * postponed/ignored - the test code hasn't been written yet

## Links ##
  * http://code.google.com/p/mb-unit/wiki/GallioObjectModel
  * http://code.google.com/p/mb-unit/wiki/TestRunnerLifecycle
  * Gallio Archimedes - a tool which could help us, but it's not yet published (http://blog.bits-in-motion.com/2008/04/gallio-archimedes.html)