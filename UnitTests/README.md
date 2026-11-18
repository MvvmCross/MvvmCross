# Unit tests

The seven test projects use [TUnit](https://tunit.dev/) and target .NET 10.
`MvvmCross.UnitTest.Common` contains shared logging helpers, rather than runnable tests.

## Running tests

From the repository root, with the .NET 10 SDK or later:

```sh
dotnet test --project UnitTests/MvvmCross.UnitTest/MvvmCross.UnitTest.csproj -p:TargetFramework=net10.0 -p:TargetFrameworks=net10.0
```

The framework overrides restrict referenced libraries to their portable .NET
target, so running unit tests does not require mobile platform workloads. Set
both properties so NuGet imports TUnit's build configuration for the chosen target.
Use the corresponding project path to run a plugin's tests.

`global.json` selects Microsoft.Testing.Platform for `dotnet test`. Pass test
options directly, for example:

```sh
dotnet test --project UnitTests/MvvmCross.UnitTest/MvvmCross.UnitTest.csproj -p:TargetFramework=net10.0 -p:TargetFrameworks=net10.0 --treenode-filter '/*/*/MvxExceptionTest/*' --report-trx --report-ctrf
```

After building the test projects, run all seven assemblies with:

```sh
dotnet test --test-modules 'UnitTests/**/bin/Debug/net10.0/*.UnitTest.dll' --results-directory TestResults
```

The Cake `UnitTest` task runs all projects and collects TRX, CTRF, and coverage
reports. `Microsoft.Testing.Extensions.CtrfReport` generates CTRF directly during
each test run; the build task copies those reports to the configured `ctrfDir`
for the GitHub test reporter.

## Writing tests

- Use `[Test]`, `[Arguments(...)]`, and typed `[MethodDataSource(...)]` data.
- Await TUnit assertions and any helpers that perform assertions.
- Capture synchronous callback/event results and assert them in the test body.
- Use `[ClassDataSource<T>]` for fixtures and `[InheritsTests]` for inherited tests.
- `TestAssemblyConfiguration.cs` serializes tests within each assembly because
  fixtures modify process-wide MvvmCross state and culture, and some tests force
  garbage collection. Each project runs in its own process.
