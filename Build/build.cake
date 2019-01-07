var target = Argument("target", "Default");
var xunitToolPath = ".\\tools\\xunit.runner.console\\tools\\net452\\xunit.console.x86.exe";

Task("Default")
	.IsDependentOn("test");

Task("test")
	.IsDependentOn("build-it")
	.IsDependentOn("test-it");

Task("cover")
	.IsDependentOn("build-it")
	.IsDependentOn("cover-it")
	.IsDependentOn("report-it");

Task("analyze")
    .IsDependentOn("analyze-it");
	
Task("build-it").Does(() => {
	MSBuild("../EasyFarm.sln", settings => settings
		.WithTarget("Clean;Rebuild")
		.SetMaxCpuCount(0)
		.SetVerbosity(Verbosity.Quiet)
		.UseToolVersion(MSBuildToolVersion.VS2017)
		.SetMSBuildPlatform(MSBuildPlatform.x86)
		.SetPlatformTarget(PlatformTarget.MSIL)
	);
});

Task("test-it").Does(() => {
	XUnit2("../**/*.Tests.dll", new XUnit2Settings() {
		ToolPath = xunitToolPath,
		ShadowCopy = false
	});
});

Task("cover-it").Does(() => {
	OpenCover(tool => {
		tool.XUnit2("../**/*.Tests.dll", new XUnit2Settings() {
			ToolPath = xunitToolPath,
			ShadowCopy = false
		});
	},
	new FilePath("./result.xml"),
	new OpenCoverSettings()
		.WithFilter("+[EasyFarm]*")
		.WithFilter("+[MemoryAPI]*")
		.WithFilter("-[EasyFarm.Tests]*")
		.WithFilter("-[EasyFarm]EasyFarm.Views.*")
	);
});

Task("report-it").Does(() => {
	ReportGenerator("./result.xml", "./coverage");
});

Task("analyze-it").Does(() => {
     DupFinder("../EasyFarm.sln", new DupFinderSettings {
     ShowStats = true,
     ShowText = true,
     OutputFile = "./duplication.xml"
    });
 });

RunTarget(target);
