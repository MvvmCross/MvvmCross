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

## [5.6.2](https://github.com/MvvmCross/MvvmCross/tree/5.6.2) (2017-12-11)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.6.1...5.6.2)

## [5.6.1](https://github.com/MvvmCross/MvvmCross/tree/5.6.1) (2017-12-11)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.6.0...5.6.1)

**Merged pull requests:**

- Fix infinite loop in modal when having an open modal [\#2457](https://github.com/MvvmCross/MvvmCross/pull/2457) ([martijn00](https://github.com/martijn00))
- Allow RootViewController animations with attributes [\#2453](https://github.com/MvvmCross/MvvmCross/pull/2453) ([willsb](https://github.com/willsb))
- Dictionary conversion [\#2444](https://github.com/MvvmCross/MvvmCross/pull/2444) ([Tyron18](https://github.com/Tyron18))

## [5.6.0](https://github.com/MvvmCross/MvvmCross/tree/5.6.0) (2017-12-10)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.5.2...5.6.0)

**Fixed bugs:**

- MvxBottomSheetDialogFragment doesn't forward View events to ViewModel [\#2431](https://github.com/MvvmCross/MvvmCross/issues/2431)
- override Close is not called when Mac Window is closed using x button [\#2199](https://github.com/MvvmCross/MvvmCross/issues/2199)
- UWP MvxSuspensionManager does not call ReloadState nor ReloadFromBundle after migration to mvvmcross 5.4 [\#2388](https://github.com/MvvmCross/MvvmCross/issues/2388)
- macOS: Add lifecycle events and presenter improvement [\#2432](https://github.com/MvvmCross/MvvmCross/pull/2432) ([nmilcoff](https://github.com/nmilcoff))
- Fix MvxViewModelRequest for fragments and improve ViewDestroy callback [\#2420](https://github.com/MvvmCross/MvvmCross/pull/2420) ([nmilcoff](https://github.com/nmilcoff))

**Closed issues:**

- Preserve.cs [\#2446](https://github.com/MvvmCross/MvvmCross/issues/2446)
- ShowNestedFragment throws exception when host fragment is not visible [\#2442](https://github.com/MvvmCross/MvvmCross/issues/2442)
- Fragments inside ViewPager are not restored  [\#2403](https://github.com/MvvmCross/MvvmCross/issues/2403)
- MvxTableViewSource CollectionChangedOnCollectionChanged can be executed on a worker thread [\#2360](https://github.com/MvvmCross/MvvmCross/issues/2360)
- Change Event subscriptions in Target Bindings on iOS to Weak [\#2145](https://github.com/MvvmCross/MvvmCross/issues/2145)
- Forms: Can not assign MvxListView CachingStrategy in XAML [\#2341](https://github.com/MvvmCross/MvvmCross/issues/2341)
- Update tvOS presenters to the new iOS presenter [\#2108](https://github.com/MvvmCross/MvvmCross/issues/2108)
- UWP After upgrading to MvvmCross 5.0.3 exception is thrown in MvxSuspensionManager.SaveAsync\(\) [\#1970](https://github.com/MvvmCross/MvvmCross/issues/1970)

**Merged pull requests:**

- Improve iOS bindings [\#2452](https://github.com/MvvmCross/MvvmCross/pull/2452) ([willsb](https://github.com/willsb))
- Add hint to set the current page in a parent page [\#2451](https://github.com/MvvmCross/MvvmCross/pull/2451) ([martijn00](https://github.com/martijn00))
- added changes for tvOS [\#2450](https://github.com/MvvmCross/MvvmCross/pull/2450) ([biozal](https://github.com/biozal))
- Add event hooks to MvxNavigationService.ChangePresentation [\#2448](https://github.com/MvvmCross/MvvmCross/pull/2448) ([nmilcoff](https://github.com/nmilcoff))
- tvOS Split View Presentation Support [\#2443](https://github.com/MvvmCross/MvvmCross/pull/2443) ([biozal](https://github.com/biozal))
- MvxTableViewSource.CollectionChangedOnCollectionChanged: Allow execution from a worker thread [\#2441](https://github.com/MvvmCross/MvvmCross/pull/2441) ([nmilcoff](https://github.com/nmilcoff))
- Fix Playground.Droid tabs [\#2439](https://github.com/MvvmCross/MvvmCross/pull/2439) ([nmilcoff](https://github.com/nmilcoff))
- Fixes \#2431 MvxBottomSheetDialogFragment not forwarding events [\#2435](https://github.com/MvvmCross/MvvmCross/pull/2435) ([tbalcom](https://github.com/tbalcom))
- Adding reloading to existing view models [\#2434](https://github.com/MvvmCross/MvvmCross/pull/2434) ([nickrandolph](https://github.com/nickrandolph))
- Fixing issue where icon disappears when navigating within master deta… [\#2429](https://github.com/MvvmCross/MvvmCross/pull/2429) ([nickrandolph](https://github.com/nickrandolph))
- tvOS presentation update to match features in other platforms [\#2414](https://github.com/MvvmCross/MvvmCross/pull/2414) ([biozal](https://github.com/biozal))

## [5.5.2](https://github.com/MvvmCross/MvvmCross/tree/5.5.2) (2017-11-29)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.5.1...5.5.2)

**Fixed bugs:**

- MvxObservableCollection:  "Add" and "AddRange" methods generates an event arguments with different structures. [\#2338](https://github.com/MvvmCross/MvvmCross/issues/2338)
- Fixes Bugs in UWP PresentationAttribute handling [\#2424](https://github.com/MvvmCross/MvvmCross/pull/2424) ([strebbin](https://github.com/strebbin))
- Fixing missing icon on ios [\#2416](https://github.com/MvvmCross/MvvmCross/pull/2416) ([nickrandolph](https://github.com/nickrandolph))
- Fixing issue with default page presentationattribute where viewmodelt… [\#2409](https://github.com/MvvmCross/MvvmCross/pull/2409) ([nickrandolph](https://github.com/nickrandolph))
- Fixes MvxObservableCollection.AddRange firing wrong changed event [\#2407](https://github.com/MvvmCross/MvvmCross/pull/2407) ([MKuckert](https://github.com/MKuckert))
- Fixing double-navigation when navigation is hosted within master-detail [\#2406](https://github.com/MvvmCross/MvvmCross/pull/2406) ([nickrandolph](https://github.com/nickrandolph))

**Merged pull requests:**

- Fixed broken MvxUIDatePickerCountdownDurationTargetBinding. [\#2419](https://github.com/MvvmCross/MvvmCross/pull/2419) ([DaRosenberg](https://github.com/DaRosenberg))
- Add UIBarButtonItem target binding [\#2413](https://github.com/MvvmCross/MvvmCross/pull/2413) ([Cheesebaron](https://github.com/Cheesebaron))
- Add change presentation hints to Forms to allow pop [\#2412](https://github.com/MvvmCross/MvvmCross/pull/2412) ([martijn00](https://github.com/martijn00))
- Fixing issue where content page with attributes set to NoHistory and … [\#2410](https://github.com/MvvmCross/MvvmCross/pull/2410) ([nickrandolph](https://github.com/nickrandolph))

## [5.5.1](https://github.com/MvvmCross/MvvmCross/tree/5.5.1) (2017-11-29)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.5.0...5.5.1)

**Fixed bugs:**

- UWP Presentation Attributes not working correctly [\#2423](https://github.com/MvvmCross/MvvmCross/issues/2423)
- empty view\(xaml page not loading\) [\#2404](https://github.com/MvvmCross/MvvmCross/issues/2404)

**Closed issues:**

- BarBackgroundColor does not work in the UWP [\#2405](https://github.com/MvvmCross/MvvmCross/issues/2405)
