using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;

namespace Flubu.Builds.VSSolutionBrowsing
{
    /// <summary>
    /// Represents a VisualStudio solution.
    /// </summary>
    public class VSSolution
    {
        /// <summary>
        /// Gets a read-only collection of all the projects in the solution.
        /// </summary>
        /// <value>A read-only collection of all the projects in the solution.</value>
        public ReadOnlyCollection<VSProjectInfo> Projects
        {
            get { return projects.AsReadOnly(); }
        }

        /// <summary>
        /// Gets or sets the VisualStudio project types dictionary.
        /// </summary>
        /// <value>The VisualStudio project types dictionary.</value>
        public VSProjectTypesDictionary ProjectTypesDictionary
        {
            get { return projectTypesDictionary; }
            set { projectTypesDictionary = value; }
        }

        public string SolutionDirectoryPath
        {
            get
            {
                return Path.GetDirectoryName (solutionFileName);
            }
        }

        public string SolutionFileName
        {
            get { return solutionFileName; }
        }

        public decimal SolutionVersion
        {
            get { return solutionVersion; }
        }

        public VSProjectInfo FindProjectById (Guid projectGuid)
        {
            foreach (VSProjectInfo projectData in projects)
                if (projectData.ProjectGuid == projectGuid)
                    return projectData;

            throw new ArgumentException ("Project not found.");
        }

        public VSProjectInfo FindProjectByName(string projectName)
        {
            foreach (VSProjectInfo projectData in projects)
                if (projectData.ProjectName == projectName)
                    return projectData;

            throw new ArgumentException("Project not found.");
        }

        /// <summary>
        /// Performs the specified action on each project of the solution.
        /// </summary>
        /// <param name="action">The action delegate to perform on each project.</param>
        public void ForEachProject (Action<VSProjectInfo> action)
        {
            projects.ForEach(action);
        }

        public static VSSolution Load (string fileName)
        {
            VSSolution solution = new VSSolution (fileName);
            ParserContext parserContext = new ParserContext ();

            using (Stream stream = File.Open (fileName, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader reader = new StreamReader (stream))
                {
                    string line;

                    line = GetNextLineToParse (reader, parserContext);

                    if (line == null)
                        throw new NotSupportedException ();

                    Match solutionMatch = VSSolution.regexSolutionVersion.Match (line);

                    if (solutionMatch.Success == false)
                        throw new NotSupportedException ();

                    solution.solutionVersion = decimal.Parse (
                        solutionMatch.Groups["version"].Value, 
                        System.Globalization.CultureInfo.InvariantCulture);

                    while (true)
                    {
                        line = GetNextLineToParse (reader, parserContext);

                        if (line == null)
                            break;

                        // exit the loop when 'Global' section appears
                        Match globalMatch = VSSolution.regexGlobal.Match (line);
                        if (globalMatch.Success)
                            break;

                        Match projectMatch = VSSolution.regexProject.Match (line);

                        if (projectMatch.Success == false)
                            throw new ArgumentException (
                                String.Format (
                                    System.Globalization.CultureInfo.InvariantCulture,
                                    "Could not parse solution file (line {0}).", 
                                    parserContext.LineCount));

                        Guid projectGuid = new Guid (projectMatch.Groups["projectGuid"].Value);
                        string projectName = projectMatch.Groups["name"].Value;
                        string projectFileName = projectMatch.Groups["path"].Value;
                        Guid projectTypeGuid = new Guid (projectMatch.Groups["projectTypeGuid"].Value);
                        VSProjectInfo projectInfo = new VSProjectInfo(
                            solution,
                            projectTypeGuid,
                            projectName,
                            projectFileName,
                            projectTypeGuid);
                        solution.projects.Add(projectInfo);

                        // parse until the EndProject
                        while (true)
                        {
                            line = GetNextLineToParse (reader, parserContext);

                            if (line == null)
                                throw new ArgumentException ("Unexpected end of solution file.");

                            Match endProjectMatch = VSSolution.regexEndProject.Match (line);

                            if (endProjectMatch.Success)
                                break;
                        }
                    }
                }
            }

            return solution;
        }

        protected VSSolution (string fileName)
        {
            this.solutionFileName = fileName;
        }

        private static string GetNextLineToParse (StreamReader reader, ParserContext parserContext)
        {
            string line = null;

            do
            {
                if (reader.EndOfStream)
                    break;

                line = reader.ReadLine ();
                parserContext.IncrementLineCount ();

                //if (log.IsDebugEnabled)
                //    log.DebugFormat ("Read line ({0}): {1}", parserContext.LineCount, line);
            } 
            while (line.Trim ().Length == 0 || line.StartsWith ("#"));

            return line;
        }

        private List<VSProjectInfo> projects = new List<VSProjectInfo> ();
        private VSProjectTypesDictionary projectTypesDictionary = new VSProjectTypesDictionary();
        private static readonly Regex regexEndProject = new Regex(@"^EndProject$");
        private static readonly Regex regexGlobal = new Regex(@"^Global$");
        private static readonly Regex regexProject = new Regex(@"^Project\(""(?<projectTypeGuid>.*)""\) = ""(?<name>.*)"", ""(?<path>.*)"", ""(?<projectGuid>.*)""$");
        private static readonly Regex regexSolutionVersion = new Regex(@"^Microsoft Visual Studio Solution File, Format Version (?<version>.+)$");
        private string solutionFileName;
        private decimal solutionVersion;

        //static readonly private ILog log = LogManager.GetLogger (typeof (VSSolution));
    }
}