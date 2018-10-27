. ./Config.ps1

&$test_reporter `
	-reports:"$test_report"  `
	-targetdir:"$coverage_dir"