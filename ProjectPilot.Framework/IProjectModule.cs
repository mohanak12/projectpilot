using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectPilot.Framework.RevisionControlHistory;

namespace ProjectPilot.Framework
{
    public interface IProjectModule
    {
        string ControllerName { get; }
        string ModuleName { get; }
    }

    public class StaticHtmlPageModule : IProjectModule
    {
        public StaticHtmlPageModule(string moduleName, string pageName)
        {
            this.moduleName = moduleName;
            this.pageName = pageName;
        }

        public string ControllerName
        {
            get { return controllerName; }
        }

        public string ModuleName
        {
            get { return moduleName; }
        }

        private string controllerName;
        private string moduleName;
        private string pageName;
    }

    public class RevisionControlStatsModule : IProjectModule
    {
        public RevisionControlStatsModule(IRevisionControlHistoryPlugIn rcHistoryPlugIn)
        {
            this.rcHistoryPlugIn = rcHistoryPlugIn;
        }

        public string ControllerName
        {
            get { return "RevisionControlStats"; }
        }

        public string ModuleName
        {
            get { return "Revision Control Stats"; }
        }

        private IRevisionControlHistoryPlugIn rcHistoryPlugIn;
    }
}
