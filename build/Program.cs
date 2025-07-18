using Cake.Common;
using Cake.Common.Diagnostics;
using Cake.Common.IO;
using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Build;
using Cake.Common.Tools.DotNet.Test;
using Cake.GitVersioning;
using Cake.Core;
using Cake.Core.IO;
using Cake.Frosting;
using Nerdbank.GitVersioning;
using Spectre.Console;
using Cake.Common.Tools.DotNet.MSBuild;
using Cake.Common.Build;
using Cake.Core.Diagnostics;
using Cake.Common.Tools.DotNet.Tool;
using Cake.Common.Tools.DotNet.Run;

namespace Build;

public static class Program
{
    public static int Main(string[] args)
    {
        return new CakeHost()
            .UseContext<BuildContext>()
            .Run(args);
    }
}

public class BuildContext : FrostingContext
{
    public string AppFileRoot { get; }
    public string SolutionName { get; set; } = "MvvmCross";
    public string RepoName { get; set; } = "mvvmcross/mvvmcross";
    public string Target { get; set; }
    public string BuildConfiguration { get; set; }
    public string ArtifactsDir { get; set; }
    public DirectoryPath OutputDir { get; set; }
    public string SonarToken { get; set; }
    public string SonarKey { get; set; }
    public string SonarOrg { get; set; }
    public VersionOracle VersionInfo { get; set; }
    public FilePath Solution { get; set; }
    public DotNetVerbosity VerbosityDotNet { get; set; }

    public BuildContext(ICakeContext context)
        : base(context)
    {
        AppFileRoot = context.Argument("root", "..");
        Target = context.Argument("target", "Default");
        BuildConfiguration = context.Argument("configuration", "Release");
        ArtifactsDir = context.Argument("artifactsDir", $"{AppFileRoot}/artifacts");
        OutputDir = new DirectoryPath(ArtifactsDir);
        SonarToken = context.Argument("sonarToken", "");
        SonarKey = context.Argument("sonarKey", "");
        SonarOrg = context.Argument("sonarOrg", "");

        var slnPath = context.IsRunningOnMacOs() ?
            $"{AppFileRoot}/MvvmCross-macos.slnf" :
            $"{AppFileRoot}/MvvmCross.sln";
        Solution = new FilePath(slnPath);

        VersionInfo = context.GitVersioningGetVersion();

        var cakeVersion = typeof(ICakeContext).Assembly.GetName().Version.ToString();

        AnsiConsole.Write(new FigletText("MvvmCross"));
        context.Information("Building version {0}, ({1}, {2}) using version {3} of Cake.",
            VersionInfo.SemVer2,
            BuildConfiguration,
            Target,
            cakeVersion);

        if (context.GitHubActions().Environment.PullRequest.IsPullRequest)
        {
            context.Information("PR HeadRef: {0}", context.GitHubActions().Environment.Workflow.HeadRef);
            context.Information("PR BaseRef: {0}", context.GitHubActions().Environment.Workflow.BaseRef);
        }

        context.Information("RefName: {0}", context.GitHubActions().Environment.Workflow.RefName);

        VerbosityDotNet = context.Log.Verbosity switch
        {
            Verbosity.Quiet => DotNetVerbosity.Quiet,
            Verbosity.Normal => DotNetVerbosity.Normal,
            Verbosity.Verbose => DotNetVerbosity.Detailed,
            Verbosity.Diagnostic => DotNetVerbosity.Diagnostic,
            _ => DotNetVerbosity.Minimal
        };
    }

    public DotNetBuildSettings GetDefaultBuildSettings(DotNetMSBuildSettings msBuildSettings = null)
    {
        msBuildSettings ??= GetDefaultDotNetMSBuildSettings();

        var settings = new DotNetBuildSettings
        {
            Configuration = BuildConfiguration,
            MSBuildSettings = msBuildSettings,
            Verbosity = VerbosityDotNet
        };

        return settings;
    }

    public DotNetMSBuildSettings GetDefaultDotNetMSBuildSettings()
    {
        var settings = new DotNetMSBuildSettings
        {
            Version = VersionInfo.SemVer2,
            PackageVersion = VersionInfo.NuGetPackageVersion,
            InformationalVersion = VersionInfo.AssemblyInformationalVersion
        };

        return settings;
    }
}

[TaskName("Clean")]
public sealed class CleanTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.CleanDirectories($"{context.AppFileRoot}/MvvmCross*/**/bin");
        context.CleanDirectories($"{context.AppFileRoot}/MvvmCross*/**/obj");
        context.CleanDirectories($"{context.AppFileRoot}/MvvmCross*/**/TestResults");
        context.CleanDirectories(context.OutputDir.FullPath);

        context.EnsureDirectoryExists(context.OutputDir);
    }
}

[TaskName("Restore")]
[IsDependentOn(typeof(CleanTask))]
public sealed class RestoreTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.DotNetRestore(context.Solution.ToString());
    }
}

[TaskName("SonarStart")]
public sealed class SonarStartTask : FrostingTask<BuildContext>
{
    public override bool ShouldRun(BuildContext context)
    {
        return !string.IsNullOrEmpty(context.SonarToken);
    }

    public override void Run(BuildContext context)
    {
        var xunitReportsPaths = context.GetFiles(context.MakeAbsolute(context.OutputDir.Combine("Tests/")) + "/**/*.trx");
        var corverageReportsPaths = context.GetFiles(context.MakeAbsolute(context.OutputDir.Combine("Tests/")) + "/**/*.coverage");

        var settings = new DotNetToolSettings
        {
            ArgumentCustomization = args => args
                .Append("begin")
                .Append("/key:{0}", context.SonarKey)
                .Append("/o:{0}", context.SonarOrg)
                .Append("/d:sonar.host.url={0}", "https://sonarcloud.io")
                .Append("/d:sonar.sources={0}", "MvvmCross*/**")
                .Append("/d:sonar.tests={0}", "UnitTests/**")
                .Append("/d:sonar.cs.vstest.reportsPaths={0}", string.Join(",", xunitReportsPaths.Select(p => p.FullPath)))
                .Append("/d:sonar.flex.cobertura.reportPaths={0}", string.Join(",", corverageReportsPaths.Select(p => p.FullPath)))
                .AppendSecret("/d:sonar.token={0}", context.SonarToken)
        };

        context.DotNetTool("sonarscanner", settings);
    }
}

[TaskName("Build")]
[IsDependentOn(typeof(RestoreTask))]
public sealed class BuildTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        var settings = context.GetDefaultBuildSettings();
        context.DotNetBuild(context.Solution.ToString(), settings);
    }
}

[TaskName("UnitTest")]
[IsDependentOn(typeof(BuildTask))]
public sealed class UnitTestTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        var testReportFolder = context.OutputDir.Combine("Tests/");
        context.EnsureDirectoryExists(testReportFolder);

        var testPaths = context.GetFiles($"{context.AppFileRoot}/UnitTests/*.UnitTest/*.UnitTest.csproj");
        foreach (var project in testPaths)
        {
            var projectName = project.GetFilenameWithoutExtension();
            var runSettings = new DotNetRunSettings
            {
                NoBuild = true,
                Configuration = context.BuildConfiguration,
                Verbosity = context.VerbosityDotNet,
                ArgumentCustomization = args => args
                    .Append("-- ")
                    .Append($"--report-xunit-trx --report-xunit-trx-filename {projectName}.trx")
                    .Append($"--report-ctrf --report-ctrf-filename {projectName}.ctrf.json")
                    .Append($"--coverage --coverage-output {projectName}.coverage --coverage-output-format cobertura")
            };

            try
            {
                context.DotNetRun(project.FullPath, runSettings);
            }
            catch
            {
                // ignore
            }

            var testTrxFiles = context.GetFiles($"{context.AppFileRoot}/**/TestResults/*.trx");
            var testCtrfFiles = context.GetFiles($"{context.AppFileRoot}/**/TestResults/*.ctrf.json");
            var coverageFiles = context.GetFiles($"{context.AppFileRoot}/**/TestResults/*.coverage");
            context.CopyFiles(testTrxFiles, testReportFolder);
            context.CopyFiles(testCtrfFiles, testReportFolder);
            context.CopyFiles(coverageFiles, testReportFolder);
        }
    }
}

[TaskName("SonarEnd")]
public sealed class SonarEndTask : FrostingTask<BuildContext>
{
    public override bool ShouldRun(BuildContext context)
    {
        return !string.IsNullOrEmpty(context.SonarToken);
    }

    public override void Run(BuildContext context)
    {
        var settings = new DotNetToolSettings
        {
            ArgumentCustomization = args => args
                .Append("end")
                .AppendSecret("/d:sonar.token={0}", context.SonarToken)
        };

        context.DotNetTool("sonarscanner", settings);
    }
}

[TaskName("GenerateSBOM")]
[IsDependentOn(typeof(BuildTask))]
public sealed class GenerateSbomTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        var sbomPath = context.MakeAbsolute(context.OutputDir.Combine("sbom/"));
        var slnPath = context.MakeAbsolute(new DirectoryPath($"{context.AppFileRoot}/MvvmCross.sln")).ToString();

        var settings = new DotNetToolSettings
        {
            ArgumentCustomization = args => args
                .Append(slnPath)
                .Append("--output {0}", sbomPath)
                .Append("--filename {0}", "MvvmCross.sbom.json")
                .Append("--json")
                .Append("--set-name MvvmCross")
                .Append("--set-type Library")
                .Append("--set-version {0}", context.VersionInfo.SemVer2)
                .Append("--recursive")
                .Append("--disable-package-restore")
        };

        context.DotNetTool("CycloneDX", settings);
    }
}

[TaskName("CopyPackages")]
[IsDependentOn(typeof(BuildTask))]
public sealed class CopyPackagesTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        var packagesDir = context.OutputDir.Combine("NuGet/");
        context.EnsureDirectoryExists(packagesDir);

        var nugetFiles = context.GetFiles($"{context.AppFileRoot}/{context.SolutionName}*/**/bin/{context.BuildConfiguration}/**/*.nupkg");
        context.CopyFiles(nugetFiles, packagesDir);
    }
}

[TaskName("Sonar")]
[IsDependentOn(typeof(CleanTask))]
[IsDependentOn(typeof(SonarStartTask))]
[IsDependentOn(typeof(BuildTask))]
[IsDependentOn(typeof(UnitTestTask))]
[IsDependentOn(typeof(SonarEndTask))]
public sealed class SonarTask : FrostingTask<BuildContext>
{
}

[TaskName("Default")]
[IsDependentOn(typeof(CleanTask))]
[IsDependentOn(typeof(BuildTask))]
[IsDependentOn(typeof(UnitTestTask))]
[IsDependentOn(typeof(GenerateSbomTask))]
[IsDependentOn(typeof(CopyPackagesTask))]
public sealed class DefaultTask : FrostingTask<BuildContext>
{
}
