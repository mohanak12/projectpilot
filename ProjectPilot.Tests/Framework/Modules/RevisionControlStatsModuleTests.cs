using System.IO;
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
                data = SubversionHistoryModule.LoadHistory(stream);
            }

            IRevisionControlHistoryModule revisionControlHistoryModule = MockRepository.GenerateStub<IRevisionControlHistoryModule>();
            revisionControlHistoryModule.Stub(action => action.FetchHistory()).Return(data);

            //IRevisionControlHistoryModule revisionControlHistoryModule = new SubversionHistoryModule(
            //    project.ProjectId,
            //    @"C:\Program Files\CollabNet Subversion\svn.exe",
            //    @"D:\svn\mobilkom.nl-bhwr\trunk\src");

            RevisionControlStatsModule module = new RevisionControlStatsModule(
                revisionControlHistoryModule,
                projectRegistry,
                fileManager,
                templateEngine);
            module.ProjectId = "bhwr";
            project.AddModule(module);

            module.ExecuteTask(null);
        }

        [SetUp]
        public void Setup()
        {
        }
    }
}
