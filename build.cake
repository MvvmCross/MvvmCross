#tool dotnet:n?package=GitVersion.Tool&version=5.10.3
#tool dotnet:n?package=dotnet-sonarscanner&version=5.8.0
#addin nuget:?package=Cake.Figlet&version=2.0.1

var solutionName = "MvvmCross";
var repoName = "mvvmcross/mvvmcross";
var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var artifactsDir = Argument("artifactsDir", "./artifacts");
var outputDir = new DirectoryPath(artifactsDir);
var gitVersionLog = new FilePath("./gitversion.log");
var nuspecDir = new DirectoryPath("./nuspec");
var verbosity = Verbosity.Minimal;
var verbosityDotNet = DotNetVerbosity.Minimal;
var sonarKey = Argument("sonarKey", "");

var githubToken = Argument("github_token", "");
var githubTokenEnv = EnvironmentVariable("CHANGELOG_GITHUB_TOKEN");
var sinceTag = Argument("since_tag", "");

var isRunningOnPipelines = AzurePipelines.IsRunningOnAzurePipelines;
GitVersion versionInfo = null;

FilePath solution;

Setup(context => 
{
    var slnPath = IsRunningOnMacOs() ? "./MvvmCross-macos.slnf" : "./MvvmCross.sln";
    solution = new FilePath(slnPath);

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

    verbosity = context.Log.Verbosity;
    verbosityDotNet = verbosity switch {
        Verbosity.Quiet => DotNetVerbosity.Quiet,
        Verbosity.Normal => DotNetVerbosity.Normal,
        Verbosity.Verbose => DotNetVerbosity.Detailed,
        Verbosity.Diagnostic => DotNetVerbosity.Diagnostic,
        _ => DotNetVerbosity.Minimal
    };
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

Task("Restore")
    .Does(() =>
{
    DotNetRestore(solution.ToString());
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
    StringBuilder args = new StringBuilder();
    args.Append("begin ");
    args.Append("/key:MvvmCross_MvvmCross ");
    args.Append("/o:mvx ");
    args.Append("/d:sonar.host.url=https://sonarcloud.io ");
    args.AppendFormat("/d:sonar.cs.xunit.reportsPaths={0} ", new DirectoryPath(outputDir + "/Tests/").FullPath);
    args.AppendFormat("/d:sonar.login={0} ", sonarKey);

    if (AzurePipelines.Environment.PullRequest.IsPullRequest)
    {
        args.AppendFormat("/d:sonar.pullrequest.key={0} ", AzurePipelines.Environment.PullRequest.Number);
        args.AppendFormat("/d:sonar.pullrequest.branch={0} ", AzurePipelines.Environment.PullRequest.SourceBranch);
        args.AppendFormat("/d:sonar.pullrequest.base={0}", AzurePipelines.Environment.PullRequest.TargetBranch);
    }
    else
    {
        args.AppendFormat("/d:sonar.branch.name={0}", versionInfo.BranchName);
    }

    DotNetTool("./", "dotnet-sonarscanner", args.ToString());
});

Task("SonarEnd")
    .WithCriteria(() => !string.IsNullOrEmpty(sonarKey))
    .Does(() => 
{   
    StringBuilder args = new StringBuilder();
    args.Append("end ");
    args.AppendFormat("/d:sonar.login={0}", sonarKey);

    DotNetTool("./", "dotnet-sonarscanner", args.ToString());
});

Task("Build")
    .IsDependentOn("Clean")
    .IsDependentOn("PatchBuildProps")
    .IsDependentOn("Restore")
    .Does(() =>
{

    var msBuildSettings = new DotNetMSBuildSettings
    {
        Version = versionInfo.SemVer,
        PackageVersion = versionInfo.SemVer,
        InformationalVersion = versionInfo.InformationalVersion
    };

    var settings = new DotNetBuildSettings
    {
         Configuration = configuration,
         MSBuildSettings = msBuildSettings,
         Verbosity = verbosityDotNet
    };

    DotNetBuild(solution.ToString(), settings);
});

Task("UnitTest")
    .IsDependentOn("Build")
    .Does(() =>
{
    EnsureDirectoryExists(outputDir + "/Tests/");

    var testPaths = GetFiles("./UnitTests/*.UnitTest/*.UnitTest.csproj");
    var settings = new DotNetTestSettings
    {
        Configuration = configuration,
        NoBuild = true,
        Verbosity = verbosityDotNet
    };

    foreach(var project in testPaths)
    {
        var projectName = project.GetFilenameWithoutExtension();
        var testXml = MakeAbsolute(new FilePath(outputDir + "/Tests/" + projectName + ".xml"));
        settings.Loggers = new string[] { $"xunit;LogFilePath={testXml.FullPath}" };
        try 
        {
            DotNetTest(project.ToString(), settings);
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