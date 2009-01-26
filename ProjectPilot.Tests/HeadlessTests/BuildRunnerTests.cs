using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Headless;
using Headless.Configuration;
using MbUnit.Framework;
using Rhino.Mocks;

namespace ProjectPilot.Tests.HeadlessTests
{
    [TestFixture]
    public class BuildRunnerTests
    {
        [Test]
        public void BuildProjectStage1Failed()
        {
            buildStage1.Task = this.mockBuildTaskFailure;
            buildStage2.Task = this.mockBuildTaskSuccess;
            buildStageAcceptTests.Task = this.mockBuildTaskSuccess;

            BuildReport report = RunBuild();

            Assert.AreEqual(BuildOutcome.Failed, report.BuildOutcome);
            Assert.AreEqual(BuildOutcome.Failed, report.StageReports[buildStage1.StageId].StageOutcome);
            Assert.AreEqual(BuildOutcome.NotExecuted, report.StageReports[buildStage2.StageId].StageOutcome);
            Assert.AreEqual(BuildOutcome.NotExecuted, report.StageReports[buildStageAcceptTests.StageId].StageOutcome);
        }

        [Test]
        public void BuildProjectSuccess()
        {
            buildStage1.Task = mockBuildTaskSuccess;
            buildStage2.Task = mockBuildTaskSuccess;
            buildStageAcceptTests.Task = mockBuildTaskSuccess;

            BuildReport report = RunBuild();

            Assert.AreEqual(BuildOutcome.Successful, report.BuildOutcome);
            Assert.AreEqual(BuildOutcome.Successful, report.StageReports[buildStage1.StageId].StageOutcome);
            Assert.AreEqual(BuildOutcome.Successful, report.StageReports[buildStage2.StageId].StageOutcome);
            Assert.AreEqual(BuildOutcome.Successful, report.StageReports[buildStageAcceptTests.StageId].StageOutcome);
        }

        [FixtureSetUp]
        public void FixtureSetup()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        [SetUp]
        public void Setup()
        {
            mockTrafficCop = MockRepository.GenerateMock<ITrafficCop>();
            mockStageRunnerFactory = MockRepository.GenerateMock<IStageRunnerFactory>();
            logger = new DefaultHeadlessLogger();

            project = new Project("MyProject");

            buildStage1 = new BuildStage("stage1", this.project);
            buildStage2 = new BuildStage("stage2", this.project);
            buildStageAcceptTests = new BuildStage("accept.tests", this.project);

            buildStageAcceptTests.BuildComputer = "other";

            buildStage2.DependsOn.Add(this.buildStage1);
            buildStageAcceptTests.DependsOn.Add(this.buildStage1);

            project.BuildStages.Add(this.buildStage1);
            project.BuildStages.Add(this.buildStage2);
            project.BuildStages.Add(this.buildStageAcceptTests);

            IStageRunner stageRunner1 = new LocalStageRunner(this.logger);
            stageRunner1.SetBuildStage(this.buildStage1);
            IStageRunner stageRunner2 = new LocalStageRunner(this.logger);
            stageRunner2.SetBuildStage(this.buildStage2);
            IStageRunner stageRunner3 = new LocalStageRunner(this.logger);
            stageRunner3.SetBuildStage(this.buildStageAcceptTests);

            this.mockStageRunnerFactory.Expect(factory => factory.CreateStageRunner(this.buildStage1)).Return(stageRunner1).Repeat.Once();
            this.mockStageRunnerFactory.Expect(factory => factory.CreateStageRunner(this.buildStage2)).Return(stageRunner2).Repeat.Once();
            this.mockStageRunnerFactory.Expect(factory => factory.CreateStageRunner(this.buildStageAcceptTests)).Return(stageRunner3).Repeat.Once();

            mockBuildTaskSuccess = MockRepository.GenerateMock<IBuildTask>();
            this.mockBuildTaskSuccess.Expect(task => task.Execute()).WhenCalled(delegate { Thread.Sleep(3000); });

            mockBuildTaskFailure = MockRepository.GenerateMock<IBuildTask>();
            this.mockBuildTaskFailure.Expect(task => task.Execute()).WhenCalled(delegate
                                                                                    {
                                                                                        Thread.Sleep(3000); 
                                                                                        throw new InvalidOperationException();
                                                                                    });

            this.mockTrafficCop.Expect(cop => cop.WaitForControlSignal(TimeSpan.Zero))
                .IgnoreArguments().WhenCalled(delegate { Thread.Sleep(5000); }).Return(TrafficCopControlSignal.NoSignal)
                .Repeat.Any();
        }

        private BuildReport RunBuild()
        {
            using (BuildRunner buildRunner = new BuildRunner(
                this.project,
                this.mockTrafficCop,
                this.mockStageRunnerFactory,
                this.logger))
            {
                return buildRunner.Run();
            }
        }

        private ITrafficCop mockTrafficCop;
        private IStageRunnerFactory mockStageRunnerFactory;
        private IHeadlessLogger logger;
        private Project project;
        private BuildStage buildStage1;
        private BuildStage buildStage2;
        private BuildStage buildStageAcceptTests;
        private IBuildTask mockBuildTaskSuccess;
        private IBuildTask mockBuildTaskFailure;
    }
}
