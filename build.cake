#tool dotnet:n?package=GitVersion.Tool&version=5.12.0
#addin nuget:?package=Cake.Figlet&version=2.0.1

var solutionName = "MvvmCross";
var repoName = "mvvmcross/mvvmcross";
var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var artifactsDir = Argument("artifactsDir", "./artifacts");
var outputDir = new DirectoryPath(artifactsDir);
var gitVersionLog = new FilePath("./gitversion.log");
var verbosity = Verbosity.Minimal;
var verbosityDotNet = DotNetVerbosity.Minimal;
var sonarToken = Argument("sonarToken", "");
var sonarKey = Argument("sonarKey", "");
var sonarOrg = Argument("sonarOrg", "");

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

    var cakeVersion = typeof(ICakeContext).Assembly.GetName().Version.ToString();

    Information(Figlet(solutionName));
    Information("Building version {0}, ({1}, {2}) using version {3} of Cake.",
        versionInfo.SemVer,
        configuration,
        target,
        cakeVersion);

    if (GitHubActions.Environment.PullRequest.IsPullRequest)
    {
        Information("PR HeadRef: {0}", GitHubActions.Environment.Workflow.HeadRef);
        Information("PR BaseRef: {0}", GitHubActions.Environment.Workflow.BaseRef);
    }

    Information("RefName: {0}", GitHubActions.Environment.Workflow.RefName);

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
    CleanDirectories("MvvmCross*/**/bin");
    CleanDirectories("MvvmCross*/**/obj");
    CleanDirectories("MvvmCross*/**/TestResults");
    CleanDirectories(outputDir.FullPath);

    EnsureDirectoryExists(outputDir);

    CopyFile(gitVersionLog, outputDir + "/gitversion.log");
});

Task("Restore").Does(() => DotNetRestore(solution.ToString()));

Task("SonarStart")
    .WithCriteria(() => !string.IsNullOrEmpty(sonarToken))
    .Does(() => 
{
    var xunitReportsPath = MakeAbsolute(new DirectoryPath(outputDir + "/Tests")) + "/**/*.xml";
    Information("XUnitReportsPath {0}", xunitReportsPath);

    ProcessArgumentBuilder PrepareSonarArguments(ProcessArgumentBuilder args)
    {
        args.Append("begin");
        args.Append("/key:{0}", sonarKey);
        args.Append("/o:{0}", sonarOrg);
        args.Append("/d:sonar.host.url={0}", "https://sonarcloud.io");
        args.Append("/d:sonar.sources={0}", "MvvmCross*/**");
        args.Append("/d:sonar.tests={0}", "UnitTests/**");
        args.Append("/d:sonar.cs.xunit.reportsPaths={0}", xunitReportsPath);
        args.AppendSecret("/d:sonar.token={0}", sonarToken);
        return args;
    }

    var settings = new DotNetToolSettings
    {
        ArgumentCustomization = PrepareSonarArguments
    };

    DotNetTool("sonarscanner", settings);
});

Task("SonarEnd")
    .WithCriteria(() => !string.IsNullOrEmpty(sonarToken))
    .Does(() => 
{
    var settings = new DotNetToolSettings
    {
        ArgumentCustomization = args => args
            .Append("end")
            .AppendSecret("/d:sonar.token={0}", sonarToken)
    };

    DotNetTool("sonarscanner", settings);
});

Task("Build")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .Does(() =>
{
    var settings = GetDefaultBuildSettings();
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
        var testTrx = MakeAbsolute(new FilePath(outputDir + "/Tests/" + projectName + ".trx"));
        var testXml= MakeAbsolute(new FilePath(outputDir + "/Tests/" + projectName + ".xml"));
        settings.Loggers = new string[]
        {
            $"trx;LogFileName={testTrx.FullPath}",
            $"xunit;LogFilePath={testXml.FullPath};Title={projectName}"
        };

        try
        {
            DotNetTest(project.ToString(), settings);
        }
        catch
        {
            // ignore
        }
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

Task("Sonar")
    .IsDependentOn("Clean")
    .IsDependentOn("SonarStart")
    .IsDependentOn("Build")
    .IsDependentOn("UnitTest")
    .IsDependentOn("SonarEnd");

Task("Default")
    .IsDependentOn("Clean")
    .IsDependentOn("Build")
    .IsDependentOn("UnitTest")
    .IsDependentOn("CopyPackages");

RunTarget(target);

DotNetBuildSettings GetDefaultBuildSettings(DotNetMSBuildSettings? msBuildSettings = null)
{
    msBuildSettings ??= GetDefaultDotNetMSBuildSettings();

    var settings = new DotNetBuildSettings
    {
        Configuration = configuration,
        MSBuildSettings = msBuildSettings,
        Verbosity = verbosityDotNet
    };

    return settings;
}

DotNetMSBuildSettings GetDefaultDotNetMSBuildSettings()
{
    var settings = new DotNetMSBuildSettings
    {
        Version = versionInfo.SemVer,
        PackageVersion = versionInfo.SemVer,
        InformationalVersion = versionInfo.InformationalVersion
    };

    return settings;
}
