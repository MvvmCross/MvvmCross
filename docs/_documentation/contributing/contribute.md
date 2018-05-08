---
layout: documentation
title: New contributions
category: Contributing
order: 1
---

Some of the best ways to contribute are to try things out, file bugs, and join conversations.

* [Pull requests](https://github.com/MvvmCross/MvvmCross/pulls): [Open](https://github.com/MvvmCross/MvvmCross/pulls?q=is%3Aopen+is%3Apr) / [Closed](https://github.com/MvvmCross/MvvmCross/pulls?q=is%3Apr+is%3Aclosed)
* [Issues](https://github.com/MvvmCross/MvvmCross/issues): [Open](https://github.com/MvvmCross/MvvmCross/issues?q=is%3Aopen+is%3Aissue) / [Closed](https://github.com/MvvmCross/MvvmCross/issues?q=is%3Aissue+is%3Aclosed)

If you would like to help make MvvmCross even better, then please do:

* **new code** - including pull requests via GitHub - or you can fork the project and build your own extensions
* **new plugins** - can be hosted in your own repositories
* please do blog about your adventures with MvvmCross!
* please suggest editions for the documentation files - we're currently light on documentation!
* if you use the framework, then please let us know - we love to see what people are doing with it

# Help updating the docs!

Everyone can contribute and help improving the docs! The docs are part of the source tree, so just go over to github and help us out!

# Work with and debug MvvmCross

The current state of Visual Studio for Mac doesn't allow us to build MvvmCross. This is because we use multi-target, but you can still compile from command line on Mac by running:

* `msbuild MvvmCross.sln /t:Restore /p:Configuration=Release`
* `msbuild MvvmCross.sln /t:Build /p:Configuration=Release`

On Windows, simply run MvvmCross in Visual Studio. Note: you need Visual Studio 2017 15.7 or higher to be able to compile.

# Make a new release locally

* Open Powershell and run the script
* `.\build.ps1`