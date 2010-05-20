using System;
using System.Globalization;
using Flubu.Tasks.Registry;

namespace Flubu.Tasks.Iis
{
    public class GetLocalIisVersionTask : TaskBase
    {
        public const string IisMajorVersion = "IIS/MajorVersion";
        public const string IisMinorVersion = "IIS/MinorVersion";

        public override string TaskDescription
        {
            get { return "Get local IIS version"; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is safe to execute in dry run mode.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is safe to execute in dry run mode; otherwise, <c>false</c>.
        /// </value>
        public override bool IsSafeToExecuteInDryRun
        {
            get
            {
                return true;
            }
        }

        public static void ExecuteTask(IScriptExecutionEnvironment environment)
        {
            GetLocalIisVersionTask task = new GetLocalIisVersionTask();
            task.Execute (environment);
        }

        public static string GetIisVersion(IScriptExecutionEnvironment environment, bool failIfNotExist)
        {
            string major = environment.GetConfigurationSettingValue(IisMajorVersion);
            if (string.IsNullOrEmpty(major))
            {
                ExecuteTask(environment);
                major = environment.GetConfigurationSettingValue(IisMajorVersion);
            }

            if (string.IsNullOrEmpty(major))
            {
                const string Msg = "IIS not installed or IIS access denied!";
                if (failIfNotExist)
                    throw new RunnerFailedException(Msg);
                environment.LogMessage(Msg);

                return "0.0";
            }

            string minor = environment.GetConfigurationSettingValue(IisMajorVersion);
            return major + "." + minor;
        }

        internal static int GetMajorVersion(string version)
        {
            if (string.IsNullOrEmpty(version))
                return 0;
            string[] split = version.Split('.');
            return split.Length != 2 ? 0 : Convert.ToInt32(split[0], CultureInfo.InvariantCulture);
        }

        //internal static int GetMinorVersion(string version)
        //{
        //    if (string.IsNullOrEmpty(version))
        //        return 0;
        //    string[] split = version.Split('.');
        //    return split.Length != 2 ? 0 : Convert.ToInt32(split[1], CultureInfo.InvariantCulture);
        //}

        protected override void DoExecute (IScriptExecutionEnvironment environment)
        {
            GetRegistryValueTask innerTask = new GetRegistryValueTask (
                Microsoft.Win32.Registry.LocalMachine,
                @"SOFTWARE\Microsoft\InetStp",
                "MajorVersion",
                IisMajorVersion);
            innerTask.Execute (environment);

            innerTask = new GetRegistryValueTask (
                Microsoft.Win32.Registry.LocalMachine,
                @"SOFTWARE\Microsoft\InetStp",
                "MinorVersion",
                IisMinorVersion);
            innerTask.Execute (environment);

            environment.LogMessage(
                "Local IIS has version {0}.{1}",
                environment.GetConfigurationSettingValue ("IIS/MajorVersion"),
                environment.GetConfigurationSettingValue ("IIS/MinorVersion"));
        }
    }
}
