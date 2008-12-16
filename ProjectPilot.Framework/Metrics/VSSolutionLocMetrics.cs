using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flubu.Builds.VSSolutionBrowsing;

namespace ProjectPilot.Framework.Metrics
{
    public class VSSolutionLocMetrics : GroupLocMetricsBase
    {
        public static VSSolutionLocMetrics CalculateLocForSolution(string solutionFileName)
        {
            VSSolutionLocMetrics metrics = new VSSolutionLocMetrics();

            VSSolution solution = VSSolution.Load(solutionFileName);
            solution.LoadProjects();

            foreach (VSProjectInfo projectInfo in solution.Projects)
            {
                if (projectInfo.ProjectTypeGuid != VSProjectType.CSharpProjectType.ProjectTypeGuid)
                    continue;

                VSProjectLocMetrics projectMetrics = VSProjectLocMetrics.CalculateLocForProject(projectInfo);
                metrics.AddLocMetrics(projectMetrics);
            }

            return metrics;
        }
    }
}
