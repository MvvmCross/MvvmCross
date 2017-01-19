#tool "nuget:?package=GitVersion.CommandLine"
#tool "nuget:?package=gitlink"

var sln = new FilePath("MvvmCross_All.sln");
var outputDir = new DirectoryPath("artifacts");
var target = Argument("target", "Default");

Task("Clean").Does(() =>
{
    CleanDirectories("./**/bin");
    CleanDirectories("./**/obj");
	CleanDirectories(outputDir.FullPath);
});

GitVersion versionInfo = null;
Task("Version").Does(() => {
	GitVersion(new GitVersionSettings {
		UpdateAssemblyInfo = true,
		OutputType = GitVersionOutput.BuildServer
	});

	versionInfo = GitVersion(new GitVersionSettings{ OutputType = GitVersionOutput.Json });
	Information("VI:\t{0}", versionInfo.FullSemVer);
});

Task("Restore").Does(() => {
	NuGetRestore(sln);
});

Task("Build")
	.IsDependentOn("Clean")
	.IsDependentOn("Version")
	.IsDependentOn("Restore")
	.Does(() =>  {
	
	DotNetBuild(sln, 
		settings => settings.SetConfiguration("Release")
							.WithProperty("DebugSymbols", "true")
            				.WithProperty("DebugType", "Full")
							.WithTarget("Build")
	);
});

Task("Package")
	.IsDependentOn("Build")
	.Does(() => {
	// if (IsRunningOnWindows()) //pdbstr.exe and costura are not xplat currently
	// 	GitLink(sln.GetDirectory(), new GitLinkSettings {
	// 		ArgumentCustomization = args => args.Append("-ignore Sample")
	// 	});
});

Task("Default")
	.IsDependentOn("Package")
	.Does(() => {
	
	});

RunTarget(target);
