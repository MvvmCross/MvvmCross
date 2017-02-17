---
layout: post
title: MvvmCross 4.0-beta3
date:   2017-02-13 11:37:35 +0100
categories: jekyll update
---

Roslyn, SQLite-PCL, and plugin changes

The MvvmCross 4.0-beta3 is here! In this release we fixed:

- Support for Roslyn analyzers and fixes [#1117](https://github.com/MvvmCross/MvvmCross/pull/1117)
- A new SQLite-PCL plugin [Plugins #1](https://github.com/MvvmCross/MvvmCross-Plugins/pull/1) Note: the old SQLite plugin is discontinued and will no longer be updated
- Support for a MvxPageViewController that dynamically creates UIViewController pages via additional child ViewModels [#1121](https://github.com/MvvmCross/MvvmCross/pull/1121)
- The plugins are moved to their own repo [Plugins #3](https://github.com/MvvmCross/MvvmCross-Plugins/pull/3) Note: the namepace of the plugins has changed to "MvvmCross.Plugins.*" now.
- The samples have been moved to their own repo [Samples #1](https://github.com/MvvmCross/MvvmCross-Samples/pull/1)
- And lots of small fixes

Overview of all repo's:

- [MvvmCross](https://github.com/MvvmCross/MvvmCross)
- [Android-Support](https://github.com/MvvmCross/MvvmCross-AndroidSupport)
- [Plugins](https://github.com/MvvmCross/MvvmCross-Plugins)
- [Samples](https://github.com/MvvmCross/MvvmCross-Samples)

The new release is available on [Nuget](https://www.nuget.org/packages?q=mvvmcross).

Note that when you use nuget 2.8.5 or below you might get errors like: 'MvvmCross.HotTuna.CrossCore' already has a dependency defined for 'MvvmCross.HotTuna.Binding'.
In that case you can update mannually using [Nuget command line](https://github.com/MvvmCross/MvvmCross/issues/1088#issuecomment-130408367) or [patch the nuget files](http://forums.xamarin.com/discussion/comment/147377/#Comment_147377)

See the [issue list](https://github.com/MvvmCross/MvvmCross/issues?q=milestone%3A4.0.0+is%3Aclosed) for an overview of all solved issue's and merged pull requests.
Those lists are also available for [Android Support](https://github.com/MvvmCross/MvvmCross-AndroidSupport/issues?q=milestone%3A4.0.0+is%3Aclosed) and [MvvmCross Forms](https://github.com/MvvmCross/MvvmCross-Forms/issues?q=milestone%3A4.0.0+is%3Aclosed)

If you need any help you can get in touch via the [JabbR #mvvmcross](https://jabbr.net/#/rooms/mvvmcross) room, or open a topic on [StackOverflow](http://stackoverflow.com/questions/new/mvvmcross) and tag it with "MvvmCross".

Let us know if you find any bugs!

- //Martijn00
- //Cheesebaron ʕ•̫͡•ʔ
