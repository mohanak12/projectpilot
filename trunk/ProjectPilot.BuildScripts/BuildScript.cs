using System;
using System.Collections.Generic;
using System.Text;

//css_ref bin\Debug\ProjectPilot.BuildScripts.dll;

namespace ProjectPilot.BuildScripts
{
    public class BuildScript
    {
        static public int Main(string[] args)
        {
            using (BuildTasks script = new BuildTasks("ProjectPilot"))
            {
                script
                    //.SetProductRootDir(@"..\..\..")
                    .SetCompanyInfo("HERMES SoftLab d.d.", "Copyright (C) 2008 HERMES SoftLab d.d.", "")

                    .ReadVersionInfo()

                    .AddProject("ProjectPilot.BuildScripts")
                    .AddProject("ProjectPilot.Framework")
                    .AddMainWebProject("ProjectPilot.Portal")
                    .AddProject("ProjectPilot.TestFramework")
                    .AddTestProject("ProjectPilot.Tests")

                    .CleanOutput()
                    .GenerateCommonAssemblyInfo()
                    .CompileSolution()
                    .FxCop()

                    .RunTests()

                    .Finished();
            }

            return 0;
        }
    }
}
