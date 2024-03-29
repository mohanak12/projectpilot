using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Messaging;
using System.Security.AccessControl;
using System.Text;
using Flubu.Tasks;
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
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.Win32;

namespace Flubu
{
    public interface IFlubuRunner : IDisposable
    {
        Stopwatch BuildStopwatch { get; }

        bool HasFailed { get; }

        IDictionary<string, IFlubuRunnerTarget> Targets { get; }
    }

    /// <summary>
    /// A base class for fluent building.
    /// </summary>
    /// <typeparam name="TRunner">The concrete type of the runner.</typeparam>
    public class FlubuRunner<TRunner> : IFlubuRunner
        where TRunner : FlubuRunner<TRunner>
    {
        public FlubuRunner(string scriptName, string logFileName, int howManyOldLogsToKeep)
        {
            scriptExecutionEnvironment = new ConsoleExecutionEnvironment(
                scriptName, 
                logFileName,
                howManyOldLogsToKeep);
            hasFailed = true;

            programRunner = new ExternalProgramRunner<TRunner>((TRunner)this);

            AddTarget("help")
                .SetDescription("Displays the available targets in the build")
                .Do(TargetHelp);

            buildStopwatch.Start();
        }

        /// <summary>
        /// Gets the default target for this runner.
        /// </summary>
        /// <remarks>The default target is the one which will be executed if
        /// the target is not specified in the command line.</remarks>
        /// <value>The default target.</value>
        public FlubuRunnerTarget<TRunner> DefaultTarget
        {
            get { return defaultTarget; }
        }

        public bool HasFailed
        {
            get { return hasFailed; }
        }

        public Stopwatch BuildStopwatch
        {
            get { return buildStopwatch; }
        }

        public ExternalProgramRunner<TRunner> ProgramRunner
        {
            get { return programRunner; }
        }

        /// <summary>
        /// Gets the list of all copied destination files that were copied during the last execution of the <see cref="CopyDirectoryStructure(string,string,bool)"/>
        /// or <see cref="CopyDirectoryStructure(string,string,bool,string,string)"/> call.
        /// </summary>
        /// <value>The last copied files list.</value>
        public IList<string> LastCopiedFilesList
        {
            get { return lastCopiedFilesList; }
        }

        public IScriptExecutionEnvironment ScriptExecutionEnvironment
        {
            get { return scriptExecutionEnvironment; }
        }

        public IDictionary<string, IFlubuRunnerTarget> Targets
        {
            get { return targets; }
        }

        public TRunner AddUserToGroup (string userName, string group)
        {
            AddUserToGroupTask.Execute(scriptExecutionEnvironment,  userName, group);
            return ReturnThisTRunner();
        }

        public FlubuRunnerTarget<TRunner> AddTarget(string targetName)
        {
            FlubuRunnerTarget<TRunner> target = new FlubuRunnerTarget<TRunner>(ReturnThisTRunner(), targetName);
            Targets.Add(target.TargetName, target);
            return target;
        }

        public TRunner AskUser(string prompt, string configurationSettingName)
        {
            AskUserTask.Execute(scriptExecutionEnvironment, prompt, configurationSettingName);
            return ReturnThisTRunner();
        }

        /// <summary>
        /// Asserts that the specified file exists. If the file does not exist,
        /// the runner will fail.
        /// </summary>
        /// <param name="fileDescription">The file description.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>The same instance of this <see cref="TRunner"/>.</returns>
        public TRunner AssertFileExists(string fileDescription, string fileName)
        {
            if (false == File.Exists(fileName))
                Fail("{0} ('{1}') does not exist", fileDescription, fileName);

            return ReturnThisTRunner();
        }

        /// <summary>
        /// Sounds a beep.
        /// </summary>
        /// <param name="messageBeepType">Type of the message beep.</param>
        /// <returns>The same instance of this <see cref="TRunner"/>.</returns>
        public TRunner Beep(MessageBeepType messageBeepType)
        {
            Console.Out.Flush();
            Beeper.MessageBeep(messageBeepType);
            return ReturnThisTRunner();
        }

        public TRunner CheckIfServiceExists(string serviceName, string configurationSetting)
        {
            CheckIfServiceExistsTask.Execute(scriptExecutionEnvironment, serviceName, configurationSetting);
            return ReturnThisTRunner();
        }

        /// <summary>
        /// Marks the runner as having completed its work sucessfully. This is the last method
        /// that should be called on the runner before it gets disposed.
        /// </summary>
        public void Complete()
        {
            hasFailed = false;
        }

        public TRunner ControlWindowsService(
            string serviceName, 
            ControlWindowsServiceMode mode, 
            TimeSpan timeout)
        {
            ControlWindowsServiceTask.Execute(scriptExecutionEnvironment, serviceName, mode, timeout);
            return ReturnThisTRunner();
        }

        /// <summary>
        /// Copies the directory structure (and the files) to the destination directory.
        /// </summary>
        /// <param name="sourcePath">The source path.</param>
        /// <param name="destinationPath">The destination path.</param>
        /// <param name="overwriteExisting">if set to <c>true</c>, existing files will be overwriten.</param>
        /// <returns>The same instance of this <see cref="TRunner"/>.</returns>
        public virtual TRunner CopyDirectoryStructure(string sourcePath, string destinationPath, bool overwriteExisting)
        {
            CopyDirectoryStructureTask task = new CopyDirectoryStructureTask(sourcePath, destinationPath, overwriteExisting);
            RunTask(task);
            lastCopiedFilesList = task.CopiedFilesList;
            return ReturnThisTRunner();
        }

        /// <summary>
        /// Copies the directory structure (and the files) to the destination directory.
        /// </summary>
        /// <param name="sourcePath">The source path.</param>
        /// <param name="destinationPath">The destination path.</param>
        /// <param name="overwriteExisting">if set to <c>true</c>, existing files will be overwriten.</param>
        /// <param name="inclusionRegexPattern">The inclusion Regular expression pattern. 
        /// All files whose paths match this regular expression
        /// will be copied. If the <see cref="inclusionRegexPattern"/> is <c>null</c>, it will be ignored.</param>
        /// <param name="exclusionRegexPattern">The exclusion Regular expression pattern. 
        /// All files whose paths match this regular expression
        /// will not be copied. If the <see cref="exclusionRegexPattern"/> is <c>null</c>, it will be ignored.</param>
        /// <returns>The same instance of this <see cref="TRunner"/>.</returns>
        public virtual TRunner CopyDirectoryStructure(
            string sourcePath, 
            string destinationPath, 
            bool overwriteExisting,
            string inclusionRegexPattern,
            string exclusionRegexPattern)
        {
            CopyDirectoryStructureTask task = new CopyDirectoryStructureTask(sourcePath, destinationPath, overwriteExisting);
            task.InclusionPattern = inclusionRegexPattern;
            task.ExclusionPattern = exclusionRegexPattern;
            
            RunTask(task);

            lastCopiedFilesList = task.CopiedFilesList;

            return ReturnThisTRunner();
        }

        public TRunner CopyFile(
            string sourceFileName,
            string destinationFileName,
            bool overwrite)
        {
            CopyFileTask.Execute(scriptExecutionEnvironment, sourceFileName, destinationFileName, overwrite);
            return ReturnThisTRunner();
        }

        /// <summary>
        /// Creates a directory.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <param name="failIfAlreadyExists">if set to <c>true</c>, the method will
        /// throw an exception if the directory already exists.</param>
        /// <returns>The same instance of this <see cref="TRunner"/>.</returns>
        public TRunner CreateDirectory(string directoryPath, bool failIfAlreadyExists)
        {
            if (false == Directory.Exists(directoryPath) || failIfAlreadyExists)
                Directory.CreateDirectory(directoryPath);

            return ReturnThisTRunner();
        }

        public TRunner CreateMessageQueue(
            string messageQueuePath, 
            bool isTransactional,
            CreateMessageQueueMode mode)
        {
            CreateMessageQueueTask task = new CreateMessageQueueTask(messageQueuePath, isTransactional, mode);
            return RunTask(task);
        }

        public TRunner CreateMessageQueue(
            string messageQueuePath,
            bool isTransactional,
            CreateMessageQueueMode mode,
            string userName,
            MessageQueueAccessRights accessRights)
        {
            CreateMessageQueueTask task = new CreateMessageQueueTask(messageQueuePath, isTransactional, mode)
                                              {
                                                  UserName = userName,
                                                  AccessRights = accessRights
                                              };
            return RunTask(task);
        }

        public TRunner CreateUserAccount(
            CreateUserAccountMode mode,
            string userName,
            string password,
            string userDescription)
        {
            CreateUserAccountTask task = new CreateUserAccountTask(mode, userName, password, userDescription);
            return RunTask(task);
        }

        public TRunner DeleteDirectory(string directoryPath, bool failIfNotExists)
        {
            DeleteDirectoryTask.Execute(scriptExecutionEnvironment, directoryPath, failIfNotExists);
            return ReturnThisTRunner();
        }

        /// <summary>
        /// Deletes files which match the file pattern.
        /// </summary>
        /// <param name="directoryPath">The directory path from which to start searching for files.</param>
        /// <param name="filePattern">The file pattern.</param>
        /// <param name="recursive">if set to <c>true</c>, the method will delete matching files in subdirectories too;
        /// otherwise it will just delete files in the top directory.</param>
        /// <returns>The same instance of this <see cref="TRunner"/>.</returns>
        public TRunner DeleteFiles(string directoryPath, string filePattern, bool recursive)
        {
            DeleteFilesTask task = new DeleteFilesTask(directoryPath, filePattern, recursive);
            return RunTask(task);
        }

        public TRunner DeleteUserAccount(string userName)
        {
            DeleteUserAccountTask.Execute(scriptExecutionEnvironment, userName);
            return ReturnThisTRunner();
        }

        public TRunner DeleteVirtualDirectoryTask(string virtualDirectoryName, bool failIfNotExist)
        {
            DeleteVirtualDirectoryTask task = new DeleteVirtualDirectoryTask(virtualDirectoryName, failIfNotExist);
            task.Execute(scriptExecutionEnvironment);
            return ReturnThisTRunner();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or
        /// resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public TRunner EditRegistryValue(
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
            return ReturnThisTRunner();
        }

        public TRunner EnsureDependenciesExecuted(string targetName)
        {
            IFlubuRunnerTarget target = Targets[targetName];
            foreach (string dependency in target.Dependencies)
            {
                if (false == executedTargets.ContainsKey(dependency))
                {
                    RunTarget(dependency);
                }
            }

            return ReturnThisTRunner();
        }

        /// <summary>
        /// Ensures the directory path exists. If it does not, the method creates all the 
        /// necessary directories in the path.
        /// </summary>
        /// <param name="path">The path to check.</param>
        /// <param name="containsFileName">if set to <c>true</c>, the path contains the file name.</param>
        /// <returns>The same instance of this <see cref="TRunner"/>.</returns>
        public TRunner EnsureDirectoryPathExists(string path, bool containsFileName)
        {
            // remove the file name if it is a part of the path
            if (containsFileName)
                return EnsureDirectoryPathExists(Path.GetDirectoryName(path), false);

            if (Directory.Exists(path))
                return ReturnThisTRunner();

            string parentPath = Path.GetDirectoryName(path);

            if (false == String.IsNullOrEmpty(parentPath) && false == Directory.Exists(parentPath))
                EnsureDirectoryPathExists(parentPath, false);

            Directory.CreateDirectory(path);

            return ReturnThisTRunner();
        }

        public TRunner EnsureSqlServerIsRunning(string machineName)
        {
            EnsureSqlServerIsRunningTask task = new EnsureSqlServerIsRunningTask(machineName);
            return RunTask(task);
        }

        public TRunner ExecuteSqlCommand(string connectionString, string sqlCommandText)
        {
            ExecuteSqlScriptTask.ExecuteSqlCommand(
                this.scriptExecutionEnvironment,
                connectionString,
                sqlCommandText);
            return ReturnThisTRunner();
        }

        public TRunner ExecuteSqlScript(string connectionString, string scriptFilePath)
        {
            ExecuteSqlScriptTask.ExecuteSqlScriptFile(
                this.scriptExecutionEnvironment,
                connectionString, 
                scriptFilePath);
            return ReturnThisTRunner();
        }

        public TRunner ExpandProperties(
            string sourceFileName, 
            string expandedFileName,
            Encoding sourceFileEncoding,
            Encoding expandedFileEncoding,
            IDictionary<string, string> properties)
        {
            ExpandPropertiesTask task = new ExpandPropertiesTask(
                sourceFileName, 
                expandedFileName,
                sourceFileEncoding,
                expandedFileEncoding);

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

            scriptExecutionEnvironment.LogError("ERROR: {0}", message);

            throw new RunnerFailedException(message);
        }

        /// <summary>
        /// Executes the specified action for each file in a directory.
        /// </summary>
        /// <param name="directory">The directory where to look for files.</param>
        /// <param name="searchPattern">The search pattern - only files matching the pattern will be used.</param>
        /// <param name="funcToExecute">The action to execute - the argument of the action will be a file name.</param>
        /// <returns>The same instance of this <see cref="TRunner"/>.</returns>
        public TRunner ForEachFile(
            string directory,
            string searchPattern,
            Action<string> funcToExecute)
        {
            foreach (string fileName in Directory.GetFiles(directory, searchPattern))
                funcToExecute(fileName);

            return ReturnThisTRunner();
        }

        /// <summary>
        /// Formats the string (using <see cref="CultureInfo.InvariantCulture"/>).
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>A formatted string.</returns>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static string FormatString(string format, params object[] args)
        {
            return String.Format(CultureInfo.InvariantCulture, format, args);
        }

        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public TRunner GetLocalIisVersionTask()
        {
            GetLocalIisVersionTask task = new GetLocalIisVersionTask();
            return RunTask(task);
        }

        public TRunner GetRegistryValue(
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
            return ReturnThisTRunner();
        }

        /// <summary>
        /// Determines whether the specified target exists.
        /// </summary>
        /// <param name="targetName">Name of the target.</param>
        /// <returns>
        ///     <c>true</c> if the specified target exists; otherwise, <c>false</c>.
        /// </returns>
        public bool HasTarget(string targetName)
        {
            return Targets.ContainsKey(targetName);
        }

        /// <summary>
        /// Impersonates a specified user.
        /// </summary>
        /// <remarks>The impersonation will be automatically revoked at the end of the runner execution.</remarks>
        /// <param name="userName">The user name.</param>
        /// <param name="domain">The user domain.</param>
        /// <param name="password">The user password.</param>
        /// <returns>The same instance of this <see cref="FlubuRunner{TRunner}"/>.</returns>
        public virtual TRunner ImpersonateUser(string userName, string domain, string password)
        {
            ImpersonateUserTask impersonateUserTask = new ImpersonateUserTask("administrator", "localhost-vm1", "jungle");
            RegisterDisposableObject(impersonateUserTask);
            return RunTask(impersonateUserTask);
        }

        public TRunner InstallAssembly(string assemblyFileName)
        {
            InstallAssemblyTask.Execute(scriptExecutionEnvironment, assemblyFileName);
            return ReturnThisTRunner();
        }

        public TRunner InstallWindowsService(
            string executablePath,
            string serviceName, 
            InstallWindowsServiceMode mode,
            TimeSpan serviceUninstallationWaitTime)
        {
            InstallWindowsServiceTask task = new InstallWindowsServiceTask(executablePath, serviceName, mode);
            task.ServiceUninstallationWaitTime = serviceUninstallationWaitTime;
            return RunTask(task);
        }

        public TRunner KillProcess(string processName)
        {
            KillProcessTask.Execute(scriptExecutionEnvironment, processName);
            return ReturnThisTRunner();
        }

        public TRunner Log(string format, params object[] args)
        {
            scriptExecutionEnvironment.LogMessage(format, args);
            return ReturnThisTRunner();
        }

        public TRunner LogEnvironment()
        {
            LogScriptEnvironmentTask task = new LogScriptEnvironmentTask();
            return RunTask(task);
        }

        public TRunner MarkTargetAsExecuted(FlubuRunnerTarget<TRunner> target)
        {
            executedTargets.Add(target.TargetName, @"dummy");
            return ReturnThisTRunner();
        }

        public TRunner PurgeMessageQueue (string messageQueuePath)
        {
            PurgeMessageQueueTask.Execute(scriptExecutionEnvironment, messageQueuePath);
            return ReturnThisTRunner();
        }

        public TRunner ReadConfigurationFromFile(string configurationFileName)
        {
            ReadConfigurationTask.ReadFromFile(scriptExecutionEnvironment, configurationFileName);
            return ReturnThisTRunner();
        }

        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "string")]
        public TRunner ReadConfigurationFromString(string configurationString)
        {
            ReadConfigurationTask.ReadFromString(scriptExecutionEnvironment, configurationString);
            return ReturnThisTRunner();
        }

        public TRunner RegisterAspNet(
            string virtualDirectoryName,
            string dotNetVersion)
        {
            RegisterAspNetTask.Execute(scriptExecutionEnvironment, virtualDirectoryName, dotNetVersion);
            return ReturnThisTRunner();
        }

        public TRunner RegisterAspNet(
            string virtualDirectoryName,
            string parentVirtualDirectoryName,
            string dotNetVersion)
        {
            RegisterAspNetTask.Execute(scriptExecutionEnvironment, virtualDirectoryName, parentVirtualDirectoryName, dotNetVersion);
            return ReturnThisTRunner();
        }

        /// <summary>
        /// Runs the specified target.
        /// </summary>
        /// <param name="targetName">Name of the target.</param>
        /// <returns>The same instance of this <see cref="TRunner"/>.</returns>
        public TRunner RunTarget(string targetName)
        {
            if (false == Targets.ContainsKey(targetName))
                throw new ArgumentException(FormatString("The target '{0}' does not exist", targetName));

            IFlubuRunnerTarget target = Targets[targetName];
            target.Execute();

            return ReturnThisTRunner();
        }

        public TRunner RunTask(ITask task)
        {
            task.Execute(scriptExecutionEnvironment);
            return ReturnThisTRunner();
        }

        public TRunner SetFileAccessRule(
            string path, 
            string identity, 
            FileSystemRights fileSystemRights, 
            AccessControlType accessControlType)
        {
            SetAccessRuleTask task = new SetAccessRuleTask(path, identity, fileSystemRights, accessControlType);
            return RunTask(task);
        }

        public TRunner SetFileAccessRule(
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

        public TRunner SetRegistryKeyPermissions(
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
            return ReturnThisTRunner();
        }

        /// <summary>
        /// Sets the default target for the runner.
        /// </summary>
        /// <param name="target">The target to be set as the default one.</param>
        /// <returns>The same instance of this <see cref="TRunner"/>.</returns>
        public TRunner SetDefaultTarget(FlubuRunnerTarget<TRunner> target)
        {
            this.defaultTarget = target;
            return ReturnThisTRunner();
        }

        public TRunner SetWindowsServiceAccount(
            string serviceName, 
            string userName, 
            string password)
        {
            SetWindowsServiceAccountTask.Execute(scriptExecutionEnvironment, serviceName, userName, password);
            return ReturnThisTRunner();
        }

        public TRunner Sleep(TimeSpan sleepPeriod)
        {
            SleepTask.Execute(scriptExecutionEnvironment, sleepPeriod);
            return ReturnThisTRunner();
        }

        public TRunner StopWindowsServiceIfExists(string serviceName)
        {
            StopWindowsServiceIfExistsTask.Execute(scriptExecutionEnvironment, serviceName);
            return ReturnThisTRunner();
        }

        /// <summary>
        /// The target for displaying help in the command line.
        /// </summary>
        /// <param name="runner">The runner.</param>
        public void TargetHelp(TRunner runner)
        {
            Log("Targets:"); 

            // first sort the targets
            SortedList<string, IFlubuRunnerTarget> sortedTargets = new SortedList<string, IFlubuRunnerTarget>();

            foreach (IFlubuRunnerTarget target in Targets.Values)
                sortedTargets.Add(target.TargetName, target);

            // now display them in sorted order
            foreach (IFlubuRunnerTarget target in sortedTargets.Values)
                if (false == target.IsHidden)
                    Log(
                        "  {0} : {1}",
                        target.TargetName,
                        target.Description);
        }

        public TRunner TransformXmlFile(string xsltFile, string inputFile, string outputFile)
        {
            XsltTransformTask.Execute(scriptExecutionEnvironment, inputFile, outputFile, xsltFile);
            return ReturnThisTRunner();
        }

        public TRunner UninstallAssembly(string assemblyName)
        {
            UninstallAssemblyTask task = new UninstallAssemblyTask(assemblyName);
            return RunTask(task);
        }

        public TRunner UninstallWindowsService(string executablePath)
        {
            UninstallWindowsServiceTask task = new UninstallWindowsServiceTask(executablePath);
            return RunTask(task);
        }

        /// <summary>
        /// Unzips the specified zip file to a destination directory.
        /// </summary>
        /// <param name="zipFileName">Name of the zip file.</param>
        /// <param name="destinationDirectory">The destination directory where files should be unzipped to.</param>
        /// <returns>The same instance of this <see cref="TRunner"/>.</returns>
        public TRunner Unzip(string zipFileName, string destinationDirectory)
        {
            this.RunTask(new CustomRunnerTask<TRunner>(
                             (TRunner)this,
                             (runner) =>
                             {
                                 FastZip fastZip = new FastZip();
                                 fastZip.CreateEmptyDirectories = true;
                                 fastZip.ExtractZip(
                                     zipFileName,
                                     destinationDirectory,
                                     FastZip.Overwrite.Always,
                                     null,
                                     null,
                                     null,
                                     true);
                             },
                             "Unzipping '{0}' to '{1}'",
                             zipFileName,
                             destinationDirectory));

            return ReturnThisTRunner();
        }

        /// <summary>
        /// Command line parser and runner helper. 
        /// </summary>
        /// <typeparam name="T">The concrete type of the runner.</typeparam>
        /// <param name="runner">See <see cref="FlubuRunner{TRunner}"/></param>
        /// <param name="arguments">Command line arguments.</param>
        /// <returns>0 if success, otherwise error code is returned.</returns>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static int Run<T>(FlubuRunner<T> runner, string[] arguments) where T : FlubuRunner<T>
        {
            try
            {
                // actual run
                if (arguments.Length == 0)
                {
                    if (runner.DefaultTarget != null && !string.IsNullOrEmpty(runner.DefaultTarget.TargetName))
                        runner.RunTarget(runner.DefaultTarget.TargetName);
                    else
                        runner.RunTarget("help");

                    runner.Complete();
                    return 0;
                }

                foreach (string argument in arguments)
                {
                    if (argument.StartsWith("-", StringComparison.OrdinalIgnoreCase) ||
                        argument.StartsWith("/", StringComparison.OrdinalIgnoreCase)) continue;
                    if (runner.HasTarget(argument)) continue;

                    runner.ScriptExecutionEnvironment.LogError(
                        "ERROR: The target '{0}' does not exist",
                        argument);
                    runner.RunTarget("help");
                    return 2;
                }

                foreach (string argument in arguments)
                {
                    if (argument.StartsWith("-", StringComparison.OrdinalIgnoreCase) ||
                        argument.StartsWith("/", StringComparison.OrdinalIgnoreCase)) continue;
                    runner.RunTarget(argument);
                }

                runner.Complete();

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 1;
            }
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
                    buildStopwatch.Stop();
                    scriptExecutionEnvironment.LogRunnerFinished(this);

                    Beep(hasFailed ? MessageBeepType.Error : MessageBeepType.Ok);

                    foreach (IDisposable disposable in stuffToDisposeOf)
                        disposable.Dispose();

                    scriptExecutionEnvironment.Dispose();
                }

                disposed = true;
            }
        }

        /// <summary>
        /// Registers a disposable object which should be disposed right before the runner itself is disposed.
        /// </summary>
        /// <param name="disposable">The disposable object.</param>
        protected void RegisterDisposableObject(IDisposable disposable)
        {
            stuffToDisposeOf.Add(disposable);
        }

        protected TRunner ReturnThisTRunner()
        {
            return (TRunner) this;
        }

        private Stopwatch buildStopwatch = new Stopwatch();
        private FlubuRunnerTarget<TRunner> defaultTarget;
        private bool disposed;
        private ExternalProgramRunner<TRunner> programRunner;
        private readonly Dictionary<string, string> executedTargets = new Dictionary<string, string>();
        private bool hasFailed;
        private IList<string> lastCopiedFilesList;
        private IScriptExecutionEnvironment scriptExecutionEnvironment;
        private List<IDisposable> stuffToDisposeOf = new List<IDisposable>();
        private readonly Dictionary<string, IFlubuRunnerTarget> targets = new Dictionary<string, IFlubuRunnerTarget>();
    }
}