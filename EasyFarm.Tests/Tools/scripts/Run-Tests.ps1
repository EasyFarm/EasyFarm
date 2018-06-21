###############################################################################
## Configuration
###############################################################################

$FindParent = "./Find-Parent.ps1"
$configuration="Debug"
$bin_folder= (&$FindParent "EasyFarm.Tests") + "\bin\$configuration\"	
$test_runner = (&$FindParent "EasyFarm") + "\packages\xunit.runner.console.*\tools\net452\xunit.console.x86.exe"
$test_assembly = "$bin_folder\EasyFarm.Tests.dll"

###############################################################################

&$test_runner $test_assembly
rm results.xml

###############################################################################