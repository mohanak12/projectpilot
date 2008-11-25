using System;
using System.Collections.Generic;
using System.Text;

namespace Flubu.Tasks.Misc
{
    public class SleepTask : TaskBase
    {
        public override string TaskDescription
        {
            get 
            { 
                return String.Format (
                    System.Globalization.CultureInfo.InvariantCulture,
                    "Sleep for {0} seconds.", 
                    sleepPeriod.TotalSeconds); 
            }
        }

        public SleepTask (TimeSpan sleepPeriod)
        {
            this.sleepPeriod = sleepPeriod;
        }

        public static void Execute (IScriptExecutionEnvironment environment, TimeSpan sleepPeriod)
        {
            SleepTask task = new SleepTask (sleepPeriod);
            task.Execute (environment);
        }

        protected override void DoExecute (IScriptExecutionEnvironment environment)
        {
            System.Threading.Thread.Sleep (sleepPeriod);
        }

        private TimeSpan sleepPeriod;
    }
}
