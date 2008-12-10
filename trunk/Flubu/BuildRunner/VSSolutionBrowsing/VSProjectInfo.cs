using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Flubu.BuildRunner.VSSolutionBrowsing
{
    /// <summary>
    /// Holds basic information about a specific VisualStudio project.
    /// </summary>
    public class VSProjectInfo
    {
        public VSProjectInfo(VSSolution ownerSolution, Guid projectTypeGuid, string projectName, string projectFileName, Guid projectGuid)
        {
            this.ownerSolution = ownerSolution;
            this.projectTypeGuid = projectTypeGuid;
            this.projectName = projectName;
            this.projectFileName = projectFileName;
            this.projectGuid = projectGuid;
        }

        public VSSolution OwnerSolution
        {
            get { return ownerSolution; }
        }

        public Guid ProjectTypeGuid
        {
            get { return projectTypeGuid; }
        }

        public string ProjectName
        {
            get { return projectName; }
        }

        public string ProjectFileName
        {
            get { return projectFileName; }
        }

        public string ProjectDirectoryPath
        {
            get
            {
                return Path.GetDirectoryName (Path.Combine (ownerSolution.SolutionDirectoryPath, ProjectFileName));
            }
        }

        public Guid ProjectGuid
        {
            get { return projectGuid; }
        }

        public XmlDocument OpenProjectFileAsXmlDocument ()
        {
            //if (log.IsDebugEnabled)
            //    log.DebugFormat ("OpenProjectFileAsXmlDocument '{0}'", this.ProjectFileName);

            using (Stream stream = File.Open (Path.Combine (ownerSolution.SolutionDirectoryPath, ProjectFileName), FileMode.Open, FileAccess.Read))
            {
                XmlDocument xmlDoc = new XmlDocument ();
                xmlDoc.Load (stream);
                return xmlDoc;
            }
        }

        //public void SaveProjectFromXmlDocument (XmlDocument xmlDoc)
        //{
        //    if (xmlDoc == null)
        //        throw new ArgumentNullException ("xmlDoc");                
            
        //    //if (log.IsDebugEnabled)
        //    //    log.DebugFormat ("SaveProjectAsXmlDocument '{0}'", this.ProjectFileName);

        //    using (FileWithBackup file = new FileWithBackup (Path.Combine (ownerSolution.SolutionDirectoryPath, ProjectFileName)))
        //    {
        //        xmlDoc.Save (file.Stream);
        //        file.MarkAsValid ();
        //    }
        //}

        private readonly VSSolution ownerSolution;
        private readonly string projectFileName;
        private readonly Guid projectGuid;
        private readonly string projectName;
        private readonly Guid projectTypeGuid;

        public const string MSBuildNamespace = @"http://schemas.microsoft.com/developer/msbuild/2003";

        //static readonly private ILog log = LogManager.GetLogger (typeof (VSProjectInfo));
    }
}