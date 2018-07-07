###############################################################################
## Configuration
###############################################################################
Param(
    [string]$configuration="Release",
    [string]$target="Rebuild",
    [string]$verbosity="Normal"
)
$FindParent = "./Find-Parent.ps1"
$vs_version = "15.0"
$solution_dir= (&$FindParent "EasyFarm") 
$msbuild = "C:\Program Files (x86)\Microsoft Visual Studio\*\*\MSBuild\$vs_version\Bin\msbuild.exe"
$nuget = "../tools/nuget.exe"

###############################################################################

&$nuget restore $solution_dir -Verbosity $verbosity
&$msbuild $solution_dir -p:Configuration=$configuration -t:$target /m /verbosity:$verbosity

###############################################################################
