###############################################################################
## Configuration
###############################################################################
Param([string]$configuration="Release")
$FindParent = "./Find-Parent.ps1"
$bin_folder= (&$FindParent "EasyFarm") + "\EasyFarm.Tests\bin\$configuration"
$test_runner = (&$FindParent "EasyFarm") + "\packages\xunit.runner.console.*\tools\net452\xunit.console.x86.exe"
$test_assembly = "$bin_folder\EasyFarm.Tests.dll"

###############################################################################

&$test_runner $test_assembly

###############################################################################