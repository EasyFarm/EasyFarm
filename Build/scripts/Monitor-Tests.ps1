###############################################################################
## Configuration
###############################################################################
Param([string]$configuration="Release")
$FindParent = "./Find-Parent.ps1"
$RunTests = "./Run-Tests.ps1"
$RunBuild = "./Run-Build.ps1"
$Workspace = (&$FindParent "EasyFarm")

###############################################################################
$watcher = New-Object System.IO.FileSystemWatcher
$watcher.Path = $Workspace
$watcher.IncludeSubdirectories = $true
$watcher.EnableRaisingEvents = $true
$watcher.NotifyFilter = [System.IO.NotifyFilters]::LastWrite -bor [System.IO.NotifyFilters]::FileName 

while($true){
	$result = $watcher.WaitForChanged([System.IO.WatcherChangeTypes]::Changed -bor [System.IO.WatcherChangeTypes]::Renamed -bor [System.IO.WatcherChangeTypes]::Created, 1000);
	if($result.TimedOut){ continue; }
	if($result.Name.StartsWith(".git")){ continue; }

	Write-Host "### Running Tests ###" -BackgroundColor Green
	""
	&$RunBuild -configuration $configuration -target "Build" -verbosity "Quiet"
	""
	&$RunTests $configuration
	""
	Write-Host "### Complete! ###" -BackgroundColor Green
	""
}

###############################################################################