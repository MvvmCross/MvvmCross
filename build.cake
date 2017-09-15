#tool nuget:?package=GitVersion.CommandLine
#tool nuget:?package=gitlink&version=2.4.0
#tool nuget:?package=vswhere
#tool nuget:?package=NUnit.ConsoleRunner
#addin nuget:?package=Cake.Incubator
#addin nuget:?package=Cake.Git

var sln = new FilePath("MvvmCross_All.sln");
var outputDir = new DirectoryPath("artifacts");
var nuspecDir = new DirectoryPath("nuspec");
var target = Argument("target", "Default");

var isRunningOnAppVeyor = AppVeyor.IsRunningOnAppVeyor;

Task("Clean").Does(() =>
{
    CleanDirectories("./**/bin");
    CleanDirectories("./**/obj");
	CleanDirectories(outputDir.FullPath);

	EnsureDirectoryExists(outputDir);
});

GitVersion versionInfo = null;
Task("Version").Does(() => {
	versionInfo = GitVersion(new GitVersionSettings {
		UpdateAssemblyInfo = true,
		OutputType = GitVersionOutput.Json
	});

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
		ToolPath = "tools/nuget.exe",
		Verbosity = NuGetVerbosity.Quiet
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
		ToolPath = msBuildPath,
		Verbosity = Verbosity.Minimal
	};

	settings.Properties.Add("DebugSymbols", new List<string> { "True" });
	settings.Properties.Add("DebugType", new List<string> { "Full" });

	MSBuild(sln, settings);
});

Task("UnitTest")
	.IsDependentOn("Build")
	.Does(() =>
{
	var testPaths = new List<string> {
		new FilePath("./MvvmCross/Test/Test/bin/Release/MvvmCross.Test.dll").FullPath,
		new FilePath("./MvvmCross/Binding/Test/bin/Release/MvvmCross.Binding.Test.dll").FullPath,
		new FilePath("./MvvmCross/Platform/Test/bin/Release/MvvmCross.Platform.Test.dll").FullPath,
		new FilePath("./MvvmCross-Plugins/Color/MvvmCross.Plugins.Color.Test/bin/Release/MvvmCross.Plugins.Color.Test.dll").FullPath,
		new FilePath("./MvvmCross-Plugins/Messenger/MvvmCross.Plugins.Messenger.Test/bin/Release/MvvmCross.Plugins.Messenger.Test.dll").FullPath,
		new FilePath("./MvvmCross-Plugins/Network/MvvmCross.Plugins.Network.Test/bin/Release/MvvmCross.Plugins.Network.Test.dll").FullPath,
        new FilePath("./MvvmCross-Plugins/JsonLocalization/MvvmCross.Plugins.JsonLocalization.Tests/bin/Release/MvvmCross.Plugins.JsonLocalization.Tests.dll").FullPath,
        new FilePath("./MvvmCross-Plugins/ResxLocalization/MvvmCross.Plugins.ResxLocalization.Tests/bin/Release/MvvmCross.Plugins.ResxLocalization.Tests.dll").FullPath
	};

    var testResultsPath = new FilePath(outputDir + "/NUnitTestResult.xml");

	NUnit3(testPaths, new NUnit3Settings {
		Timeout = 30000,
		OutputFile = new FilePath(outputDir + "/NUnitOutput.txt"),
		Results = testResultsPath
	});

    if (isRunningOnAppVeyor)
    {
        AppVeyor.UploadTestResults(testResultsPath, AppVeyorTestResultsType.NUnit3);
    }
});

Task("GitLink")
	.IsDependentOn("UnitTest")
	//pdbstr.exe and costura are not xplat currently
	.WithCriteria(() => IsRunningOnWindows())
	.WithCriteria(() => 
		StringComparer.OrdinalIgnoreCase.Equals(versionInfo.BranchName, "develop") || 
		IsMasterOrReleases())
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
		"playground.core",
		"playground.ios",
        "playground.mac",
		"playground.wpf",
        "Playground.Droid",
        "Playground.Uwp",
        "Playground.Forms.Droid",
        "Playground.Forms.iOS",
		"MvxBindingsExample",
		"MvxBindingsExample.Android",
		"MvxBindingsExample.iOS",
		"MvxBindingsExample.UWP"
	};

	GitLink("./", 
		new GitLinkSettings {
			RepositoryUrl = "https://github.com/mvvmcross/mvvmcross",
			Configuration = "Release",
			SolutionFileName = "MvvmCross_All.sln",
			ArgumentCustomization = args => args.Append("-ignore " + string.Join(",", projectsToIgnore))
		});
});

Task("Package")
	.IsDependentOn("GitLink")
	.WithCriteria(() => 
		StringComparer.OrdinalIgnoreCase.Equals(versionInfo.BranchName, "develop") || 
		IsMasterOrReleases())
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
		"MvvmCross.Droid.Support.Core.UI.nuspec",
		"MvvmCross.Droid.Support.Core.Utils.nuspec",
		"MvvmCross.Droid.Support.Design.nuspec",
		"MvvmCross.Droid.Support.Fragment.nuspec",
		"MvvmCross.Droid.Support.V7.AppCompat.nuspec",
		"MvvmCross.Droid.Support.V7.Preference.nuspec",
		"MvvmCross.Droid.Support.V7.RecyclerView.nuspec",
		"MvvmCross.Droid.Support.V14.Preference.nuspec",
		"MvvmCross.Droid.Support.V17.Leanback.nuspec",
		"MvvmCross.Forms.nuspec",
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
		"MvvmCross.Forms.StarterPack.nuspec",
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
    .WithCriteria(() => 
		StringComparer.OrdinalIgnoreCase.Equals(versionInfo.BranchName, "develop") || 
		IsMasterOrReleases())
    .Does (() =>
{
	if (StringComparer.OrdinalIgnoreCase.Equals(versionInfo.BranchName, "master") && !IsTagged())
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

Task("UploadAppVeyorArtifact")
	.IsDependentOn("Package")
	.WithCriteria(() => !AppVeyor.Environment.PullRequest.IsPullRequest)
	.WithCriteria(() => isRunningOnAppVeyor)
	.Does(() => {

	Information("Artifacts Dir: {0}", outputDir.FullPath);

	foreach(var file in GetFiles(outputDir.FullPath + "/*")) {
		Information("Uploading {0}", file.FullPath);
		AppVeyor.UploadArtifact(file.FullPath);
	}
});

Task("Default")
	.IsDependentOn("PublishPackages")
	.IsDependentOn("UploadAppVeyorArtifact")
	.Does(() => 
{
});

RunTarget(target);

bool IsMasterOrReleases()
{
	if (StringComparer.OrdinalIgnoreCase.Equals(versionInfo.BranchName, "master"))
		return true;

	if (versionInfo.BranchName.Contains("releases/"))
		return true;

	return false;
}

bool IsRepository(string repoName)
{
	if (isRunningOnAppVeyor)
	{
		var buildEnvRepoName = AppVeyor.Environment.Repository.Name;
		Information("Checking repo name: {0} against build repo name: {1}", repoName, buildEnvRepoName);
		return StringComparer.OrdinalIgnoreCase.Equals(repoName, buildEnvRepoName);
	}
	else
	{
		try
		{
			var path = MakeAbsolute(sln).GetDirectory().FullPath;
			using (var repo = new LibGit2Sharp.Repository(path))
			{
				var origin = repo.Network.Remotes.FirstOrDefault(
					r => r.Name.ToLowerInvariant() == "origin");
				return origin.Url.ToLowerInvariant() == 
					"https://github.com/" + repoName.ToLowerInvariant();
			}
		}
		catch(Exception ex)
		{
			Information("Failed to lookup repository: {0}", ex);
			return false;
		}
	}
}

bool IsTagged()
{
	var path = MakeAbsolute(sln).GetDirectory().FullPath;
	using (var repo = new LibGit2Sharp.Repository(path))
	{
		var head = repo.Head;
		var headSha = head.Tip.Sha;
		
		var tag = repo.Tags.FirstOrDefault(t => t.Target.Sha == headSha);
		if (tag == null)
		{
			Information("HEAD is not tagged");
			return false;
		}

		Information("HEAD is tagged: {0}", tag.FriendlyName);
		return true;
	}
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
	else
	{
		if (StringComparer.OrdinalIgnoreCase.Equals(versionInfo.BranchName, "develop"))
		{
			apiKeyKey = "NUGET_APIKEY_DEVELOP";
			sourceKey = "NUGET_SOURCE_DEVELOP";
		}
		else if (IsMasterOrReleases())
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
