using System.Diagnostics.CodeAnalysis;
using MbUnit.Framework;
using Rhino.Mocks;
using Stump.Presenters;
using Stump.Views;

namespace ProjectPilot.Tests.StumpTests
{
    [TestFixture]
    public class LogTabsTests
    {
        [Test]
        [SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "Stump.Presenters.LogTabsPresenter")]
        public void ShowMultipleLogTabs()
        {
            StumpMother mother = new StumpMother();

            ILogTabsView view = MockRepository.GenerateMock<ILogTabsView>();
            view.Expect(v => v.AddTab(new LogTabData("log1.txt", "d:/log1.txt")));
            view.Expect(v => v.AddTab(new LogTabData("log2.txt", "d:/log2.txt")));
            view.Expect(v => v.AddTab(new LogTabData("log3.txt", "d:/log3.txt")));
            view.Expect(v => v.SwitchToLog(0));

            LogTabsPresenter presenter = new LogTabsPresenter(view, mother.Workspace);

            view.VerifyAllExpectations();
        }

        [Test]
        public void SelectTab()
        {
            StumpMother mother = new StumpMother();

            ILogTabsView view = MockRepository.GenerateMock<ILogTabsView>();
            view.Expect(v => v.SwitchToLog(1));

            LogTabsPresenter presenter = new LogTabsPresenter(view, mother.Workspace);
            presenter.OnTabSelected(1);

            view.VerifyAllExpectations();            
        }
    }
}