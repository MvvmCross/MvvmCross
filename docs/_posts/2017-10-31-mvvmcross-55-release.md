---
layout: post
title: MvvmCross 5.5
date:   2017-11-23 14:00:00 +0100
categories: mvvmcross
---

# Announcing MvvmCross 5.5!

A new MvvmCross version is available on [NuGet](https://www.nuget.org/packages/MvvmCross/5.5.0)! You can always find the latest [changelog in the root of the repository](https://github.com/MvvmCross/MvvmCross/blob/develop/CHANGELOG.md) to see what has changed.

## New UWP ViewPresenter

This time it's UWP turn! We have a brand new ViewPresenter that uses the same attribute system as the others ViewPresenters. It support the following presentation modes:

- Page (default)
- SplitView (pane and content)
- Region

Note that as part of this change the old ViewPresenter has been removed. 

In order to use the new attributes, all you need to do is to place any of them over your view class:

```c#
[MvxPagePresentation]
public sealed partial class RootView : MvxWindowsPage
{
    public RootView()
    {
        this.InitializeComponent();
    }
}
```

Check out the [Playground.UWP](https://github.com/MvvmCross/MvvmCross/tree/develop/TestProjects/Playground/Playground.Uwp) sample to see it in action! The new presenter is now also available for Xamarin.Forms on UWP!

## ViewModels lifecycle

We are very happy to announce that we have fixed several issues related to navigation and we have finally stabilized the new ViewModel Lifecycle! This is what it looks like:

1. Construction
2. Prepare
3. Initialize

There is also a stable mechanism to handle tombstoning situations: SaveState and ReloadState issues are solved!

Some other highlights include:
- You can watch the state (and even bind views) of Initialize using the property `ViewModel.InitializeTask`.
- ViewModels loaded manually (using MvxViewModelLoader) do call Prepare and Initialize as expected.

We highly recommend you to read the [ViewModel Lifecycle document](https://www.mvvmcross.com/documentation/fundamentals/viewmodel-lifecycle) to understand how everything works.

## Async operations with MvxNotifyTask

We have added a super useful helper when it comes to async/await: [MvxNotifyTask](https://github.com/MvvmCross/MvvmCross/blob/develop/MvvmCross/Core/Core/ViewModels/MvxNotifyTask.cs).

MvxNotifyTask provides you with an object that can watch for different Task states and raise property-changed notifications that you can subscribe to: This means you can bind any Task properties in your Views.

Other than that, this class acts as a sandbox for async operations: If a Task fails and raises an exception, your app won’t crash, and the exception will be available for you through `MvxNotifyTask.Exception`.

If this looks interesting to you, don't hesitate to read [the official documentation for it](https://www.mvvmcross.com/documentation/fundamentals/mvxnotifytask?scroll=225). And of course, you can start using it in your apps right now!

# Change Log

## [5.5.0](https://github.com/MvvmCross/MvvmCross/tree/5.5.0) (2017-11-23)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.4.2...5.5.0)

**Fixed bugs:**

- Forms: TargetInvocationException when using ShowViewModel with parameter [\#2363](https://github.com/MvvmCross/MvvmCross/issues/2363)
- ViewModel Initialize method not called using MvxViewPagerFragmentInfo [\#2297](https://github.com/MvvmCross/MvvmCross/issues/2297)
- ViewModel's SaveState/ReloadState and NavigationService [\#2167](https://github.com/MvvmCross/MvvmCross/issues/2167)
- Feedback: The new Navigation Service and the life cycle it introduces [\#2105](https://github.com/MvvmCross/MvvmCross/issues/2105)
- UWP MvxSuspensionManager does not call ReloadState nor ReloadFromBundle after migration to mvvmcross 5.4 [\#2388](https://github.com/MvvmCross/MvvmCross/issues/2388)
- MasterDetailExample.UWP crashes with 'System.NullReferenceException: Object reference not set to an instance of an object' [\#2304](https://github.com/MvvmCross/MvvmCross/issues/2304)
- Improvements to UIDatePicker target bindings [\#2375](https://github.com/MvvmCross/MvvmCross/pull/2375) ([DaRosenberg](https://github.com/DaRosenberg))

**Closed issues:**

- AndroidPresenter: Close all Fragments of Activity when doing CloseActivity\(\) [\#2398](https://github.com/MvvmCross/MvvmCross/issues/2398)
- Navigation with Results and Configuration Change cause premature delivery of null result [\#2384](https://github.com/MvvmCross/MvvmCross/issues/2384)
- Could not install package 'MvvmCross.Plugin.PictureChooser 5.4.2' into a "Profile78" project [\#2369](https://github.com/MvvmCross/MvvmCross/issues/2369)
- Initialize not called when manually instantiating an MvxViewModel [\#1972](https://github.com/MvvmCross/MvvmCross/issues/1972)
- UWP crash at launch "Failed to resolve type MvvmCross.Core.Views.IMvxViewPresenter" [\#2397](https://github.com/MvvmCross/MvvmCross/issues/2397)
- Resuming app with tabs as root page causes duplicate page to load as new navigation \(MvvmCross 5.4.2 / MvxTabbedPage root\) [\#2373](https://github.com/MvvmCross/MvvmCross/issues/2373)
- Playground.Forms.Droid can't resume after being hidden by back key [\#2332](https://github.com/MvvmCross/MvvmCross/issues/2332)
- Migration issue from 5.0.3 to 5.1.1 - Application is null / MvxFormsAppCompatActivity / base.OnCreate\(bundle\) / Resolved, but useful info [\#2129](https://github.com/MvvmCross/MvvmCross/issues/2129)

**Merged pull requests:**

- Fix logging in netstandard [\#2399](https://github.com/MvvmCross/MvvmCross/pull/2399) ([willsb](https://github.com/willsb))
- Fix Droid Activities when navigating for result [\#2400](https://github.com/MvvmCross/MvvmCross/pull/2400) ([nmilcoff](https://github.com/nmilcoff))
- Add possibility to get translation directly instead of binding to object [\#2396](https://github.com/MvvmCross/MvvmCross/pull/2396) ([martijn00](https://github.com/martijn00))
- Update documentation about Xamarin.Forms Android [\#2395](https://github.com/MvvmCross/MvvmCross/pull/2395) ([wcoder](https://github.com/wcoder))
- Fix typo in 2017-11-15-joining-net-foundation.md [\#2393](https://github.com/MvvmCross/MvvmCross/pull/2393) ([ejlofgren](https://github.com/ejlofgren))
- Update 2017-11-15-joining-net-foundation.md [\#2391](https://github.com/MvvmCross/MvvmCross/pull/2391) ([Cheesebaron](https://github.com/Cheesebaron))
- Add .net foundation announcement post [\#2390](https://github.com/MvvmCross/MvvmCross/pull/2390) ([Cheesebaron](https://github.com/Cheesebaron))
- Fixing Master-Detail Forms implementation [\#2387](https://github.com/MvvmCross/MvvmCross/pull/2387) ([nickrandolph](https://github.com/nickrandolph))
- Fixing visual studio load issue with playground projects [\#2386](https://github.com/MvvmCross/MvvmCross/pull/2386) ([nickrandolph](https://github.com/nickrandolph))
- Update docs for registering unbound generics. [\#2383](https://github.com/MvvmCross/MvvmCross/pull/2383) ([Slowbad](https://github.com/Slowbad))
- Add ability to show a different host view for a Forms page [\#2382](https://github.com/MvvmCross/MvvmCross/pull/2382) ([martijn00](https://github.com/martijn00))
- Separation of concerns for Playground sample v2 [\#2381](https://github.com/MvvmCross/MvvmCross/pull/2381) ([nickrandolph](https://github.com/nickrandolph))
- Add support for multiple resources in resx files [\#2380](https://github.com/MvvmCross/MvvmCross/pull/2380) ([martijn00](https://github.com/martijn00))
- Improve parameter overview [\#2377](https://github.com/MvvmCross/MvvmCross/pull/2377) ([mvanbeusekom](https://github.com/mvanbeusekom))
- Add information on how to run Jekyll locally [\#2376](https://github.com/MvvmCross/MvvmCross/pull/2376) ([mvanbeusekom](https://github.com/mvanbeusekom))
- Update doc to net standard 2 support [\#2371](https://github.com/MvvmCross/MvvmCross/pull/2371) ([Plac3hold3r](https://github.com/Plac3hold3r))
- Make builds faster by enabling parallel build [\#2367](https://github.com/MvvmCross/MvvmCross/pull/2367) ([Cheesebaron](https://github.com/Cheesebaron))
- Improve UWP view presenter to use presentation attributes v2 [\#2366](https://github.com/MvvmCross/MvvmCross/pull/2366) ([nickrandolph](https://github.com/nickrandolph))
- Load viewmodels via request when loading directly [\#2364](https://github.com/MvvmCross/MvvmCross/pull/2364) ([martijn00](https://github.com/martijn00))
- Run Prepare and Initialize from MvxViewModelLoader [\#2359](https://github.com/MvvmCross/MvvmCross/pull/2359) ([nmilcoff](https://github.com/nmilcoff))

## [5.4.2](https://github.com/MvvmCross/MvvmCross/tree/5.4.2) (2017-11-07)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.4.1...5.4.2)

**Fixed bugs:**

- Setting Detail in MvxMasterDetailView added to stack and not replacing Detail [\#2347](https://github.com/MvvmCross/MvvmCross/issues/2347)
- Unable to use MvxTabbedPage in MvvmCross 5.4.0 [\#2345](https://github.com/MvvmCross/MvvmCross/issues/2345)
- Default root page will not load, if the MasterDetailPage is the app startup page [\#2309](https://github.com/MvvmCross/MvvmCross/issues/2309)
- Strange behaviour of the navigation stack with MVX [\#2308](https://github.com/MvvmCross/MvvmCross/issues/2308)
- Toolbar color can not be changed [\#2301](https://github.com/MvvmCross/MvvmCross/issues/2301)
- Add support for nested root pages of any type [\#2361](https://github.com/MvvmCross/MvvmCross/pull/2361) ([martijn00](https://github.com/martijn00))

**Closed issues:**

- MvxTabLayoutPresentation not working in Fragment [\#2335](https://github.com/MvvmCross/MvvmCross/issues/2335)
- IMvxNavigationService Stack Issue in Xamarin Forms [\#2202](https://github.com/MvvmCross/MvvmCross/issues/2202)

## [5.4.1](https://github.com/MvvmCross/MvvmCross/tree/5.4.1) (2017-11-07)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.4.0...5.4.1)

**Fixed bugs:**

- Xamarin.Forms / Setting NoHistory member has no effect [\#2320](https://github.com/MvvmCross/MvvmCross/issues/2320)
- mvx:MvxBind binding structure inconsistant [\#2299](https://github.com/MvvmCross/MvvmCross/issues/2299)
- Rotation crashes device [\#2274](https://github.com/MvvmCross/MvvmCross/issues/2274)
- iOS application crashes when start running on real device [\#2351](https://github.com/MvvmCross/MvvmCross/issues/2351)
- Null reference exception in type initializer for ConsoleLogProvider on iOS 11 device [\#2342](https://github.com/MvvmCross/MvvmCross/issues/2342)
- 'System.TypeInitializationException' In 'MvvmCross.Core.Platform.LogProviders.ConsoleLogProvider' On UWP Projects [\#2333](https://github.com/MvvmCross/MvvmCross/issues/2333)
- App fails to launch when initial page has WrapInNavigationPage = false [\#2329](https://github.com/MvvmCross/MvvmCross/issues/2329)

**Closed issues:**

- MvxLayoutInflater Disposed exception [\#1924](https://github.com/MvvmCross/MvvmCross/issues/1924)
- Win10 Uap + MvxSuspensionManager + .Net Native  [\#1148](https://github.com/MvvmCross/MvvmCross/issues/1148)
- mvx:MvxBind not setting selected item in MvxListView [\#2355](https://github.com/MvvmCross/MvvmCross/issues/2355)
- UIDatePikcer CountdownDuration Binding [\#2352](https://github.com/MvvmCross/MvvmCross/issues/2352)

**Merged pull requests:**

- Adds CountDownDuration target binding for UIDatePicker [\#2353](https://github.com/MvvmCross/MvvmCross/pull/2353) ([sfmskywalker](https://github.com/sfmskywalker))
- Add event source for Forms views [\#2357](https://github.com/MvvmCross/MvvmCross/pull/2357) ([martijn00](https://github.com/martijn00))
- Fix bindings without forms base type [\#2356](https://github.com/MvvmCross/MvvmCross/pull/2356) ([martijn00](https://github.com/martijn00))
- Improve issue template [\#2354](https://github.com/MvvmCross/MvvmCross/pull/2354) ([willsb](https://github.com/willsb))
- Make no history work with nested stacks [\#2350](https://github.com/MvvmCross/MvvmCross/pull/2350) ([martijn00](https://github.com/martijn00))
- Remove duplicate heading [\#2349](https://github.com/MvvmCross/MvvmCross/pull/2349) ([Cheesebaron](https://github.com/Cheesebaron))
- Update ViewPresenter files and add links for docs [\#2346](https://github.com/MvvmCross/MvvmCross/pull/2346) ([nmilcoff](https://github.com/nmilcoff))
- Update view-presenters.md [\#2344](https://github.com/MvvmCross/MvvmCross/pull/2344) ([turibbio](https://github.com/turibbio))
- Add linker include to forms starterpack [\#2343](https://github.com/MvvmCross/MvvmCross/pull/2343) ([martijn00](https://github.com/martijn00))
- Cleanup of logging within Setup [\#2339](https://github.com/MvvmCross/MvvmCross/pull/2339) ([martijn00](https://github.com/martijn00))
- Fix replacing of mainpage root in forms [\#2337](https://github.com/MvvmCross/MvvmCross/pull/2337) ([martijn00](https://github.com/martijn00))
- Fix console log provider [\#2334](https://github.com/MvvmCross/MvvmCross/pull/2334) ([willsb](https://github.com/willsb))
