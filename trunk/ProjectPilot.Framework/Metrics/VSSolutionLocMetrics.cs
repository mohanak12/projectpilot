using System.Xml;
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
        public LocStatsMap LocStatsMap 
        { 
            get { return locStatsMap; } 
        }

        /// <summary>
        /// Calculates the loc for the whole solution.
        /// </summary>
        /// <param name="solutionFileName">Name of the solution file.</param>
        public void CalculateLocForSolution(string solutionFileName)
        {
            //VSSolutionLocMetrics metrics = new VSSolutionLocMetrics();

            //Load the solution, appropriate projects and their compile items.
            VSSolution solution = VSSolution.Load(solutionFileName);
            solution.LoadProjects();

            foreach (VSProjectInfo projectInfo in solution.Projects)
            {
                //just C# projects
                if (projectInfo.ProjectTypeGuid != VSProjectType.CSharpProjectType.ProjectTypeGuid)
                    continue;

                //Calculate the metrics for each containing project.
                VSProjectLocMetrics projectMetrics = VSProjectLocMetrics.CalculateLocForProject(projectInfo, locStatsMap);
                this.AddLocMetrics(projectMetrics);
            }

            //return metrics;
        }

        public void GenerateXmlReport(string solutionFileName, string filePath)
        {
            XmlTextWriter xmlWriter = new XmlTextWriter(filePath, System.Text.Encoding.UTF8);
            xmlWriter.Formatting = Formatting.Indented;
            xmlWriter.WriteProcessingInstruction("xml", "version='1.0' encoding='UTF-8'");
            xmlWriter.WriteStartElement("Root");
            xmlWriter.Close();

            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.Load(filePath);

            XmlNode sln = xmlDoc.DocumentElement, csproj;

            //Load the solution, appropriate projects and their compile items.
            VSSolution solution = VSSolution.Load(solutionFileName);
            solution.LoadProjects();

            csproj = this.InsertItem(sln, xmlDoc);
            foreach (VSProjectInfo projectInfo in solution.Projects)
            {
                //just C# projects
                if (projectInfo.ProjectTypeGuid != VSProjectType.CSharpProjectType.ProjectTypeGuid)
                    continue;

                VSProjectLocMetrics projectMetrics = new VSProjectLocMetrics(projectInfo);
                projectMetrics.GenerateXmlReport(projectInfo, csproj, xmlDoc);
            }

            xmlDoc.Save(filePath);
        }

        private LocStatsMap locStatsMap = new LocStatsMap();
    }
}
