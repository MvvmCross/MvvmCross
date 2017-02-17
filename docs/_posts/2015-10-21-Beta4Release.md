---
layout: post
title: MvvmCross 4.0-beta4
date:   2017-02-13 11:37:35 +0100
categories: jekyll update
---

New logo, Android support improvements, UWP presenter and bug fixes

In this release we introduced the first part of a series of changes to the website, blog and documentation: the logo!
We've changed it for all the Nuget packages, the current blog and other communication channels. We are proud to present it here:

![alt text](http://i.imgur.com/BvdAtgT.png "New MvvmCross Logo")

In this release we fixed:

New projects for Android Support packages

- Leanback [#90](https://github.com/MvvmCross/MvvmCross-AndroidSupport/pull/90) and [#99](https://github.com/MvvmCross/MvvmCross-AndroidSupport/pull/99)
- Preference [#91](https://github.com/MvvmCross/MvvmCross-AndroidSupport/pull/91)
- Design [#103](https://github.com/MvvmCross/MvvmCross-AndroidSupport/pull/103)

Note that some Android packages are on beta5 because beta4 had some issue's.

The [samples for Android-Support](https://github.com/MvvmCross/MvvmCross-AndroidSupport/tree/master/Samples) have been improved to handle more situations in an apps lifecycle, and show the new features MvvmCross has.
If you are missing functionality in the new projects please [add an issue](https://github.com/MvvmCross/MvvmCross-AndroidSupport/issues/new) so we can look into it. Any help / PR's would be appreciated!

- A custom multiple region presenter for Windows UWP has been added [#1157](https://github.com/MvvmCross/MvvmCross/pull/1157)
- New Android specific MvxPropertyChangedListener [#1145](https://github.com/MvvmCross/MvvmCross/pull/1145)
- Possibility to get fragment info by tag in MvxCachingFragmentActivity [#1143](https://github.com/MvvmCross/MvvmCross/pull/1143)
- MvvmCross Forms updated to the latest version [#25](https://github.com/MvvmCross/MvvmCross-Forms/pull/25)

The assembly versions on plugins have been bumped to 4.0.0.0. If you run into trouble after updating, make sure that you have all references set to the latest version.

# Overview of all repo's

- [MvvmCross](https://github.com/MvvmCross/MvvmCross)
- [Android-Support](https://github.com/MvvmCross/MvvmCross-AndroidSupport)
- [Forms](https://github.com/MvvmCross/MvvmCross-Forms)
- [Plugins](https://github.com/MvvmCross/MvvmCross-Plugins)
- [Samples](https://github.com/MvvmCross/MvvmCross-Samples)

# Overview of changes in this release

## MvvmCross

- Value type conversions when binding to nfloat, nint or uint fixed
- GetFragmentInfoByTag added in MvxCachingFragmentActivity
- Added missing AssemblyInfo files for many projects
- Added Android specific PropertyChanged listener to avoid crashes
- WindowsCommon plugins are now valid for WindowsUWP projects
- Discover CanExecuteXyz Properties
- Added Multi region presenter for Windows projects
- Removed dependency on Bindings from CrossCore
- Added dependency on CrossCore from Bindings

## MvvmCross-Plugins

- Use BitmapCompat to get image sizes in DownloadCache
- Added missing AssemblyInfo files

## MvvmCross-AndroidSupport

- Improved samples
- Added ReplaceMode for Fragments in MvxCachingFragmentActivity
- Now comparing bundles to determine whether to replace a fragment or not
- GetFragmentInfoByTag added in MvxCachingFragmentActivity
- Built against the new v23 Android Support packages
- Added new porjects for leanback, design and preference

## MvvmCross-Forms

- Updated Xamarin.Forms package
- Built against new MvvmCross betas

The new release is available on [Nuget](https://www.nuget.org/packages?q=mvvmcross).

See the [issue list](https://github.com/MvvmCross/MvvmCross/issues?q=milestone%3A4.0.0+is%3Aclosed) for an overview of all solved issue's and merged pull requests.
Those lists are also available for [Android Support](https://github.com/MvvmCross/MvvmCross-AndroidSupport/issues?q=milestone%3A4.0.0+is%3Aclosed) and [MvvmCross Forms](https://github.com/MvvmCross/MvvmCross-Forms/issues?q=milestone%3A4.0.0+is%3Aclosed)

If you need any help you can get in touch via the [JabbR #mvvmcross](https://jabbr.net/#/rooms/mvvmcross) room, or open a topic on [StackOverflow](http://stackoverflow.com/questions/new/mvvmcross) and tag it with "MvvmCross".

Let us know if you find any bugs!

- Martijn00
- Cheesebaron ʕ•̫͡•ʔ
