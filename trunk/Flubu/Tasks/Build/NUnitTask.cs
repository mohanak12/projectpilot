﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Flubu.Tasks.Processes;

namespace Flubu.Tasks.Build
{
    public class NUnitTask : TaskBase
    {
        public NUnitTask(string workingDirectory, string assemblyToTest)
        {
            NUnitPath = @"lib\NUnit\bin\net-2.0\nunit-console-x86.exe";
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
        /// Gets or sets NUnit application path.
        /// </summary>
        public string NUnitPath { get; set; }
        
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
                "Execute NUnit unit tests. Assembly:{0}",
                AssemblyToTest);
            }
        }

        public string ExcludeCategories { get; set; }

        /// <summary>
        /// Abstract method defining the actual work for a task.
        /// </summary>
        /// <remarks>This method has to be implemented by the inheriting task.</remarks>
        /// <param name="environment">The script execution environment.</param>
        protected override void DoExecute(IScriptExecutionEnvironment environment)
        {
            StringBuilder argumentLineBuilder = new StringBuilder();
            argumentLineBuilder.AppendFormat("\"{0}\" ", AssemblyToTest);
            argumentLineBuilder.AppendFormat("\"{0}\" ", "/nodots");
            argumentLineBuilder.AppendFormat("\"{0}\" ", "/labels");
            if (!string.IsNullOrEmpty(ExcludeCategories))
                argumentLineBuilder.AppendFormat("\"/exclude={0}\" ", ExcludeCategories);

            RunProgramTask task = new RunProgramTask(NUnitPath, argumentLineBuilder.ToString(), new TimeSpan(0, 1, 0, 0))
            {
                WorkingDirectory = WorkingDirectory,
            };
            task.Execute(environment);
        }
    }
}
