using System;
using System.Collections.Generic;
using System.IO;
using log4net;
using log4net.Appender;
using log4net.Layout;

namespace Flubu
{
    public class ConsoleExecutionEnvironment : ScriptExecutionEnvironmentBase
    {
        public ConsoleExecutionEnvironment (string scriptName)
            : this (scriptName, true)
        {
        }

        public ConsoleExecutionEnvironment (string scriptName, bool configureLog4Net)
        {
            this.ScriptName = scriptName;
            this.configureLog4Net = configureLog4Net;

            if (this.configureLog4Net)
            {
                RollingFileAppender appender = new RollingFileAppender ();
                PatternLayout layout = new PatternLayout ("%date [%3thread] %-5level %-50logger{3} - %message%newline");
                layout.ActivateOptions ();
                appender.Layout = layout;
                string logFileName = String.Format (
                    System.Globalization.CultureInfo.InvariantCulture,
                    "{0}.log", 
                    this.ScriptName);
                appender.File = Path.Combine (
                    Environment.CurrentDirectory, 
                    logFileName);
                appender.AppendToFile = false;
                appender.MaxSizeRollBackups = 20;
                appender.RollingStyle = RollingFileAppender.RollingMode.Once;
                appender.Name = "ScriptingAppender";
                appender.ActivateOptions ();

                log4net.Config.BasicConfigurator.Configure (appender);
            }
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

        //public override void Log (string message)
        //{
        //    System.Console.Out.WriteLine ("{0}[{1:T}] {2}", Indent(), DateTime.Now, message);
        //    log.Info (message);
        //}

        //public override void Log (string format, params object[] parameters)
        //{
        //    if (format == null)
        //        throw new ArgumentNullException ("format");                
            
        //    string message = String.Format (
        //        System.Globalization.CultureInfo.InvariantCulture,
        //        format, 
        //        parameters);
        //    log (message);
        //}

        //public override void ReportTaskStarted (ITask task)
        //{
        //    if (task == null)
        //        throw new ArgumentNullException ("task");

        //    System.Console.Out.WriteLine ("{0}[{1:T}] TASK: {2}", Indent (), DateTime.Now, task.TaskDescription);
        //    nestedTasksCounter++;
        //    log.InfoFormat ("Task started: {0}", task.TaskDescription);
        //}

        //public override void ReportTaskExecuted (ITask task)
        //{
        //    if (task == null)
        //        throw new ArgumentNullException ("task");

        //    nestedTasksCounter--;
        //    System.Console.Out.WriteLine ("{0}[{1:T}] DONE", Indent (), DateTime.Now);
        //    log.InfoFormat ("Task done: {0}", task.TaskDescription);
        //}

        //public override void ReportTaskFailed (ITask task, Exception ex)
        //{
        //    if (task == null)
        //        throw new ArgumentNullException ("task");                
            
        //    if (ex == null)
        //        throw new ArgumentNullException ("ex");                
            
        //    nestedTasksCounter--;
        //    System.Console.Out.WriteLine ("{0}[{1:T}] FAILED", Indent (), DateTime.Now);
        //    System.Console.Out.WriteLine ("{0}{1}", Indent(), ex.ToString ());
        //    log.FatalFormat ("Task failed: task='{0}', exception='{1}'", task.TaskDescription, ex);
        //}

        //public override void ReportTaskFailed (ITask task, string reason)
        //{
        //    if (task == null)
        //        throw new ArgumentNullException ("task");

        //    nestedTasksCounter--;
        //    System.Console.Out.WriteLine ("{0}[{1:T}] FAILED", Indent (), DateTime.Now);
        //    System.Console.Out.WriteLine ("{0}{1}", Indent (), reason);
        //    log.FatalFormat ("Task failed: task='{0}', reason='{1}'", task.TaskDescription, reason);
        //}

        public override string ReceiveInput (string prompt)
        {
            Console.Out.Write (prompt);
            return Console.In.ReadLine ();
        }

        //private string Indent ()
        //{
        //    return new string ('\t', nestedTasksCounter);
        //}

        //private int nestedTasksCounter = 0;
        private Dictionary<string, string> configurationSettings = new Dictionary<string, string> ();
        private bool configureLog4Net;

        //private static readonly ILog log = LogManager.GetLogger("Main");
    }
}
