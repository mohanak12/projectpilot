using System.Diagnostics.CodeAnalysis;
using System.Windows.Forms;
using Stump.Models;
using Stump.Presenters;
using Stump.Views;

namespace Stump.ViewImplementations
{
    /// <summary>
    /// The main form of the application.
    /// </summary>
    public partial class MainForm : Form, ILogTabsView
    {
        public MainForm()
        {
            InitializeComponent();

            Workspace workspace = new Workspace();

            logTabsPresenter = new LogTabsPresenter(this, workspace);
        }

        public void AddTab(LogTabData logTabData)
        {
            TabPage tabPage = new TabPage(logTabData.TabText);
            tabPage.ToolTipText = logTabData.ToolTipText;

            tabControl.TabPages.Add(tabPage);
        }

        public void SwitchToLog(int logIndex)
        {
            tabControl.SelectedIndex = logIndex;
        }

        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private LogTabsPresenter logTabsPresenter;
    }
}
