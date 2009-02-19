using System.Diagnostics.CodeAnalysis;
using MbUnit.Framework;
using Rhino.Mocks;
using Stump.Models;
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
            Workspace workspace = StumpMother.CreateWorkspace();

            ILogTabsView view = MockRepository.GenerateMock<ILogTabsView>();
            view.Expect(v => v.AddTab(new LogTabData("log1.txt", "d:/log1.txt")));
            view.Expect(v => v.AddTab(new LogTabData("log2.txt", "d:/log2.txt")));
            view.Expect(v => v.AddTab(new LogTabData("log3.txt", "d:/log3.txt")));

            LogTabsPresenter presenter = new LogTabsPresenter(view, workspace);

            view.VerifyAllExpectations();
        }
    }
}