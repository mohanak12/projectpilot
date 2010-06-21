
using System;
using System.Globalization;
using System.IO;
using System.Text;
using Flubu.Tasks.Processes;

namespace Flubu.Tasks.Build
{
    public class NCoverTask : TaskBase
    {
        public NCoverTask(string workingDirectory, string assemblyToTest)
        {
            WorkingDirectory = workingDirectory;
            AssemblyToTest = assemblyToTest;
            NCoverExecutablePath = ".\\lib\\NCover\\NCover.Console.exe";
            NCoverRoot = Path.GetFullPath(NCoverExecutablePath);
            TestToolPath = @"lib\NUnit\bin\net-2.0\nunit-console-x86.exe";
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
        /// Gets or sets unit test tool path. (Default: lib\NUnit\bin\net-2.0\nunit-console-x86.exe)
        /// </summary>
        public string TestToolPath { get; set; }

        /// <summary>
        /// Gets or sets Full NCover test tool path. (Default: .\\lib\\NCover\\NCover.Console.exe)
        /// </summary>
        public string NCoverExecutablePath { get; set; }

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
                    "Execute unit tests with NCover. Assembly:{0}",
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
            string ncoverdll = Path.Combine(NCoverRoot, "CoverLib.dll");
            RunProgramTask task = new RunProgramTask("regsvr32.exe", "/s " + ncoverdll, new TimeSpan(0, 0, 0, 10));
            task.Execute(environment);

            StringBuilder args = new StringBuilder();
            args.AppendFormat("\"{0}\" ", TestToolPath);
            args.AppendFormat("\"{0}\" ", AssemblyToTest);
            args.AppendFormat("\"{0}\" ", "//l Coverage.log");
            args.AppendFormat("\"{0}\" ", "//x Coverage.xml");
            args.AppendFormat("\"{0}\" ", "//w " + WorkingDirectory);

            task = new RunProgramTask(NCoverExecutablePath, args.ToString(), new TimeSpan(0, 1, 0, 0))
                       {
                           WorkingDirectory = WorkingDirectory
                       };
            task.Execute(environment);

            task = new RunProgramTask("regsvr32.exe", "/s /u " + ncoverdll, new TimeSpan(0, 0, 0, 10));
            task.Execute(environment);
        }

        private string NCoverRoot { get; set; }
    }
}
