using System;
using System.Collections.Generic;
using System.Text;
using Flubu;

//css_ref ProjectPilot.BuildScripts.dll;

namespace ProjectPilot.BuildScripts
{
    public class BuildScript
    {
        public static int Main(string[] args)
        {
            using (BuildRunner script = new BuildRunner("ProjectPilot"))
            {
                script.ScriptExecutionEnvironment.Logger = new NAntLikeFlubuLogger(System.Console.Out);
                script
                    //.SetProductRootDir(@"..\..\..")
                    .PrepareBuildDirectory()
                    .SetCompanyInfo("HERMES SoftLab d.d.", "Copyright (C) 2008 HERMES SoftLab d.d.", String.Empty)
                    .ReadVersionInfo();

                script
                    .LoadSolution("ProjectPilot.sln")
                    .RegisterAsWebProject("ProjectPilot.Portal", "http://localhost/ProjectPilot");

                script
                    .CleanOutput()
                    .GenerateCommonAssemblyInfo()
                    .CompileSolution()
                    .FxCop()
                    .RunTests("ProjectPilot.Tests");

                script
                    .Complete();
            }

            return 0;
        }
    }
}
