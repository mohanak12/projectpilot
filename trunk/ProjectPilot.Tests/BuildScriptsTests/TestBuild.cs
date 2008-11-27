using System;
using MbUnit.Framework;
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
    }
}
