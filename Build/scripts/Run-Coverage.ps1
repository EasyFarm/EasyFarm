###############################################################################
## Configuration
###############################################################################
Param([string]$configuration="Release")
$FindParent = "./Find-Parent.ps1"
$bin_folder= (&$FindParent "EasyFarm") + "\EasyFarm.Tests\bin\$configuration\"	
$test_runner = (&$FindParent "EasyFarm") + "\packages\xunit.runner.console.*\tools\net452\xunit.console.x86.exe"
$test_coverer = (&$FindParent "EasyFarm") +"\packages\OpenCover.*\tools\OpenCover.Console.exe"
$test_assembly = "$bin_folder\EasyFarm.Tests.dll"
$test_coverer_filters = "+[EasyFarm]* +[MemoryAPI]* -[EasyFarm.Tests]*"
$test_report = "..\results.xml"

###############################################################################

$test_runner = $(ls "$test_runner")[0].FullName
&$test_coverer `
	-target:"$test_runner" `
	-targetargs:"$test_assembly" `
	-filter:"$test_coverer_filters" `
	-register:"Path32" `
	-searchdirs:"$bin_folder"
mv -Force results.xml $test_report

###############################################################################