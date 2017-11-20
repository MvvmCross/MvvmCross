#tool nuget:?package=GitVersion.CommandLine
#tool nuget:?package=vswhere
#tool nuget:?package=NUnit.ConsoleRunner
#addin nuget:?package=Cake.Incubator&version=1.5.0
#addin nuget:?package=Cake.Git&version=0.16.0
#addin nuget:?package=Cake.Figlet

var sln = new FilePath("./NetStandard/MvvmCross.sln");
var outputDir = new DirectoryPath("artifacts");
var nuspecDir = new DirectoryPath("nuspec");
var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
GitVersion versionInfo = null;

var isRunningOnAppVeyor = AppVeyor.IsRunningOnAppVeyor;

Setup(context => 
{
    Information(Figlet("MvvmCross"));

    versionInfo = GitVersion(new GitVersionSettings {
        UpdateAssemblyInfo = true,
        OutputType = GitVersionOutput.Json
    });

    var cakeVersion = typeof(ICakeContext).Assembly.GetName().Version.ToString();

    Information("Building version {0}, ({1}, {2}) using version {3} of Cake.",
        versionInfo.SemVer,
        configuration,
        target,
        cakeVersion);
});

Task("Clean").Does(() =>
{
    CleanDirectories("./**/bin");
    CleanDirectories("./**/obj");
    CleanDirectories(outputDir.FullPath);

    EnsureDirectoryExists(outputDir);
});

Task("UpdateAppVeyorBuildNumber")
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
    MSBuild(sln, settings => settings.WithTarget("Restore")
        .SetVerbosity(Verbosity.Diagnostic));
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
        Verbosity = Verbosity.Diagnostic,
        ArgumentCustomization = args => args.Append("/m")
    };

    MSBuild(sln, settings);
});

Task("UnitTest")
    .IsDependentOn("Build")
    .Does(() =>
{
    var testPaths = new List<string> {
        new FilePath("./NetStandard/MvvmCross.Tests/bin/" + configuration + "/netcoreapp2.0/MvvmCross.Tests.dll").FullPath,
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

    foreach(var nugetFile in nugetFiles)
    {
        NuGetPush(nugetFile, new NuGetPushSettings {
            Source = source,
            ApiKey = apiKey
        });
    }
});

Task("UploadAppVeyorArtifact")
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
