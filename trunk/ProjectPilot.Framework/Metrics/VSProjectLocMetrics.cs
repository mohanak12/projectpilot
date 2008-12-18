using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Flubu.Builds.VSSolutionBrowsing;

namespace ProjectPilot.Framework.Metrics
{
    /// <summary>
    /// Implementation of the GroupLocMetricsBase abstarct class.
    /// Represents a Visual studio project with a list of source
    /// files and their loc metrics.
    /// </summary>
    public class VSProjectLocMetrics : GroupLocMetricsBase
    {
        public VSProjectLocMetrics(VSProjectInfo projectInfo)
        {
            this.projectPath = projectInfo.ProjectDirectoryPath + @"\";
            this.projectName = projectInfo.ProjectName;
        }

        /// <summary>
        /// Gets the project full path.
        /// </summary>
        /// <value>The project full path.</value>
        public string ProjectPath
        {
            get { return projectPath; }
        }

        /// <summary>
        /// Gets the name of the project.
        /// </summary>
        /// <value>The name of the project.</value>
        public string ProjectName
        {
            get { return projectName; }
        }

        /// <summary>
        /// Calculates the loc metrics for the whole project.
        /// </summary>
        /// <param name="projectInfo">The project info.</param>
        /// <param name="map">The map of <see cref="ILocStats"/> objects which can calculate LoC metrics for different source file types.</param>
        /// <returns>
        /// Returns the VSProjectLocMetrics instance.
        /// </returns>
        public static VSProjectLocMetrics CalculateLocForProject(VSProjectInfo projectInfo, LocStatsMap map)
        {
            VSProjectLocMetrics projectMetrics = new VSProjectLocMetrics(projectInfo);

            //For each source file in project
            foreach (VSProjectCompileItem compileItem in projectInfo.Project.CompileItems)
            {
                string filePath = Path.Combine(projectMetrics.ProjectPath, compileItem.Compile);
                SourceFileLocMetrics sourceFile = new SourceFileLocMetrics(filePath);

                sourceFile.CalcLocStatData(filePath, map);

                projectMetrics.AddLocMetrics(sourceFile);
            }

            return projectMetrics;
        }

        private string projectPath;
        private string projectName;
    }
}
