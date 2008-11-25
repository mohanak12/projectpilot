using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using ProjectPilot.BuildScripts;

namespace ProjectPilot.Tests.BuildScriptsTests
{
    [TestFixture]
    public class TestBuild
    {
        [Test,Ignore]
        public void Test()
        {
            using (BuildTasks script = new BuildTasks("ProjectPilot"))
            {
                script
                    .SetProductRootDir(@"..\..\..")
                    .SetCompanyInfo("HERMES SoftLab d.d.", "Copyright (C) 2008 HERMES SoftLab d.d.", "")

                    .ReadVersionInfo()

                    .AddProject("ProjectPilot.BuildScripts")
                    .AddProject("ProjectPilot.Framework")
                    .AddMainWebProject("ProjectPilot.Portal")
                    .AddProject("Accipio")
                    .AddTestProject("ProjectPilot.Tests")

                    .CleanOutput()
                    .GenerateCommonAssemblyInfo()
                    .CompileSolution()
                    .FxCop()

                    .RunTests()

                    .Finished();
            }
        }
    }
}
