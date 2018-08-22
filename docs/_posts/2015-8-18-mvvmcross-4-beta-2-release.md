---
layout: post
title: Second MvvmCross 4 beta
date:   2015-08-18 11:37:35 +0100
categories: mvvmcross
---

UWP support and other improvements!

This is the second beta for MvvmCross 4! In this release we fixed:

- Added UWP projects support [#1101](https://github.com/MvvmCross/MvvmCross/pull/1101)
- Improved support for 'dotnet' nugets [#1089](https://github.com/MvvmCross/MvvmCross/pull/1089)
- A nicer way to handle handling multiple presentation hints [#1097](https://github.com/MvvmCross/MvvmCross/pull/1097)
- Versioned dlls [#1095](https://github.com/MvvmCross/MvvmCross/pull/1095)
- Added Text focus bindings [#1094](https://github.com/MvvmCross/MvvmCross/pull/1094)
- And more small fixes

The new release is available on [Nuget](https://www.nuget.org/packages?q=mvvmcross).

Note that when you use nuget 2.8.5 or below you might get errors like: 'MvvmCross.HotTuna.CrossCore' already has a dependency defined for 'MvvmCross.HotTuna.Binding'.
In that case you can update mannually using [Nuget command line](https://github.com/MvvmCross/MvvmCross/issues/1088#issuecomment-130408367)

Please also note that this release is built with Visual Studio 2015, which means that there is no longer Windows 8 or Windows Phone 8 support. Visual Studio 2015 requires to retarget these projects to Windows 8.1 and Windows Phone 8.1.

See the [issue list](https://github.com/MvvmCross/MvvmCross/issues?q=milestone%3A4.0.0+is%3Aclosed) for an overview of all solved issue's and merged pull requests.
Those lists are also available for [Android Support](https://github.com/MvvmCross/MvvmCross-AndroidSupport/issues?q=milestone%3A4.0.0+is%3Aclosed) and [MvvmCross Forms](https://github.com/MvvmCross/MvvmCross-Forms/issues?q=milestone%3A4.0.0+is%3Aclosed)

If you need any help you can get in touch via the [JabbR #mvvmcross](https://jabbr.net/#/rooms/mvvmcross) room, or open a topic on [StackOverflow](http://stackoverflow.com/questions/new/mvvmcross) and tag it with "MvvmCross".

Let us know if you find any bugs!

- //Martijn00
- //Cheesebaron ʕ•̫͡•ʔ
