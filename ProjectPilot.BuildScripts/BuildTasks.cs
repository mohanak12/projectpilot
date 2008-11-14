using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;

namespace ProjectPilot.BuildScripts
{
    public class BuildTasks : IDisposable
    {
        public BuildTasks(string productId)
        {
            this.productId = productId;
            this.productName = productId;
            hasFailed = true;
        }

        public bool IsRunningUnderCruiseControl
        {
            get { return isRunningUnderCruiseControl; }
        }

        public BuildTasks AddProgramArgument(string argument)
        {
            programArgs.Add(argument);
            return this;
        }

        public BuildTasks AddProgramArgument (string format, params object[] args)
        {
            programArgs.Add(string.Format(format, args));
            return this;
        }

        public BuildTasks AddMainProject(string projectName)
        {
            ProjectToBuild project = new ProjectToBuild(projectName);
            project.IsMainProject = true;
            projects.Add(project);
            return this;
        }

        public BuildTasks AddMainWebProject(string projectName)
        {
            ProjectToBuild project = new ProjectToBuild(projectName);
            project.IsMainProject = true;
            project.IsWebProject = true;
            projects.Add(project);
            return this;
        }

        public BuildTasks AddProject (string projectName)
        {
            ProjectToBuild project = new ProjectToBuild(projectName);
            projects.Add(project);
            return this;
        }

        public BuildTasks AddTestProject(string projectName)
        {
            ProjectToBuild project = new ProjectToBuild(projectName);
            project.IsTestProject = true;
            projects.Add(project);
            return this;
        }

        public BuildTasks AddWebProject(string projectName)
        {
            ProjectToBuild project = new ProjectToBuild(projectName);
            project.IsWebProject = true;
            projects.Add(project);
            return this;
        }

        public BuildTasks AssertFileExists(string fileDescription, string fileName)
        {
            if (false == File.Exists(fileName))
                Fail("{0} ('{1}') does not exist", fileDescription, fileName);

            return this;
        }

        public BuildTasks CleanOutput()
        {
            foreach (ProjectToBuild project in projects)
            {
                // first check that the project directory exists
                string projectPath = Path.Combine(productRootDir, project.ProjectName);
                if (false == Directory.Exists(projectPath))
                    Fail("Project directory '{0}' does not exist.", projectPath);

                string projectBinPath;
                if (false == project.IsWebProject)
                    projectBinPath = String.Format(CultureInfo.InvariantCulture,
                                                   @"{0}\bin\{1}", project.ProjectName, buildConfiguration);
                else
                    projectBinPath = String.Format(CultureInfo.InvariantCulture,
                                                   @"{0}\bin", project.ProjectName);

                projectBinPath = Path.Combine(productRootDir, projectBinPath);

                DeleteDirectory(projectBinPath);

                string projectObjPath = String.Format(CultureInfo.InvariantCulture,
                                                      @"{0}\obj\{1}", project.ProjectName, buildConfiguration);
                projectObjPath = Path.Combine(productRootDir, projectObjPath);
                DeleteDirectory(projectObjPath);
            }

            return this;
        }

        public BuildTasks CompileSolution()
        {
            AddProgramArgument(GetFullPath(productId, ".sln"));
            AddProgramArgument("/p:Configuration={0}", buildConfiguration);
            AddProgramArgument("/consoleloggerparameters:NoSummary");

            return RunProgram(@"C:\Windows\Microsoft.NET\Framework\v3.5\msbuild.exe");           
        }

        public BuildTasks DeleteDirectory(string directoryPath)
        {
            try
            {
                foreach (string fileToDelete in Directory.GetFiles(directoryPath, "*", SearchOption.AllDirectories))
                    File.Delete(fileToDelete);

                Directory.Delete(directoryPath, true);
            }
            catch (IOException)
            {
            }

            return this;
        }

        public void Dispose()
        {
            if (false == hasFailed)
                Log("BUILD SUCCESSFUL");
        }

        public BuildTasks GenerateCommonAssemblyInfo()
        {
            if (fileVersion == null)
                Fail("Assembly file version is not set.");

            using (Stream stream = File.Open(Path.Combine(productRootDir, "CommonAssemblyInfo.cs"), FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(@"using System.Reflection;

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
            fileVersion.ToString(2)
            );
                }
            }

            return this;
        }

        public void Fail (string format, params object[] arguments)
        {
            string message = String.Format(CultureInfo.InvariantCulture,
                format, arguments);

            Log(message);
            Log("BUILD FAILED");

            throw new BuildFailedException(message);
        }

        public void Finished()
        {
            hasFailed = false;
        }

        public BuildTasks FxCop()
        {
            string fxProjectPath = GetFullPath(productId, ".FxCop");

            AssertFileExists("FxCop project file", fxProjectPath);

            string fxReportPath = Path.Combine(productRootDir, buildDir);
            fxReportPath = Path.Combine(fxReportPath, productId);
            fxReportPath = String.Format(CultureInfo.InvariantCulture, "{0}.FxCopReport.xml", fxReportPath);

            AddProgramArgument(@"/project:{0}", fxProjectPath);
            AddProgramArgument(@"/out:{0}", fxReportPath);
            AddProgramArgument(@"/dictionary:CustomDictionary.xml");
            AddProgramArgument(@"/ignoregeneratedcode");
            RunProgram(GetFullPath(@".\lib\Microsoft FxCop 1.36\FxCopCmd.exe"));

            // check if the report file was generated
            if (File.Exists(fxReportPath))
            {
                if (false == isRunningUnderCruiseControl)
                {
                    // run FxCop GUI
                    AddProgramArgument(fxProjectPath);
                    RunProgram(GetFullPath(@".\lib\Microsoft FxCop 1.36\FxCop.exe"));
                }
                else
                    File.Copy(fxReportPath, cruiseControlDir);

                Fail("FxCop found violations in the code.");
            }

            return this;
        }

        public BuildTasks Log (string format, params object[] arguments)
        {
            logger.Log(format,  arguments);
            return this;
        }

        public BuildTasks ReadVersionInfo()
        {
            Log("Reading project version");

            using (Stream stream = File.Open(GetFullPath(productId, ".ProjectVersion.txt"), FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    string versionAsString = reader.ReadLine();
                    fileVersion = new Version(versionAsString);
                }
            }

            return this;
        }

        public BuildTasks RunProgram (string programExePath)
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
                processStartInfo.UseShellExecute = false;

                process.StartInfo = processStartInfo;
                process.Start();
                process.WaitForExit();

                Log("Exit code: {0}", process.ExitCode);

                if (process.ExitCode != 0)
                    Fail("Program '{0}' returned exit code {1}.", programExePath, process.ExitCode);
            }

            programArgs.Clear();

            return this;
        }

        public BuildTasks RunTests()
        {
            string unitTestResultsDir = GetFullPath(buildDir);
            unitTestResultsDir = Path.Combine(unitTestResultsDir, "UnitTestResults");

            int testRun = 0;
            foreach (ProjectToBuild project in projects)
            {
                if (false == project.IsTestProject)
                    continue;

                AddProgramArgument(GetFullPath(project.GetProjectAssemblyFileName(buildConfiguration)));
                AddProgramArgument("/report-directory:{0}", unitTestResultsDir);
                AddProgramArgument("/report-name-format:TestResults{0}-results", testRun);
                AddProgramArgument("/report-type:xml");
                AddProgramArgument("/verbosity:verbose");
                RunProgram(GetFullPath(@"lib\Gallio\bin\Gallio.Echo.exe"));

                testRun++;
            }

            return this;
        }

        public BuildTasks SetCompanyInfo (
            string companyName,
            string companyCopyright,
            string companyTrademark)
        {
            this.companyName = companyName;
            this.companyCopyright = companyCopyright;
            this.companyTrademark = companyTrademark;

            return this;
        }

        public BuildTasks SetProductName(string productName)
        {
            this.productName = productName;
            return this;
        }

        public BuildTasks SetProductRootDir (string productRootDir)
        {
            this.productRootDir = productRootDir;
            return this;
        }

        private string GetFullPath(string fileName)
        {
            return Path.Combine(productRootDir, fileName);
        }

        private string GetFullPath(string fileName, string extension)
        {
            return Path.Combine(productRootDir, String.Format(CultureInfo.InvariantCulture,
                "{0}{1}", fileName, extension));
        }

        private string buildConfiguration = "Release";
        private string buildDir = "Builds";
        private string companyCopyright;
        private string companyName;
        private string companyTrademark;
        private string cruiseControlDir;
        private Version fileVersion;
        private bool hasFailed;
        private bool isRunningUnderCruiseControl;
        private IBuildLogger logger = new BuildLogger();
        private readonly string productId;
        private string productName;
        private string productRootDir = "";
        private List<string> programArgs = new List<string>();
        private List<ProjectToBuild> projects = new List<ProjectToBuild>();
    }
}