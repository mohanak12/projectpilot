using System;
using Flubu;

namespace Flubu.Builds
{
    /// <summary>
    /// Custom build listener for Flubu. This listener writes the X last messages to a file
    /// for viewing the build progress in CCNET.
    /// The extra listenfile is XML-based, consisting in the hierarchy of the NAnt project
    /// and the last X messages under the last target of the NAnt project.
    /// </summary>
    /// <remarks>The code is adopted from the custom build listener for NAnt (http://confluence.public.thoughtworks.org/display/CCNETCOMM/Viewing+build+progress+with+Nant+and+MSBuild).
    /// </remarks>
    public class FlubuCCNetListener : IFlubuLogger
    {
        public void BuildFinished (object sender, BuildEventArgs e)
        {
            // log the error in the queue, so it displays in the dashboard 
            // this way the error is available in the extra listen file, so it can be 
            // shown to CCTray, Dashboard if wanted
            if (e.Exception != null)
            {
                string Data = string.Format ("<Item Time=\"{0}\" Data=\"Finished Build {1}\"  />", GetTimeStamp (), CleanUpMessageForXMLLogging (e.Exception.ToString ()));
                this._MessageQueue.Enqueue (Data);
                WriteQueueData ();
            }
            this._ProjectTargetStack.Pop ();
        }

        public void BuildStarted (object sender, BuildEventArgs e)
        {
            this._tracingEnabled = e.Project.Properties.Contains (CCNetListenerFile_PropertyName);

            this._traceFileName = e.Project.Properties[CCNetListenerFile_PropertyName];

            string Data = String.Format ("<Item Time=\"{0}\" Data=\"Starting Build {1}\"  />", GetTimeStamp (), e.Project.ProjectName);
            this._ProjectTargetStack.Push (Data);

            this._MessageQueue.Clear ();
        }

        public void MessageLogged (object sender, BuildEventArgs e)
        {
            string Data = string.Format ("<Item Time=\"{0}\" Data=\"{1}\" />", GetTimeStamp (), CleanUpMessageForXMLLogging (e.Message));

            if (this._MessageQueue.Count > _MessageQueueLength)
            { this._MessageQueue.Dequeue (); }

            this._MessageQueue.Enqueue (Data);

            WriteQueueData ();
        }

        public void TargetFinished (object sender, BuildEventArgs e)
        {
            this._ProjectTargetStack.Pop ();
        }

        public void TargetStarted (object sender, BuildEventArgs e)
        {
            string Data = string.Format ("<Item Time=\"{0}\" Data=\"Starting Target {1}\"  />", GetTimeStamp (), e.Target.Name);
            this._ProjectTargetStack.Push (Data);
            this._MessageQueue.Clear ();
        }

        public void TaskFinished (object sender, BuildEventArgs e)
        {
        }

        public void TaskStarted (object sender, BuildEventArgs e)
        {
        }

        private string CleanUpMessageForXMLLogging (string msg)
        {
            return msg.Replace ("\"", string.Empty);
        }

        private string GetTimeStamp ()
        {
            return System.DateTime.Now.ToString ("yyyy-MM-dd hh:mm:ss");
        }

        private void WriteQueueData ()
        {
            if (!this._tracingEnabled) return;

            System.IO.StreamWriter TraceFile;

            System.Collections.ArrayList ProjectsTargets;
            ProjectsTargets = new System.Collections.ArrayList ();

            // empty file
            try
            {
                TraceFile = new System.IO.StreamWriter (this._traceFileName, false);
            }
            catch
            {
                return;
            }

            TraceFile.AutoFlush = false;
            TraceFile.WriteLine ("<data>");

            // write hierarchy
            ProjectsTargets.AddRange (this._ProjectTargetStack.ToArray ());
            ProjectsTargets.Reverse ();

            foreach (string data in ProjectsTargets)
            {
                TraceFile.WriteLine ("{0}", data);
            }

            // write messages
            foreach (string data in this._MessageQueue.ToArray ())
            {
                TraceFile.WriteLine ("{0}", data);
            }
            TraceFile.WriteLine ("</data>");

            TraceFile.Close ();
        }

        private const string CCNetListenerFile_PropertyName = "CCNetListenerFile";
        private const Int32 _MessageQueueLength = 10;

        private string _traceFileName;
        private Boolean _tracingEnabled;

        private System.Collections.Queue _MessageQueue = new System.Collections.Queue ();
        private System.Collections.Stack _ProjectTargetStack = new System.Collections.Stack ();

        public void Dispose ()
        {
            throw new System.NotImplementedException ();
        }

        public void Log (string message)
        {
            throw new System.NotImplementedException ();
        }

        public void Log (string format, params object[] args)
        {
            throw new System.NotImplementedException ();
        }

        public void LogExternalProgramOutput (string output)
        {
            throw new System.NotImplementedException ();
        }

        public void ReportRunnerFinished (bool success, TimeSpan buildDuration)
        {
            throw new System.NotImplementedException ();
        }

        public void ReportTaskStarted (ITask task)
        {
            throw new System.NotImplementedException ();
        }

        public void ReportTaskExecuted (ITask task)
        {
            throw new System.NotImplementedException ();
        }

        public void ReportTaskFailed (ITask task, Exception ex)
        {
            throw new System.NotImplementedException ();
        }

        public void ReportTaskFailed (ITask task, string reason)
        {
            throw new System.NotImplementedException ();
        }
    }
}