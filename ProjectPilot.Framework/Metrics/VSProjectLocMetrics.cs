using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Flubu.Builds.VSSolutionBrowsing;

namespace ProjectPilot.Framework.Metrics
{
    public class VSProjectLocMetrics : GroupLocMetricsBase
    {
        public VSProjectLocMetrics(VSProjectInfo projectInfo)
        {
            this.projectPath = projectInfo.ProjectDirectoryPath + @"\";
        }

        public string ProjectPath
        {
            get { return projectPath; }
        }

        public static VSProjectLocMetrics CalculateLocForProject(VSProjectInfo projectInfo)
        {
            VSProjectLocMetrics projectMetrics = new VSProjectLocMetrics(projectInfo);

            foreach (VSProjectCompileItem compileItem in projectInfo.Project.CompileItems)
            {
                //SourceFileLocMetrics sourceFile = new SourceFileLocMetrics(compileItem.Compile.Substring(compileItem.Compile.LastIndexOf(@"\")));
                SourceFileLocMetrics sourceFile = new SourceFileLocMetrics("bla bla");

                string filePath = projectMetrics.ProjectPath + compileItem.Compile; 

                Stream streamOfFile = File.OpenRead(filePath);
                
                sourceFile.CalcLocStatData(streamOfFile);
                projectMetrics.AddLocMetrics(sourceFile);
            }

            return projectMetrics;
        }

        private string projectPath;
    }
}
