#tool "nuget:?package=xunit.runner.console"
#tool "nuget:?package=OpenCover"
#tool "nuget:?package=ReportGenerator"

var target = Argument("target", "default");

Task("default")
	.IsDependentOn("build")
	.IsDependentOn("cover")
	.IsDependentOn("report");

Task("build").Does(() => {
	MSBuild("../EasyFarm.sln");
});

Task("test").Does(() => {
	XUnit2("../**/bin/Debug/*.Tests.dll", new XUnit2Settings() {
		ToolPath = "./tools/xunit.runner.console/tools/xunit.console.x86.exe",
		ShadowCopy = false
	});
});

Task("cover").Does(() => {
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

Task("report").Does(() => {
	ReportGenerator("./result.xml", "./coverage");
});

RunTarget(target);
