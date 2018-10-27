# Functions
$FindParent = "./Find-Parent.ps1"
$RunTests = "./Run-Tests.ps1"
$RunBuild = "./Run-Build.ps1"

# Tools
$msbuild = "C:\Program Files (x86)\Microsoft Visual Studio\*\*\MSBuild\$vs_version\Bin\msbuild.exe"
$nuget = "../tools/nuget.exe"
$test_runner = (&$FindParent "EasyFarm") + "\packages\xunit.runner.console.*\tools\net452\xunit.console.x86.exe"
$test_coverer = (&$FindParent "EasyFarm") +"\packages\OpenCover.*\tools\OpenCover.Console.exe"
$test_reporter = (&$FindParent "EasyFarm") + "\packages\ReportGenerator.*\tools\ReportGenerator.exe"

# Tool Settings
$vs_version = "15.0"
$test_coverer_filters = "+[EasyFarm]* +[MemoryAPI]* -[EasyFarm.Tests]*"
$test_report = "../results.xml"
$coverage_dir = "../coverage"

# Locations
$Workspace = (&$FindParent "EasyFarm")
$solution_dir= (&$FindParent "EasyFarm") 
$bin_folder= (&$FindParent "EasyFarm") + "\EasyFarm.Tests\bin\$configuration"
$test_assembly = "$bin_folder\EasyFarm.Tests.dll"