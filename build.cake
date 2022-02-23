#tool dotnet:n?package=GitVersion.Tool&version=5.8.2
#tool nuget:?package=vswhere&version=2.9.3-g21bcdb639c
#tool nuget:?package=MSBuild.SonarQube.Runner.Tool&version=4.8.0
#addin nuget:?package=Cake.Figlet&version=2.0.1
#addin nuget:?package=Cake.Sonar&version=1.1.30

var solutionName = "MvvmCross";
var repoName = "mvvmcross/mvvmcross";
var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var verbosityArg = Argument("verbosity", "Minimal");
var artifactsDir = Argument("artifactsDir", "./artifacts");
var sln = new FilePath("./" + solutionName + ".sln");
var outputDir = new DirectoryPath(artifactsDir);
var gitVersionLog = new FilePath("./gitversion.log");
var nuspecDir = new DirectoryPath("./nuspec");
var verbosity = Verbosity.Minimal;
var sonarKey = Argument("sonarKey", "");

var githubToken = Argument("github_token", "");
var githubTokenEnv = EnvironmentVariable("CHANGELOG_GITHUB_TOKEN");
var sinceTag = Argument("since_tag", "");

var isRunningOnPipelines = AzurePipelines.IsRunningOnAzurePipelines;
GitVersion versionInfo = null;

Setup(context => 
{
    versionInfo = context.GitVersion(new GitVersionSettings 
    {
        UpdateAssemblyInfo = true,
        OutputType = GitVersionOutput.Json,
        LogFilePath = gitVersionLog.MakeAbsolute(context.Environment)
    });

    if (isRunningOnPipelines)
    {
        var buildNumber = versionInfo.InformationalVersion + "-" + AzurePipelines.Environment.Build.Number;
        buildNumber = buildNumber.Replace("/", "-");
        AzurePipelines.Commands.UpdateBuildNumber(buildNumber);
    }

    var cakeVersion = typeof(ICakeContext).Assembly.GetName().Version.ToString();

    Information(Figlet(solutionName));
    Information("Building version {0}, ({1}, {2}) using version {3} of Cake.",
        versionInfo.SemVer,
        configuration,
        target,
        cakeVersion);

    verbosity = (Verbosity) Enum.Parse(typeof(Verbosity), verbosityArg, true);
});

Task("Clean")
    .Does(() =>
{
    EnsureDirectoryExists(outputDir.FullPath);

    CleanDirectories("MvvmCross*/**/bin");
    CleanDirectories("MvvmCross*/**/obj");
    CleanDirectories(outputDir.FullPath);

    CopyFile(gitVersionLog, outputDir + "/gitversion.log");
});

FilePath msBuildPath;
Task("ResolveBuildTools")
    .WithCriteria(() => IsRunningOnWindows())
    .Does(() => 
{
    var vsWhereSettings = new VSWhereLatestSettings
    {
        IncludePrerelease = true,
        Requires = "Component.Xamarin"
    };
    
    var vsLatest = VSWhereLatest(vsWhereSettings);
    msBuildPath = (vsLatest == null)
        ? null
        : vsLatest.CombineWithFilePath("./MSBuild/Current/Bin/MSBuild.exe");

    if (msBuildPath != null)
        Information("Found MSBuild at {0}", msBuildPath.ToString());
});

Task("Restore")
    .IsDependentOn("ResolveBuildTools")
    .Does(() => 
{
    var settings = GetDefaultBuildSettings()
        .WithTarget("Restore");
    MSBuild(sln, settings);
});

Task("PatchBuildProps")
    .Does(() => 
{
    var buildProp = new FilePath("./Directory.Build.props");
    XmlPoke(buildProp, "//Project/PropertyGroup/Version", versionInfo.SemVer);
});

Task("SonarStart")
    .WithCriteria(() => !string.IsNullOrEmpty(sonarKey))
    .Does(() => 
{
    var settings = new SonarBeginSettings
    {
        Key = "MvvmCross_MvvmCross",
        Url = "https://sonarcloud.io",
        Organization = "mvx",
        Login = sonarKey,
        XUnitReportsPath = new DirectoryPath(outputDir + "/Tests/").FullPath
    };

    if (AzurePipelines.Environment.PullRequest.IsPullRequest)
    {
        settings.PullRequestKey = AzurePipelines.Environment.PullRequest.Number;
        settings.PullRequestBranch = AzurePipelines.Environment.PullRequest.SourceBranch;
        settings.PullRequestBase = AzurePipelines.Environment.PullRequest.TargetBranch;
    }
    else
    {
        settings.Branch = versionInfo.BranchName;
    }

    SonarBegin(settings);
});

Task("SonarEnd")
    .WithCriteria(() => !string.IsNullOrEmpty(sonarKey))
    .Does(() => 
{   
    SonarEnd(new SonarEndSettings
    {
        Login = sonarKey
    });
});

Task("Build")
    .IsDependentOn("ResolveBuildTools")
    .IsDependentOn("Clean")
    .IsDependentOn("PatchBuildProps")
    .IsDependentOn("Restore")
    .Does(() =>  {

    var settings = GetDefaultBuildSettings()
        .WithProperty("Version", versionInfo.SemVer)
        .WithProperty("PackageVersion", versionInfo.SemVer)
        .WithProperty("InformationalVersion", versionInfo.InformationalVersion)
        .WithProperty("NoPackageAnalysis", "True")
        .WithTarget("Build");
	
    MSBuild(sln, settings);
});

Task("UnitTest")
    .IsDependentOn("Build")
    .Does(() =>
{
    EnsureDirectoryExists(outputDir + "/Tests/");

    var testPaths = GetFiles("./UnitTests/*.UnitTest/*.UnitTest.csproj");
    var settings = new DotNetCoreTestSettings
    {
        Configuration = configuration,
        NoBuild = true
    };

    foreach(var project in testPaths)
    {
        var projectName = project.GetFilenameWithoutExtension();
        var testXml = MakeAbsolute(new FilePath(outputDir + "/Tests/" + projectName + ".xml"));
        settings.Loggers = new string[] { $"xunit;LogFilePath={testXml.FullPath}" };
        try 
        {
            DotNetCoreTest(project.ToString(), settings);
        }
        catch
        {
            // ignore
        }
    }

    if (isRunningOnPipelines)
    {
        var data = new AzurePipelinesPublishTestResultsData
        {
            Configuration = configuration,
            TestResultsFiles = GetFiles(outputDir + "/Tests/*.xml").ToList(),
            TestRunner = AzurePipelinesTestRunnerType.XUnit,
            TestRunTitle = "MvvmCross Unit Tests",
            MergeTestResults = true
        };
        AzurePipelines.Commands.PublishTestResults(data);
    }
});

Task("CopyPackages")
    .IsDependentOn("Build")
    .Does(() => 
{
    EnsureDirectoryExists(outputDir + "/NuGet/");

    var nugetFiles = GetFiles(solutionName + "*/**/bin/" + configuration + "/**/*.nupkg");
    CopyFiles(nugetFiles, new DirectoryPath(outputDir + "/NuGet/"));
});

Task("UpdateChangelog")
    .Does(() => 
{
    var arguments = new ProcessArgumentBuilder();
    if (!string.IsNullOrEmpty(githubToken))
        arguments.Append("--token {0}", githubToken);
    else if (!string.IsNullOrEmpty(githubTokenEnv))
        arguments.Append("--token {0}", githubTokenEnv);

    // Exclude labels
    var excludeLabels = new [] {
        "t/question",
        "s/wont-fix",
        "s/duplicate",
        "s/invalid",
        "s/needs-more-info",
    };
    arguments.Append("--exclude-labels {0}", string.Join(",", excludeLabels));

    // bug labels
    arguments.Append("--bug-labels {0}", "t/bug");

    // enhancement labels
    arguments.Append("--enhancement-labels {0}", "t/feature,t/enhancement");
    arguments.Append("--breaking-labels {0}", "t/breaking");
    arguments.Append("--security-labels {0}", "t/security");
    arguments.Append("--deprecated_labels {0}", "t/deprecated");
    arguments.Append("--no-issues_wo_labels");

    arguments.Append("--max-issues 200");
    arguments.Append("--user MvvmCross");
    arguments.Append("--project MvvmCross");

    if (!string.IsNullOrEmpty(sinceTag))
        arguments.Append("--since-tag {0}", sinceTag);

    if (versionInfo.BranchName.Contains("release/"))
        arguments.Append("--future-release {0}", versionInfo.MajorMinorPatch);

    Information("Starting github_changelog_generator with arguments: {0}", arguments.Render());

    using(var process = StartAndReturnProcess("github_changelog_generator",
        new ProcessSettings { Arguments = arguments }))
    {
        process.WaitForExit();
        Information("Exit code: {0}", process.GetExitCode());
    }
});

Task("Sonar")
    .IsDependentOn("Clean")
    .IsDependentOn("SonarStart")
    .IsDependentOn("Build")
    .IsDependentOn("UnitTest")
    .IsDependentOn("SonarEnd")
    .Does(() => 
{
});

Task("Default")
    .IsDependentOn("Clean")
    .IsDependentOn("Build")
    .IsDependentOn("UnitTest")
    .IsDependentOn("CopyPackages")
    .Does(() => 
{
});

RunTarget(target);

MSBuildSettings GetDefaultBuildSettings()
{
    var settings = new MSBuildSettings 
    {
        Configuration = configuration,
        ToolPath = msBuildPath,
        Verbosity = verbosity
    };

    if (isRunningOnPipelines)
    {
        // remove this when Xamarin.Android supports JDK11
        var javaSdkDir = EnvironmentVariable("JAVA_HOME_8_X64");
        Information("Setting JavaSdkDirectory to: " + javaSdkDir);
        settings = settings.WithProperty("JavaSdkDirectory", javaSdkDir);
    }

    return settings;
}