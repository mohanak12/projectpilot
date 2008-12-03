using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Flubu;
using ProjectPilot.BuildScripts.VSSolutionBrowsing;

namespace ProjectPilot.BuildScripts
{
    public class BuildRunner : FlubuRunner
    {
        public BuildRunner(string productId)
        {
            this.productId = productId;
            this.productName = productId;
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
        /// Gets the <see cref="VSSolution"/> object for the loaded VisualStudio solution.
        /// </summary>
        /// <value>The <see cref="VSSolution"/> object.</value>
        public VSSolution Solution
        {
            get { return solution; }
        }

        public BuildRunner AssertFileExists(string fileDescription, string fileName)
        {
            if (false == File.Exists(fileName))
                Fail("{0} ('{1}') does not exist", fileDescription, fileName);

            return this;
        }

        public BuildRunner CleanOutput()
        {
            LogTarget("CleanOutput");

            solution.ForEachProject(delegate (VSProjectInfo projectInfo)
            {             
                // skip non-C# projects
                if (projectInfo.ProjectTypeGuid != VSProjectType.CSharpProjectType.ProjectTypeGuid)
                    return;

                bool isWebProject = false;
                if (projectExtendedInfos.ContainsKey(projectInfo.ProjectName))
                {
                    VSProjectExtendedInfo extendedInfo = projectExtendedInfos[projectInfo.ProjectName];
                    isWebProject = extendedInfo.IsWebProject;
                }

                string projectBinPath;
                if (false == isWebProject)
                    projectBinPath = String.Format(
                        CultureInfo.InvariantCulture,
                        @"{0}\bin\{1}",
                        projectInfo.ProjectName,
                        buildConfiguration);
                else
                    projectBinPath = String.Format(
                        CultureInfo.InvariantCulture,
                        @"{0}\bin",
                        projectInfo.ProjectName);

                projectBinPath = Path.Combine(productRootDir, projectBinPath);

                DeleteDirectory(projectBinPath, false);

                string projectObjPath = String.Format(
                    CultureInfo.InvariantCulture,
                    @"{0}\obj\{1}",
                    projectInfo.ProjectName,
                    buildConfiguration);
                projectObjPath = Path.Combine(productRootDir, projectObjPath);
                DeleteDirectory(projectObjPath, false);
            });

            return this;
        }

        public BuildRunner CompileSolution()
        {
            LogTarget("CompileSolution");

            AddProgramArgument(MakePathFromRootDir(productId) + ".sln");
            AddProgramArgument("/p:Configuration={0}", buildConfiguration);
            AddProgramArgument("/p:Platform=Any CPU");
            AddProgramArgument("/consoleloggerparameters:NoSummary");

            return (BuildRunner) RunProgram(@"C:\Windows\Microsoft.NET\Framework\v3.5\msbuild.exe");           
        }

        /// <summary>
        /// Formats the string (using <see cref="CultureInfo.InvariantCulture"/>).
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>A formatted string.</returns>
        public static string FormatString(string format, params object[] args)
        {
            return String.Format(CultureInfo.InvariantCulture, format, args);
        }

        public BuildRunner FxCop()
        {
            LogTarget("FxCop");

            string fxProjectPath = MakePathFromRootDir(productId) + ".FxCop";

            AssertFileExists("FxCop project file", fxProjectPath);

            string fxReportPath = Path.Combine(productRootDir, buildDir);
            fxReportPath = Path.Combine(fxReportPath, productId);
            fxReportPath = String.Format(CultureInfo.InvariantCulture, "{0}.FxCopReport.xml", fxReportPath);

            AddProgramArgument(@"/project:{0}", fxProjectPath);
            AddProgramArgument(@"/out:{0}", fxReportPath);
            AddProgramArgument(@"/dictionary:CustomDictionary.xml");
            AddProgramArgument(@"/ignoregeneratedcode");
            RunProgram(MakePathFromRootDir(@".\lib\Microsoft FxCop 1.36\FxCopCmd.exe"));

            // check if the report file was generated
            if (File.Exists(fxReportPath))
            {
                if (false == IsRunningUnderCruiseControl)
                {
                    // run FxCop GUI
                    AddProgramArgument(fxProjectPath);
                    RunProgram(MakePathFromRootDir(@".\lib\Microsoft FxCop 1.36\FxCop.exe"));
                }
                else
                    File.Copy(fxReportPath, ccnetDir);

                Fail("FxCop found violations in the code.");
            }

            return this;
        }

        public BuildRunner GenerateCommonAssemblyInfo()
        {
            LogTarget("GenerateCommonAssemblyInfo");

            if (fileVersion == null)
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
            fileVersion,
            fileVersion.ToString(2));
                }
            }

            return this;
        }

        public BuildRunner LoadSolution (string solutionFileName)
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

            return this;
        }

        public BuildRunner PrepareBuildDirectory()
        {
            LogTarget("PrepareBuildDirectory");

            string fullBuildDir = MakePathFromRootDir(buildDir);

            DeleteDirectory(fullBuildDir, false);

            CreateDirectory(fullBuildDir);

            return this;
        }

        public BuildRunner ReadVersionInfo()
        {
            LogTarget("ReadVersionInfo");

            string projectVersionFileName = MakePathFromRootDir(productId) + ".ProjectVersion.txt";

            if (false == File.Exists(projectVersionFileName))
                Fail("Project version file ('{0}') is missing.", projectVersionFileName);

            using (Stream stream = File.Open(projectVersionFileName, FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    string versionAsString = reader.ReadLine();
                    fileVersion = new Version(versionAsString);
                }
            }

            return this;
        }

        /// <summary>
        /// Registers the specified project as a web project.
        /// </summary>
        /// <param name="projectName">Name of the project.</param>
        /// <param name="webApplicationUrl">The project's Web application URL.</param>
        /// <returns>The same instance of this <see cref="BuildRunner"/>.</returns>
        public BuildRunner RegisterAsWebProject(string projectName, string webApplicationUrl)
        {
            VSProjectExtendedInfo projectExtendedInfo = projectExtendedInfos[projectName];
            projectExtendedInfo.IsWebProject = true;
            projectExtendedInfo.WebApplicationUrl = new Uri(webApplicationUrl);
            return this;
        }

        public BuildRunner RunTests(string projectName)
        {
            LogTarget("RunTests");

            string unitTestResultsDir = MakePathFromRootDir(buildDir);
            unitTestResultsDir = Path.Combine(unitTestResultsDir, "UnitTestResults");

            VSProjectInfo projectInfo = solution.FindProjectByName(projectName);
            string testedAssembly = projectInfo.ProjectDirectoryPath;
            testedAssembly = Path.Combine(
                testedAssembly, 
                String.Format(
                    CultureInfo.InvariantCulture,
                    @"bin\{0}\{1}.dll",
                    buildConfiguration,
                    projectInfo.ProjectName));

            AddProgramArgument(testedAssembly);
            AddProgramArgument("/report-directory:{0}", unitTestResultsDir);
            AddProgramArgument("/report-name-format:TestResults-{0}", testRuns);
            AddProgramArgument("/report-type:xml");
            AddProgramArgument("/verbosity:verbose");
            RunProgram(MakePathFromRootDir(@"lib\Gallio\bin\Gallio.Echo.exe"));

            testRuns++;

            return this;
        }

        public BuildRunner SetCompanyInfo (
            string companyName,
            string companyCopyright,
            string companyTrademark)
        {
            this.companyName = companyName;
            this.companyCopyright = companyCopyright;
            this.companyTrademark = companyTrademark;

            return this;
        }

        public BuildRunner SetProductName(string productName)
        {
            this.productName = productName;
            return this;
        }

        public BuildRunner SetProductRootDir (string productRootDir)
        {
            this.productRootDir = productRootDir;
            return this;
        }

        protected IDictionary<string, VSProjectExtendedInfo> ProjectExtendedInfos
        {
            get { return projectExtendedInfos; }
        }

        protected void LogTarget (string targetName)
        {
            Log(String.Empty);
            Log("{0}:", targetName);
            Log(String.Empty);

            if (IsRunningUnderCruiseControl)
            {
                string ccnetListenerFile = System.Environment.GetEnvironmentVariable("CCNetListenerFile");

                // if the CCNet emits the listener file path, we will produce the file
                if (ccnetListenerFile != null)
                {
                    // delete the old contents of the listener file on the first use
                    if (false == isCcnetListenerFileInitialized
                        && File.Exists(ccnetListenerFile))
                    {
                        File.Delete(ccnetListenerFile);
                        isCcnetListenerFileInitialized = true;
                    }

                    string logText = String.Format(
                        CultureInfo.InvariantCulture,
                        @"<Item Time='{0}' Data='{1}'/>",
                        //@"<data><Item Time='{0}' Data='{1}'/></data>",
                        DateTime.Now.ToString("u"),
                        targetName);

                    using (Stream stream = File.Open(ccnetListenerFile, FileMode.OpenOrCreate))
                    {
                        using (StreamWriter writer = new StreamWriter(stream))
                        {
                            writer.WriteLine(logText);
                        }
                    }
                }
            }
        }

        protected string MakePathFromRootDir(string fileName)
        {
            return Path.Combine(productRootDir, fileName);
        }

        private string buildConfiguration = "Release";
        private string buildDir = "Builds";
        private string companyCopyright;
        private string companyName;
        private string companyTrademark;
        private string ccnetDir = "CruiseControl";
        private Version fileVersion;
        private bool isCcnetListenerFileInitialized;
        private IBuildLogger logger = new BuildLogger();
        private readonly string productId;
        private string productName;
        private string productRootDir = String.Empty;
        private Dictionary<string, VSProjectExtendedInfo> projectExtendedInfos = new Dictionary<string, VSProjectExtendedInfo>();
        private VSSolution solution;
        private int testRuns = 0;
    }
}