###############################################################################
## Configuration
###############################################################################

$FindParent = "./Find-Parent.ps1"
$vs_version = "15.0"
$configuration="Debug"
$solution_dir= (&$FindParent "EasyFarm") 
$msbuild = "C:\Program Files (x86)\Microsoft Visual Studio\*\*\MSBuild\$vs_version\Bin\msbuild.exe"

###############################################################################

&$msbuild $solution_dir -p:Configuration=$configuration

###############################################################################