using System;
using System.Diagnostics;
using System.IO;
using System.Security;

namespace Flubu.Tasks.Processes
{
    /// <summary>
    /// Runs an external program.
    /// </summary>
    public class RunProgramTask : TaskBase
    {
        /// <summary>
        /// Gets the task description.
        /// </summary>
        /// <value>The task description.</value>
        public override string TaskDescription
        {
            get
            {
                return String.Format(
                    System.Globalization.CultureInfo.InvariantCulture,
                    "Run program {2}:'{0}' ['{1}']", 
                    programFilePath, 
                    arguments,
                    WorkingDirectory);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RunProgramTask"/> class using the
        /// specified program file path, command line arguments and program execution timeout.
        /// </summary>
        /// <param name="programFilePath">The program file path.</param>
        /// <param name="arguments">Command line arguments.</param>
        /// <param name="timeout">The program execution timeout.</param>
        public RunProgramTask (string programFilePath, string arguments, TimeSpan timeout)
        {
            this.programFilePath = programFilePath;
            this.arguments = arguments;
            this.timeout = timeout;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RunProgramTask"/> class using the
        /// specified program file path, command line arguments, program execution timeout
        /// and a specified user account.
        /// </summary>
        /// <param name="programFilePath">The program file path.</param>
        /// <param name="arguments">Command line arguments.</param>
        /// <param name="timeout">The program execution timeout.</param>
        /// <param name="userName">UserName under which the external program should be executed.</param>
        /// <param name="userDomain">User's domain.</param>
        /// <param name="password">User's password.</param>
        public RunProgramTask (
            string programFilePath, 
            string arguments, 
            TimeSpan timeout,
            string userName, 
            string userDomain, 
            string password) 
            : this (programFilePath, arguments, timeout)
        {
            this.userDomain = userDomain;
            this.userName = userName;
            this.password = password;
        }

        /// <summary>
        /// Runs an external program using the specified command line arguments.
        /// </summary>
        /// <param name="environment">The script execution environment.</param>
        /// <param name="programFilePath">The program file path.</param>
        /// <param name="arguments">The arguments.</param>
        /// <param name="timeout">The timeout.</param>
        public static void Execute (
            IScriptExecutionEnvironment environment,
            string programFilePath, 
            string arguments, 
            TimeSpan timeout)
        {
            RunProgramTask task = new RunProgramTask (programFilePath, arguments, timeout);
            task.Execute (environment);
        }

        /// <summary>
        /// Runs an external program using the specified command line arguments and running
        /// under a specified user account.
        /// </summary>
        /// <param name="environment">The script execution environment.</param>
        /// <param name="programFilePath">The program file path.</param>
        /// <param name="arguments">Command line arguments.</param>
        /// <param name="timeout">The program execution timeout.</param>
        /// <param name="userName">UserName under which the external program should be executed.</param>
        /// <param name="userDomain">User's domain.</param>
        /// <param name="password">User's password.</param>
        public static void Execute (
            IScriptExecutionEnvironment environment,
            string programFilePath, 
            string arguments, 
            TimeSpan timeout,
            string userName, 
            string userDomain, 
            string password)
        {
            RunProgramTask task = new RunProgramTask (
                programFilePath, 
                arguments, 
                timeout,
                userName, 
                userDomain, 
                password);
            task.Execute (environment);
        }

        /// <summary>
        /// Logs important enviroment information (machine name, OS version, etc).
        /// </summary>
        /// <param name="environment">The script execution environment.</param>
        protected override void DoExecute (IScriptExecutionEnvironment environment)
        {
            ScriptExecutionEnvironment = environment;
            using (Process process = new Process())
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo(programFilePath, arguments)
                                                        {
                                                            CreateNoWindow = true,
                                                            ErrorDialog = false,
                                                            RedirectStandardError = true,
                                                            RedirectStandardOutput = true,
                                                            UseShellExecute = false
                                                        };
                if (userName != null)
            {
                processStartInfo.UserName = userName;
                processStartInfo.UseShellExecute = false;

                if (userDomain != null)
                    processStartInfo.Domain = userDomain;
                if (password != null)
                {
                    SecureString passwordString = new SecureString ();

                    for (int i = 0; i < password.Length; i++)
                        passwordString.AppendChar (password[i]);

                    processStartInfo.Password = passwordString;
                }
            }

                processStartInfo.WorkingDirectory = WorkingDirectory ?? Path.GetDirectoryName(programFilePath);

                process.StartInfo = processStartInfo;
                process.ErrorDataReceived += Process_ErrorDataReceived;
                process.OutputDataReceived += Process_OutputDataReceived;
                process.Start();

                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                process.WaitForExit((int)timeout.TotalMilliseconds);

                if (process.ExitCode != 0)
                        throw new RunnerFailedException (
                            String.Format (
                                System.Globalization.CultureInfo.InvariantCulture,
                                "The program '{0}' returned exit code {1}.",
                                programFilePath, 
                                process.ExitCode));
            }
        }

        private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            ScriptExecutionEnvironment.LogError(e.Data);
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            ScriptExecutionEnvironment.LogMessage(e.Data);
        }

        /// <summary>
        /// Gets or sets working directory for process. 
        /// </summary>
        public string WorkingDirectory { get; set; }

        private readonly string programFilePath;
        private readonly string arguments;
        private TimeSpan timeout;
        private readonly string userName;
        private readonly string userDomain;
        private readonly string password;

        private IScriptExecutionEnvironment ScriptExecutionEnvironment { get; set; }
    }
}
