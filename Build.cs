using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using ProjectPilot.BuildScripts;

//css_ref ProjectPilot.BuildScripts\bin\Debug\ProjectPilot.BuildScripts.dll;

namespace BuildScripts
{
    public class Build
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
                    .AddProject("ProjectPilot.Tests")

                    .CleanOutput()
                    .GenerateCommonAssemblyInfo()
                    .CompileSolution()
                    .FxCop()

                    .Finished();
            }

            return 0;
	    }
    }
}