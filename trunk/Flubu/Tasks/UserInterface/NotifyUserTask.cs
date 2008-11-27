using System;
using System.Collections.Generic;
using System.Text;

namespace Flubu.Tasks.UserInterface
{
    public class NotifyUserTask : TaskBase
    {
        public override string TaskDescription
        {
            get { return "Notify user"; }
        }

        public NotifyUserTask (string message)
        {
            this.message = message;
        }

        public static void Execute (IScriptExecutionEnvironment environment, string message)
        {
            NotifyUserTask task = new NotifyUserTask (message);
            task.Execute (environment);
        }

        protected override void DoExecute (IScriptExecutionEnvironment environment)
        {
            environment.Logger.Log(message);
        }

        private string message;
    }
}
