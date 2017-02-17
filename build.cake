#tool "nuget:?package=GitVersion.CommandLine"
#tool "nuget:?package=gitlink"

var sln = new FilePath("MvvmCross_All.sln");
var outputDir = new DirectoryPath("artifacts");
var nuspecDir = new DirectoryPath("nuspec");
var target = Argument("target", "Default");

var local = BuildSystem.IsLocalBuild;
var isDevelopBranch = StringComparer.OrdinalIgnoreCase.Equals("develop", AppVeyor.Environment.Repository.Branch);
var isReleaseBranch = StringComparer.OrdinalIgnoreCase.Equals("master", AppVeyor.Environment.Repository.Branch);
var isTagged = AppVeyor.Environment.Repository.Tag.IsTag;

var isRunningOnAppVeyor = AppVeyor.IsRunningOnAppVeyor;
var isPullRequest = AppVeyor.Environment.PullRequest.IsPullRequest;
var isRepository = StringComparer.OrdinalIgnoreCase.Equals("mvvmcross/mvvmcross", AppVeyor.Environment.Repository.Name);

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

Task("UpdateAppVeyorBuildNumber")
	.IsDependentOn("Version")
    .WithCriteria(() => isRunningOnAppVeyor)
    .Does(() =>
{
    AppVeyor.UpdateBuildVersion(versionInfo.FullBuildMetaData);
});

Task("Restore").Does(() => {
	NuGetRestore(sln);
});

Task("Build")
	.IsDependentOn("Clean")
	.IsDependentOn("UpdateAppVeyorBuildNumber")
	.IsDependentOn("Restore")
	.Does(() =>  {
	
	DotNetBuild(sln, 
		settings => settings.SetConfiguration("Release")
							.WithProperty("DebugSymbols", "true")
            				.WithProperty("DebugType", "Full")
							.WithTarget("Build"));
});

Task("GitLink")
	.IsDependentOn("Build")
	//pdbstr.exe and costura are not xplat currently
	.WithCriteria(() => IsRunningOnWindows())
	.Does(() => 
{
	GitLink(sln.GetDirectory(), 
		new GitLinkSettings {
			RepositoryUrl = "https://github.com/mvvmcross/mvvmcross",
			ArgumentCustomization = args => args.Append("-ignore MasterDetailExample.Core,MasterDetailExample.Droid,MasterDetailExample.iOS,MasterDetailExample.UWP,PageRendererExample.Core,PageRendererExample.Droid,PageRendererExample.iOS,PageRendererExample.WindowsUWP,MvvmCross.iOS.Support.ExpandableTableView.Core,MvvmCross.iOS.Support.ExpandableTableView.iOS,MvvmCross.iOS.Support.JASidePanelsSample.Core,MvvmCross.iOS.Support.JASidePanelsSample.iOS,MvvmCross.iOS.Support.Tabs.Core,MvvmCross.iOS.Support.Tabs.iOS,MvvmCross.iOS.Support.XamarinSidebarSample.Core,MvvmCross.iOS.Support.XamarinSidebarSample.iOS,mvvmcross.codeanalysis.vsix,example,example.android,example.ios,example.windowsphone,example.core,example.droid,Example.W81,Eventhooks.Core,Eventhooks.Droid,Eventhooks.iOS,Eventhooks.Uwp,Eventhooks.Wpf,RoutingExample.Core,RoutingExample.iOS,RoutingExample.Droid,MvvmCross.TestProjects.CustomBinding.Core,MvvmCross.TestProjects.CustomBinding.iOS,MvvmCross.TestProjects.CustomBinding.Droid")
		});
});

Task("Package")
	.IsDependentOn("GitLink")
	.Does(() => 
{
	var nugetSettings = new NuGetPackSettings {
		Authors = new [] { "MvvmCross contributors" },
		Owners = new [] { "MvvmCross" },
		IconUrl = new Uri("http://i.imgur.com/BvdAtgT.png"),
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

	EnsureDirectoryExists(outputDir);

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
		"MvvmCross.iOS.Support.JASidePanels.nuspec",
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
		"MvvmCross.Plugin.ThreadUtils.nuspec",
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
    .WithCriteria(() => !local)
    .WithCriteria(() => !isPullRequest)
    .WithCriteria(() => isRepository)
    .WithCriteria(() => isDevelopBranch || isReleaseBranch)
    .Does (() =>
{
	if (isReleaseBranch && !isTagged)
    {
        Information("Packages will not be published as this release has not been tagged.");
        return;
    }

	// Resolve the API key.
    var apiKey = EnvironmentVariable("NUGET_APIKEY");
    if (string.IsNullOrEmpty(apiKey))
    {
        throw new Exception("The NUGET_APIKEY environment variable is not defined.");
    }

    var source = EnvironmentVariable("NUGET_SOURCE");
    if (string.IsNullOrEmpty(source))
    {
        throw new Exception("The NUGET_SOURCE environment variable is not defined.");
    }

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
	.Does(() => {
	
	});

RunTarget(target);
