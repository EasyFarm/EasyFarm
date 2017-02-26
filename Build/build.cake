#tool "nuget:?package=xunit.runner.console"
#tool "nuget:?package=OpenCover"
#tool "nuget:?package=ReportGenerator"
#tool "nuget:?package=JetBrains.ReSharper.CommandLineTools"

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

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
		.SetConfiguration(configuration)
		.UseToolVersion(MSBuildToolVersion.VS2015)
		.SetMSBuildPlatform(MSBuildPlatform.x86)
		.SetPlatformTarget(PlatformTarget.MSIL)
	);
});

Task("test-it").Does(() => {
	XUnit2("../**/bin/" + configuration + "/*.Tests.dll", new XUnit2Settings() {
		ToolPath = "./tools/xunit.runner.console/tools/xunit.console.x86.exe",
		ShadowCopy = false
	});
});

Task("cover-it").Does(() => {
	OpenCover(tool => {
		tool.XUnit2("../**/bin/" + configuration + "/*.Tests.dll", new XUnit2Settings() {
			ToolPath = "./tools/xunit.runner.console/tools/xunit.console.x86.exe",
			ShadowCopy = false
		});
	},
	new FilePath("./result.xml"),
	new OpenCoverSettings()
		.WithFilter("+[EasyFarm]*")
		.WithFilter("+[MemoryAPI]*")
		.WithFilter("-[EasyFarm.Tests]*"));
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
