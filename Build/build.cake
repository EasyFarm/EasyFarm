#tool "nuget:?package=xunit.runner.console"

var target = Argument("target", "Default");

Task("Default")
	.IsDependentOn("build")
	.IsDependentOn("test");

Task("build")
	.Does(() => 
{
	MSBuild("../EasyFarm.sln");
});

Task("test")
	.Does(() => 
{
	XUnit2("../EasyFarm.Tests/bin/Debug/EasyFarm.Tests.dll", new XUnit2Settings(){
		ToolPath = "./tools/xunit.runner.console/tools/xunit.console.x86.exe"
	});
});

RunTarget(target);
