echo Prepares BuildScript.exe for running the build

lib\cs-script\cscs.exe /e ProjectPilot.BuildScripts\BuildScript.cs
xcopy ProjectPilot.BuildScripts\BuildScript.exe Scripts /y
xcopy ProjectPilot.BuildScripts\bin\Debug\*.dll Scripts /y
xcopy ProjectPilot.BuildScripts\bin\Debug\*.pdb Scripts /y
