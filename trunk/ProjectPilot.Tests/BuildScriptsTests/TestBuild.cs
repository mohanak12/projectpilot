using System;
using System.IO;
using Flubu;
using MbUnit.Framework;
using ProjectPilot.BuildScripts;
using ProjectPilot.BuildScripts.SolutionBrowsing.MsBuildSchema;
using ProjectPilot.BuildScripts.VSSolutionBrowsing;

namespace ProjectPilot.Tests.BuildScriptsTests
{
    [TestFixture]
    public class TestBuild
    {
        /// <summary>
        /// Tests loading of the ProjectPilot solution file and all of its project files.
        /// </summary>
        [Test]
        public void LoadSolution()
        {
            VSSolution solution = VSSolution.Load(@"..\..\..\ProjectPilot.sln");

            solution.ForEachProject(delegate (VSProjectInfo projectInfo)
            {
                VSProjectType projectType = solution.ProjectTypesDictionary.FindProjectType(projectInfo.ProjectTypeGuid);
                System.Console.Out.WriteLine("{0} ({1})", projectInfo.ProjectFileName, projectType.ProjectTypeName);

                if (projectType == VSProjectType.CSharpProjectType)
                {
                    Project projectFile = projectInfo.LoadProjectFile();
                }
            });
        }

        [Test,Explicit]
        public void TestBuildScript()
        {
            using (BuildRunner script = new BuildRunner("ProjectPilot"))
            {
                script.ScriptExecutionEnvironment.Logger = new NAntLikeFlubuLogger(System.Console.Out);
                script
                    .SetProductRootDir(@"..\..\..")
                    .SetCompanyInfo("HERMES SoftLab d.d.", "Copyright (C) 2008 HERMES SoftLab d.d.", String.Empty)
                    .ReadVersionInfo();

                script
                    .LoadSolution("ProjectPilot.sln");
                script
                    .RegisterAsWebProject("ProjectPilot.Portal", "http://localhost/ProjectPortal");

                script
                    .CleanOutput();
                script
                    .GenerateCommonAssemblyInfo();
                script
                    .CompileSolution();
                script
                    .FxCop();
                script
                    .RunTests("ProjectPilot.Tests");
                script
                    .Complete();
            }
        }
    }
}
