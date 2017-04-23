#tool nuget:?package=GitVersion.CommandLine
#tool nuget:?package=gitlink
#tool nuget:?package=vswhere
#addin "Cake.Incubator"

var sln = new FilePath("MvvmCross_All.sln");
var outputDir = new DirectoryPath("artifacts");
var nuspecDir = new DirectoryPath("nuspec");
var target = Argument("target", "Default");

var isRunningOnAppVeyor = AppVeyor.IsRunningOnAppVeyor;
var isRunningOnVSTS = TFBuild.IsRunningOnVSTS;

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
	Information("GitVersion -> {0}", versionInfo.Dump());
});

Task("UpdateAppVeyorBuildNumber")
	.IsDependentOn("Version")
    .WithCriteria(() => isRunningOnAppVeyor)
    .Does(() =>
{
    AppVeyor.UpdateBuildVersion(versionInfo.FullBuildMetaData);
});

FilePath msBuildPath;
Task("ResolveBuildTools")
	.Does(() => 
{
	var vsLatest = VSWhereLatest();
	msBuildPath = (vsLatest == null)
		? null
		: vsLatest.CombineWithFilePath("./MSBuild/15.0/Bin/MSBuild.exe");
});

Task("Restore")
	.IsDependentOn("ResolveBuildTools")
	.Does(() => {
	NuGetRestore(sln, new NuGetRestoreSettings {
		ToolPath = "tools/nuget.exe"
	});
	// MSBuild(sln, settings => settings.WithTarget("Restore"));
});

Task("Build")
	.IsDependentOn("ResolveBuildTools")
	.IsDependentOn("Clean")
	.IsDependentOn("UpdateAppVeyorBuildNumber")
	.IsDependentOn("Restore")
	.Does(() =>  {

	var settings = new MSBuildSettings 
	{
		Configuration = "Release",
		ToolPath = msBuildPath
	};

	settings.Properties.Add("DebugSymbols", new List<string> { "True" });
	settings.Properties.Add("DebugType", new List<string> { "Full" });

	MSBuild(sln, settings);
});

Task("GitLink")
	.IsDependentOn("Build")
	//pdbstr.exe and costura are not xplat currently
	.WithCriteria(() => IsRunningOnWindows())
	.Does(() => 
{
	var projectsToIgnore = new string[] {
		"MasterDetailExample.Core",
		"MasterDetailExample.Droid",
		"MasterDetailExample.iOS",
		"MasterDetailExample.UWP",
		"PageRendererExample.Core",
		"PageRendererExample.Droid",
		"PageRendererExample.iOS",
		"PageRendererExample.WindowsUWP",
		"MvvmCross.iOS.Support.ExpandableTableView.Core",
		"MvvmCross.iOS.Support.ExpandableTableView.iOS",
		"MvvmCross.iOS.Support.JASidePanelsSample.Core",
		"MvvmCross.iOS.Support.JASidePanelsSample.iOS",
		"MvvmCross.iOS.Support.Tabs.Core",
		"MvvmCross.iOS.Support.Tabs.iOS",
		"MvvmCross.iOS.Support.XamarinSidebarSample.Core",
		"MvvmCross.iOS.Support.XamarinSidebarSample.iOS",
		"mvvmcross.codeanalysis.vsix",
		"example",
		"example.android",
		"example.ios",
		"example.windowsphone",
		"example.core",
		"example.droid",
		"Example.W81",
		"Eventhooks.Core",
		"Eventhooks.Droid",
		"Eventhooks.iOS",
		"Eventhooks.Uwp",
		"Eventhooks.Wpf",
		"RoutingExample.Core",
		"RoutingExample.iOS",
		"RoutingExample.Droid",
		"MvvmCross.TestProjects.CustomBinding.Core",
		"MvvmCross.TestProjects.CustomBinding.iOS",
		"MvvmCross.TestProjects.CustomBinding.Droid",
		"playground",
		"playground.ios"
	};

	GitLink(sln.GetDirectory(), 
		new GitLinkSettings {
			RepositoryUrl = "https://github.com/mvvmcross/mvvmcross",
			ArgumentCustomization = args => args.Append("-ignore " + string.Join(",", projectsToIgnore))
		});
});

Task("Package")
	.IsDependentOn("GitLink")
	.Does(() => 
{
	var nugetSettings = new NuGetPackSettings {
		Authors = new [] { "MvvmCross contributors" },
		Owners = new [] { "MvvmCross" },
		IconUrl = new Uri("http://i.imgur.com/Baucn8c.png"),
		ProjectUrl = new Uri("https://github.com/MvvmCross/MvvmCross"),
		LicenseUrl = new Uri("https://raw.githubusercontent.com/MvvmCross/MvvmCross/develop/LICENSE"),
		Copyright = "Copyright (c) MvvmCross",
		RequireLicenseAcceptance = false,
		Version = versionInfo.NuGetVersion,
		Symbols = false,
		NoPackageAnalysis = true,
		OutputDirectory = outputDir,
		Verbosity = NuGetVerbosity.Detailed,
		BasePath = "./nuspec"
	};

	var nuspecs = new List<string> {
		"MvvmCross.nuspec",
		"MvvmCross.Binding.nuspec",
		"MvvmCross.CodeAnalysis.nuspec",
		"MvvmCross.Console.Platform.nuspec",
		"MvvmCross.Core.nuspec",
		"MvvmCross.Droid.FullFragging.nuspec",
		"MvvmCross.Droid.Shared.nuspec",
		"MvvmCross.Droid.Support.Core.UI.nuspec",
		"MvvmCross.Droid.Support.Core.Utils.nuspec",
		"MvvmCross.Droid.Support.Design.nuspec",
		"MvvmCross.Droid.Support.Fragment.nuspec",
		"MvvmCross.Droid.Support.V4.nuspec",
		"MvvmCross.Droid.Support.V7.AppCompat.nuspec",
		"MvvmCross.Droid.Support.V7.Fragging.nuspec",
		"MvvmCross.Droid.Support.V7.Preference.nuspec",
		"MvvmCross.Droid.Support.V7.RecyclerView.nuspec",
		"MvvmCross.Droid.Support.V14.Preference.nuspec",
		"MvvmCross.Droid.Support.V17.Leanback.nuspec",
		"MvvmCross.Forms.Presenter.nuspec",
		"MvvmCross.iOS.Support.nuspec",
		"MvvmCross.iOS.Support.XamarinSidebar.nuspec",
		"MvvmCross.Platform.nuspec",
		"MvvmCross.Plugin.Accelerometer.nuspec",
		"MvvmCross.Plugin.All.nuspec",
		"MvvmCross.Plugin.Color.nuspec",
		"MvvmCross.Plugin.DownloadCache.nuspec",
		"MvvmCross.Plugin.Email.nuspec",
		"MvvmCross.Plugin.FieldBinding.nuspec",
		"MvvmCross.Plugin.File.nuspec",
		"MvvmCross.Plugin.Json.nuspec",
		"MvvmCross.Plugin.JsonLocalization.nuspec",
		"MvvmCross.Plugin.Location.nuspec",
		"MvvmCross.Plugin.Location.Fused.nuspec",
		"MvvmCross.Plugin.Messenger.nuspec",
		"MvvmCross.Plugin.MethodBinding.nuspec",
		"MvvmCross.Plugin.Network.nuspec",
		"MvvmCross.Plugin.PhoneCall.nuspec",
		"MvvmCross.Plugin.PictureChooser.nuspec",
		"MvvmCross.Plugin.ResourceLoader.nuspec",
		"MvvmCross.Plugin.ResxLocalization.nuspec",
		"MvvmCross.Plugin.Share.nuspec",
		"MvvmCross.Plugin.Visibility.nuspec",
		"MvvmCross.Plugin.WebBrowser.nuspec",
		"MvvmCross.StarterPack.nuspec",
		"MvvmCross.Tests.nuspec"
	};

	foreach(var nuspec in nuspecs)
	{
		NuGetPack(nuspecDir + "/" + nuspec, nugetSettings);
	}
});

Task("PublishPackages")
    .IsDependentOn("Package")
    .WithCriteria(() => !BuildSystem.IsLocalBuild)
    .WithCriteria(() => IsRepository("mvvmcross/mvvmcross"))
    .WithCriteria(() => versionInfo.BranchName == "master" || versionInfo.BranchName == "develop")
    .Does (() =>
{
	if (versionInfo.BranchName == "master" && !IsTagged())
    {
        Information("Packages will not be published as this release has not been tagged.");
        return;
    }

	// Resolve the API key.
	var nugetKeySource = GetNugetKeyAndSource();
	var apiKey = nugetKeySource.Item1;
	var source = nugetKeySource.Item2;

	var nugetFiles = GetFiles(outputDir + "/*.nupkg");

	foreach(var nugetFile in nugetFiles)
	{
    	NuGetPush(nugetFile, new NuGetPushSettings {
            Source = source,
            ApiKey = apiKey
        });
	}
});

Task("Default")
	.IsDependentOn("PublishPackages")
	.Does(() => 
{ 
	Information("Is Local Build: {0}", BuildSystem.IsLocalBuild);
});

RunTarget(target);

bool IsRepository(string repoName)
{
	var buildEnvRepoName = string.Empty;

	if (isRunningOnAppVeyor)
		buildEnvRepoName = AppVeyor.Environment.Repository.Name;

	Information("Checking repo name: {0} against build repo name: {1}", repoName, buildEnvRepoName);

	// repo name on VSTS is empty :(
	if (isRunningOnVSTS) return true;

	return StringComparer.OrdinalIgnoreCase.Equals(repoName, buildEnvRepoName);
}

bool IsTagged()
{
	var toReturn = false;
	int commitsSinceTag = -1;
	if (int.TryParse(versionInfo.CommitsSinceVersionSource, out commitsSinceTag))
	{
		// if tag is current commit this will be 0
		if (commitsSinceTag == 0)
			toReturn = true;
	}

	Information("Commits since tag {0}, is tagged: {1}", commitsSinceTag, toReturn);

	return toReturn;
}

Tuple<string, string> GetNugetKeyAndSource()
{
	var apiKeyKey = string.Empty;
	var sourceKey = string.Empty;
	if (isRunningOnAppVeyor)
	{
		apiKeyKey = "NUGET_APIKEY";
		sourceKey = "NUGET_SOURCE";
	}
	else if (isRunningOnVSTS)
	{
		if (versionInfo.BranchName == "develop")
		{
			apiKeyKey = "NUGET_APIKEY_DEVELOP";
			sourceKey = "NUGET_SOURCE_DEVELOP";
		}
		else if (versionInfo.BranchName == "master")
		{
			apiKeyKey = "NUGET_APIKEY_MASTER";
			sourceKey = "NUGET_SOURCE_MASTER";
		}
	}

	var apiKey = EnvironmentVariable(apiKeyKey);
	if (string.IsNullOrEmpty(apiKey))
		throw new Exception(string.Format("The {0} environment variable is not defined.", apiKeyKey));

	var source = EnvironmentVariable(sourceKey);
	if (string.IsNullOrEmpty(source))
		throw new Exception(string.Format("The {0} environment variable is not defined.", sourceKey));

	return Tuple.Create(apiKey, source);
}