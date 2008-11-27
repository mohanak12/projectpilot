using System;
using System.Collections.Generic;
using System.Text;

//css_ref bin\Debug\ProjectPilot.BuildScripts.dll;

namespace ProjectPilot.BuildScripts
{
    public class BuildScript
    {
        public static int Main(string[] args)
        {
            using (BuildRunner script = new BuildRunner("ProjectPilot"))
            {
                script
                    //.SetProductRootDir(@"..\..\..")
                    .SetCompanyInfo("HERMES SoftLab d.d.", "Copyright (C) 2008 HERMES SoftLab d.d.", String.Empty)

                    .ReadVersionInfo()

                    .CleanOutput()
                    .GenerateCommonAssemblyInfo()
                    .CompileSolution()
                    .FxCop()

                    .RunTests()

                    .Complete();
            }

            return 0;
        }
    }
}
