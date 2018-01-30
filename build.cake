#tool nuget:?package=GitVersion.CommandLine
#tool nuget:?package=vswhere
#tool nuget:?package=xunit.runner.console
#addin nuget:?package=Cake.Incubator&version=1.6.0
#addin nuget:?package=Cake.Git&version=0.16.0
#addin nuget:?package=Polly

using Polly;

var sln = new FilePath("MvvmCross.sln");
var outputDir = new DirectoryPath("artifacts");
var nuspecDir = new DirectoryPath("nuspec");
var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

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
    AppVeyor.UpdateBuildVersion(versionInfo.InformationalVersion);
});

FilePath msBuildPath;
Task("ResolveBuildTools")
    .WithCriteria(() => IsRunningOnWindows())
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
    MSBuild(sln, settings => settings.WithTarget("Restore"));
});

Task("Build")
    .IsDependentOn("ResolveBuildTools")
    .IsDependentOn("Clean")
    .IsDependentOn("UpdateAppVeyorBuildNumber")
    .IsDependentOn("Restore")
    .Does(() =>  {

    var settings = new MSBuildSettings 
    {
        Configuration = configuration,
        ToolPath = msBuildPath,
        Verbosity = Verbosity.Minimal,
        ArgumentCustomization = args => args.Append("/m")
    };

    settings = settings
        .WithProperty("DebugSymbols", "True")
        .WithProperty("DebugType", "Embedded")
        .WithProperty("PackageVersion", versionInfo.SemVer)
        .WithProperty("NoPackageAnalysis", "True");

    MSBuild(sln, settings);
});

Task("UnitTest")
    .IsDependentOn("Build")
    .Does(() =>
{
    var testPaths = GetFiles("./MvvmCross.Tests/*.UnitTest/bin/Release/netcoreapp2.0/*.UnitTest.dll");

    var testResultsPath = new DirectoryPath(outputDir + "/Tests/");
    
    var testSettings = new XUnit2Settings
    {
        XmlReport = true,
        OutputDirectory = testResultsPath
    };

    XUnit2(testPaths, testSettings);

    if (isRunningOnAppVeyor)
    {
        foreach(var testResult in GetFiles(outputDir + "/Tests/*.xml"))
            AppVeyor.UploadTestResults(testResult, AppVeyorTestResultsType.XUnit);
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

    var nugetFiles = GetFiles("MvvmCross*/**/bin/" + configuration + "/**/*.nupkg");

    var policy = Policy
  		.Handle<Exception>()
        .WaitAndRetry(5, retryAttempt => 
            TimeSpan.FromSeconds(Math.Pow(1.5, retryAttempt)));

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
    .WithCriteria(() => isRunningOnAppVeyor)
    .Does(() => {

    Information("Artifacts Dir: {0}", outputDir.FullPath);

    var uploadSettings = new AppVeyorUploadArtifactsSettings();

    var artifacts = GetFiles("MvvmCross*/**/bin/" + configuration + "/**/*.nupkg")
        + GetFiles(outputDir.FullPath + "/**/*");

    foreach(var file in artifacts) {
        Information("Uploading {0}", file.FullPath);

        if (file.GetExtension() == "nupkg")
            uploadSettings.ArtifactType = AppVeyorUploadArtifactType.NuGetPackage;
        else
            uploadSettings.ArtifactType = AppVeyorUploadArtifactType.Auto;

        AppVeyor.UploadArtifact(file.FullPath, uploadSettings);
    }
});

Task("Default")
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
