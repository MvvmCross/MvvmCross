---
layout: post
title: MvvmCross 4.0-beta8
date:   2016-01-14 11:37:35 +0100
categories: mvvmcross
---

The big breaking change
 
We're getting closer to the stable 4.0 release. Yesterday, we pushed out new NuGet packages with all the changes since beta7. There have been many. We have decided to make it more clear what each NuGet package does and have abandoned the `HotTuna` and `Cirrious` from the package names and from the namespaces. Everything that was named `Touch` (as in MonoTouch) is now named `iOS` (as in Xamarin.iOS). Also, we have decided to restructure the project while we were at it. This means going from beta7 to beta8 will be a bit involved, because of these namespace changes. To help you get started with the upgrade, please have a look at the [upgrade documentation](https://mvvmcross.readme.io/docs/upgrade-to-mvvmcross-40). See [the package list](https://mvvmcross.readme.io/docs/installing-mvvmcross) for a full list of the new package names.
We're getting closer to the stable 4.0 release. Yesterday, we pushed out new NuGet packages with all the changes since beta7. There have been many. We have decided to make it more clear what each NuGet package does and have abandoned the `HotTuna` and `Cirrious` from the package names and from the namespaces. Everything that was named `Touch` (as in MonoTouch) is now named `iOS` (as in Xamarin.iOS). Also, we have decided to restructure the project while we were at it. This means going from beta7 to beta8 will be a bit involved, because of these namespace changes. To help you get started with the upgrade, please have a look at the [upgrade documentation](https://mvvmcross.readme.io/docs/upgrade-to-mvvmcross-40). See [the package list](https://mvvmcross.readme.io/docs/installing-mvvmcross) for a full list of the new package names. 
 

Xamarin.Mac
 
There have been changes on the Mac side, too. While versions since 3.5 supported MonoMac, Xamarin.Mac (Classic), Xamarin.Mac (Unified), beta8 and later versions will only support Xamarin.Mac (Unified). The good news: beta8 is the first version to have working Nugets for Mac. If you're new to MvvmCross on Mac, simply create a PCL and a Xamarin.Mac (Unified) project in your solution and add the [`MvvmCross.StarterPack`](https://www.nuget.org/packages/MvvmCross.StarterPack) Nuget to those projects to have sample code created to help you get started.
There have been changes on the Mac side, too. While versions since 3.5 supported MonoMac, Xamarin.Mac (Classic), Xamarin.Mac (Unified), beta8 and later versions will only support Xamarin.Mac (Unified). The good news: beta8 is the first version to have working Nugets for Mac. If you're new to MvvmCross on Mac, simply create a PCL and a Xamarin.Mac (Unified) project in your solution and add the [`MvvmCross.StarterPack`](https://www.nuget.org/packages/MvvmCross.StarterPack) Nuget to those projects to have sample code created to help you get started. 
 

Other changes
 
For a full list of changes see the merged PR's of the repositories.

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
- Kerry
