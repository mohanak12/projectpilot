using Gallio;
using MbUnit.Framework;
using Rhino.Mocks;
using Rhino.Mocks.Constraints;
using Stump.Presenters;
using Stump.Services;
using Stump.Views;

namespace ProjectPilot.Tests.StumpTests
{
    public class LogViewingTests
    {
        [Test]
        public void CreateLogViewWhichIsActive()
        {
            StumpMother mother = new StumpMother();

            ILogView view = MockRepository.GenerateMock<ILogView>();
            ILogMonitor monitor = mother.LogMonitors[0];

            mother.LogUpdaterQueue.Expect(lr => lr.FetchLogContents("d:/log1.txt", null))
                .Constraints(Is.Equal("d:/log1.txt"), Is.Anything())
                .Do((Action<string, LogContentsFetchedCallback>) delegate { view.ShowLogContents("logContents1"); });

            view.Expect(v => v.IsLogDisplayActive).Return(true);
            view.Expect(v => v.ShowLogContents("logContents1"));

            monitor.Expect(m => m.StartMonitoring("d:/log1.txt", null, null, null, null))
                .IgnoreArguments().Repeat.Once();
            monitor.Expect(m => m.StopMonitoring()).Repeat.Once();

            using (LogPresenter presenter = new LogPresenter(
                view,
                monitor,
                mother.LogUpdaterQueue,
                mother.Workspace.LogFiles[0]))
            {
            }

            view.VerifyAllExpectations();
            monitor.VerifyAllExpectations();
        }

        [Test]
        public void CreateLogViewWhichIsNotActive()
        {
            StumpMother mother = new StumpMother();

            ILogView view = MockRepository.GenerateMock<ILogView>();
            ILogMonitor monitor = mother.LogMonitors[0];

            view.Expect(v => v.IsLogDisplayActive).Return(false);

            monitor.Expect(m => m.StartMonitoring("d:/log1.txt", null, null, null, null))
                .IgnoreArguments().Repeat.Once();
            monitor.Expect(m => m.StopMonitoring()).Repeat.Once();

            using (LogPresenter presenter = new LogPresenter(
                view,
                monitor,
                mother.LogUpdaterQueue,
                mother.Workspace.LogFiles[0]))
            {
            }

            view.AssertWasNotCalled(v => v.ShowLogContents("logContents1"));
            view.VerifyAllExpectations();
            monitor.VerifyAllExpectations();
        }

        [Test]
        public void CreateLogViewWhichIsActiveButNotMonitored()
        {
            StumpMother mother = new StumpMother();
            mother.Workspace.LogFiles[0].IsActive = false;

            ILogView view = MockRepository.GenerateMock<ILogView>();
            ILogMonitor monitor = mother.LogMonitors[0];

            view.Expect(v => v.IsLogDisplayActive).Return(true);
            view.Expect(v => v.IndicateLogFileNotMonitored());

            using (LogPresenter presenter = new LogPresenter(
                view,
                monitor,
                mother.LogUpdaterQueue,
                mother.Workspace.LogFiles[0]))
            {
            }

            monitor.AssertWasNotCalled(m => m.StartMonitoring(null, null, null, null, null));
            view.VerifyAllExpectations();
            monitor.VerifyAllExpectations();
        }

        [Test]
        public void StopMonitoring()
        {
            StumpMother mother = new StumpMother();

            ILogView view = MockRepository.GenerateMock<ILogView>();
            ILogMonitor monitor = mother.LogMonitors[0];

            view.Expect(v => v.IsLogDisplayActive).Return(true);
            view.Expect(v => v.IndicateLogFileNotMonitored());
            
            using (LogPresenter presenter = new LogPresenter(
                view,
                monitor,
                mother.LogUpdaterQueue,
                mother.Workspace.LogFiles[0]))
            {
                // turn off monitoring
                presenter.OnMonitoringEnabledToggled();
            }

            view.VerifyAllExpectations();
            monitor.VerifyAllExpectations();
        }
    }
}