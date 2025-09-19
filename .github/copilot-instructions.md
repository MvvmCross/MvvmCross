# MvvmCross Development Instructions

MvvmCross is a cross-platform MVVM framework for .NET supporting Android, iOS, MacCatalyst, TvOS, macOS, WinUI, and WPF. This guide provides essential commands and workflows for GitHub Copilot to work effectively with the codebase.

**Always reference these instructions first and fallback to search or bash commands only when you encounter unexpected information that does not match the info here.**

## Platform Requirements

**CRITICAL PLATFORM LIMITATION:** All MvvmCross projects require iOS workloads even for Android and Core development due to multi-targeting. **Full builds and tests require Windows or macOS.**

### System Requirements

**Required**
- .NET 9.0.304 SDK (used in CI/CD, see .github/actions/shared/action.yml)
- Git with proper autocrlf configuration

**Windows (Full Development)**
- All workloads: `dotnet workload install android ios tvos macos maccatalyst maui-ios maui-android`
- Use solution file: `MvvmCross.slnx`

**macOS (Full Development)** 
- All workloads: `dotnet workload install android ios tvos macos maccatalyst maui-ios maui-android`
- Use solution filter: `MvvmCross-macos.slnf`

**Linux (Limited Development)**
- Android workload only: `dotnet workload install android`
- Individual project builds fail due to iOS dependencies
- Format checking and some tools work on individual projects

## Working Effectively

### Initial Setup Commands

Always run these commands after cloning:

```bash
# Install .NET 9.0.304 (used in CI/CD)
curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --version 9.0.304

# Restore .NET tools (includes SonarScanner, ReportGenerator, CycloneDX)
dotnet tool restore

# Install required workloads (Windows/macOS only - full command)
dotnet workload install android ios tvos macos maccatalyst maui-ios maui-android

# Linux limited workload
dotnet workload install android

# Configure git line endings
# Windows: git config --global core.autocrlf true  
# Mac/Linux: git config --global core.autocrlf input

# CRITICAL: Ensure full git history for version calculations
git fetch --unshallow
```

### Build and Test Commands

**NEVER CANCEL builds or tests. Use 60+ minute timeouts.**

#### Full Build (Windows/macOS Only)

```bash
# Primary build command - takes 15-25 minutes. NEVER CANCEL. Set timeout to 60+ minutes.
dotnet run --project build/Build.csproj -- --verbosity=Minimal

# Build with specific configuration
dotnet run --project build/Build.csproj -- --configuration=Debug --verbosity=Minimal

# Build with artifacts output
dotnet run --project build/Build.csproj -- --artifactsDir=./output --ctrfDir=./ctrf
```

#### Unit Tests (Windows/macOS Only)

```bash  
# Run unit tests - takes 10-15 minutes. NEVER CANCEL. Set timeout to 30+ minutes.
dotnet run --project build/Build.csproj -- --target=UnitTest --verbosity=Minimal

# Tests generate reports in artifacts/Tests/ and ctrf/ directories
```

#### Available Build Targets

- `Clean` - Clean all build outputs
- `Restore` - Restore NuGet packages  
- `Build` - Build all projects
- `UnitTest` - Run all unit tests (depends on Build)
- `Sonar` - Full SonarCloud analysis pipeline
- `GenerateSBOM` - Generate Software Bill of Materials
- `CopyPackages` - Copy built packages to artifacts
- `Default` - Clean + Restore + Build + UnitTest

### Code Quality and Formatting

**Always run before committing changes:**

```bash
# Format code - REQUIRED before PR submission or CI fails
# Windows/macOS with full solution:
dotnet format whitespace --verify-no-changes MvvmCross.slnx

# Linux/Individual project approach (when solution fails):
dotnet format whitespace --verify-no-changes build/Build.csproj
dotnet format whitespace --verify-no-changes MvvmCross/MvvmCross.csproj

# Format specific plugin projects
dotnet format whitespace --verify-no-changes MvvmCross.Plugins/Color/MvvmCross.Plugin.Color.csproj

# Check formatting without changes
dotnet format whitespace --verify-no-changes --verbosity=diagnostic
```

## Key Projects and Structure

### Core Framework Projects
- `MvvmCross/MvvmCross.csproj` - Main framework library
- `MvvmCross.Tests/MvvmCross.Tests.csproj` - Integration tests
- `UnitTests/MvvmCross.UnitTest/MvvmCross.UnitTest.csproj` - Unit tests

### Plugin Projects (All in MvvmCross.Plugins/)
- `Color/MvvmCross.Plugin.Color.csproj` - Color utilities
- `Json/MvvmCross.Plugin.Json.csproj` - JSON serialization
- `Messenger/MvvmCross.Plugin.Messenger.csproj` - Pub/Sub messaging
- `Visibility/MvvmCross.Plugin.Visibility.csproj` - UI visibility helpers
- `All/MvvmCross.Plugin.All.csproj` - Meta-package for all plugins

### Platform Extensions
- `MvvmCross.DroidX/` - AndroidX support libraries
- `MvvmCross.Wpf/` - WPF platform support

### Example Applications  
- `Projects/Playground/` - Comprehensive sample app for all platforms
  - `Playground.Core/` - Shared business logic
  - `Playground.Droid/` - Android implementation
  - `Playground.iOS/` - iOS implementation  
  - `Playground.WinUi3/` - Windows implementation
  - `Playground.Mac/` - macOS implementation

### Build Infrastructure
- `build/Build.csproj` - Cake Frosting build automation
- `.github/workflows/build.yml` - Main CI pipeline (Windows)
- `.github/workflows/dotnet-format.yml` - Code formatting checks

## Common Development Tasks

### Creating New Features
1. Always build and test before changes: `dotnet run --project build/Build.csproj`
2. Make changes to appropriate projects
3. Add/update unit tests in corresponding UnitTest projects
4. Test changes with Playground projects
5. Format code: `dotnet format whitespace --verify-no-changes`
6. Run full build again to validate

### Working with Plugins
- Follow existing plugin patterns in `MvvmCross.Plugins/`
- Inherit from `IMvxPlugin` and use `MvxPluginAttribute`
- Register services in the `Load` method
- Add corresponding unit tests in `UnitTests/Plugins.*.UnitTest/`

### Debugging and Testing
- Use Playground projects to test framework changes
- Playground.Core contains comprehensive test scenarios
- Each platform project demonstrates platform-specific features
- Run Playground projects to validate cross-platform functionality

## Limitations and Known Issues

### Platform-Specific Limitations
- **Linux:** Cannot build most projects due to iOS workload dependency
- **Linux:** Format command fails on .slnx files - use individual projects: `dotnet format whitespace build/Build.csproj`
- **Solution Files:** .slnx format not supported by all tools - use individual projects when needed
- **macOS Filter:** MvvmCross-macos.slnf is a solution filter, not directly buildable with dotnet build

### Build Considerations  
- Shallow git clones break version calculations - always `git fetch --unshallow`
- Multi-targeting requires all platform workloads even for single platform development
- Build times are significant (15-25 minutes) - always use long timeouts
- CI builds use Windows runners for full compatibility

### Testing Requirements
- Unit tests require all platform workloads to run
- Use `dotnet run --project build/Build.csproj -- --target=UnitTest`
- Test reports generated in `artifacts/Tests/` and `ctrf/` directories
- Tests take 10-15 minutes - NEVER CANCEL, use 30+ minute timeouts

## Validation Scenarios

**Always run these validation steps after making changes:**

1. **Code Formatting:** 
   - Windows/macOS: `dotnet format whitespace --verify-no-changes MvvmCross.slnx`
   - Linux: `dotnet format whitespace --verify-no-changes build/Build.csproj` (and other individual projects)
2. **Full Build:** `dotnet run --project build/Build.csproj -- --verbosity=Minimal` (15-25 min)
3. **Unit Tests:** `dotnet run --project build/Build.csproj -- --target=UnitTest` (10-15 min)
4. **Playground Testing:** Build and run appropriate Playground projects to test functionality
5. **Cross-Platform Check:** Verify changes work across target platforms using Playground

## Quick Reference Commands

```bash
# Repository root structure
ls -la
# Key items: MvvmCross/, Projects/, UnitTests/, build/, .github/

# Common file locations
find . -name "*.csproj" -path "*/MvvmCross/*" | head -5
find . -name "*.csproj" -path "*/UnitTests/*" | head -5

# Build status and version info  
dotnet run --project build/Build.csproj -- --showdescription
```

**Remember:** NEVER CANCEL long-running builds or tests. The build system is designed for substantial compile times. Use 60+ minute timeouts for builds and 30+ minute timeouts for tests.