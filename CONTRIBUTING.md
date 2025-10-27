# Contributing to MvvmCross

We are happy to receive Pull Requests adding new features and solving bugs. As for new features, please contact us before doing major work. To ensure you are not working on something that will be rejected due to not fitting into the roadmap or ideal of the framework.

## Installation

To develop on MvvmCross you will need to install the [.NET 10 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0).

On Windows and macOS install the following workloads:
```bash
dotnet workload install android ios tvos macos maccatalyst
```

On Linux install the following workload:

```bash
dotnet workload install android
```

## IDE

It is up to you which IDE you use, the solution should work well regardless if whether you are using Visual Studio, Visual Studio Code or Rider.

For Visual Studio Code, make sure to install [C# Dev Kit](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit) and [.NET MAUI](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.dotnet-maui) extensions. When opening the project in Visual Studio Code, it should suggest which extensions to install which are defined in the [.vscode/extensions.json](https://github.com/MvvmCross/MvvmCross/blob/develop/.vscode/extensions.json) file.

On Windows you can open the `MvvmCross.slnx` file. If you are developing on macOS, use the solution filter `MvvmCross-macos.slnf`. For Linux you can use the `MvvmCross-linux.slnf`, this will only build the Android targets which are the only workloads supported on Linux.

## Building

If you just want to build the project you can run the Cake Frosting build script using

```bash
dotnet run --project build/Build.csproj
```

## Git setup

Since Windows and UNIX-based systems differ in terms of line endings, it is a very good idea to configure git autocrlf settings.

On *Windows* we recommend setting `core.autocrlf` to `true`.

```
git config --global core.autocrlf true
```

On *Mac* we recommend setting `core.autocrlf` to `input`.

```
git config --global core.autocrlf input
```

## Code style guidelines

We have a .editorconfig file in place, please use an IDE that respects the settings, such as Visual Studio, Rider or VS Code.

When creating Pull Requests `dotnet format` command will run, to ensure formatting on code.

### Project Workflow

Our workflow is loosely based on [Github Flow](http://scottchacon.com/2011/08/31/github-flow.html).
We actively do development on the **develop** branch. This means that all pull requests by contributors need to be develop and requested against the develop branch.

The master branch contains tags reflecting what is currently on NuGet.org.

### Submitting Pull Requests

Make sure you can build the code. Familiarize yourself with the project workflow and our coding conventions. If you don't know what a pull request is
read this https://help.github.com/articles/using-pull-requests.

Before submitting a feature or substantial code contribution please discuss it with the team and ensure it follows the MvvmCross roadmap.
Note that code submissions will be reviewed and tested. Only code that meets quality and design/roadmap appropriateness will be merged into the source. [Don't "Push" Your Pull Requests](https://www.igvita.com/2011/12/19/dont-push-your-pull-requests/)

Before submitting a Pull Request, make sure that the entire project builds and tests are passing using `dotnet run --project build/Build.csproj`.

## Finding an issue to work on

We have issue labeled with [`up-for-grabs`](https://github.com/MvvmCross/MvvmCross/labels/up-for-grabs) or [`first-timers-only`](https://github.com/MvvmCross/MvvmCross/labels/first-timers-only) to get you started on easy work.

If you'd like to work on something that isn't in a current issue, especially if
it would be a big change, please open a new issue for discussion!
