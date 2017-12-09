---
layout: post
title: MvvmCross 5.6
date:   2017-11-23 14:00:00 +0100
categories: mvvmcross
---

# Announcing MvvmCross 5.6!

A new MvvmCross version is available on [NuGet](https://www.nuget.org/packages/MvvmCross/5.6.0)! You can always find the latest [changelog in the root of the repository](https://github.com/MvvmCross/MvvmCross/blob/develop/CHANGELOG.md) to see what has changed.

## Much improved support for TvOS
This time we have improved support for TvOS.  We have a brand new ViewPresenter (`MvxTvosViewPresenter`) that uses the same attribute system that the other ViewPresenters.  This is a breaking change and if you were using prior versions you will need to update your app to support the new ViewPresenter.  It supports the following presentation modes:

- Stack navigation
- Tabs
- SplitView (Master/Detail)
- Modal
- Modal navigation

If your app needs another kind of presentation mode, you can also easily extend it!  

## TvOS Presentation Attributes
The presenter uses a set of `PresentationAttributes` to define how a view will be displayed. The existing attributes are:

* MvxRootPresentationAttribute
* MvxChildPresentationAttribute
* MvxModalPresentationAttribute
* MvxTabPresentationAttribute
* MvxMasterDetailPresentationAttribute

## TvOS Sample please!
You can browse the code of the [Playground](https://github.com/MvvmCross/MvvmCross/tree/master/TestProjects/Playground) tvOS project to see this presenter with attributes in action.

## TvOS StarterPack Nuget Package
We will not be providing a nuget package in this release.  To setup your tvOS application add the standard nuget packages to your project and setup your AppDelegate and Setup class like the sample on the Playground.

## MacOS improvements

https://github.com/MvvmCross/MvvmCross/pull/2432

## UWP susension fixes

https://github.com/MvvmCross/MvvmCross/pull/2434

## Android fragment changes

https://github.com/MvvmCross/MvvmCross/pull/2420

## Xamarin.Forms bug fixes

https://github.com/MvvmCross/MvvmCross/pull/2429

## Events on ChangePresentation

https://github.com/MvvmCross/MvvmCross/pull/2448

# Change Log

Coming soon!