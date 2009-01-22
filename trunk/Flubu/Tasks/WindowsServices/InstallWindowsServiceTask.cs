using System;
using System.Collections;
using Flubu.Tasks.Misc;

namespace Flubu.Tasks.WindowsServices
{
    /// <summary>
    /// Installs a specified Windows service.
    /// </summary>
    public class InstallWindowsServiceTask : TaskBase
    {
        /// <summary>
        /// Gets or sets the period to wait after service uninstallation before continuing with reinstallation.
        /// </summary>
        /// <value>The service uninstallation wait time.</value>
        public TimeSpan ServiceUninstallationWaitTime
        {
            get { return serviceUninstallationWaitTime; }
            set { serviceUninstallationWaitTime = value; }
        }

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
                    "Install Windows service '{0}'", 
                    executablePath);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InstallWindowsServiceTask"/> class
        /// with the specified path to the Windows service executable.
        /// </summary>
        /// <param name="executablePath">The executable path.</param>
        /// <param name="serviceName">The Windows service name.</param>
        /// <param name="mode">Mode of the installation.</param>
        public InstallWindowsServiceTask (string executablePath, string serviceName, InstallWindowsServiceMode mode)
        {
            this.executablePath = executablePath;
            this.serviceName = serviceName;
            this.mode = mode;
        }

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <param name="executablePath">The executable path.</param>
        /// <param name="serviceName">Name of the service.</param>
        /// <param name="mode">The task mode.</param>
        public static void Execute (
            IScriptExecutionEnvironment environment, 
            string executablePath,
            string serviceName, 
            InstallWindowsServiceMode mode)
        {
            InstallWindowsServiceTask task = new InstallWindowsServiceTask (executablePath, serviceName, mode);
            task.Execute (environment);
        }

        /// <summary>
        /// Internal task execution code.
        /// </summary>
        /// <param name="environment">The script execution environment.</param>
        protected override void DoExecute (IScriptExecutionEnvironment environment)
        {
            string configSettingName = String.Format (
                System.Globalization.CultureInfo.InvariantCulture,
                "ServicesExist/{0}", 
                serviceName);

            CheckIfServiceExistsTask checkIfServiceExistsTask = new CheckIfServiceExistsTask (serviceName, configSettingName);
            checkIfServiceExistsTask.Execute (environment);

            if (bool.Parse (environment.GetConfigurationSettingValue (configSettingName)) == true)
            {
                switch (mode)
                {
                    case InstallWindowsServiceMode.DoNothingIfExists:
                        return;

                    case InstallWindowsServiceMode.FailIfAlreadyInstalled:
                        throw new RunnerFailedException (
                            String.Format (
                                System.Globalization.CultureInfo.InvariantCulture,
                                "The Windows service '{0}' already exists.", 
                                serviceName));

                    case InstallWindowsServiceMode.ReinstallIfExists:
                        UninstallWindowsServiceTask uninstallWindowsServiceTask = new UninstallWindowsServiceTask (executablePath);
                        uninstallWindowsServiceTask.Execute (environment);

                        // wait for a while to ensure the service is really deleted
                        SleepTask.Execute (environment, serviceUninstallationWaitTime);
                        break;

                    default:
                        throw new NotSupportedException ();
                    }
            }

            IDictionary savedState = new Hashtable ();
            string[] commandLine = new string[0];

            using (System.Configuration.Install.AssemblyInstaller assemblyInstaller
                = new System.Configuration.Install.AssemblyInstaller (executablePath, commandLine))
            {
                try
                {
                    assemblyInstaller.UseNewContext = true;

                    assemblyInstaller.Install (savedState);
                    assemblyInstaller.Commit (savedState);
                }
                catch (System.ComponentModel.Win32Exception ex)
                {
                    // 1073
                    environment.LogMessage(
                            "ex.ErrorCode = {0}", 
                            ex.NativeErrorCode);
                    throw;
                }
            }
        }

        private string executablePath;
        private InstallWindowsServiceMode mode;
        private string serviceName;
        private TimeSpan serviceUninstallationWaitTime = TimeSpan.FromSeconds(5);
    }
}
