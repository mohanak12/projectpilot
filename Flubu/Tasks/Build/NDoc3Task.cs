using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using Flubu.Tasks.Processes;

namespace Flubu.Tasks.Build
{
    public class NDoc3Task : TaskBase
    {
        public static void Execute(IScriptExecutionEnvironment environment, params string[] parameters)
        {
            NDoc3Task task = new NDoc3Task(parameters);
            task.DoExecute(environment);
        }

        /// <summary>
        /// Gets or sets documentation working directory. (default=.)
        /// </summary>
        public string WorkingDirectory { get; set; }

        /// <summary>
        /// Gets or sets documentation output directory. (default=.\doc)
        /// </summary>
        public string OutputDirectory { get; set; }

        /// <summary>
        /// Gets assemblies to document.
        /// </summary>
        public Collection<string> AssembliesToDocument { get; private set; }

        /// <summary>
        /// Gets or sets NDoc3 application path.
        /// </summary>
        public string NDoc3Path { get; set; }

        public NDoc3Task(Collection<string> assembliesToDocument)
        {
            NDoc3Path = "lib\\NDoc3\\NDoc3Console.exe";
            WorkingDirectory = ".";
            AssembliesToDocument = assembliesToDocument;
        }

        public NDoc3Task(params string[] arguments)
        {
            WorkingDirectory = ".";
            NDoc3Path = "lib\\NDoc3\\NDoc3Console.exe";
            AssembliesToDocument = new Collection<string>(arguments);
        }

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
                "Execute NDoc task.");
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
            foreach (string assembly in AssembliesToDocument)
            {
                argumentLineBuilder.AppendFormat("\"{0}\" ", assembly);
            }

            argumentLineBuilder.AppendFormat("\"{0}\" ", "-OutputTarget=Web");
            
            if (!string.IsNullOrEmpty(OutputDirectory))
                argumentLineBuilder.AppendFormat("\"-OutputDirectory={0}\" ", OutputDirectory);

            RunProgramTask task = new RunProgramTask(NDoc3Path, argumentLineBuilder.ToString(), new TimeSpan(0, 1, 0, 0))
            {
                WorkingDirectory = WorkingDirectory,
            };

            task.Execute(environment);
        }
    }
}
