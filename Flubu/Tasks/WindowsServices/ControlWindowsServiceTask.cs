using System;
using System.ServiceProcess;

namespace Flubu.Tasks.WindowsServices
{
    public class ControlWindowsServiceTask : TaskBase
    {
        public override string TaskDescription
        {
            get
            {
                return String.Format (
                    System.Globalization.CultureInfo.InvariantCulture,
                    "{1} Windows service '{0}'.", 
                    serviceName, 
                    mode);
            }
        }

        public ControlWindowsServiceTask (string serviceName, ControlWindowsServiceMode mode, TimeSpan timeout)
        {
            this.serviceName = serviceName;
            this.mode = mode;
            this.timeout = timeout;
            MachineName = ".";
        }

        public ControlWindowsServiceTask(string machineName, string serviceName, ControlWindowsServiceMode mode, TimeSpan timeout)
        {
            this.serviceName = serviceName;
            this.mode = mode;
            this.timeout = timeout;
            MachineName = machineName;
        }

        public static void Execute (
            IScriptExecutionEnvironment environment,
            string serviceName, 
            ControlWindowsServiceMode mode, 
            TimeSpan timeout)
        {
            ControlWindowsServiceTask task = new ControlWindowsServiceTask (serviceName, mode, timeout);
            task.Execute (environment);
        }

        public static void Execute(
            IScriptExecutionEnvironment environment,
            string machineName,
            string serviceName,
            ControlWindowsServiceMode mode,
            TimeSpan timeout)
        {
            ControlWindowsServiceTask task = new ControlWindowsServiceTask(machineName, serviceName, mode, timeout);
            task.Execute(environment);
        }

        protected override void DoExecute (IScriptExecutionEnvironment environment)
        {
            using (ServiceController serviceController = new ServiceController (serviceName, MachineName))
            {
                ServiceControllerStatus status = ServiceControllerStatus.Running;
                switch (mode)
                {
                    case ControlWindowsServiceMode.Start:
                        status = ServiceControllerStatus.Running;
                        break;
                    case ControlWindowsServiceMode.Stop:
                        status = ServiceControllerStatus.Stopped;
                        break;
                }

                switch (status)
                {
                    case ServiceControllerStatus.Running:
                        if (serviceController.Status != ServiceControllerStatus.Running)
                            serviceController.Start ();
                        break;

                    case ServiceControllerStatus.Stopped:
                        if (serviceController.Status != ServiceControllerStatus.Stopped)
                            serviceController.Stop ();
                        break;
                }

                int timeSoFar = 0;
                for (serviceController.Refresh(); serviceController.Status != status; serviceController.Refresh())
                {
                    System.Threading.Thread.Sleep(500);
                    timeSoFar += 500;

                    if (timeSoFar >= timeout.TotalMilliseconds)
                        throw new RunnerFailedException(
                            String.Format(
                                System.Globalization.CultureInfo.InvariantCulture,
                                "Timeout waiting for '{0}' service to reach status {1}.", 
                                serviceName, 
                                status));
                }
            }
        }

        private string MachineName { get; set; }

        private string serviceName;
        private ControlWindowsServiceMode mode;
        private TimeSpan timeout;
    }
}
