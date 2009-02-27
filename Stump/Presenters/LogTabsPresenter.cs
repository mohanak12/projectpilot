using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Stump.Models;
using Stump.Views;

namespace Stump.Presenters
{
    public class LogTabsPresenter
    {
        public LogTabsPresenter(
            ILogTabsView view, 
            Workspace workspace)
        {
            this.view = view;
            this.workspace = workspace;

            foreach (MonitoredLogFile logFile in workspace.LogFiles)
            {
                LogTabData data = new LogTabData(Path.GetFileName(logFile.FileName), logFile.FileName);
                view.AddTab(data);
            }

            view.SwitchToLog(0);
        }

        public void OnTabSelected(int tabIndex)
        {
            view.SwitchToLog(tabIndex);
        }

        private readonly ILogTabsView view;
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private readonly Workspace workspace;
    }
}