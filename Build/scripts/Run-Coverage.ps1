Param([string]$configuration="Release")

. ./Config.ps1

$test_runner = $(ls "$test_runner")[0].FullName
&$test_coverer `
	-target:"$test_runner" `
	-targetargs:"$test_assembly" `
	-filter:"$test_coverer_filters" `
	-register:"Path32" `
	-searchdirs:"$bin_folder"
mv -Force results.xml $test_report