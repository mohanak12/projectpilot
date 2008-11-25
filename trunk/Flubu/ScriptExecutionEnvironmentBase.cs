using System;
using System.IO;

namespace Flubu
{
    public abstract class ScriptExecutionEnvironmentBase : IScriptExecutionEnvironment
    {
        #region IScriptExecutionEnvironment Members

        /// <summary>
        /// Gets or sets the name of the script.
        /// </summary>
        /// <value>The name of the script.</value>
        public string ScriptName
        {
            get { return scriptName; }
            set { scriptName = value; }
        }

        /// <summary>
        /// Gets a value indicating whether the script is running on Windows Server 2003.
        /// </summary>
        /// <value>
        ///     <c>true</c> if the script is running on Windows Server 2003; otherwise, <c>false</c>.
        /// </value>
        public bool IsWinServer2003
        {
            get
            {
                return Environment.OSVersion.Platform == PlatformID.Win32NT
                    && Environment.OSVersion.Version.Major == 5
                    && Environment.OSVersion.Version.Minor == 2;
            }
        }

        /// <summary>
        /// Gets the .NET version number for .NET 1.0.
        /// </summary>
        /// <value>.NET version number for .NET 1.0.</value>
        public string Net10VersionNumber
        {
            get { return "v1.0.3705"; }
        }

        /// <summary>
        /// Gets the .NET version number for .NET 1.1.
        /// </summary>
        /// <value>.NET version number for .NET 1.1.</value>
        public string Net11VersionNumber
        {
            get { return "v1.1.4322"; }
        }

        /// <summary>
        /// Gets the .NET version number for .NET 2.0.
        /// </summary>
        /// <value>.NET version number for .NET 2.0.</value>
        public string Net20VersionNumber
        {
            get { return "v2.0.50727"; }
        }

        /// <summary>
        /// Gets the .NET version number for .NET 3.0.
        /// </summary>
        /// <value>.NET version number for .NET 3.0.</value>
        public string Net30VersionNumber
        {
            get { return "v3.0"; }
        }

        /// <summary>
        /// Gets the Windows system root directory path.
        /// </summary>
        /// <value>The Windows system root directory path.</value>
        public string SystemRootDir
        {
            get { return Environment.GetEnvironmentVariable ("SystemRoot"); }
        }

        /// <summary>
        /// Gets the path to the .NET Framework directory.
        /// </summary>
        /// <param name="dotNetVersion">The version of the .NET (example: "v2.0.50727").</param>
        /// <returns>
        /// The path to the .NET Framework directory.
        /// </returns>
        public string GetDotNetFWDir (string dotNetVersion)
        {
            string fwRootDir = Path.Combine (SystemRootDir, @"Microsoft.NET\Framework");
            return Path.Combine (fwRootDir, dotNetVersion);
        }

        /// <summary>
        /// Gets or sets a value indicating whether to dry-run the script. 
        /// When set to <c>true</c>, the tasks are not really executed,
        /// instead just a log of activities is created.
        /// </summary>
        public bool DryRun
        {
            get { return dryRun; } set { dryRun = value; }
        }

        public abstract string GetConfigurationSettingValue (string settingName);

        public abstract void SetConfigurationSettingValue (string settingName, string settingValue);

        public abstract void ReportMessage (string message);

        public abstract void ReportMessage (string format, params object[] parameters);

        public abstract void ReportTaskStarted (ITask task);

        public abstract void ReportTaskExecuted (ITask task);

        public abstract void ReportTaskFailed (ITask task, Exception ex);

        public abstract void ReportTaskFailed (ITask task, string reason);

        public abstract string ReceiveInput (string prompt);

        #endregion

        private string scriptName;
        private bool dryRun;
    }
}
