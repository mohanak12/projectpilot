using System;
using System.Globalization;
using Headless.Configuration;
using log4net.Core;

namespace Headless
{
    public class LogEvent
    {
        public LogEvent(string sourceId, Level level, string format, params object[] args)
        {
            this.sourceId = sourceId;
            this.level = level;
            this.message = string.Format(CultureInfo.InvariantCulture, format, args);
        }

        public Level Level
        {
            get { return level; }
        }

        public string Message
        {
            get { return message; }
        }

        public string SourceId
        {
            get { return sourceId; }
        }

        public static LogEvent ForBuildStage(BuildStage buildStage, Level level, string format, params object[] args)
        {
            return new LogEvent(
                string.Format(
                    CultureInfo.InvariantCulture, 
                    "Project.{0}.Stage.{1}", 
                    buildStage.Project.ProjectId,
                    buildStage.StageId),
                level,
                format,
                args);
        }

        public static LogEvent ForProject(string projectId, Level level, string format, params object[] args)
        {
            return new LogEvent(
                string.Format(CultureInfo.InvariantCulture, "Project.{0}", projectId),
                level, 
                format,
                args);
        }

        private Level level;
        private string message;
        private string sourceId;
    }
}