using System.Diagnostics.CodeAnalysis;
using MbUnit.Framework;
using Rhino.Mocks;
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

            view.Expect(v => v.IsLogDisplayActive).Return(true);
            view.Expect(v => v.ShowLogContents("logContents1"));

            monitor.Expect(m => m.StartMonitoring("d:/log1.txt", null, null))
                .IgnoreArguments().Repeat.Once();
            monitor.Expect(m => m.StopMonitoring()).Repeat.Once();

            using (LogPresenter presenter = new LogPresenter(
                view,
                monitor,
                mother.LogReader,
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

            monitor.Expect(m => m.StartMonitoring("d:/log1.txt", null, null))
                .IgnoreArguments().Repeat.Once();
            monitor.Expect(m => m.StopMonitoring()).Repeat.Once();

            using (LogPresenter presenter = new LogPresenter(
                view,
                monitor,
                mother.LogReader,
                mother.Workspace.LogFiles[0]))
            {
            }

            view.AssertWasNotCalled(v => v.ShowLogContents("logContents1"));
            view.VerifyAllExpectations();
            monitor.VerifyAllExpectations();
        }
    }
}