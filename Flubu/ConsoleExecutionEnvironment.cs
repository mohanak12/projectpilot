using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using log4net.Appender;
using log4net.Layout;

namespace Flubu
{
    public class ConsoleExecutionEnvironment : ScriptExecutionEnvironmentBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleExecutionEnvironment"/> class.
        /// </summary>
        /// <param name="scriptName">Name of the script.</param>
        /// <param name="logFileName">Name of the log file (including the path).</param>
        /// <param name="howManyOldLogsToKeep">The how many old log files the script should keep.</param>
        public ConsoleExecutionEnvironment (
            string scriptName, 
            string logFileName,
            int howManyOldLogsToKeep)
        {
            this.ScriptName = scriptName;

            RollingFileAppender appender = new RollingFileAppender ();
            PatternLayout layout = new PatternLayout ("%date %message%newline");
            layout.ActivateOptions ();
            appender.Layout = layout;
            appender.File = Path.Combine (
                Environment.CurrentDirectory, 
                logFileName);
            appender.AppendToFile = false;
            appender.MaxSizeRollBackups = howManyOldLogsToKeep;
            appender.RollingStyle = RollingFileAppender.RollingMode.Once;
            appender.Name = "ScriptingAppender";
            appender.ActivateOptions ();

            log4net.Config.BasicConfigurator.Configure (appender);
        }

        public override string GetConfigurationSettingValue (string settingName)
        {
            if (settingName == null)
                throw new ArgumentNullException ("settingName");                
            
            if (false == configurationSettings.ContainsKey (settingName))
            {
                System.Console.Out.Write ("Enter value for setting '{0}': ", settingName);
                System.Console.Out.Flush ();
                string val = System.Console.In.ReadLine ();
                configurationSettings.Add (settingName, val);
            }

            return configurationSettings[settingName];
        }

        public override void SetConfigurationSettingValue (string settingName, string settingValue)
        {
            if (settingName == null)
                throw new ArgumentNullException ("settingName");                
            
            configurationSettings[settingName] = settingValue;
        }

        public override string ReceiveInput (string prompt)
        {
            Console.Out.Write (prompt);
            return Console.In.ReadLine ();
        }

        private Dictionary<string, string> configurationSettings = new Dictionary<string, string> ();
    }
}
