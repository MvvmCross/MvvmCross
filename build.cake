#tool nuget:?package=GitVersion.CommandLine&version=5.0.1
#tool nuget:?package=vswhere&version=2.7.1
#addin nuget:?package=Cake.Figlet&version=1.3.1
#addin nuget:?package=Cake.Git&version=0.21.0

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

var githubToken = Argument("github_token", "");
var githubTokenEnv = EnvironmentVariable("CHANGELOG_GITHUB_TOKEN");
var sinceTag = Argument("since_tag", "");

var isRunningOnPipelines = TFBuild.IsRunningOnAzurePipelines || TFBuild.IsRunningOnAzurePipelinesHosted;
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
        var buildNumber = versionInfo.InformationalVersion + "-" + TFBuild.Environment.Build.Number;
        buildNumber = buildNumber.Replace("/", "-");
        TFBuild.Commands.UpdateBuildNumber(buildNumber);
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

Task("Clean").Does(() =>
{
    EnsureDirectoryExists(outputDir.FullPath);

    CleanDirectories("./**/bin");
    CleanDirectories("./**/obj");
    CleanDirectories(outputDir.FullPath);

    CopyFile(gitVersionLog, outputDir + "gitversion.log");
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
    var buildProp = new FilePath("./Directory.build.props");
    XmlPoke(buildProp, "//Project/PropertyGroup/Version", versionInfo.SemVer);
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
    var testsFailed = false;

    var settings = new DotNetCoreTestSettings
    {
        Configuration = configuration,
        NoBuild = true
    };

    foreach(var project in testPaths)
    {
        var projectName = project.GetFilenameWithoutExtension();
        var testXml = MakeAbsolute(new FilePath(outputDir + "/Tests/" + projectName + ".xml"));
        settings.Logger = $"xunit;LogFilePath={testXml.FullPath}";
        try 
        {
            DotNetCoreTest(project.ToString(), settings);
        }
        catch
        {
            testsFailed = true;
        }
    }

    if (isRunningOnPipelines)
    {
        var data = new TFBuildPublishTestResultsData
        {
            Configuration = configuration,
            TestResultsFiles = GetFiles(outputDir + "/Tests/*.xml").ToList(),
            TestRunner = TFTestRunnerType.XUnit,
            TestRunTitle = "MvvmCross Unit Tests",
            MergeTestResults = true
        };
        TFBuild.Commands.PublishTestResults(data);
    }

    if (testsFailed)
        throw new Exception("Tests failed :(");
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
        "s/deprecated",
        "s/invalid"
    };
    arguments.Append("--exclude-labels {0}", string.Join(",", excludeLabels));

    // bug labels
    arguments.Append("--bug-labels {0}", "t/bug");

    // enhancement labels
    arguments.Append("--enhancement-labels {0}", "t/feature");

    // breaking labels (enable when github_changelog_generator 1.15 is released)
    //arguments.Append("--breaking-labels {0}", "t/breaking");

    arguments.Append("--max-issues 200");

    if (!string.IsNullOrEmpty(sinceTag) && versionInfo.BranchName.Contains("release/"))
    {
        arguments.Append("--between-tags {0},{1}", sinceTag, versionInfo.MajorMinorPatch);

        arguments.Append("--future-release {0}", versionInfo.MajorMinorPatch);
    }
    else 
    {
        if (!string.IsNullOrEmpty(sinceTag))
            arguments.Append("--since-tag {0}", sinceTag);

        if (versionInfo.BranchName.Contains("release/"))
            arguments.Append("--future-release {0}", versionInfo.MajorMinorPatch);
    }

    Information("Starting github_changelog_generator with arguments: {0}", arguments.Render());

    using(var process = StartAndReturnProcess("github_changelog_generator",
        new ProcessSettings { Arguments = arguments }))
    {
        process.WaitForExit();
        Information("Exit code: {0}", process.GetExitCode());
    }
});

Task("Default")
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

    // workaround for derped Java Home ENV vars
    if (IsRunningOnWindows() && isRunningOnPipelines)
    {
        var javaSdkDir = EnvironmentVariable("JAVA_HOME_8_X64");
        Information("Setting JavaSdkDirectory to: " + javaSdkDir);
        settings = settings.WithProperty("JavaSdkDirectory", javaSdkDir);
    }

    return settings;
}