using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flubu.Builds.VSSolutionBrowsing;

namespace ProjectPilot.Framework.Metrics
{
    /// <summary>
    /// Implementation of the GroupLocMetricsBase abstarct class.
    /// Represents a Visual studio solution with a list of
    /// VS projects.
    /// </summary>
    public class VSSolutionLocMetrics : GroupLocMetricsBase
    {
        /// <summary>
        /// Calculates the loc for the whole solution.
        /// </summary>
        /// <param name="solutionFileName">Name of the solution file.</param>
        /// <returns>Returnes the VSSolutionLocMetrics instance.</returns>
        public static VSSolutionLocMetrics CalculateLocForSolution(string solutionFileName)
        {
            VSSolutionLocMetrics metrics = new VSSolutionLocMetrics();

            //Load the solution, appropriate projects and their compile items.
            VSSolution solution = VSSolution.Load(solutionFileName);
            solution.LoadProjects();

            foreach (VSProjectInfo projectInfo in solution.Projects)
            {
                //just C# projects
                if (projectInfo.ProjectTypeGuid != VSProjectType.CSharpProjectType.ProjectTypeGuid)
                    continue;

                //Calculate the metrics for each containing project.
                VSProjectLocMetrics projectMetrics = VSProjectLocMetrics.CalculateLocForProject(projectInfo);
                metrics.AddLocMetrics(projectMetrics);
            }

            return metrics;
        }
    }
}
