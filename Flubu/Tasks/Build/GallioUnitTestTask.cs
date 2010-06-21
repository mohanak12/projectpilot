using System;
using System.Globalization;
using System.Text;
using Flubu.Tasks.Processes;

namespace Flubu.Tasks.Build
{
    /// <summary>
    /// Task for running unit tests with gallio echo runner.
    /// </summary>
    public class GallioUnitTestTask : TaskBase
    {
        public GallioUnitTestTask(string workingDirectory, string assemblyToTest)
        {
            GallioEchoPath = ".\\lib\\Gallio\\bin\\Gallio.Echo.exe";
            AssemblyToTest = assemblyToTest;
            WorkingDirectory = workingDirectory;
        }

        /// <summary>
        /// Gets or sets unit test working directory.
        /// </summary>
        public string WorkingDirectory { get; set; }
        
        /// <summary>
        /// Gets or sets assembly to test.
        /// </summary>
        public string AssemblyToTest { get; set; }
        
        /// <summary>
        /// Gets or sets full path to Gallio.Echo.exe (.\lib\Gallio\bin\Gallio.Echo.exe)
        /// </summary>
        public string GallioEchoPath { get; set; }
        
        /// <summary>
        /// Gets or sets test fixtures to run. (Fixture1, Fixture2, Fixture2)
        /// </summary>
        public string FixtureToRun { get; set; }
        
        /// <summary>
        /// Gets or sets tests to run. (Test1, Test2, Test3)
        /// </summary>
        public string TestToRun { get; set; }

        /// <summary>
        /// Gets the task description.
        /// </summary>
        /// <value>The task description.</value>
        public override string TaskDescription
        {
            get
            {
                return string.Format(
                  CultureInfo.InvariantCulture,
                  "Execute MbUnit unit test. Assembly:{0}",
                  AssemblyToTest);
            }
        }

        /// <summary>
        /// Abstract method defining the actual work for a task.
        /// </summary>
        /// <remarks>This method has to be implemented by the inheriting task.</remarks>
        /// <param name="environment">The script execution environment.</param>
        protected override void DoExecute(IScriptExecutionEnvironment environment)
        {
            StringBuilder argumentLineBuilder = new StringBuilder();
            argumentLineBuilder.AppendFormat("\"{0}\" ", AssemblyToTest);
            argumentLineBuilder.AppendFormat("\"{0}\" ", "/verbosity:debug");
            argumentLineBuilder.AppendFormat("\"{0}\" ", "/rt:Html");
            argumentLineBuilder.AppendFormat("\"{0}\" ", "/rt:Xml");

            string filter = null;
            if (!string.IsNullOrEmpty(FixtureToRun))
                filter = "ExactType: " + FixtureToRun;

            if (!string.IsNullOrEmpty(TestToRun))
            {
                if (!string.IsNullOrEmpty(filter))
                    filter += " and ";

                filter += "Member: " + TestToRun;
            }

            if (!string.IsNullOrEmpty(filter))
                argumentLineBuilder.AppendFormat("\"/f:{0}\" ", filter);

            RunProgramTask task = new RunProgramTask(GallioEchoPath, argumentLineBuilder.ToString(), new TimeSpan(0, 1, 0, 0))
                                      {
                                          WorkingDirectory = WorkingDirectory,
                                      };
            task.Execute(environment);
        }
    }
}
