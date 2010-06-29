using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;

namespace Flubu.Builds.VSSolutionBrowsing
{
    /// <summary>
    /// Holds information about a VisualStudio project.
    /// </summary>
    public class VSProjectWithFileInfo : VSProjectInfo
    {
        public VSProjectWithFileInfo(
            VSSolution ownerSolution, 
            Guid projectGuid, 
            string projectName, 
            string projectFileName, 
            Guid projectTypeGuid) : base (ownerSolution, projectGuid, projectName, projectTypeGuid)
        {
            this.projectFileName = projectFileName;
        }

        /// <summary>
        /// Gets or sets the <see cref="VSProject"/> object holding the detailed information about this VisualStudio
        /// project.
        /// </summary>
        /// <value>The <see cref="VSProject"/> object .</value>
        public VSProject Project { get; set; }

        /// <summary>
        /// Gets the path to the directory where the project file is located.
        /// </summary>
        /// <value>The project directory path.</value>
        public string ProjectDirectoryPath
        {
            get
            {
                return Path.GetDirectoryName (Path.Combine (OwnerSolution.SolutionDirectoryPath, ProjectFileName));
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
                    Path.Combine(OwnerSolution.SolutionDirectoryPath, ProjectFileName));
            }
        }

        public IXPathNavigable OpenProjectFileAsXmlDocument ()
        {
            //if (log.IsDebugEnabled)
            //    log.DebugFormat ("OpenProjectFileAsXmlDocument '{0}'", this.ProjectFileName);

            using (Stream stream = File.Open (Path.Combine (OwnerSolution.SolutionDirectoryPath, ProjectFileName), FileMode.Open, FileAccess.Read))
            {
                XmlDocument xmlDoc = new XmlDocument ();
                xmlDoc.Load (stream);
                return xmlDoc;
            }
        }

        public override void Parse (VSSolutionFileParser parser)
        {
            while (true)
            {
                string line = parser.NextLine();

                if (line == null)
                    parser.ThrowParserException ("Unexpected end of solution file.");

                Match endProjectMatch = VSSolution.RegexEndProject.Match(line);

                if (endProjectMatch.Success)
                    break;
            }
        }

        private readonly string projectFileName;
        public const string MSBuildNamespace = @"http://schemas.microsoft.com/developer/msbuild/2003";
    }
}