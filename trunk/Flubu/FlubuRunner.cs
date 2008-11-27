using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Security.AccessControl;
using System.Text;
using Flubu.Tasks.Configuration;
using Flubu.Tasks.EnterpriseServices;
using Flubu.Tasks.FileSystem;
using Flubu.Tasks.Iis;
using Flubu.Tasks.Misc;
using Flubu.Tasks.Msmq;
using Flubu.Tasks.Processes;
using Flubu.Tasks.Registry;
using Flubu.Tasks.SqlServer;
using Flubu.Tasks.Text;
using Flubu.Tasks.UserAccounts;
using Flubu.Tasks.UserInterface;
using Flubu.Tasks.WindowsServices;
using Microsoft.Win32;

namespace Flubu
{
    public class FlubuRunner : IDisposable
    {
        public FlubuRunner()
        {
            scriptExecutionEnvironment = new ConsoleExecutionEnvironment("script");
            hasFailed = true;
        }

        public FlubuRunner AddUserToGroup (string userName, string group)
        {
            AddUserToGroupTask.Execute(scriptExecutionEnvironment,  userName, group);
            return this;
        }

        public FlubuRunner AddProgramArgument(string argument)
        {
            programArgs.Add(argument);
            return this;
        }

        public FlubuRunner AddProgramArgument(string format, params object[] args)
        {
            programArgs.Add(string.Format(CultureInfo.InvariantCulture, format, args));
            return this;
        }

        public FlubuRunner AskUser (string prompt, string configurationSettingName)
        {
            AskUserTask.Execute(scriptExecutionEnvironment, prompt, configurationSettingName);
            return this;
        }

        public FlubuRunner CheckIfServiceExists (string serviceName, string configurationSetting)
        {
            CheckIfServiceExistsTask.Execute(scriptExecutionEnvironment, serviceName, configurationSetting);
            return this;
        }

        public FlubuRunner ControlApplicationPool (
            string applicationPoolName, 
            ControlApplicationPoolAction action,
            bool failIfNotExist)
        {
            ControlApplicationPoolTask.Execute(scriptExecutionEnvironment, applicationPoolName, action, failIfNotExist);
            return this;
        }

        public FlubuRunner ControlWindowsService (
            string serviceName, 
            ControlWindowsServiceMode mode, 
            TimeSpan timeout)
        {
            ControlWindowsServiceTask.Execute(scriptExecutionEnvironment, serviceName, mode, timeout);
            return this;
        }

        public FlubuRunner CopyDirectoryStructure (string sourcePath, string destinationPath, bool overwriteExisting)
        {
            CopyDirectoryStructureTask.Execute(scriptExecutionEnvironment, sourcePath, destinationPath, overwriteExisting);
            return this;
        }

        public FlubuRunner CopyDirectoryStructure(
            string sourcePath, 
            string destinationPath, 
            bool overwriteExisting,
            string exclusionPattern)
        {
            CopyDirectoryStructureTask task = new CopyDirectoryStructureTask(sourcePath, destinationPath, overwriteExisting);
            task.ExclusionPattern = exclusionPattern;
            return RunTask(task);
        }

        public FlubuRunner CopyFile (
            string sourceFileName,
            string destinationFileName,
            bool overwrite)
        {
            CopyFileTask.Execute(scriptExecutionEnvironment, sourceFileName, destinationFileName, overwrite);
            return this;
        }

        public FlubuRunner CreateApplicationPool (string applicationPoolName, CreateApplicationPoolMode mode)
        {
            CreateApplicationPoolTask.Execute(scriptExecutionEnvironment, applicationPoolName, mode);
            return this;
        }

        public FlubuRunner CreateDirectory (string directoryPath)
        {
            CreateDirectoryTask.Execute(scriptExecutionEnvironment, directoryPath);
            return this;
        }

        public FlubuRunner CreateMessageQueue (
            string messageQueuePath, 
            bool isTransactional,
            CreateMessageQueueMode mode)
        {
            CreateMessageQueueTask task = new CreateMessageQueueTask(messageQueuePath, isTransactional, mode);
            return RunTask(task);
        }

        public FlubuRunner CreateUserAccount (
            CreateUserAccountMode mode,
            string userName,
            string password,
            string userDescription)
        {
            CreateUserAccountTask task = new CreateUserAccountTask(mode, userName, password, userDescription);
            return RunTask(task);
        }

        public FlubuRunner DeleteDirectory(string directoryPath)
        {
            DeleteDirectoryTask.Execute(scriptExecutionEnvironment, directoryPath);
            return this;
        }

        public FlubuRunner DeleteFiles(string directoryPath, string filePattern)
        {
            DeleteFilesTask.Execute(scriptExecutionEnvironment, directoryPath, filePattern);
            return this;
        }

        public FlubuRunner DeleteUserAccount(string userName)
        {
            DeleteUserAccountTask.Execute(scriptExecutionEnvironment, userName);
            return this;
        }

        public FlubuRunner DeleteVirtualDirectoryTask (string virtualDirectoryName, bool failIfNotExist)
        {
            DeleteVirtualDirectoryTask task = new DeleteVirtualDirectoryTask(virtualDirectoryName, failIfNotExist);
            return RunTask(task);
        }

        public FlubuRunner EditRegistryValue (
            RegistryKey rootKey,
            string registryKeyPath,
            string registryValueName,
            object registryValueValue)
        {
            EditRegistryValueTask.Execute(
                scriptExecutionEnvironment, 
                rootKey, 
                registryKeyPath,
                registryValueName,
                registryValueValue);
            return this;
        }

        public FlubuRunner EnsureSqlServerIsRunning (string machineName)
        {
            EnsureSqlServerIsRunningTask task = new EnsureSqlServerIsRunningTask(machineName);
            return RunTask(task);
        }

        public FlubuRunner ExecuteSqlScript (string connectionString, string scriptFilePath)
        {
            ExecuteSqlScriptTask task = new ExecuteSqlScriptTask(connectionString, scriptFilePath);
            return RunTask(task);
        }

        public FlubuRunner ExpandProperties (
            string sourceFileName, 
            string expandedFileName,
            IDictionary<string, string> properties)
        {
            ExpandPropertiesTask task = new ExpandPropertiesTask(sourceFileName, expandedFileName);

            foreach (KeyValuePair<string, string> pair in properties)
                task.AddPropertyToExpand(pair.Key, pair.Value);

            return RunTask(task);
        }

        public void Fail(string format, params object[] arguments)
        {
            string message = String.Format(
                CultureInfo.InvariantCulture,
                format,
                arguments);

            Log(message);

            throw new RunnerFailedException(message);
        }

        /// <summary>
        /// Marks the runner as having completed its work sucessfully. This is the last method
        /// that should be called on the runner before it gets disposed.
        /// </summary>
        public void Complete()
        {
            hasFailed = false;
        }

        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public FlubuRunner GetLocalIisVersionTask()
        {
            GetLocalIisVersionTask task = new GetLocalIisVersionTask();
            return RunTask(task);
        }

        public FlubuRunner GetRegistryValue(
            RegistryKey rootKey,
            string registryKeyPath,
            string registryValueName,
            string configurationSettingName)
        {
            GetRegistryValueTask.Execute(
                scriptExecutionEnvironment,
                rootKey,
                registryKeyPath,
                registryValueName,
                configurationSettingName);
            return this;
        }

        public FlubuRunner InstallAssembly(string assemblyFileName)
        {
            InstallAssemblyTask.Execute(scriptExecutionEnvironment, assemblyFileName);
            return this;
        }

        public FlubuRunner InstallWindowsService (
            string executablePath,
            string serviceName, 
            InstallWindowsServiceMode mode)
        {
            InstallWindowsServiceTask.Execute(scriptExecutionEnvironment, executablePath, serviceName, mode);
            return this;
        }

        public FlubuRunner KillProcess (string processName)
        {
            KillProcessTask.Execute(scriptExecutionEnvironment, processName);
            return this;
        }

        public FlubuRunner Log (string format, params object[] args)
        {
            scriptExecutionEnvironment.Logger.Log(format, args);
            return this;
        }

        public FlubuRunner LogEnvironment()
        {
            LogScriptEnvironmentTask task = new LogScriptEnvironmentTask();
            return RunTask(task);
        }

        public FlubuRunner ReadConfigurationFromFile(string configurationFileName)
        {
            ReadConfigurationTask.ReadFromFile(scriptExecutionEnvironment, configurationFileName);
            return this;
        }

        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "string")]
        public FlubuRunner ReadConfigurationFromString(string configurationString)
        {
            ReadConfigurationTask.ReadFromString(scriptExecutionEnvironment, configurationString);
            return this;
        }

        public FlubuRunner RegisterAspNet(
            string virtualDirectoryName,
            string dotNetVersion)
        {
            RegisterAspNetTask.Execute(scriptExecutionEnvironment, virtualDirectoryName, dotNetVersion);
            return this;
        }

        public FlubuRunner RegisterAspNet (
            string virtualDirectoryName,
            string parentVirtualDirectoryName,
            string dotNetVersion)
        {
            RegisterAspNetTask.Execute(scriptExecutionEnvironment, virtualDirectoryName, parentVirtualDirectoryName, dotNetVersion);
            return this;
        }

        public FlubuRunner RunProgram(string programExePath)
        {
            using (Process process = new Process())
            {
                StringBuilder argumentLineBuilder = new StringBuilder();
                foreach (string programArg in programArgs)
                    argumentLineBuilder.AppendFormat("\"{0}\" ", programArg);

                Log("Running program '{0}' ('{1}')", programExePath, argumentLineBuilder);

                ProcessStartInfo processStartInfo = new ProcessStartInfo(programExePath, argumentLineBuilder.ToString());
                processStartInfo.CreateNoWindow = true;
                processStartInfo.ErrorDialog = false;
                processStartInfo.RedirectStandardError = true;
                processStartInfo.RedirectStandardOutput = true;
                processStartInfo.UseShellExecute = false;

                process.StartInfo = processStartInfo;
                process.ErrorDataReceived += new DataReceivedEventHandler(Process_ErrorDataReceived);
                process.OutputDataReceived += new DataReceivedEventHandler(Process_OutputDataReceived);
                process.Start();

                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                process.WaitForExit();

                //Log("Exit code: {0}", process.ExitCode);

                if (process.ExitCode != 0)
                    Fail("Program '{0}' returned exit code {1}.", programExePath, process.ExitCode);
            }

            programArgs.Clear();

            return this;
        }

        public FlubuRunner RunTask(ITask task)
        {
            task.Execute(scriptExecutionEnvironment);
            return this;
        }

        public FlubuRunner SetFileAccessRule (
            string path, 
            string identity, 
            FileSystemRights fileSystemRights, 
            AccessControlType accessControlType)
        {
            SetAccessRuleTask task = new SetAccessRuleTask(path, identity, fileSystemRights, accessControlType);
            return RunTask(task);
        }

        public FlubuRunner SetFileAccessRule(
            string path,
            IEnumerable<string> identities,
            FileSystemRights fileSystemRights,
            AccessControlType accessControlType)
        {
            SetAccessRuleTask task = new SetAccessRuleTask(path, fileSystemRights, accessControlType);
            foreach (string identity in identities)
                task.AddIdentity(identity);
            return RunTask(task);
        }

        public FlubuRunner SetRegistryKeyPermissions (
            RegistryKey rootKey,
            string registryKeyPath,
            string identity,
            RegistryRights registryRights,
            AccessControlType accessControlType)
        {
            SetRegistryKeyPermissionsTask.Execute(
                scriptExecutionEnvironment,
                rootKey,
                registryKeyPath,
                identity,
                registryRights,
                accessControlType);
            return this;
        }

        public FlubuRunner SetWindowsServiceAccount (
            string serviceName, 
            string userName, 
            string password)
        {
            SetWindowsServiceAccountTask.Execute(scriptExecutionEnvironment, serviceName, userName, password);
            return this;
        }

        public FlubuRunner Sleep (TimeSpan sleepPeriod)
        {
            SleepTask.Execute(scriptExecutionEnvironment, sleepPeriod);
            return this;
        }

        public FlubuRunner StopWindowsServiceIfExists (string serviceName)
        {
            StopWindowsServiceIfExistsTask.Execute(scriptExecutionEnvironment, serviceName);
            return this;
        }

        public FlubuRunner TransformXmlFile (string xsltFile, string inputFile, string outputFile)
        {
            XsltTransformTask.Execute(scriptExecutionEnvironment, inputFile, outputFile, xsltFile);
            return this;
        }

        public FlubuRunner UninstallAssembly (string assemblyName)
        {
            UninstallAssemblyTask task = new UninstallAssemblyTask(assemblyName);
            return RunTask(task);
        }

        public FlubuRunner UninstallWindowsService (string executablePath)
        {
            UninstallWindowsServiceTask task = new UninstallWindowsServiceTask(executablePath);
            return RunTask(task);
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or
        /// resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the object.
        /// </summary>
        /// <param name="disposing">If <code>false</code>, cleans up native resources. 
        /// If <code>true</code> cleans up both managed and native resources</param>
        protected virtual void Dispose(bool disposing)
        {
            if (false == disposed)
            {
                if (disposing)
                {
                    scriptExecutionEnvironment.Logger.ReportRunnerFinished(!hasFailed);
                }

                disposed = true;
            }
        }

        private bool disposed;

        #endregion

        private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Log("     [exec] {0}", e.Data);
        }

        private bool hasFailed;
        private List<string> programArgs = new List<string>();
        private IScriptExecutionEnvironment scriptExecutionEnvironment;
    }
}