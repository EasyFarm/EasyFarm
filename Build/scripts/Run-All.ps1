Param([string]$configuration="Release")

. ./Config.ps1

./Run-Build $configuration
./Run-Coverage $configuration
./Run-Reporter