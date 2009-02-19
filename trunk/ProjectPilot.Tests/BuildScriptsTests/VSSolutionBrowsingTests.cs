using System;
using System.IO;
using Flubu;
using Flubu.Builds;
using Flubu.Builds.VSSolutionBrowsing;
using MbUnit.Framework;

namespace ProjectPilot.Tests.BuildScriptsTests
{
    [TestFixture]
    public class VSSolutionBrowsingTests
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

        /// <summary>
        /// Tests loading of VS solution and projects.
        /// </summary>
        [Test]
        public void LoadProjects ()
        {
            VSSolution solution = VSSolution.Load(@"..\..\..\ProjectPilot.sln"); 

            solution.LoadProjects();

            int vsProjectObjectsFound = 0;
            solution.ForEachProject(
                delegate(VSProjectInfo projectInfo)
                    {
                        if (projectInfo.Project != null)
                            vsProjectObjectsFound++;
                    });

            Assert.AreEqual(13, vsProjectObjectsFound);
        }

        /// <summary>
        /// Tests the <see cref="BuildRunner{TRunner}.GetProjectOutputPath"/> method.
        /// </summary>
        [Test]
        public void ProjectOutputPaths()
        {
            using (ConcreteBuildRunner runner = new ConcreteBuildRunner ("Test"))
            {
                runner.LoadSolution(@"..\..\..\ProjectPilot.sln");
                string outputPath = runner.GetProjectOutputPath("ProjectPilot.Framework");
                Assert.AreEqual(@"bin\Release\", outputPath);

                outputPath = runner.GetProjectOutputPath("ProjectPilot.Portal");
                Assert.AreEqual (@"bin\", outputPath);
            }
        }
    }
}
