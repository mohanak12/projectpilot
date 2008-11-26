using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using ProjectPilot.BuildScripts.SolutionBrowsing.MsBuildSchema;
using ProjectPilot.BuildScripts.VSSolutionBrowsing;

namespace ProjectPilot.BuildScripts.VSSolutionBrowsing
{
    public class VSProject
    {
        public Guid ProjectTypeGuid
        {
            get { return projectTypeGuid; }
            set { projectTypeGuid = value; }
        }

        public string ProjectName
        {
            get { return projectName; }
            set { projectName = value; }
        }

        public string ProjectFileName
        {
            get { return projectFileName; }
            set { projectFileName = value; }
        }

        public string ProjectDirectoryPath
        {
            get
            {
                return Path.GetDirectoryName (Path.Combine (solution.SolutionDirectoryPath, ProjectFileName));
            }
        }

        public Guid ProjectGuid
        {
            get { return projectGuid; }
            set { projectGuid = value; }
        }

        public VSProject (VSSolution solution)
        {
            this.solution = solution;
        }

        public Project OpenProject()
        {
            //if (log.IsDebugEnabled)
            //    log.DebugFormat ("OpenProject '{0}'", this.ProjectFileName);

            if (ProjectTypeGuid != VSSolution.ProjectTypeCSGuid)
                throw new ArgumentException ("Only C# project are currently supported.");

            using (Stream stream = File.Open (Path.Combine (solution.SolutionDirectoryPath, ProjectFileName), FileMode.Open, FileAccess.Read))
            {
                XmlSerializer serializer = new XmlSerializer (typeof (Project), "http://schemas.microsoft.com/developer/msbuild/2003");

                // this removes any namespaces declarations in the object's element tag
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces ();
                ns.Add (String.Empty, "http://schemas.microsoft.com/developer/msbuild/2003");

                Project project = (Project) serializer.Deserialize (stream);

                return project;
            }
        }

        public XmlDocument OpenProjectAsXmlDocument ()
        {
            //if (log.IsDebugEnabled)
            //    log.DebugFormat ("OpenProjectAsXmlDocument '{0}'", this.ProjectFileName);

            using (Stream stream = File.Open (Path.Combine (solution.SolutionDirectoryPath, ProjectFileName), FileMode.Open, FileAccess.Read))
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

        //    using (FileWithBackup file = new FileWithBackup (Path.Combine (solution.SolutionDirectoryPath, ProjectFileName)))
        //    {
        //        xmlDoc.Save (file.Stream);
        //        file.MarkAsValid ();
        //    }
        //}

        private VSSolution solution;
        private Guid projectTypeGuid;
        private string projectName;
        private string projectFileName;
        private Guid projectGuid;

        public const string MSBuildNamespace = @"http://schemas.microsoft.com/developer/msbuild/2003";

        //static readonly private ILog log = LogManager.GetLogger (typeof (VSProject));
    }
}