using System;
using Flubu;
using Flubu.Builds;

//css_ref bin\Debug\Flubu.dll;
//css_ref ProjectPilot.BuildScripts.dll;

namespace ProjectPilot.BuildScripts
{
    public class BuildScript
    {
        public static int Main(string[] args)
        {
            using (ConcreteBuildRunner runner = new ConcreteBuildRunner("ProjectPilot"))
            {
                try
                {
                    runner.ScriptExecutionEnvironment.Logger = new NAntLikeFlubuLogger();

                    runner.AddTarget("load.solution").Do(TargetLoadSolution);
                    runner.AddTarget("compile").Do(TargetCompile).DependsOn("load.solution");
                    runner.AddTarget("stage.1").DependsOn("compile", "package");
                    runner.AddTarget("unit.tests").Do(r => r.RunTests("ProjectPilot.Tests", false)).DependsOn("load.solution");
                    runner.AddTarget("prepare.web").Do(r => runner.PrepareWebApplications()).DependsOn("load.solution");
                    runner.AddTarget("rebuild").SetAsDefault().DependsOn("stage.1");
                    runner.AddTarget("package").Do(TargetPackage).DependsOn("load.solution");

                    // actual run
                    if (args.Length == 0)
                        runner.RunTarget(runner.DefaultTarget.TargetName);
                    else
                        runner.RunTarget(args[0]);

                    runner
                        .Complete();

                    return 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return 1;
                }
                finally
                {
                    runner.MergeCoverageReports();
                    runner.CopyBuildLogsToCCNet();
                }
            }
        }

        private static void TargetCompile(ConcreteBuildRunner runner)
        {
            runner
                .PrepareBuildDirectory()
                .SetCompanyInfo(
                    "HERMES SoftLab d.d.",
                    "Copyright (C) 2008 HERMES SoftLab d.d.",
                    String.Empty)

                .CleanOutput()
                .GenerateCommonAssemblyInfo()
                .CompileSolution()
                .FxCop();
            runner
                .RunTarget("unit.tests");
        }

        private static void TargetLoadSolution(ConcreteBuildRunner runner)
        {
            runner
                .LoadSolution("ProjectPilot.sln");
            runner
                .FetchBuildVersion()
                .RegisterAsWebProject("ProjectPilot.Portal", "http://localhost/projectpilot");
        }

        private static void TargetPackage(ConcreteBuildRunner runner)
        {
            runner
                .BuildProducts
                    .AddProject("accipio", "Accipio.Console")
                    .AddProject("flubu", "Flubu");
            runner
                .CopyBuildProductFiles()
                .PackageBuildProduct("Accipio-{1}.zip", "accipio")
                .CopyBuildProductToCCNet(@"packages\Accipio\Accipio-latest.zip")
                .CopyBuildProductToCCNet(@"packages\Accipio\{2}.{3}\{4}")

                .PackageBuildProduct("Flubu-{1}.zip", "flubu")
                .CopyBuildProductToCCNet(@"packages\Flubu\Flubu-latest.zip")
                .CopyBuildProductToCCNet(@"packages\Flubu\{2}.{3}\{4}");
        }
    }
}
