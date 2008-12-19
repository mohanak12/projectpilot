using System.Diagnostics.CodeAnalysis;
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
            : base(projectInfo.ProjectName)
        {
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
            projectMetrics.projectInfo = projectInfo;

            //For each source file in project
            foreach (VSProjectCompileItem compileItem in projectInfo.Project.CompileItems)
            {
                string filePath = Path.Combine(
                    projectMetrics.ProjectInfo.ProjectDirectoryPath, 
                    compileItem.Compile);
                SourceFileLocMetrics sourceFile = SourceFileLocMetrics.CalcLocStatData(filePath, map);

                // make sure the file was not ignored (it wasn't a source file)
                if (sourceFile != null)
                    projectMetrics.AddLocMetrics(sourceFile);
            }
            
            //TODO Jure: refactor!!!
            foreach (VSProjectContentItem contentItem in projectInfo.Project.ContentItems)
            {
                string filePath = Path.Combine(projectMetrics.ProjectInfo.ProjectDirectoryPath, contentItem.Content);
                SourceFileLocMetrics sourceFile = SourceFileLocMetrics.CalcLocStatData(filePath, map);

                if (sourceFile != null)
                    projectMetrics.AddLocMetrics(sourceFile);
            }

            return projectMetrics;
        }

        //[SuppressMessage("Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes", MessageId = "System.Xml.XmlNode")]
        //public void GenerateXmlReport(VSProjectInfo projectInfo, XmlWriter xmlWriter)
        //{
        //    this.WriteXml(xmlWriter);

        //    VSProjectLocMetrics projectMetrics = new VSProjectLocMetrics(projectInfo);

        //    foreach (VSProjectCompileItem compileItem in projectInfo.Project.CompileItems)
        //    {
        //        string filePath = Path.Combine(
        //            projectMetrics.ProjectInfo.ProjectDirectoryPath, 
        //            compileItem.Compile);
        //        SourceFileLocMetrics sourceFile = new SourceFileLocMetrics(filePath);

        //        sourceFile.WriteXml(xmlWriter);
        //    }
        //}

        private VSProjectInfo projectInfo;
    }
}
