using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using ProjectPilot.Framework;
using ProjectPilot.Framework.Modules;
using ProjectPilot.Framework.RevisionControlHistory;
using ProjectPilot.Framework.Subversion;

namespace ProjectPilot.Tests.Framework.Modules
{
    [TestFixture]
    public class RevisionControlStatsModuleTests
    {
        [Test]
        public void Test()
        {
            IRevisionControlHistoryPlugIn revisionControlHistoryPlugIn = new SubversionHistoryPlugIn(
                @"C:\Program Files\CollabNet Subversion\svn.exe",
                @"D:\svn\mobilkom.nl-bhwr\trunk\src");
            ProjectRegistry projectRegistry = new ProjectRegistry();
            IFileManager fileManager = new DefaultFileManager(projectRegistry);
            projectRegistry.FileManager = fileManager;
            RevisionControlStatsModule module = new RevisionControlStatsModule(
                "bhwr", 
                revisionControlHistoryPlugIn,
                fileManager);

            module.Generate();
        }
    }
}
