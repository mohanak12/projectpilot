using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Stump.Models;
using Stump.Views;

namespace Stump.Presenters
{
    public class LogTabsPresenter
    {
        public LogTabsPresenter(ILogTabsView view, Workspace workspace)
        {
            this.workspace = workspace;

            foreach (MonitoredLogFile logFile in workspace.LogFiles)
            {
                LogTabData data = new LogTabData(Path.GetFileName(logFile.FileName), logFile.FileName);
                view.AddTab(data);
            }
        }

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "tabIndex")]
        public void OnTabSelected(int tabIndex)
        {
            throw new NotImplementedException();
        }

        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private readonly Workspace workspace;
    }
}