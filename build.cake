#tool nuget:?package=GitVersion.CommandLine
#tool nuget:?package=gitlink&version=2.4.0
#tool nuget:?package=vswhere
#tool nuget:?package=NUnit.ConsoleRunner
#addin nuget:?package=Cake.Incubator&version=1.6.0
#addin nuget:?package=Cake.Git&version=0.16.0
#addin nuget:?package=Polly

using Polly;

var sln = new FilePath("./NetStandard/MvvmCross.sln");
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

MSBuildSettings GetMSBuildSettings()
{
    var settings = new MSBuildSettings 
    {
        Configuration = "Release",
        ToolPath = msBuildPath,
        ArgumentCustomization = args => args.Append("/m")
    };

    settings = settings.WithProperty("DebugSymbols", "True")
        .WithProperty("DebugType", "Embedded");

    if (versionInfo != null)
    {
        settings =
            settings.WithProperty("PackageVersion", versionInfo.NuGetVersion)
            .WithProperty("AssemblyInformationalVersion", versionInfo.InformationalVersion)
            .WithProperty("AssemblyVersion", versionInfo.AssemblySemVer)
            .WithProperty("AssemblyFileVersion", versionInfo.Major + versionInfo.Minor + "0.0");
    }

    return settings;
}

Task("Restore")
    .IsDependentOn("ResolveBuildTools")
    .Does(() => {
    var settings = GetMSBuildSettings().WithTarget("Restore");
    MSBuild(sln, settings);
});

Task("Build")
    .IsDependentOn("ResolveBuildTools")
    .IsDependentOn("Clean")
    .IsDependentOn("UpdateAppVeyorBuildNumber")
    .IsDependentOn("Restore")
    .Does(() =>  
{
    MSBuild(sln, GetMSBuildSettings());
});

Task("UnitTest")
    .IsDependentOn("Build")
    .Does(() =>
{
    var testPaths = new List<string> {
        new FilePath("./NetStandard/MvvmCross.Tests/MvvmCross.Tests/bin/Release/netcoreapp2.0/MvvmCross.Tests.dll").FullPath,
        new FilePath("./NetStandard/Plugins.Color.Test/bin/Release/netcoreapp2.0/MvvmCross.Plugins.Color.Tests.dll").FullPath,
        new FilePath("./NetStandard/Plugins.JsonLocalization.Tests/bin/Release/netcoreapp2.0/MvvmCross.Plugins.JsonLocalization.Tests.dll").FullPath,
        new FilePath("./NetStandard/Plugins.Messenger.Test/bin/Release/netcoreapp2.0/MvvmCross.Plugins.Messenger.Tests.dll").FullPath,
        new FilePath("./NetStandard/Plugins.Network.Test/bin/Release/netcoreapp2.0/MvvmCross.Plugins.Network.Tests.dll").FullPath,
        new FilePath("./NetStandard/Plugins.ResourceLoader.Test/bin/Release/netcoreapp2.0/MvvmCross.Plugins.ResourceLoader.Tests.dll").FullPath,
        new FilePath("./NetStandard/Plugins.ResxLocalization.Tests/bin/Release/netcoreapp2.0/MvvmCross.Plugins.ResxLocalization.Tests.dll").FullPath,
    };

    NUnit3(testPaths, new NUnit3Settings {
        Timeout = 30000,
        OutputFile = new FilePath(outputDir + "/NUnitOutput.txt"),
        Work = outputDir
    });

    if (isRunningOnAppVeyor)
    {
        var testResults = GetFiles(outputDir.ToString() + "/*.xml");
        foreach(var testResult in testResults)
            AppVeyor.UploadTestResults(testResult, AppVeyorTestResultsType.NUnit3);
    }
});

Task("PublishPackages")
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

    var policy = Policy
  		.Handle<Exception>()
  		.WaitAndRetry(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(1.5, retryAttempt)));

    foreach(var nugetFile in nugetFiles)
    {
        policy.Execute(() =>
            NuGetPush(nugetFile, new NuGetPushSettings {
                Source = source,
                ApiKey = apiKey
            })
        );
    }
});

Task("UploadAppVeyorArtifact")
    .WithCriteria(() => !AppVeyor.Environment.PullRequest.IsPullRequest)
    .WithCriteria(() => isRunningOnAppVeyor)
    .Does(() => {

    Information("Artifacts Dir: {0}", outputDir.FullPath);

    var uploadSettings = new AppVeyorUploadArtifactsSettings();

    foreach(var file in GetFiles(outputDir.FullPath + "/*")) {
        Information("Uploading {0}", file.FullPath);

        if (file.GetExtension() == "nupkg")
            uploadSettings.ArtifactType = AppVeyorUploadArtifactType.NuGetPackage;
        else
            uploadSettings.ArtifactType = AppVeyorUploadArtifactType.Auto;

        AppVeyor.UploadArtifact(file.FullPath, uploadSettings);
    }
});

Task("Default")
    .IsDependentOn("Clean")
    .IsDependentOn("Version")
    .IsDependentOn("Restore")
    .IsDependentOn("Build")
    .IsDependentOn("UnitTest")
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
