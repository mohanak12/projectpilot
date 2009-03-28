using System;
using System.IO;
using System.Xml;
using System.Xml.XPath;

namespace Flubu.Builds.VSSolutionBrowsing
{
    /// <summary>
    /// Holds information about a VisualStudio project.
    /// </summary>
    public class VSProjectInfo
    {
        public VSProjectInfo(
            VSSolution ownerSolution, 
            Guid projectGuid, 
            string projectName, 
            string projectFileName, 
            Guid projectTypeGuid)
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

        /// <summary>
        /// Gets or sets the <see cref="VSProject"/> object holding the detailed information about this VisualStudio
        /// project.
        /// </summary>
        /// <value>The <see cref="VSProject"/> object .</value>
        public VSProject Project
        {
            get { return project; }
            set { project = value; }
        }

        /// <summary>
        /// Gets the path to the directory where the project file is located.
        /// </summary>
        /// <value>The project directory path.</value>
        public string ProjectDirectoryPath
        {
            get
            {
                return Path.GetDirectoryName (Path.Combine (ownerSolution.SolutionDirectoryPath, ProjectFileName));
            }
        }

        /// <summary>
        /// Gets the name of the project file. The file name is relative to the solution's directory.
        /// </summary>
        /// <remarks>The full path to the project file can be retrieved using the <see cref="ProjectFileNameFull"/>
        /// property.</remarks>
        /// <value>The name of the project file.</value>
        public string ProjectFileName
        {
            get { return projectFileName; }
        }

        /// <summary>
        /// Gets the full path to the project file.
        /// </summary>
        /// <value>The full path to the project file.</value>
        public string ProjectFileNameFull
        {
            get
            {
                return Path.GetFullPath(
                    Path.Combine(ownerSolution.SolutionDirectoryPath, ProjectFileName));
            }
        }

        public Guid ProjectGuid
        {
            get { return projectGuid; }
        }

        public string ProjectName
        {
            get { return projectName; }
        }

        public Guid ProjectTypeGuid
        {
            get { return projectTypeGuid; }
        }

        public IXPathNavigable OpenProjectFileAsXmlDocument ()
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

        private readonly VSSolution ownerSolution;
        private VSProject project;
        private readonly string projectFileName;
        private readonly Guid projectGuid;
        private readonly string projectName;
        private readonly Guid projectTypeGuid;

        public const string MSBuildNamespace = @"http://schemas.microsoft.com/developer/msbuild/2003";
    }
}