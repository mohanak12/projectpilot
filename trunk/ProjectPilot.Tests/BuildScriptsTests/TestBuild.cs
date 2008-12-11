using System;
using System.IO;
using Flubu;
using Flubu.Builds;
using Flubu.Builds.VSSolutionBrowsing;
using MbUnit.Framework;

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
            });
        }

        [Test,Explicit]
        public void TestBuildScript()
        {
            using (ConcreteBuildRunner script = new ConcreteBuildRunner("ProjectPilot"))
            {
                script.ScriptExecutionEnvironment.Logger = new NAntLikeFlubuLogger();
                script
                    .SetProductRootDir(@"..\..\..")
                    .SetCompanyInfo("HERMES SoftLab d.d.", "Copyright (C) 2008 HERMES SoftLab d.d.", String.Empty);
                    //.ReadVersionInfo();

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
                    .RunTests("ProjectPilot.Tests", false);
                script
                    .Complete();
            }
        }
    }
}
