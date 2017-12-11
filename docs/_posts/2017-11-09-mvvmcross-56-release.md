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

We've added extension methods for Segues in Storyboard on MacOS! Also it now supports view events in the viewmodels. Finally there are some bug fixes in the presenter.

## UWP suspension fixes

There were some problems on UWP with reloading viewmodels in `OnSuspending`. This has now been fixed, look at the Playground sample for UWP for the implementation.

## Android fragment changes

When using Fragments and for example rotating the device, the viewmodel was not restored correctly. This has now been fixed.

We also change the ViewModel parameter for ViewDestroy to indicate if it just destroys the view, or if it is fully killed.

```c#
public override void ViewDestroy(bool viewFinishing = true)
{
}
```

## Xamarin.Forms bug fixes

For Forms we've fixed a couple of issue's with the master detail implementation, modals and other small improvements.

There are now new presentation hints that lets you set the current page in a `TabbedPage` or `CarouselPage` that already is open. The second one is to remove a `Page` from the stack. Also there are hints to pop and pop to root of the stack.

```c#
_navigationService.ChangePresentation(new MvxPagePresentationHint(typeof(Tab1ViewModel)));
_navigationService.ChangePresentation(new MvxRemovePresentationHint(typeof(Tab1ViewModel)));
_navigationService.ChangePresentation(new MvxPopPresentationHint(typeof(Tab1ViewModel)));
_navigationService.ChangePresentation(new MvxPopToRootPresentationHint());
```

## Events on ChangePresentation

You can now subscribe to events on ChangePresentation in the `MvxNavigationService`.

# Change Log

Coming soon!
