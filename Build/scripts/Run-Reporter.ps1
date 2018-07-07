###############################################################################
## Configuration
###############################################################################

$FindParent = "./Find-Parent.ps1"
$test_reporter = (&$FindParent "EasyFarm") + "\packages\ReportGenerator.*\tools\ReportGenerator.exe"
$test_report = "../results.xml"
$coverage_dir = "../coverage"

###############################################################################

&$test_reporter `
	-reports:"$test_report"  `
	-targetdir:"$coverage_dir"

###############################################################################