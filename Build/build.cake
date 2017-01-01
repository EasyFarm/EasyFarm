#tool "nuget:?package=xunit.runner.console"
#tool "nuget:?package=OpenCover"
#tool "nuget:?package=ReportGenerator"

var target = Argument("target", "Default");

Task("Default")
	.IsDependentOn("test");

Task("test")
	.IsDependentOn("build-it")
	.IsDependentOn("test-it");

Task("cover")
	.IsDependentOn("build-it")
	.IsDependentOn("cover-it")
	.IsDependentOn("report-it");

Task("build-it").Does(() => {
	MSBuild("../EasyFarm.sln");
});

Task("test-it").Does(() => {
	XUnit2("../**/bin/Debug/*.Tests.dll", new XUnit2Settings() {
		ToolPath = "./tools/xunit.runner.console/tools/xunit.console.x86.exe",
		ShadowCopy = false
	});
});

Task("cover-it").Does(() => {
	OpenCover(tool => {
		tool.XUnit2("../**/bin/Debug/*.Tests.dll", new XUnit2Settings() {
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

RunTarget(target);
