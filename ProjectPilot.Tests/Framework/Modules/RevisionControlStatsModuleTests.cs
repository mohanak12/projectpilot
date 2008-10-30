using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using ProjectPilot.Framework;
using ProjectPilot.Framework.Modules;
using ProjectPilot.Framework.RevisionControlHistory;
using ProjectPilot.Framework.Subversion;
using Rhino.Mocks;

namespace ProjectPilot.Tests.Framework.Modules
{
    [TestFixture]
    public class RevisionControlStatsModuleTests
    {
        [Test]
        public void Test()
        {
            ProjectPilotConfiguration projectPilotConfiguration = new ProjectPilotConfiguration();
            projectPilotConfiguration.ProjectPilotWebAppRootUrl = "http://localhost/projectpilot/";

            ProjectRegistry projectRegistry = new ProjectRegistry();
            Project project = new Project("bhwr", "Mobilkom BHWR");
            projectRegistry.AddProject(project);

            IFileManager fileManager = new DefaultFileManager("", projectPilotConfiguration, projectRegistry);
            projectRegistry.FileManager = fileManager;

            IFileManager templateFileManager = MockRepository.GenerateStub<IFileManager>();
            templateFileManager.Stub(action => action.GetFullFileName(null, null)).IgnoreArguments().Return(@"..\..\..\Data\Templates\RevisionControlHistory.vm");

            ITemplateEngine templateEngine = new DefaultTemplateEngine(templateFileManager);

            // prepare test history data
            RevisionControlHistoryData data;
            using (Stream stream = File.OpenRead(@"..\..\..\Data\Samples\svn-log.xml"))
            {
                data = SubversionHistoryPlugIn.LoadHistory(stream);
            }

            IRevisionControlHistoryPlugIn revisionControlHistoryPlugIn = MockRepository.GenerateStub<IRevisionControlHistoryPlugIn>();
            revisionControlHistoryPlugIn.Stub(action => action.FetchHistory()).Return(data);

            //IRevisionControlHistoryPlugIn revisionControlHistoryPlugIn = new SubversionHistoryPlugIn(
            //    project.ProjectId,
            //    @"C:\Program Files\CollabNet Subversion\svn.exe",
            //    @"D:\svn\mobilkom.nl-bhwr\trunk\src");

            RevisionControlStatsModule module = new RevisionControlStatsModule(
                project, 
                revisionControlHistoryPlugIn,
                fileManager,
                templateEngine);

            module.Generate();
        }

        [SetUp]
        public void Setup()
        {
        }
    }
}
