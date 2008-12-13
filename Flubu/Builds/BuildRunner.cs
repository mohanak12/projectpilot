using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Flubu.Builds.VSSolutionBrowsing;
using Flubu.Tasks.FileSystem;
using Flubu.Tasks.Iis;

namespace Flubu.Builds
{
    /// <summary>
    /// A specialization of the <see cref="FlubuRunner{TRunner}"/> oriented towards building .NET solutions.
    /// </summary>
    /// <typeparam name="TRunner">The concrete type of the runner.</typeparam>
    public class BuildRunner<TRunner> : FlubuRunner<TRunner>
        where TRunner : BuildRunner<TRunner>
    {
        public BuildRunner(string productId) : base (productId, @"logs\Flubu.Build.log", 0)
        {
            this.productId = productId;
            this.productName = productId;
            this.buildProducts = new BuildProductsRegistry<TRunner>(ReturnThisTRunner());
        }

        public string BuildConfiguration
        {
            get { return buildConfiguration; }
        }

        public string BuildDir
        {
            get { return buildDir; }
        }

        /// <summary>
        /// Gets the build packages directory.
        /// </summary>
        /// <value>The build packages directory.</value>
        public string BuildPackagesDir
        {
            get { return Path.Combine(BuildDir, "packages"); }
        }

        public BuildProductsRegistry<TRunner> BuildProducts
        {
            get { return buildProducts; }
        }

        public Version BuildVersion
        {
            get { return buildVersion; }
            set { buildVersion = value; }
        }

        public string CcnetArtifactDirectory
        {
            get
            {
                string dir = System.Environment.GetEnvironmentVariable("CCNetArtifactDirectory");
                if (dir == null)
                    throw new InvalidOperationException("CCNetArtifactDirectory environment variable is missing.");
                return dir;
            }
        }

        public bool IsRunningUnderCruiseControl
        {
            get
            {
                string ccnetBuildCondition = System.Environment.GetEnvironmentVariable("CCNetBuildCondition");
                return ccnetBuildCondition != null;
            }
        }

        /// <summary>
        /// Gets the file name of the last zip package that was produced by calling the <see cref="PackageBuildProduct"/> method.
        /// </summary>
        /// <value>The file name of the last zip package.</value>
        public string LastZipPackageFileName
        {
            get { return lastZipPackageFileName; }
        }

        public string ProductId
        {
            get { return productId; }
        }

        public string ProductName
        {
            get { return productName; }
        }

        public string ProductRootDir
        {
            get { return productRootDir; }
        }

        /// <summary>
        /// Gets the dictionary of extended information about VisualStudio projects in the currently
        /// loaded solution. The dictionary key represents the project name.
        /// </summary>
        /// <value>The dictionary of VS project extended information.</value>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
        public IDictionary<string, VSProjectExtendedInfo> ProjectExtendedInfos
        {
            get { return projectExtendedInfos; }
        }

        /// <summary>
        /// Gets the <see cref="VSSolution"/> object for the loaded VisualStudio solution.
        /// </summary>
        /// <value>The <see cref="VSSolution"/> object.</value>
        public VSSolution Solution
        {
            get { return solution; }
        }

        public int TestRuns
        {
            get { return testRuns; }
        }

        public TRunner CleanOutput()
        {
            ScriptExecutionEnvironment.LogTaskStarted("Cleaning solution outputs");

            solution.ForEachProject(
                delegate (VSProjectInfo projectInfo)
                    {             
                        string projectOutputPath = GetProjectOutputPath(projectInfo.ProjectName);

                        if (projectOutputPath == null)
                            return;

                        projectOutputPath = Path.Combine(projectInfo.ProjectDirectoryPath, projectOutputPath);

                        DeleteDirectory(projectOutputPath, false);

                        string projectObjPath = String.Format(
                            CultureInfo.InvariantCulture,
                            @"{0}\obj\{1}",
                            projectInfo.ProjectName,
                            buildConfiguration);
                        projectObjPath = Path.Combine(productRootDir, projectObjPath);
                        DeleteDirectory(projectObjPath, false);
                    });

            ScriptExecutionEnvironment.LogTaskFinished();
            return ReturnThisTRunner();
        }

        public TRunner CompileSolution()
        {
            ScriptExecutionEnvironment.LogTaskStarted ("Compiling the solution");

            AddProgramArgument(MakePathFromRootDir(productId) + ".sln");
            AddProgramArgument("/p:Configuration={0}", buildConfiguration);
            AddProgramArgument("/p:Platform=Any CPU");
            AddProgramArgument("/consoleloggerparameters:NoSummary");

            RunProgram(@"C:\Windows\Microsoft.NET\Framework\v3.5\msbuild.exe");

            ScriptExecutionEnvironment.LogTaskFinished ();
            return ReturnThisTRunner ();
        }

        /// <summary>
        /// Copies all of the build results to the CCNet artifact directory.
        /// </summary>
        /// <returns>The same instance of this <see cref="BuildRunner"/>.</returns>
        public TRunner CopyBuildLogsToCCNet()
        {
            if (false == IsRunningUnderCruiseControl)
                return ReturnThisTRunner();

            string ccnetArtifactDirectory = CcnetArtifactDirectory;
            Log("Copying build logs to CCNet artifact directory '{0}'", ccnetArtifactDirectory);

            string buildLogsDir = EnsureBuildLogsTestDirectoryExists();

            CopyDirectoryStructure(buildLogsDir, ccnetArtifactDirectory, true);

            return ReturnThisTRunner();
        }

        /// <summary>
        /// Copies build products' files to an extra location in order to be able to zip them into packages later.
        /// </summary>
        /// <returns>The same instance of this <see cref="BuildRunner"/>.</returns>
        public TRunner CopyBuildProductFiles()
        {
            // skip this step if we don't have any products to package
            if (false == buildProducts.HasAnyProducts)
                return ReturnThisTRunner();

            Log("Packaging build products");

            // create root directory for the package
            string packageName = FormatString("{0}-{1}", ProductId, BuildVersion);

            string packagesDir = Path.GetFullPath(MakePathFromRootDir(BuildPackagesDir));
            CreateDirectory(packagesDir, true);
            packagesDir = Path.Combine(packagesDir, packageName);
            CreateDirectory(packagesDir, true);

            // copy all the necessary files to the package directory
            buildProducts.CopyProducts(packagesDir);

            return ReturnThisTRunner();
        }

        /// <summary>
        /// Copies the last packaged build product to CruiseControl.NET artifact directory.
        /// </summary>
        /// <param name="buildProductDestinationFileNameFormat">Specifies the format of the build product destination file. The format uses the following
        /// arguments:
        /// <list type="bullet">
        ///         <item>
        ///             <term>{0}</term>
        ///             <description>ProductId</description>
        ///         </item>
        ///         <item>
        ///             <term>{1}</term>
        ///             <description>BuildVersion (4 numbers)</description>
        ///         </item>
        ///         <item>
        ///             <term>{2}</term>
        ///             <description>BuildVersion (major)</description>
        ///         </item>
        ///         <item>
        ///             <term>{3}</term>
        ///             <description>BuildVersion (minor)</description>
        ///         </item>
        ///         <item>
        ///             <term>{4}</term>
        ///             <description>File name of the last zip package (without the directory path)</description>
        ///         </item>
        ///     </list></param>
        /// <returns>The same instance of this <see cref="BuildRunner{TRunner}"/>.</returns>
        public TRunner CopyBuildProductToCCNet(string buildProductDestinationFileNameFormat)
        {
            if (false == IsRunningUnderCruiseControl)
                return ReturnThisTRunner();

            // construct the actual destination path
            string destinationPath = String.Format(
                CultureInfo.InvariantCulture,
                buildProductDestinationFileNameFormat,
                ProductId,
                BuildVersion,
                BuildVersion.Major,
                BuildVersion.Minor,
                Path.GetFileName(LastZipPackageFileName));

            // combine this with the CCNet artifacts directory
            destinationPath = Path.Combine(CcnetArtifactDirectory, destinationPath);

            this.EnsureDirectoryPathExists(destinationPath, true);
            this.CopyFile(lastZipPackageFileName, destinationPath, true);

            return ReturnThisTRunner();
        }

        /// <summary>
        /// Executes the specified SQL script file.
        /// </summary>
        /// <param name="scriptFileName">Name of the script file.</param>
        /// <param name="userName">Name of the database user.</param>
        /// <param name="password">The database user's password.</param>
        /// <returns>The same instance of this <see cref="BuildRunner"/>.</returns>
        public TRunner ExecuteSqlScriptFile(
            string scriptFileName, 
            string userName,
            string password)
        {
            return ExecuteSqlScriptFile(scriptFileName, userName, password, null);
        }

        /// <summary>
        /// Executes the specified SQL script file.
        /// </summary>
        /// <param name="scriptFileName">Name of the script file.</param>
        /// <param name="userName">Name of the database user.</param>
        /// <param name="password">The database user's password.</param>
        /// <param name="useDatabase">The database to run the script under. If <c>null</c>, the script will run under
        /// the default database.</param>
        /// <returns>The same instance of this <see cref="BuildRunner"/>.</returns>
        public TRunner ExecuteSqlScriptFile(
            string scriptFileName, 
            string userName,
            string password,
            string useDatabase)
        {
            AddProgramArgument("-U");
            AddProgramArgument(userName);
            AddProgramArgument("-P");
            AddProgramArgument(password);
            AddProgramArgument("-i");
            AddProgramArgument(scriptFileName);
            //AddProgramArgument("-b");
            AddProgramArgument("-r1");
            AddProgramArgument("-m-1");
            //AddProgramArgument("-V");
            //AddProgramArgument("0");
            //AddProgramArgument("-o");
            //AddProgramArgument("test.txt");

            if (useDatabase != null)
            {
                AddProgramArgument("-d");
                AddProgramArgument(useDatabase);
            }

            RunProgram(@"C:\Program Files\Microsoft SQL Server\90\Tools\Binn\osql.exe", false);

            return ReturnThisTRunner();
        }

        /// <summary>
        /// Fetches the build version, either from the local version info file or from CCNet
        /// (if the build is running under CCNet).
        /// </summary>
        /// <returns>
        /// The same instance of this <see cref="GaneshaBuildRunner"/>.
        /// </returns>
        public TRunner FetchBuildVersion()
        {
            ScriptExecutionEnvironment.LogTaskStarted("Fetching the build version");

            if (IsRunningUnderCruiseControl)
            {
                string ccnetLabel = Environment.GetEnvironmentVariable("CCNetLabel");
                Regex ccnetLabelRegex = new Regex(@"(?<version>([0-9]+.){3}[0-9]+)", RegexOptions.ExplicitCapture);
                Match match = ccnetLabelRegex.Match(ccnetLabel);
                if (false == match.Success)
                {
                    Log(
                        "CCNet build label '{0}' could not be parsed to extract the build version. Using the default version instead",
                        ccnetLabel);
                }
                else
                    BuildVersion = new Version(match.Groups["version"].Value);
            }
            else
            {
                string projectVersionFileName = MakePathFromRootDir(ProductId) + ".ProjectVersion.txt";

                if (false == File.Exists(projectVersionFileName))
                    Fail("Project version file ('{0}') is missing.", projectVersionFileName);

                using (Stream stream = File.Open(projectVersionFileName, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string versionAsString = reader.ReadLine();
                        BuildVersion = new Version(versionAsString);
                    }
                }
            }

            Log("Project build version: {0}", BuildVersion);

            ScriptExecutionEnvironment.LogTaskFinished();
            return ReturnThisTRunner();
        }

        public TRunner FxCop()
        {
            ScriptExecutionEnvironment.LogTargetStarted("Running FxCop analysis");

            string fxProjectPath = MakePathFromRootDir(productId) + ".FxCop";

            AssertFileExists("FxCop project file", fxProjectPath);

            string fxReportPath = EnsureBuildLogsTestDirectoryExists();
            fxReportPath = Path.Combine(fxReportPath, productId);
            fxReportPath = String.Format(CultureInfo.InvariantCulture, "{0}.FxCopReport.xml", fxReportPath);

            AddProgramArgument(@"/project:{0}", fxProjectPath);
            AddProgramArgument(@"/out:{0}", fxReportPath);
            AddProgramArgument(@"/dictionary:CustomDictionary.xml");
            AddProgramArgument(@"/ignoregeneratedcode");
            RunProgram(MakePathFromRootDir(@".\lib\Microsoft FxCop 1.36\FxCopCmd.exe"), true);

            // check if the report file was generated
            bool isReportFileGenerated = File.Exists(fxReportPath);

            // FxCop returns different exit codes for different things
            // see http://msdn.microsoft.com/en-us/library/bb164705(VS.80).aspx for the list of exit codes
            // exit code 4 means "Project load error" but it occurs when the old FxCop violations exist
            // which are then removed from the code
            if ((LastExitCode != 0 && LastExitCode != 4) || isReportFileGenerated)
            {
                if (false == IsRunningUnderCruiseControl)
                {
                    // run FxCop GUI
                    AddProgramArgument(fxProjectPath);
                    RunProgram(MakePathFromRootDir(@".\lib\Microsoft FxCop 1.36\FxCop.exe"));
                }
                else if (File.Exists(fxReportPath))
                    File.Copy(fxReportPath, ccnetDir);

                Fail("FxCop found violations in the code.");
            }

            ScriptExecutionEnvironment.LogTargetFinished();
            return ReturnThisTRunner();
        }

        public TRunner GenerateCommonAssemblyInfo()
        {
            ScriptExecutionEnvironment.LogTaskStarted("Generating CommonAssemblyInfo file");

            if (buildVersion == null)
                Fail("Assembly file version is not set.");

            using (Stream stream = File.Open(Path.Combine(productRootDir, "CommonAssemblyInfo.cs"), FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(
                        @"using System.Reflection;

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: AssemblyConfigurationAttribute(""{0}"")]
[assembly: AssemblyCompanyAttribute(""{1}"")]
[assembly: AssemblyProductAttribute(""{2}"")]
[assembly: AssemblyCopyrightAttribute(""{3}"")]
[assembly: AssemblyTrademarkAttribute(""{4}"")]
[assembly: AssemblyCultureAttribute("""")]
[assembly: AssemblyFileVersionAttribute(""{5}"")]
[assembly: AssemblyInformationalVersionAttribute(""{6}"")]
",
                        buildConfiguration,
                        companyName,
                        productName,
                        companyCopyright,
                        companyTrademark,
                        buildVersion,
                        buildVersion.ToString(2));
                }
            }

            ScriptExecutionEnvironment.LogTaskFinished();

            return ReturnThisTRunner();
        }

        /// <summary>
        /// Gets the output path for a specified VisualStudio project. The output path is relative
        /// to the directory where the project file is located.
        /// </summary>
        /// <param name="projectName">Name of the project.</param>
        /// <returns>The output path or <c>null</c> if the project is not compatibile.</returns>
        /// <exception cref="ArgumentException">The method could not extract the data from the project file.</exception>
        public string GetProjectOutputPath(string projectName)
        {
            VSProjectInfo projectInfo = solution.FindProjectByName(projectName);

            // skip non-C# projects
            if (projectInfo.ProjectTypeGuid != VSProjectType.CSharpProjectType.ProjectTypeGuid)
                return null;

            // find the project configuration
            string condition = FormatString(" '$(Configuration)|$(Platform)' == '{0}|AnyCPU' ", buildConfiguration);
            VSProjectConfiguration projectConfiguration = projectInfo.Project.FindConfiguration(condition);
            if (projectConfiguration == null)
                throw new ArgumentException(
                    FormatString(
                        "Could not find '{0}' configuration for the project '{1}'.",
                        condition,
                        projectName));

            if (false == projectConfiguration.Properties.ContainsKey("OutputPath"))
                throw new ArgumentException (
                    FormatString (
                        "Missing OutputPath for the '{0}' configuration of the project '{1}'.",
                        buildConfiguration,
                        projectName));

            return projectConfiguration.Properties["OutputPath"];
        }

        public TRunner LoadSolution (string solutionFileName)
        {
            solution = VSSolution.Load(MakePathFromRootDir(solutionFileName));

            solution.ForEachProject(delegate (VSProjectInfo projectInfo)
                                        {
                                            if (projectInfo.ProjectTypeGuid != VSProjectType.CSharpProjectType.ProjectTypeGuid)
                                                return;

                                            projectExtendedInfos.Add(
                                                projectInfo.ProjectName, 
                                                new VSProjectExtendedInfo(projectInfo));
                                        });

            // also load project files
            solution.LoadProjects();

            return ReturnThisTRunner();
        }

        //public override TRunner LogTarget(string targetName)
        //{
        //    base.LogTarget(targetName);

        //    if (IsRunningUnderCruiseControl)
        //    {
        //        string ccnetListenerFile = System.Environment.GetEnvironmentVariable("CCNetListenerFile");

        //        // if the CCNet emits the listener file path, we will produce the file
        //        if (ccnetListenerFile != null)
        //        {
        //            string logText = String.Format(
        //                CultureInfo.InvariantCulture,
        //                @"<Item Time='{0}' Data='{1}'/>",
        //                //@"<data><Item Time='{0}' Data='{1}'/></data>",
        //                DateTime.Now.ToString("u", CultureInfo.InvariantCulture),
        //                targetName);

        //            using (Stream stream = File.Open(ccnetListenerFile, FileMode.Append))
        //            {
        //                using (StreamWriter writer = new StreamWriter(stream))
        //                {
        //                    writer.WriteLine(logText);
        //                }
        //            }
        //        }
        //    }

        //    return ReturnThisTRunner();
        //}

        public TRunner MergeCoverageReports()
        {
            // do this only if there were actual tests run
            if (coverageResultsExist)
            {
                string buildLogsDir = EnsureBuildLogsTestDirectoryExists();
                string ncoverExplorerConfigFileName = MakePathFromRootDir(ProductId) + ".NCoverExplorer.config";

                AddProgramArgument(Path.Combine(buildLogsDir, "Coverage*.xml"));
                if (File.Exists(ncoverExplorerConfigFileName))
                    AddProgramArgument("/c:{0}", ncoverExplorerConfigFileName);
                AddProgramArgument("/x:{0}", Path.Combine(buildLogsDir, "MergedCoverageReport.xml"));
                AddProgramArgument("/r:5");
                AddProgramArgument("/p:{0}", ProductName);
                RunProgram(@"lib\NCoverExplorer-1.3.6.36\NCoverExplorer.Console.exe");
            }

            return ReturnThisTRunner();
        }

        /// <summary>
        /// Packages the build product into a zip file.
        /// </summary>
        /// <param name="zipFileNameFormat">
        /// The zip file name format. The format uses the following arguments:
        /// <list type="bullet">
        ///     <item>
        ///         <term>{0}</term>
        ///         <description>ProductId</description>
        ///     </item>
        ///     <item>
        ///         <term>{1}</term>
        ///         <description>BuildVersion (4 numbers)</description>
        ///     </item>
        /// </list> 
        /// </param>
        /// <param name="productPartsIds">The list of product parts IDs which should be included in the package.
        /// If the list is empty, the method packages all available product part IDs.</param>
        /// <returns>The same instance of this <see cref="TRunner"/>.</returns>
        public TRunner PackageBuildProduct(
            string zipFileNameFormat,
            params string[] productPartsIds)
        {
            IEnumerable<string> filesToZip = buildProducts.ListFilesForProductParts(productPartsIds);

            string zipFileName = String.Format(
                CultureInfo.InvariantCulture,
                zipFileNameFormat,
                ProductId,
                BuildVersion);

            string packagesDir = MakePathFromRootDir(BuildPackagesDir);
            zipFileName = Path.Combine(packagesDir, zipFileName);
            
            ZipFilesTask task = new ZipFilesTask(
                zipFileName, 
                Path.GetFullPath(MakePathFromRootDir(BuildPackagesDir)), 
                filesToZip);
            RunTask(task);

            lastZipPackageFileName = zipFileName;

            return ReturnThisTRunner();
        }

        public TRunner PrepareBuildDirectory()
        {
            ScriptExecutionEnvironment.LogTaskStarted("Preparing the build directory");

            string fullBuildDir = MakePathFromRootDir(buildDir);

            DeleteDirectory(fullBuildDir, false);
            CreateDirectory(fullBuildDir, true);

            ScriptExecutionEnvironment.LogTaskFinished();

            return ReturnThisTRunner();
        }

        public TRunner PrepareDatabase(
            string dbScriptsDirectory,
            string dbName,
            string dbUserName,
            string dbUserPassword)
        {
            Dictionary<string, string> properties = new Dictionary<string, string>();
            properties.Add("${ScriptDBName}", dbName);
            properties.Add("${ScriptDBUserName}", dbUserName);
            properties.Add("${ScriptDBUserPassword}", dbUserPassword);

            string fileFilter = FormatString("{0}.*.sql", dbName);
            string expandedFilesDirectory = MakePathFromRootDir(Path.Combine(dbScriptsDirectory, "expanded"));

            // create the "expanded" subdirectory if it doesn't already exist
            CreateDirectory(expandedFilesDirectory, false);

            // expand all .SQL files for this database and store them into a separate directory
            this.ForEachFile(
                dbScriptsDirectory, 
                "*.sql", 
                file => this.ExpandProperties(
                  file, 
                  Path.Combine(expandedFilesDirectory, Path.GetFileName(file)),
                  Encoding.UTF8,
                  new UTF8Encoding(false), 
                  properties));

            string dbAdminUserName = "BuildMaster";
            string dbAdminPassword = "masterbuild";

            string sqlFileToExecute = FormatString("{0}.CreateDatabase.sql", dbName);
            sqlFileToExecute = Path.Combine(expandedFilesDirectory, sqlFileToExecute);
            ExecuteSqlScriptFile(sqlFileToExecute, dbAdminUserName, dbAdminPassword);

            sqlFileToExecute = FormatString("{0}.DbScript.sql", dbName);
            sqlFileToExecute = Path.Combine(expandedFilesDirectory, sqlFileToExecute);
            ExecuteSqlScriptFile(sqlFileToExecute, dbAdminUserName, dbAdminPassword, dbName);

            return ReturnThisTRunner();
        }

        /// <summary>
        /// Prepares web applications in IIS for registered VisualStudio Web projects.
        /// </summary>
        /// <returns>The same instance of this <see cref="BuildRunner"/>.</returns>
        public TRunner PrepareWebApplications()
        {
            foreach (KeyValuePair<string, VSProjectExtendedInfo> info in this.ProjectExtendedInfos)
            {
                if (info.Value.IsWebProject)
                {
                    string projectName = info.Key;

                    Uri webApplicationUrl = info.Value.WebApplicationUrl;
                    if (0 != String.Compare(webApplicationUrl.Host, "localhost", StringComparison.OrdinalIgnoreCase))
                        throw new ArgumentException(
                            FormatString(
                                "Cannot create the Web application '{0}'. Only localhost is currently supported.",
                                webApplicationUrl));

                    string virtualDirectoryName = webApplicationUrl.GetComponents(UriComponents.Path, UriFormat.SafeUnescaped);
                    string localAppPath =
                        Path.GetFullPath(
                            MakePathFromRootDir(Solution.FindProjectByName(projectName).ProjectDirectoryPath));

                    CreateVirtualDirectoryTask task = new CreateVirtualDirectoryTask(
                        virtualDirectoryName,
                        localAppPath,
                        CreateVirtualDirectoryMode.UpdateIfExists);

                    task.Execute(this.ScriptExecutionEnvironment);
                }
            }

            return ReturnThisTRunner();
        }

        /// <summary>
        /// Registers the specified project as a web project.
        /// </summary>
        /// <param name="projectName">Name of the project.</param>
        /// <param name="webApplicationUrl">The project's Web application URL.</param>
        /// <returns>The same instance of this <see cref="BuildRunner{TRunner}"/>.</returns>
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#")]
        public TRunner RegisterAsWebProject(string projectName, string webApplicationUrl)
        {
            VSProjectExtendedInfo projectExtendedInfo = projectExtendedInfos[projectName];
            projectExtendedInfo.IsWebProject = true;
            projectExtendedInfo.WebApplicationUrl = new Uri(webApplicationUrl);
            return ReturnThisTRunner();
        }

        public TRunner RunTests(string projectName, bool collectCoverageData)
        {
            Log("Runs tests on '{0}' assembly ({1})", projectName, collectCoverageData ? "with coverage" : "without coverage");

            string buildLogsDir = EnsureBuildLogsTestDirectoryExists();

            VSProjectInfo testProjectInfo = Solution.FindProjectByName(projectName);
            string testedAssemblyFileName = testProjectInfo.ProjectDirectoryPath;
            string path2 = String.Format(
                CultureInfo.InvariantCulture,
                @"bin\{0}\{1}.dll",
                BuildConfiguration,
                testProjectInfo.ProjectName);
            testedAssemblyFileName = Path.Combine(
                testedAssemblyFileName,
                path2);

            testedAssemblyFileName = Path.GetFullPath(MakePathFromRootDir(testedAssemblyFileName));

            string gallioEchoExePath = MakePathFromRootDir(@"lib\Gallio\bin\Gallio.Echo.exe");

            if (collectCoverageData)
                AddProgramArgument(gallioEchoExePath);

            AddProgramArgument(testedAssemblyFileName);
            AddProgramArgument("/report-directory:{0}", buildLogsDir);
            AddProgramArgument("/report-name-format:TestResults-{0}", TestRuns);
            AddProgramArgument("/report-type:xml");
            AddProgramArgument("/verbosity:verbose");
            //AddProgramArgument("/runner:IsolatedAppDomain");

            //// create a list of our assemblies which we want to measure coverage of
            //StringBuilder assembliesList = new StringBuilder();
            //this.Solution.ForEachProject(delegate(VSProjectInfo projectInfo)
            //{
            // if (projectInfo.ProjectTypeGuid == VSProjectType.CSharpProjectType.ProjectTypeGuid)
            //     assembliesList.AppendFormat("{0};", projectInfo.ProjectName);
            //});

            if (collectCoverageData)
            {
                AddProgramArgument("//reg");
                //AddProgramArgument("//a");
                //AddProgramArgument(assembliesList.ToString());
                AddProgramArgument("//x");
                AddProgramArgument(@"{0}\Coverage-{1}.xml", buildLogsDir, TestRuns);
                AddProgramArgument("//ea");
                AddProgramArgument("MbUnit.Framework.TestFixtureAttribute");
                AddProgramArgument("//w");
                AddProgramArgument(Path.GetDirectoryName(testedAssemblyFileName));
                AddProgramArgument("//v");
                RunProgram(MakePathFromRootDir(@"lib\NCover v1.5.8\NCover.Console.exe"));

                coverageResultsExist = true;
            }
            else
                RunProgram(gallioEchoExePath);

            IncrementTestRunsCounter();

            return ReturnThisTRunner();
        }

        public TRunner SetCompanyInfo (
            string companyName,
            string companyCopyright,
            string companyTrademark)
        {
            this.companyName = companyName;
            this.companyCopyright = companyCopyright;
            this.companyTrademark = companyTrademark;

            return ReturnThisTRunner();
        }

        public TRunner SetProductName(string productName)
        {
            this.productName = productName;
            return ReturnThisTRunner();
        }

        public TRunner SetProductRootDir (string productRootDir)
        {
            this.productRootDir = productRootDir;
            return ReturnThisTRunner();
        }

        /// <summary>
        /// Ensures the build logs directory exists and returns a full path to it.
        /// </summary>
        /// <returns>A full path to the build logs directory.</returns>
        protected string EnsureBuildLogsTestDirectoryExists()
        {
            string buildLogsDir = Path.GetFullPath(Path.Combine(BuildDir, "BuildLogs"));
            CreateDirectory(buildLogsDir, false);
            return buildLogsDir;
        }

        /// <summary>
        /// Increments the test runs counter.
        /// </summary>
        protected void IncrementTestRunsCounter()
        {
            testRuns++;
        }

        protected string MakePathFromRootDir(string fileName)
        {
            return Path.Combine(productRootDir, fileName);
        }

        private string buildConfiguration = "Release";
        private string buildDir = "Builds";
        private readonly BuildProductsRegistry<TRunner> buildProducts;
        private Version buildVersion = new Version("1.0.0.0");
        private string ccnetDir = "CruiseControl";
        private string companyCopyright;
        private string companyName;
        private string companyTrademark;
        private bool coverageResultsExist;
        private string lastZipPackageFileName;
        private readonly string productId;
        private string productName;
        private string productRootDir = String.Empty;
        private Dictionary<string, VSProjectExtendedInfo> projectExtendedInfos = new Dictionary<string, VSProjectExtendedInfo>();
        private VSSolution solution;
        private int testRuns;
    }
}