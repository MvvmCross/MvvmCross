---
layout: post
title: MvvmCross 4.0-beta5
date:   2017-02-13 11:37:35 +0100
categories: jekyll update
---

Cross-platform sidemenu sample, fragment and plugin fixes


- Fixed bug to set images on UI thread: [commit](https://github.com/MvvmCross/MvvmCross/commit/a63eade49ae15f00a4e9305f45e9b6675c0f0e7d)
- WPF for SQLite PCL plugin [#26](https://github.com/MvvmCross/MvvmCross-Plugins/pull/26)
- Added methods to reload a viewmodel when coming from the background [#1165](https://github.com/MvvmCross/MvvmCross/pull/1165)
- Added more support for Android Leanback [#107](https://github.com/MvvmCross/MvvmCross-AndroidSupport/pull/107)
- Refactored caching fragments to properly handle all situations [#86](https://github.com/MvvmCross/MvvmCross-AndroidSupport/pull/86)

Thanks to [Jammer](https://github.com/jamsoft) there is a new [cross-platform sample](https://github.com/MvvmCross/MvvmCross-Samples/tree/master/Samples/XPlatformMenus) which uses sidemenu to navigate.

# Overview of all repo's

- [MvvmCross](https://github.com/MvvmCross/MvvmCross)
- [Android-Support](https://github.com/MvvmCross/MvvmCross-AndroidSupport)
- [Forms](https://github.com/MvvmCross/MvvmCross-Forms)
- [Plugins](https://github.com/MvvmCross/MvvmCross-Plugins)
- [Samples](https://github.com/MvvmCross/MvvmCross-Samples)


The new release is available on [Nuget](https://www.nuget.org/packages?q=mvvmcross).

See the [issue list](https://github.com/MvvmCross/MvvmCross/issues?q=milestone%3A4.0.0+is%3Aclosed) for an overview of all solved issue's and merged pull requests.
Those lists are also available for [Android Support](https://github.com/MvvmCross/MvvmCross-AndroidSupport/issues?q=milestone%3A4.0.0+is%3Aclosed) and [MvvmCross Forms](https://github.com/MvvmCross/MvvmCross-Forms/issues?q=milestone%3A4.0.0+is%3Aclosed)

If you need any help you can get in touch via [Slack](https://xamarinchat.herokuapp.com/), or open a topic on [StackOverflow](http://stackoverflow.com/questions/new/mvvmcross) and tag it with "MvvmCross".

Let us know if you find any bugs!

- Martijn00
- Cheesebaron ʕ•̫͡•ʔ
