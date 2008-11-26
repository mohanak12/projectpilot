using MbUnit.Framework;
using ProjectPilot.BuildScripts.VSSolutionBrowsing;

namespace ProjectPilot.Tests.BuildScriptsTests
{
    [TestFixture]
    public class TestBuild
    {
        [Test]
        public void LoadSolution()
        {
            VSSolution solution = VSSolution.Load(@"..\..\..\ProjectPilot.sln");

            solution.ForEachProject(
                project 
                    => 
                    System.Console.Out.WriteLine(project.ProjectFileName));
        }
    }
}
