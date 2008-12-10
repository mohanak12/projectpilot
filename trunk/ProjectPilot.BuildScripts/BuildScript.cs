using System;
using Flubu;
using Flubu.Builds;

//css_ref ProjectPilot.BuildScripts.dll;

namespace ProjectPilot.BuildScripts
{
    public class BuildScript
    {
        public static int Main(string[] args)
        {
            using (BuildRunner script = new BuildRunner("ProjectPilot"))
            {
                NAntLikeFlubuLogger logger = new NAntLikeFlubuLogger();

                script.ScriptExecutionEnvironment.Logger = new NAntLikeFlubuLogger();
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
