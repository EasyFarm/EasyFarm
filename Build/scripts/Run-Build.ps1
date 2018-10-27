Param(
    [string]$configuration="Release",
    [string]$target="Rebuild",
    [string]$verbosity="Normal"
)

. ./Config.ps1

&$nuget restore $solution_dir -Verbosity $verbosity
&$msbuild $solution_dir -p:Configuration=$configuration -t:$target /m /verbosity:$verbosity