﻿using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Xml;
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
            : base(projectInfo.ProjectFileNameFull)
        {
            this.projectInfo = projectInfo;
        }

        public VSProjectInfo ProjectInfo
        {
            get { return projectInfo; }
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

            //For each Item file in project
            foreach (VSProjectItem item in projectInfo.Project.Items)
            {
                if (item.ItemType == VSProjectItem.CompileItem ||
                    item.ItemType == VSProjectItem.Content)
                {
                    string filePath = Path.Combine(
                        projectMetrics.ProjectInfo.ProjectDirectoryPath,
                        item.Item);
                    SourceFileLocMetrics sourceFile = SourceFileLocMetrics.CalcLocStatData(filePath, map);

                    // make sure the file was not ignored (it wasn't a source file)
                    if (sourceFile != null)
                        projectMetrics.AddLocMetrics(sourceFile);
                }
            }
            
            return projectMetrics;
        }

        private VSProjectInfo projectInfo;
    }
}
