using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using ProjectPilot.Framework.Modules;

namespace ProjectPilot.Framework.CCNet
{
    public class CCNetProjectStatisticsModule : IProjectModule, IViewable, ITask
    {
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "graphs")]
        public CCNetProjectStatisticsModule(
            CCNetProjectStatisticsPlugIn ccnetPlugIn,
            IEnumerable<CCNetProjectStatisticsGraph> graphs)
        {
            this.ccnetPlugIn = ccnetPlugIn;
            throw new NotImplementedException();
        }

        public string ModuleId
        {
            get { return "CCNetProjectStatistics";  }
        }

        public string ModuleName
        {
            get { return "CCNet Project Statistics"; }
        }

        public string ProjectId
        {
            get { return projectId; }
            set { projectId = value; }
        }

        public ITrigger Trigger
        {
            get { return trigger; }
            set { trigger = value; }
        }

        public string FetchHtmlReport()
        {
            throw new System.NotImplementedException();
        }

        public void ExecuteTask(WaitHandle stopSignal)
        {
            throw new System.NotImplementedException();
        }

        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private CCNetProjectStatisticsPlugIn ccnetPlugIn;
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private List<CCNetProjectStatisticsGraph> graphs = new List<CCNetProjectStatisticsGraph>();
        private string projectId;
        private ITrigger trigger = new NullTrigger();
    }
}