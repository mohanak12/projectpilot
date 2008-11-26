using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;

namespace ProjectPilot.BuildScripts.VSSolutionBrowsing
{
    public class VSSolution
    {
        public ReadOnlyCollection<VSProject> Projects
        {
            get { return projects.AsReadOnly(); }
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

        public VSProject FindProjectById (Guid projectGuid)
        {
            foreach (VSProject projectData in projects)
                if (projectData.ProjectGuid == projectGuid)
                    return projectData;

            throw new ArgumentException ("Project not found.");
        }

        /// <summary>
        /// Performs the specified action on each project of the solution.
        /// </summary>
        /// <param name="action">The action delegate to perform on each project.</param>
        public void ForEachProject (Action<VSProject> action)
        {
            projects.ForEach(action);
        }

        //public void ForEachProjectFile (Action<VSProject> action)
        //{
        //    projects.ForEach (action);
        //}

        public static VSSolution Load (string fileName)
        {
            VSSolution solution = new VSSolution (fileName);
            ParserContext parserContext = new ParserContext ();

            Regex solutionVersionRegex = new Regex (@"^Microsoft Visual Studio Solution File, Format Version (?<version>.+)$");
            Regex projectRegex = new Regex (@"^Project\(""(?<projectTypeGuid>.*)""\) = ""(?<name>.*)"", ""(?<path>.*)"", ""(?<projectGuid>.*)""$");
            Regex endProjectRegex = new Regex (@"^EndProject$");
            Regex globalRegex = new Regex (@"^Global$");

            using (Stream stream = File.Open (fileName, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader reader = new StreamReader (stream))
                {
                    string line;

                    line = GetNextLineToParse (reader, parserContext);

                    if (line == null)
                        throw new NotSupportedException ();

                    Match solutionMatch = solutionVersionRegex.Match (line);

                    if (solutionMatch.Success == false)
                        throw new NotSupportedException ();

                    solution.solutionVersion = decimal.Parse (solutionMatch.Groups["version"].Value, 
                                                              System.Globalization.CultureInfo.InvariantCulture);

                    while (true)
                    {
                        line = GetNextLineToParse (reader, parserContext);

                        if (line == null)
                            break;

                        // exit the loop when 'Global' section appears
                        Match globalMatch = globalRegex.Match (line);
                        if (globalMatch.Success)
                            break;

                        Match projectMatch = projectRegex.Match (line);

                        if (projectMatch.Success == false)
                            throw new ArgumentException (String.Format (System.Globalization.CultureInfo.InvariantCulture,
                                                                        "Could not parse solution file (line {0}).", parserContext.LineCount));

                        VSProject project = new VSProject (solution);
                        project.ProjectGuid = new Guid (projectMatch.Groups["projectGuid"].Value);
                        project.ProjectName = projectMatch.Groups["name"].Value;
                        project.ProjectFileName = projectMatch.Groups["path"].Value;
                        project.ProjectTypeGuid = new Guid (projectMatch.Groups["projectTypeGuid"].Value);
                        solution.projects.Add (project);

                        // parse until the EndProject
                        while (true)
                        {
                            line = GetNextLineToParse (reader, parserContext);

                            if (line == null)
                                throw new ArgumentException ("Unexpected end of solution file.");

                            Match endProjectMatch = endProjectRegex.Match (line);

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

            } while (line.Trim ().Length == 0 || line.StartsWith ("#"));

            return line;
        }

        private string solutionFileName;
        private decimal solutionVersion;
        private List<VSProject> projects = new List<VSProject> ();

        //static readonly private ILog log = LogManager.GetLogger (typeof (VSSolution));

        static public readonly Guid ProjectTypeCSGuid = new Guid ("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}");
        static public readonly Guid ProjectTypeSolutionFolderGuid = new Guid ("{2150E333-8FDC-42A3-9474-1A3956D46DE8}");
    }
}