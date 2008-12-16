﻿using System.IO;
using Flubu.Builds.VSSolutionBrowsing;
using MbUnit.Framework;
using ProjectPilot.Framework.Metrics;

namespace ProjectPilot.Tests.Framework.Metrics
{
    [TestFixture]
    public class LocStatsTest
    {
        [Test]
        public void TestLocOnSampleFile()
        {
            ILocStats locStats = new LocStats();

            Stream stream = File.OpenRead(@"..\..\..\Data\Samples\LocSample.cs");
            
            LocStatsData data = locStats.CountLocString(stream);

            Assert.AreEqual(14, data.Cloc);
            Assert.AreEqual(9, data.Eloc);
            Assert.AreEqual(56, data.Sloc);
        }

        [Test]
        public void SingleFileLocMetrics()
        {
            //LocMetricsBase sourceFile = new SourceFileLocMetrics(@"..\..\..\Data\Samples\", "LocSample.cs");
            //LocStatsData data = sourceFile.GetLocStatsData();

            //Assert.AreEqual(14, data.Cloc);
            //Assert.AreEqual(9, data.Eloc);
            //Assert.AreEqual(56, data.Sloc);
        }

        [Test]
        public void SolutionLocMetrics()
        {
            int a = 5;

            VSSolutionLocMetrics metrics = VSSolutionLocMetrics.CalculateLocForSolution(
                @"..\..\..\ProjectPilot.sln");

            a++;

            LocStatsData data = metrics.GetLocStatsData();
        }

        //[Test]
        //public void SolutionLocMetrics()
        //{
            //VSSolutionLocMetrics metrics = new VSSolutionLocMetrics.CalculateLocForSolution(
            //    @"..\..\..\ProjectPilot.sln");

            //LocStatsData data = metrics.GetLocStatsData();

            //foreach (VSProjectInfo projectInfo in solution.Projects)
            //{
            //    VSProjectLocMetrics projectMetrics = new VSProjectLocMetrics(projectInfo);
            //    if (projectInfo.ProjectTypeGuid == VSProjectType.CSharpProjectType.ProjectTypeGuid)
            //        projectMetrics.CalculateLocForProject(projectInfo.Project);
            //}
        //}
    }
}
