---
layout: post
title: MvvmCross 6.2
date:   2018-4-14 14:00:00 -0300
categories: mvvmcross
---

# Announcing MvvmCross 6.2!

A new MvvmCross version is available on [NuGet](https://www.nuget.org/packages/MvvmCross/6.2.0)! You can always find the latest [changelog in the root of the repository](https://github.com/MvvmCross/MvvmCross/blob/develop/CHANGELOG.md) to see what has changed.

Here is a quick overview:

## About the MvvmCross project

### Signed NuGet packages

You might have already noticed, but we are pleased to announce that our NuGet packages are now signed! You can now verify their origin.

### Improving our community 

[@nickrandolph](https://github.com/nickrandolph) has started a great discussion about building a more active community of contributors. Please feel free to jump [into the conversation](https://github.com/MvvmCross/MvvmCross/issues/3034) and help us take MvvmCross to the next level!

We are currently working on making use of GitHub teams to delegate and distribute responsibilities. As part of this work a codeowners file has been [added](https://github.com/MvvmCross/MvvmCross/pull/3061) (huge thanks to [vatsalyagoel](https://github.com/vatsalyagoel)!).

### Issue templates

[@willsb](https://github.com/willsb) introduced different type of issue templates. When you try to submit a new issue, you will be prompted to choose one among these categories: regression, bug report, feature request, enhancement proposal and question/help.

### Speeding up build times while working in our source code

Thanks to [@nickrandolph](https://github.com/nickrandolph)'s work, you can now set a conditional flag to build a single platform. Please look at [this PR](https://github.com/MvvmCross/MvvmCross/pull/3015) to see the details.


## Core / Shared code

### Breaking change in `MvxViewModel`

Previously, `MvxViewModel` had properties for `IMvxNavigationService` and `IMvxLogProvider` which were resolved internally. After a long discussion we decided it to remove them and add a new ViewModel type: `MvxNavigationViewModel`, which has them as dependencies in the constructor. The interfaces `IMvxNavigationViewModel` and `IMvxLogViewModel` have been removed as well. Please look at [this PR](https://github.com/MvvmCross/MvvmCross/pull/3044) for the details.

### IMvxNavigationService methods now return `Task<bool>` and events like `BeforeNavigateEventHandler` changed a parameter type

- The boolean result indicates success / failure for the navigation action. 
- The parameter of type `NavigateEventArgs` is now `IMvxNavigateEventArgs`. You can now trigger cancellation using that parameter from within your event handler.

### Support for async Startup

[@nickrandolph](https://github.com/nickrandolph) made it possible to perform async operations during the `Initialize` method of the first shown ViewModel. Please just be aware that this could have a direct impact on the UX of your app (SplashScreen could take longer to dissapear). 

__Note:__ This change introduced a few breaking changes: Some methods now return `Task` instead of void.

### ViewPresenter methods are now async

Async operations in ViewPresenter have always been fire and forget. But thanks to [@nickrandolph](https://github.com/nickrandolph) this has changed and now all methods return `Task<bool>` and async operations are `awaited`. This change prevents some race conditions and weird issues that you might have had while using MvvmCross.

### Changes to `Mvx`

As part of a code improvement, `Mvx` methods have been marked as obsolete and will be removed probably for v7. You should update your app and replace calls like `Mvx.Resolve` for `Mvx.IoCProvider.Resolve`.

## Xamarin.Forms

- Compatibility with Xamarin.Forms 3.1 has been added. Please note that your app needs to be updated to that version as well.
- On Android, the method `MvxFormsAppCompatActivity.OnBackPressed` assumed the app was using the default `MvxFormsPagePresenter` and this could cause your app to crash in some cases. The problem has been fixed.
- Do you use the XAML Designer in your MvvmCross app? Yes? Then good news! [@cheesebaron](https://github.com/cheesebaron) [fixed](https://github.com/MvvmCross/MvvmCross/pull/3094) a problem which caused our controls not to be rendered correctly.
- If you tried to resolve `IMvxFormsPagePresenter` using the IoC you could see your app crash. Thanks to [vatsalyagoel](https://github.com/vatsalyagoel) this has been [fixed](https://github.com/MvvmCross/MvvmCross/pull/2972).
- If a binding had something else than a page as a root, your app could crash. Thanks to [@martijn00](https://github.com/martijn00) this has been [fixed](https://github.com/MvvmCross/MvvmCross/pull/3002).
- UWP apps were [crashing](https://github.com/MvvmCross/MvvmCross/pull/3023) when being ressumed. Thanks to [@nickrandolph](https://github.com/nickrandolph) this doesn't happen anymore.
- You can now reuse you ValueConverters within Xamarin.Forms XAML, following the same strategy that makes them usable in UWP (`MvxNativeValueConverter` has been added). If you want to see this in detail, please look at the awesome [PR](https://github.com/MvvmCross/MvvmCross/pull/3047) raised by [@MartinZikmund](https://github.com/MartinZikmund).

## Android

- We have improved our support for [nested fragments](https://github.com/MvvmCross/MvvmCross/pull/3001) overall. ViewPager Fragments are now correctly managed when the ViewPager's host is a Fragment and not an Activity. Thanks to [@nmilcoff](https://github.com/nmilcoff) for working on that.
- `MvxAutoCompleteTextView` binding for PartialText was not working correctly. Thanks to [@cheesebaron](https://github.com/cheesebaron) this has been [fixed](https://github.com/MvvmCross/MvvmCross/pull/3027).
- We have fixed a memory leak related to our ViewModel caching mechanism. If you want to see the details please look at the [PR](https://github.com/MvvmCross/MvvmCross/pull/3055) raised by [@nmilcoff](https://github.com/nmilcoff).
- In certain scenarios, the first shown Activity was navigated to twice. Thanks to [@tbalcom](https://github.com/tbalcom) for detecting and fixing this problem!
- If you repeatedly pressed the back button while the SplashScreen was being shown in your app, you might have noticed that it crashed. Thanks to [@tbalcom](https://github.com/tbalcom) for applying a [fix](https://github.com/MvvmCross/MvvmCross/pull/3085).
- The default `DropDownItemTemplate` for `MvxAppCompatSpinner` now displays a string formed by `yourModel.ToString()` instead of nothing. This improvement has been done by [@tbalcom](https://github.com/tbalcom).
- You can now bind `Android.Support.V7.Preferences.Preference.PreferenceClick` to an `ICommand`. This addition was made by [@tbalcom](https://github.com/tbalcom).
- `MvxApplicationCallbacksCurrentTopActivity.Activities` is now protected, which makes it easier to customize. This improvement has been made by [@daividssilverio](https://github.com/daividssilverio).
- `MvxAppCompatViewPresenter` now supports `MvxPagePresentationHint`, which in other words means that you can now change the selected tab of a ViewPager by using `navigationService.ChangePresentation`. This implementation was introduced by [@markuspalme](https://github.com/markuspalme).

## iOS
 
- When binding the ItemsSource property of `MvxExpandableTableViewSource`, a `AmbiguousMatchException` was thrown. Thanks to [@cheesebaron](https://github.com/cheesebaron) this has been [fixed](https://github.com/MvvmCross/MvvmCross/pull/3024).
- We found out that our default LinkerPleaseInclude was missing some methods for `UITextField`. Please make sure to grab the latest version from [here](https://github.com/MvvmCross/MvvmCross/blob/master/ContentFiles/iOS/LinkerPleaseInclude.cs).
- You can now use extension methods in Fluent bindings to bind `Preference.PreferenceClick` and `UIBarButtonItem.Clicked`. Huge thanks to [@Plac3hold3r](https://github.com/Plac3hold3r).
- `MvxIosViewPresenter` now supports `MvxPagePresentationHint`, which in other words means that you can now change the selected tab by using `navigationService.ChangePresentation`. This implementation was introduced by [@markuspalme](https://github.com/markuspalme).
- `MvxTabBarViewController.SelectedViewController` now returns null instead of throwing a `NullReferenceException`. Kudos to [@andrechi1](https://github.com/andrechi1)!

## macOS

- `MvxTableViewSource.GetOrCreateViewFor` is now virtual, and you can provide your own View. This improvement has been made by [@cheesebaron](https://github.com/cheesebaron).

## WPF

- `MvxWpfViewPresenter` used to assume your app was using `MvxWindow`s instead of `IMvxWindow`s. This isn't the case anymore thanks to [@cheesebaron](https://github.com/cheesebaron).

## Plugins

- `PictureChooser` was not working properly on iOS because of a crash when being injected. The creation of some components has been delayed so that this doesn't happen. Just make sure you make the plugin calls from the UI thread (iOS needs this!). Kudos to [@cheesebaron](https://github.com/cheesebaron)!

## Docs

- `MvxRecyclerView` is now properly documented thanks to [@cheesebaron](https://github.com/cheesebaron)'s work.
- `MvxSpinner` is now properly documented. Kudos to [@cheesebaron](https://github.com/cheesebaron)!
- Links to code were fixed by [@nmilcoff](https://github.com/nmilcoff). 


## Others

- The framework now supports raising `PropertyChanging` events. This is enabled by default. Thanks to [@nickrandolph](https://github.com/nickrandolph) for implementing [this](https://github.com/MvvmCross/MvvmCross/pull/2943).
- If you are using `Fody.PropertyChanged` please be sure you upgrade your app to the latest version of that package. [@borbmizzet](https://github.com/borbmizzet) has made [a PR](https://github.com/Fody/PropertyChanged/pull/346) fixing our compatibility with it.
- Exceptions raised in event handlers for `PropertyChanged` won't make your app crash anymore. Thanks to [@nickrandolph](https://github.com/nickrandolph) for this improvement.
- When cancelling a navigation to a ViewModel, the action wasn't always being stopped. This has been [fixed](https://github.com/MvvmCross/MvvmCross/pull/3006) by [@martijn00](https://github.com/martijn00).
- Exceptions raised during the initialization of Setup are no longer swallowed. This fix was done by [@nickrandolph](https://github.com/nickrandolph).
- When using Fluent bindings, you can now take advantage of the newly introduced method `ApplyWithClearBindingKey`. You can of course clear the entire binding set later on. This addition was done by [@Plac3hold3r](https://github.com/Plac3hold3r).






# Change Log

## [6.0.0](https://github.com/MvvmCross/MvvmCross/tree/6.0.0) (2018-04-14)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/6.0.0-beta8...6.0.0)

**Fixed bugs:**

- Playground.Droid creates multiple instances of RootViewModel [\#2782](https://github.com/MvvmCross/MvvmCross/issues/2782)
- MvvmCross doesn't work with F\# Android app resources [\#2772](https://github.com/MvvmCross/MvvmCross/issues/2772)
- Xamarin Forms Images is not shown on Android when using mvvmcross [\#2770](https://github.com/MvvmCross/MvvmCross/issues/2770)
- UWP Photo chooser distorts photo on windows phone [\#2588](https://github.com/MvvmCross/MvvmCross/issues/2588)
- Fixing issue with CurrentActivity being null in Playground.Droid [\#2775](https://github.com/MvvmCross/MvvmCross/pull/2775) ([nickrandolph](https://github.com/nickrandolph))
- Find resource type based on Android.Runtime.ResourceDesignerAttribute [\#2774](https://github.com/MvvmCross/MvvmCross/pull/2774) ([nosami](https://github.com/nosami))

**Closed issues:**

- Generic UWP views break compiled bindings [\#2653](https://github.com/MvvmCross/MvvmCross/issues/2653)

**Merged pull requests:**

- Making it easier to override creation of injected pages [\#2793](https://github.com/MvvmCross/MvvmCross/pull/2793) ([nickrandolph](https://github.com/nickrandolph))
- Improving Forms Android support for setup [\#2790](https://github.com/MvvmCross/MvvmCross/pull/2790) ([nickrandolph](https://github.com/nickrandolph))
- Split out WPF [\#2789](https://github.com/MvvmCross/MvvmCross/pull/2789) ([martijn00](https://github.com/martijn00))
- Update 3rd-party-plugins.md [\#2788](https://github.com/MvvmCross/MvvmCross/pull/2788) ([vurf](https://github.com/vurf))
- Bugfix/aapt error workaround [\#2787](https://github.com/MvvmCross/MvvmCross/pull/2787) ([Cheesebaron](https://github.com/Cheesebaron))

## [6.0.0-beta8](https://github.com/MvvmCross/MvvmCross/tree/6.0.0-beta8) (2018-04-10)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/6.0.0-beta7...6.0.0-beta8)

**Fixed bugs:**

- Crash when Close Viewmodel With Result using MasterDetail [\#2757](https://github.com/MvvmCross/MvvmCross/issues/2757)
- Adjusting the resolution of the resource assembly [\#2777](https://github.com/MvvmCross/MvvmCross/pull/2777) ([nickrandolph](https://github.com/nickrandolph))
- Make show and close of iOS views respect Animated [\#2767](https://github.com/MvvmCross/MvvmCross/pull/2767) ([martijn00](https://github.com/martijn00))
- MvxUISliderValueTargetBinding: Add missing return [\#2750](https://github.com/MvvmCross/MvvmCross/pull/2750) ([nmilcoff](https://github.com/nmilcoff))

**Closed issues:**

- ViewControllers animation can't be disabled for VM Navigate\(\) / Close\(\) [\#2762](https://github.com/MvvmCross/MvvmCross/issues/2762)
- \[iOS\] Using VS AppCenter "AppCenter.Start" while MvxApplication.Initialize results in deadlock since MVX 6 beta7 [\#2745](https://github.com/MvvmCross/MvvmCross/issues/2745)
- WPF Presenter documentation is out of date [\#2743](https://github.com/MvvmCross/MvvmCross/issues/2743)
- NuGet package descriptions are missing from csproj files [\#2742](https://github.com/MvvmCross/MvvmCross/issues/2742)

**Merged pull requests:**

- Added Margin target binding for Android [\#2780](https://github.com/MvvmCross/MvvmCross/pull/2780) ([Cheesebaron](https://github.com/Cheesebaron))
- Fix duplicated entry for SplitView attribute on iOS ViewPresenter [\#2779](https://github.com/MvvmCross/MvvmCross/pull/2779) ([nmilcoff](https://github.com/nmilcoff))
- Update json.md [\#2778](https://github.com/MvvmCross/MvvmCross/pull/2778) ([dawidstefaniak](https://github.com/dawidstefaniak))
- Make build.sh executable so that `./build.sh` works [\#2773](https://github.com/MvvmCross/MvvmCross/pull/2773) ([nosami](https://github.com/nosami))
- Making IMvxViewDispatcher async aware [\#2771](https://github.com/MvvmCross/MvvmCross/pull/2771) ([nickrandolph](https://github.com/nickrandolph))
- Make setup interface standard [\#2769](https://github.com/MvvmCross/MvvmCross/pull/2769) ([martijn00](https://github.com/martijn00))
- Match docs with code for WPF presenter [\#2768](https://github.com/MvvmCross/MvvmCross/pull/2768) ([martijn00](https://github.com/martijn00))
- Descriptions from old nuspecs in csprojs added [\#2766](https://github.com/MvvmCross/MvvmCross/pull/2766) ([orzech85](https://github.com/orzech85))
- Upgrading XF references [\#2764](https://github.com/MvvmCross/MvvmCross/pull/2764) ([nickrandolph](https://github.com/nickrandolph))
- Fixing issue with navigating back with result from master details [\#2763](https://github.com/MvvmCross/MvvmCross/pull/2763) ([nickrandolph](https://github.com/nickrandolph))
- Adding Application Startup method to be invoked on UI thread [\#2761](https://github.com/MvvmCross/MvvmCross/pull/2761) ([nickrandolph](https://github.com/nickrandolph))
- Switch to new Project SDK style for MSBuildExtras + build log [\#2759](https://github.com/MvvmCross/MvvmCross/pull/2759) ([Cheesebaron](https://github.com/Cheesebaron))
- Update appveyor and Android support library [\#2758](https://github.com/MvvmCross/MvvmCross/pull/2758) ([martijn00](https://github.com/martijn00))
- Fix MvvmCross.Forms project dependencies [\#2756](https://github.com/MvvmCross/MvvmCross/pull/2756) ([Cheesebaron](https://github.com/Cheesebaron))
- Adding Async dispatcher [\#2755](https://github.com/MvvmCross/MvvmCross/pull/2755) ([nickrandolph](https://github.com/nickrandolph))
- Reverts IsXamarinForms changes in Playground [\#2754](https://github.com/MvvmCross/MvvmCross/pull/2754) ([tbalcom](https://github.com/tbalcom))
- Only set build script names once [\#2753](https://github.com/MvvmCross/MvvmCross/pull/2753) ([martijn00](https://github.com/martijn00))
- Improve a couple of build and project files [\#2752](https://github.com/MvvmCross/MvvmCross/pull/2752) ([martijn00](https://github.com/martijn00))
- Update ContentFiles Referenced In Readme.txt To Use MVVMCross 6.x Namespaces \(& Remove MvxTrace References\) [\#2748](https://github.com/MvvmCross/MvvmCross/pull/2748) ([andrewtechhelp](https://github.com/andrewtechhelp))
- Mark iOS and tvOS AppDelegate windows as virtual [\#2747](https://github.com/MvvmCross/MvvmCross/pull/2747) ([nmilcoff](https://github.com/nmilcoff))
- MvxSetupSingleton optimizations / Fix SplashScreen initialization on Android [\#2746](https://github.com/MvvmCross/MvvmCross/pull/2746) ([nmilcoff](https://github.com/nmilcoff))
- Add doc about linking [\#2744](https://github.com/MvvmCross/MvvmCross/pull/2744) ([Cheesebaron](https://github.com/Cheesebaron))
- Update messenger.md [\#2741](https://github.com/MvvmCross/MvvmCross/pull/2741) ([orzech85](https://github.com/orzech85))

## [6.0.0-beta7](https://github.com/MvvmCross/MvvmCross/tree/6.0.0-beta7) (2018-03-30)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/6.0.0-beta6...6.0.0-beta7)

**Fixed bugs:**

- Playground SheetView crashes Android application [\#2722](https://github.com/MvvmCross/MvvmCross/issues/2722)
- Android app hangs on SplashScreen [\#2721](https://github.com/MvvmCross/MvvmCross/issues/2721)
- MvxViewPagerAdapter and MvxStateViewPagerAdapter ignore the presence of view model instance inside MvxViewPagerFragmentInfo [\#2718](https://github.com/MvvmCross/MvvmCross/issues/2718)
- App didn't show the right view after add SplashScreen on WPF [\#2684](https://github.com/MvvmCross/MvvmCross/issues/2684)
- Language files are not loaded in iOS project [\#2678](https://github.com/MvvmCross/MvvmCross/issues/2678)
- Sometimes open the app and then it crashes [\#2599](https://github.com/MvvmCross/MvvmCross/issues/2599)
- MvvmCross.Forms cannot replace app's MainPage [\#2577](https://github.com/MvvmCross/MvvmCross/issues/2577)
- Status Bar Style jumps back to default after navigation \(iOS\) [\#2463](https://github.com/MvvmCross/MvvmCross/issues/2463)
- MvxAppCompatDialogFragment Attempt to invoke virtual method on a null object reference [\#2378](https://github.com/MvvmCross/MvvmCross/issues/2378)
- Fixes \#2722 [\#2730](https://github.com/MvvmCross/MvvmCross/pull/2730) ([tbalcom](https://github.com/tbalcom))
- Make sure Forms is loaded when not using a splashscreen [\#2729](https://github.com/MvvmCross/MvvmCross/pull/2729) ([martijn00](https://github.com/martijn00))
- Android add MvxViewVodelRequest to fragment forward life cycle events [\#2728](https://github.com/MvvmCross/MvvmCross/pull/2728) ([Plac3hold3r](https://github.com/Plac3hold3r))
- Android Dialogs: Fix close & do not keep references to instances [\#2711](https://github.com/MvvmCross/MvvmCross/pull/2711) ([nmilcoff](https://github.com/nmilcoff))
- Improvements & register fix for Visibility / Messenger / PictureChooser plugins [\#2704](https://github.com/MvvmCross/MvvmCross/pull/2704) ([nmilcoff](https://github.com/nmilcoff))

**Closed issues:**

- Add the ability to only use one instance of a given fragment [\#2694](https://github.com/MvvmCross/MvvmCross/issues/2694)
- Need an example of custom activity transitions [\#2659](https://github.com/MvvmCross/MvvmCross/issues/2659)
- -	Skipping DigitalWorkReport.Droid.Resource.String.fab\_scroll\_shrink\_grow\_autohide\_behavior [\#2645](https://github.com/MvvmCross/MvvmCross/issues/2645)
- MvvmCross.Forms version out of sync with Xamarin.Forms Tutorial [\#2620](https://github.com/MvvmCross/MvvmCross/issues/2620)
- Pin NuGet package dependencies [\#2581](https://github.com/MvvmCross/MvvmCross/issues/2581)
- OnCreate is called after first ContentPage [\#2549](https://github.com/MvvmCross/MvvmCross/issues/2549)
- Reason why app crashed MvxSetup.InitializePrimary\(\) called from void? [\#2508](https://github.com/MvvmCross/MvvmCross/issues/2508)
- Documentation missing for Xamarin.Forms ViewPresenter  [\#2497](https://github.com/MvvmCross/MvvmCross/issues/2497)
- Feature request: Broader fragment usage [\#2495](https://github.com/MvvmCross/MvvmCross/issues/2495)
- \[Android\] \[MvxRecyclerView\] MvxTemplateSelector\<TItem\> missing ItemTemplateId issue [\#2422](https://github.com/MvvmCross/MvvmCross/issues/2422)

**Merged pull requests:**

- Improvements for v6 docs [\#2739](https://github.com/MvvmCross/MvvmCross/pull/2739) ([nmilcoff](https://github.com/nmilcoff))
- Fix linker problem [\#2737](https://github.com/MvvmCross/MvvmCross/pull/2737) ([martijn00](https://github.com/martijn00))
- Added generic versions of MvxApplication for WPF and UWP [\#2735](https://github.com/MvvmCross/MvvmCross/pull/2735) ([nmilcoff](https://github.com/nmilcoff))
- Clean up some long time obsolete API's [\#2734](https://github.com/MvvmCross/MvvmCross/pull/2734) ([Plac3hold3r](https://github.com/Plac3hold3r))
- Update nuget packages [\#2733](https://github.com/MvvmCross/MvvmCross/pull/2733) ([martijn00](https://github.com/martijn00))
- MvxAppStart: Add non-generic version [\#2732](https://github.com/MvvmCross/MvvmCross/pull/2732) ([nmilcoff](https://github.com/nmilcoff))
- Setup fixes [\#2731](https://github.com/MvvmCross/MvvmCross/pull/2731) ([martijn00](https://github.com/martijn00))
- Add missing license to SelectedItemRecyclerAdapter [\#2727](https://github.com/MvvmCross/MvvmCross/pull/2727) ([Plac3hold3r](https://github.com/Plac3hold3r))
- Make it easier to build on Mac and fix some build bugs [\#2726](https://github.com/MvvmCross/MvvmCross/pull/2726) ([martijn00](https://github.com/martijn00))
- Android add Shared Elements support [\#2725](https://github.com/MvvmCross/MvvmCross/pull/2725) ([Plac3hold3r](https://github.com/Plac3hold3r))
- Removed no longer relevant comment in MvxAppCompatSetup [\#2724](https://github.com/MvvmCross/MvvmCross/pull/2724) ([Plac3hold3r](https://github.com/Plac3hold3r))
- Helpers for testing RaiseCanExecuteChanged [\#2720](https://github.com/MvvmCross/MvvmCross/pull/2720) ([jacobduijzer](https://github.com/jacobduijzer))
- \#2718 Ensure that existing fragment info's VM instance is used whenev… [\#2719](https://github.com/MvvmCross/MvvmCross/pull/2719) ([wh1t3cAt1k](https://github.com/wh1t3cAt1k))
- Add SplashScreen for WPF Playground to demonstrate loading issue [\#2716](https://github.com/MvvmCross/MvvmCross/pull/2716) ([Happin3ss](https://github.com/Happin3ss))
- Move bootstrap explanation to the upgrade [\#2715](https://github.com/MvvmCross/MvvmCross/pull/2715) ([martijn00](https://github.com/martijn00))
- Target WPF 4.6.1 so it is aligned with .net standard [\#2714](https://github.com/MvvmCross/MvvmCross/pull/2714) ([martijn00](https://github.com/martijn00))
- Refactoring Setup Singleton  [\#2713](https://github.com/MvvmCross/MvvmCross/pull/2713) ([nickrandolph](https://github.com/nickrandolph))
- Avoid using reflection to create the setup [\#2710](https://github.com/MvvmCross/MvvmCross/pull/2710) ([nickrandolph](https://github.com/nickrandolph))
- ItemTemplateId added to IMvxTemplateSelector [\#2709](https://github.com/MvvmCross/MvvmCross/pull/2709) ([orzech85](https://github.com/orzech85))
- Update WPF playground [\#2708](https://github.com/MvvmCross/MvvmCross/pull/2708) ([jz5](https://github.com/jz5))
- Remove MvxImageView and the likes [\#2706](https://github.com/MvvmCross/MvvmCross/pull/2706) ([nmilcoff](https://github.com/nmilcoff))
- V6 blog post & migration guide [\#2705](https://github.com/MvvmCross/MvvmCross/pull/2705) ([nmilcoff](https://github.com/nmilcoff))
- Make application class virtual [\#2703](https://github.com/MvvmCross/MvvmCross/pull/2703) ([martijn00](https://github.com/martijn00))
- Align setup and not override window [\#2702](https://github.com/MvvmCross/MvvmCross/pull/2702) ([martijn00](https://github.com/martijn00))

## [6.0.0-beta6](https://github.com/MvvmCross/MvvmCross/tree/6.0.0-beta6) (2018-03-19)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.7.0...6.0.0-beta6)

**Fixed bugs:**

- RegisterAttribute doesn't always match the new MvvmCross 6 namespace [\#2688](https://github.com/MvvmCross/MvvmCross/issues/2688)
- Lack of Initialization from MvxSplashScreenActivity causes App start from external Intent \(ie, Uri routing\) to fail in Forms app [\#2624](https://github.com/MvvmCross/MvvmCross/issues/2624)
- Xamarin.Forms Android - First page cannot reference Application level StaticResources [\#2622](https://github.com/MvvmCross/MvvmCross/issues/2622)
- MvxAppStart swallows exceptions [\#2586](https://github.com/MvvmCross/MvvmCross/issues/2586)

**Closed issues:**

- Using MvvmCross 5.6 NavigationService with autofac IoC [\#2636](https://github.com/MvvmCross/MvvmCross/issues/2636)
- \[Android\] Inconsistency with MvxRecyclerView vs. MvxListView & MvxSpinner [\#2544](https://github.com/MvvmCross/MvvmCross/issues/2544)

**Merged pull requests:**

- Fix readme and some small issue's [\#2701](https://github.com/MvvmCross/MvvmCross/pull/2701) ([martijn00](https://github.com/martijn00))
- Change platform to platforms [\#2700](https://github.com/MvvmCross/MvvmCross/pull/2700) ([martijn00](https://github.com/martijn00))
- Update ios-tables-and-cells.md [\#2699](https://github.com/MvvmCross/MvvmCross/pull/2699) ([jfversluis](https://github.com/jfversluis))
- Make it easier to change csproj versions and update nugets [\#2695](https://github.com/MvvmCross/MvvmCross/pull/2695) ([martijn00](https://github.com/martijn00))
- Lists inconsistencies [\#2693](https://github.com/MvvmCross/MvvmCross/pull/2693) ([orzech85](https://github.com/orzech85))
- MvxPreferenceValueTargetBinding: Fix method call: Warn -\> Warning [\#2692](https://github.com/MvvmCross/MvvmCross/pull/2692) ([nmilcoff](https://github.com/nmilcoff))
- Updates RegisterAttribute to match new Mvx 6 namespaces [\#2691](https://github.com/MvvmCross/MvvmCross/pull/2691) ([tbalcom](https://github.com/tbalcom))
- v6 Blog post & migration guide updates [\#2689](https://github.com/MvvmCross/MvvmCross/pull/2689) ([nmilcoff](https://github.com/nmilcoff))
- Release/5.7.0 [\#2687](https://github.com/MvvmCross/MvvmCross/pull/2687) ([Cheesebaron](https://github.com/Cheesebaron))
- Create 2018-03-14-mvvmcross-5.7.0-release.md [\#2686](https://github.com/MvvmCross/MvvmCross/pull/2686) ([Cheesebaron](https://github.com/Cheesebaron))
- Use previous version of VS2017 on AppVeyor [\#2685](https://github.com/MvvmCross/MvvmCross/pull/2685) ([Cheesebaron](https://github.com/Cheesebaron))
- Fixes calling methods deprecated in Android API26. [\#2679](https://github.com/MvvmCross/MvvmCross/pull/2679) ([tbalcom](https://github.com/tbalcom))
- Ensuring Start completed before initializing Xamarin Forms [\#2674](https://github.com/MvvmCross/MvvmCross/pull/2674) ([nickrandolph](https://github.com/nickrandolph))

## [5.7.0](https://github.com/MvvmCross/MvvmCross/tree/5.7.0) (2018-03-14)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/6.0.0-beta5...5.7.0)

**Fixed bugs:**

- \[iOS\] Text Replacement does not apply change through the binding [\#2681](https://github.com/MvvmCross/MvvmCross/issues/2681)

**Merged pull requests:**

- Add a playground page to test whether MvxContentViews load their view models [\#2683](https://github.com/MvvmCross/MvvmCross/pull/2683) ([jessewdouglas](https://github.com/jessewdouglas))
- \[iOS\] Update UITextField binding when editing did end \(Text Replacement fix\) [\#2682](https://github.com/MvvmCross/MvvmCross/pull/2682) ([alexshikov](https://github.com/alexshikov))
- Update docs broken Playground sample link [\#2677](https://github.com/MvvmCross/MvvmCross/pull/2677) ([fedemkr](https://github.com/fedemkr))
- Move hints and attributes into the right folder [\#2675](https://github.com/MvvmCross/MvvmCross/pull/2675) ([martijn00](https://github.com/martijn00))
- Update changelog [\#2673](https://github.com/MvvmCross/MvvmCross/pull/2673) ([Cheesebaron](https://github.com/Cheesebaron))

## [6.0.0-beta5](https://github.com/MvvmCross/MvvmCross/tree/6.0.0-beta5) (2018-03-09)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/6.0.0-beta4...6.0.0-beta5)

**Fixed bugs:**

- Fix moving items in the MvxRecyclerAdapter [\#2664](https://github.com/MvvmCross/MvvmCross/pull/2664) ([kjeremy](https://github.com/kjeremy))

**Closed issues:**

- Support for netstandard1.4 in version 6.0 [\#2649](https://github.com/MvvmCross/MvvmCross/issues/2649)
- ParameterValues doesn't set in NavigationService [\#2646](https://github.com/MvvmCross/MvvmCross/issues/2646)
- get view api [\#2643](https://github.com/MvvmCross/MvvmCross/issues/2643)
- MvvmCross.Plugins.Location.Fused.Droid.Plugin does not load [\#2637](https://github.com/MvvmCross/MvvmCross/issues/2637)
- Cleanup "Sidebar" plugin [\#2626](https://github.com/MvvmCross/MvvmCross/issues/2626)
- StarterPack does not generate files on vs17 [\#2595](https://github.com/MvvmCross/MvvmCross/issues/2595)
- MvxFormsApplication Start, Sleep and Resume gets not called on iOS [\#2512](https://github.com/MvvmCross/MvvmCross/issues/2512)
- View shown before ViewModel initialization complete [\#2478](https://github.com/MvvmCross/MvvmCross/issues/2478)

**Merged pull requests:**

- Added RaiseCanExecuteChanged interface definition to IMvxCommand\<T\> [\#2672](https://github.com/MvvmCross/MvvmCross/pull/2672) ([jnosek](https://github.com/jnosek))
- Cleanup csproj files and add missing headers [\#2671](https://github.com/MvvmCross/MvvmCross/pull/2671) ([martijn00](https://github.com/martijn00))
- Add check in navigation service to see if viewmodels are available [\#2670](https://github.com/MvvmCross/MvvmCross/pull/2670) ([martijn00](https://github.com/martijn00))
- Add generic setup to all platforms [\#2668](https://github.com/MvvmCross/MvvmCross/pull/2668) ([martijn00](https://github.com/martijn00))
- Update upgrade-to-mvvmcross-60.md [\#2667](https://github.com/MvvmCross/MvvmCross/pull/2667) ([asterixorobelix](https://github.com/asterixorobelix))
- Fix some projects not building in specific configs [\#2663](https://github.com/MvvmCross/MvvmCross/pull/2663) ([martijn00](https://github.com/martijn00))
- Delete empty IMvxModalIosView [\#2660](https://github.com/MvvmCross/MvvmCross/pull/2660) ([martijn00](https://github.com/martijn00))
- Add readme.txt file to open on nuget install [\#2658](https://github.com/MvvmCross/MvvmCross/pull/2658) ([martijn00](https://github.com/martijn00))
- Rename test to tests to align with old nuget package and current naming [\#2657](https://github.com/MvvmCross/MvvmCross/pull/2657) ([martijn00](https://github.com/martijn00))
- Version bump for UWP and SDKExtras nuget packages [\#2656](https://github.com/MvvmCross/MvvmCross/pull/2656) ([nickrandolph](https://github.com/nickrandolph))
- Documentation: MvxNotifyTask, ViewModel-Lifecycle location, Samples [\#2655](https://github.com/MvvmCross/MvvmCross/pull/2655) ([nmilcoff](https://github.com/nmilcoff))
- Merge master back into develop to update docs [\#2654](https://github.com/MvvmCross/MvvmCross/pull/2654) ([nmilcoff](https://github.com/nmilcoff))
- add throw for Exception in MvxNavigationService [\#2651](https://github.com/MvvmCross/MvvmCross/pull/2651) ([kvandake](https://github.com/kvandake))
- Use the basic Forms application type instead of the Mvx one [\#2617](https://github.com/MvvmCross/MvvmCross/pull/2617) ([martijn00](https://github.com/martijn00))

## [6.0.0-beta4](https://github.com/MvvmCross/MvvmCross/tree/6.0.0-beta4) (2018-03-02)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/6.0.0-beta3...6.0.0-beta4)

**Fixed bugs:**

- MvxBaseTableViewSource: Fix wrong height for xib based cells  [\#2644](https://github.com/MvvmCross/MvvmCross/pull/2644) ([nmilcoff](https://github.com/nmilcoff))
- Apply default templates to MvxAppCompatSpinner [\#2640](https://github.com/MvvmCross/MvvmCross/pull/2640) ([kjeremy](https://github.com/kjeremy))
- Binding types fix [\#2632](https://github.com/MvvmCross/MvvmCross/pull/2632) ([Saratsin](https://github.com/Saratsin))

**Closed issues:**

- MvvmCross.Plugin.Location.Fused 5.6.3 not compatible with MvvmCross.Droid.Support.v7.AppCompat 5.6.3 [\#2633](https://github.com/MvvmCross/MvvmCross/issues/2633)
- MvvmCross.Plugin.Location.Fused 5.6.3 is not compatible with netstandard2.0 [\#2607](https://github.com/MvvmCross/MvvmCross/issues/2607)

**Merged pull requests:**

- Revert "MvxNavigationService.cs. Add "throw" for an exception in the method NavigationRouteRequest." [\#2650](https://github.com/MvvmCross/MvvmCross/pull/2650) ([Cheesebaron](https://github.com/Cheesebaron))
- Add docs for resharper annotations [\#2648](https://github.com/MvvmCross/MvvmCross/pull/2648) ([mterwoord](https://github.com/mterwoord))
- MvxNavigationService.cs. Add "throw" for an exception in the method NavigationRouteRequest. [\#2647](https://github.com/MvvmCross/MvvmCross/pull/2647) ([kvandake](https://github.com/kvandake))
- MvxNotifyTask improvements [\#2642](https://github.com/MvvmCross/MvvmCross/pull/2642) ([nmilcoff](https://github.com/nmilcoff))
- Update nuget packages [\#2639](https://github.com/MvvmCross/MvvmCross/pull/2639) ([martijn00](https://github.com/martijn00))
- Don't put a user dependency on Microsoft.CSharp [\#2638](https://github.com/MvvmCross/MvvmCross/pull/2638) ([martijn00](https://github.com/martijn00))
- Adding support for custom renderer assemblies to MvxFormsWindowsSetup [\#2635](https://github.com/MvvmCross/MvvmCross/pull/2635) ([MartinZikmund](https://github.com/MartinZikmund))
- Rename folders from iOS to Ios [\#2630](https://github.com/MvvmCross/MvvmCross/pull/2630) ([nmilcoff](https://github.com/nmilcoff))
- Fix the compiling error of Playground.Forms.Uwp [\#2628](https://github.com/MvvmCross/MvvmCross/pull/2628) ([flyingxu](https://github.com/flyingxu))
- Align namespaces: Rename iOS namespaces to Ios [\#2627](https://github.com/MvvmCross/MvvmCross/pull/2627) ([nmilcoff](https://github.com/nmilcoff))
- Remove DownloadCache plugin [\#2625](https://github.com/MvvmCross/MvvmCross/pull/2625) ([nmilcoff](https://github.com/nmilcoff))
- Add observable collection tests [\#2618](https://github.com/MvvmCross/MvvmCross/pull/2618) ([Cheesebaron](https://github.com/Cheesebaron))
- Reducing code to get started for UWP projects [\#2615](https://github.com/MvvmCross/MvvmCross/pull/2615) ([nickrandolph](https://github.com/nickrandolph))
- Updating various startup logic [\#2593](https://github.com/MvvmCross/MvvmCross/pull/2593) ([nickrandolph](https://github.com/nickrandolph))
- Blog post & Migration Guide for v6 [\#2590](https://github.com/MvvmCross/MvvmCross/pull/2590) ([nmilcoff](https://github.com/nmilcoff))

## [6.0.0-beta3](https://github.com/MvvmCross/MvvmCross/tree/6.0.0-beta3) (2018-02-14)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/6.0.0-beta2...6.0.0-beta3)

**Fixed bugs:**

- PluginLoaders not found for platform specific plugins [\#2611](https://github.com/MvvmCross/MvvmCross/issues/2611)
- Child View Presentation does not work when using More Tabs \(more than five tabs\) \[iOS\]  [\#2609](https://github.com/MvvmCross/MvvmCross/issues/2609)
- MvxObservableCollection reports wrong index when doing AddRange [\#2515](https://github.com/MvvmCross/MvvmCross/issues/2515)
- MvxWindowsPage cannot navigate to MvxContentPage [\#2466](https://github.com/MvvmCross/MvvmCross/issues/2466)
- Fixed issue \#2515 where MvxObservableCollection.AddRange\(\) reports wrong index [\#2614](https://github.com/MvvmCross/MvvmCross/pull/2614) ([Strifex](https://github.com/Strifex))

**Merged pull requests:**

- Improve the IMvxPluginManager interface [\#2616](https://github.com/MvvmCross/MvvmCross/pull/2616) ([willsb](https://github.com/willsb))
- Fix a bunch of Warnings [\#2613](https://github.com/MvvmCross/MvvmCross/pull/2613) ([Cheesebaron](https://github.com/Cheesebaron))
- Update Forms namespaces to match traditional Xamarin  [\#2612](https://github.com/MvvmCross/MvvmCross/pull/2612) ([nmilcoff](https://github.com/nmilcoff))
- Allow more than 5 children in MoreNavigationController [\#2610](https://github.com/MvvmCross/MvvmCross/pull/2610) ([thefex](https://github.com/thefex))
- New plugin architecture [\#2603](https://github.com/MvvmCross/MvvmCross/pull/2603) ([willsb](https://github.com/willsb))
- More unittests [\#2596](https://github.com/MvvmCross/MvvmCross/pull/2596) ([Cheesebaron](https://github.com/Cheesebaron))

## [6.0.0-beta2](https://github.com/MvvmCross/MvvmCross/tree/6.0.0-beta2) (2018-02-12)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/6.0.0-beta1...6.0.0-beta2)

## [6.0.0-beta1](https://github.com/MvvmCross/MvvmCross/tree/6.0.0-beta1) (2018-02-12)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.6.3...6.0.0-beta1)

**Fixed bugs:**

- Fragment close does not work if fragment presentation attribute has backstack set to false [\#2600](https://github.com/MvvmCross/MvvmCross/issues/2600)
- Platform.Mac startup exception Foundation.You\_Should\_Not\_Call\_base\_In\_This\_Method [\#2591](https://github.com/MvvmCross/MvvmCross/issues/2591)
- Custom Presentation Hint Handler is ignored [\#2589](https://github.com/MvvmCross/MvvmCross/issues/2589)
- UITextview binding - missing source event info in MvxWeakEventSubscription Parameter name: sourceEventInfo [\#2543](https://github.com/MvvmCross/MvvmCross/issues/2543)
- MvxBottomSheetDialogFragment OnDestroy does not consider finsished parameter [\#2525](https://github.com/MvvmCross/MvvmCross/issues/2525)
- RegisterNavigationServiceAppStart vs RegisterAppStart [\#2447](https://github.com/MvvmCross/MvvmCross/issues/2447)
- MvxExpandableListView GroupClick binding replaces group expanding functionality [\#2408](https://github.com/MvvmCross/MvvmCross/issues/2408)
- 'System.TypeInitializationException' In 'MvvmCross.Core.Platform.LogProviders.ConsoleLogProvider' On UWP Projects [\#2333](https://github.com/MvvmCross/MvvmCross/issues/2333)
- Inconsistent PCL profile for PictureChooser [\#2295](https://github.com/MvvmCross/MvvmCross/issues/2295)
-  \#2600 no backstack fragment close does not work hotfix [\#2601](https://github.com/MvvmCross/MvvmCross/pull/2601) ([thefex](https://github.com/thefex))
- Fix that change presentation add handler is ignored in forms [\#2592](https://github.com/MvvmCross/MvvmCross/pull/2592) ([martijn00](https://github.com/martijn00))
- Fix inheritance for MvxBaseSplitViewController class with constraint [\#2564](https://github.com/MvvmCross/MvvmCross/pull/2564) ([nmilcoff](https://github.com/nmilcoff))
- Fixing Android attributes [\#2529](https://github.com/MvvmCross/MvvmCross/pull/2529) ([nickrandolph](https://github.com/nickrandolph))
- MvxBottomSheetDialogFragment: Fix OnDestroy [\#2526](https://github.com/MvvmCross/MvvmCross/pull/2526) ([nmilcoff](https://github.com/nmilcoff))
- Improve implementation of IMvxAndroidCurrentTopActivity [\#2513](https://github.com/MvvmCross/MvvmCross/pull/2513) ([nmilcoff](https://github.com/nmilcoff))
-  Make Mvx...Cell inherit from IMvxCell instead of IMvxElement [\#2511](https://github.com/MvvmCross/MvvmCross/pull/2511) ([mubold](https://github.com/mubold))
- Fix MvxTabBarViewController.CloseTab: Pick correct ViewController [\#2506](https://github.com/MvvmCross/MvvmCross/pull/2506) ([nmilcoff](https://github.com/nmilcoff))
- Switch parameters in MvxException so that first exception is InnerException [\#2504](https://github.com/MvvmCross/MvvmCross/pull/2504) ([mubold](https://github.com/mubold))
- MvxNavigationServiceAppStart: Don't swallow exceptions [\#2471](https://github.com/MvvmCross/MvvmCross/pull/2471) ([nmilcoff](https://github.com/nmilcoff))

**Closed issues:**

- Navigation service: ChangePresentation should be async [\#2602](https://github.com/MvvmCross/MvvmCross/issues/2602)
- \[Question\] - thoughts on routing for navigation [\#2563](https://github.com/MvvmCross/MvvmCross/issues/2563)
- Remove usage of MvxTrace from code [\#2541](https://github.com/MvvmCross/MvvmCross/issues/2541)
- Switch NUnit tests to XUnit [\#2540](https://github.com/MvvmCross/MvvmCross/issues/2540)
- System.Reflection GetCustomAttributes [\#2535](https://github.com/MvvmCross/MvvmCross/issues/2535)
- Inconsistency between MvxCommand\<T\> and MvxAsyncCommand\<T\> implementing IMvxCommand [\#2520](https://github.com/MvvmCross/MvvmCross/issues/2520)
- Remove IMvxIosModalHost [\#2475](https://github.com/MvvmCross/MvvmCross/issues/2475)
- WithConversion\<...\> does not appear do the same as WithConversion\(new ...\) [\#2449](https://github.com/MvvmCross/MvvmCross/issues/2449)
- The new logger infrastructure should not send null messages to IMvxLog [\#2437](https://github.com/MvvmCross/MvvmCross/issues/2437)
- \[Android\] MvxSplashScreenActivity should not have to be the first Activity [\#2261](https://github.com/MvvmCross/MvvmCross/issues/2261)
- MvxViewModel\<TParameter\>.Init should be removed [\#2257](https://github.com/MvvmCross/MvvmCross/issues/2257)

**Merged pull requests:**

- Cleanup: Create Presenters & Commands folders / Remove {PlatformName}.Base folders [\#2606](https://github.com/MvvmCross/MvvmCross/pull/2606) ([nmilcoff](https://github.com/nmilcoff))
- Cleanup windows and touch names [\#2605](https://github.com/MvvmCross/MvvmCross/pull/2605) ([martijn00](https://github.com/martijn00))
- Make ChangePresentation signature async [\#2604](https://github.com/MvvmCross/MvvmCross/pull/2604) ([martijn00](https://github.com/martijn00))
- IMvxFormsViewPresenter in UWP sample [\#2598](https://github.com/MvvmCross/MvvmCross/pull/2598) ([MartinZikmund](https://github.com/MartinZikmund))
- Using IActivatedEventArgs in UWP [\#2597](https://github.com/MvvmCross/MvvmCross/pull/2597) ([MartinZikmund](https://github.com/MartinZikmund))
- Updates the namespaces for all projects [\#2594](https://github.com/MvvmCross/MvvmCross/pull/2594) ([martijn00](https://github.com/martijn00))
- Fixing references and UWP/WPF playground samples [\#2585](https://github.com/MvvmCross/MvvmCross/pull/2585) ([nickrandolph](https://github.com/nickrandolph))
- Add back the other playground projects [\#2584](https://github.com/MvvmCross/MvvmCross/pull/2584) ([martijn00](https://github.com/martijn00))
- Move actual unit tests and test project to correct folders [\#2583](https://github.com/MvvmCross/MvvmCross/pull/2583) ([martijn00](https://github.com/martijn00))
- Correctly set nuget dependencies [\#2582](https://github.com/MvvmCross/MvvmCross/pull/2582) ([Cheesebaron](https://github.com/Cheesebaron))
- Open up localization for custom implementations [\#2579](https://github.com/MvvmCross/MvvmCross/pull/2579) ([martijn00](https://github.com/martijn00))
- Fix type in interface [\#2576](https://github.com/MvvmCross/MvvmCross/pull/2576) ([Cheesebaron](https://github.com/Cheesebaron))
- Move files into correct folders [\#2570](https://github.com/MvvmCross/MvvmCross/pull/2570) ([martijn00](https://github.com/martijn00))
- Add support for Tizen [\#2569](https://github.com/MvvmCross/MvvmCross/pull/2569) ([martijn00](https://github.com/martijn00))
- Add .NET Foundation file headers to all files [\#2568](https://github.com/MvvmCross/MvvmCross/pull/2568) ([martijn00](https://github.com/martijn00))
- Fix a couple of warnings [\#2567](https://github.com/MvvmCross/MvvmCross/pull/2567) ([martijn00](https://github.com/martijn00))
- Convert UnitTests to XUnit [\#2566](https://github.com/MvvmCross/MvvmCross/pull/2566) ([Cheesebaron](https://github.com/Cheesebaron))
- Fixing startup issue where ioc isn't initialized [\#2565](https://github.com/MvvmCross/MvvmCross/pull/2565) ([nickrandolph](https://github.com/nickrandolph))
- Add back Android playground projects [\#2562](https://github.com/MvvmCross/MvvmCross/pull/2562) ([martijn00](https://github.com/martijn00))
- Remove memory inefficient droid layouts [\#2561](https://github.com/MvvmCross/MvvmCross/pull/2561) ([willsb](https://github.com/willsb))
- Add back WPF [\#2560](https://github.com/MvvmCross/MvvmCross/pull/2560) ([martijn00](https://github.com/martijn00))
- Remove ShowViewModel in favor for MvxNavigationService [\#2559](https://github.com/MvvmCross/MvvmCross/pull/2559) ([martijn00](https://github.com/martijn00))
- Align view presenters, registration and remove modal host [\#2558](https://github.com/MvvmCross/MvvmCross/pull/2558) ([martijn00](https://github.com/martijn00))
- Rename Android NativeColor method for consistency [\#2557](https://github.com/MvvmCross/MvvmCross/pull/2557) ([willsb](https://github.com/willsb))
- Netstandard fixes for the logging mechanism [\#2556](https://github.com/MvvmCross/MvvmCross/pull/2556) ([willsb](https://github.com/willsb))
- Implement IMvxCommand in MvxAsyncCommand\<T\> [\#2552](https://github.com/MvvmCross/MvvmCross/pull/2552) ([kipters](https://github.com/kipters))
- Add UWP StarterPack content [\#2551](https://github.com/MvvmCross/MvvmCross/pull/2551) ([kipters](https://github.com/kipters))
- Small fix downloadcache.md [\#2546](https://github.com/MvvmCross/MvvmCross/pull/2546) ([wcoder](https://github.com/wcoder))
- Updates for the docs [\#2542](https://github.com/MvvmCross/MvvmCross/pull/2542) ([martijn00](https://github.com/martijn00))
- Fixing build for MvvmCross.Forms [\#2531](https://github.com/MvvmCross/MvvmCross/pull/2531) ([nickrandolph](https://github.com/nickrandolph))
- Update to .NET Standard [\#2530](https://github.com/MvvmCross/MvvmCross/pull/2530) ([martijn00](https://github.com/martijn00))
- Fixing layout of files for plugins for multi-targetting [\#2528](https://github.com/MvvmCross/MvvmCross/pull/2528) ([nickrandolph](https://github.com/nickrandolph))
- Update navigation.md [\#2524](https://github.com/MvvmCross/MvvmCross/pull/2524) ([asterixorobelix](https://github.com/asterixorobelix))
- Adding NativePage to Playground sample for UWP [\#2523](https://github.com/MvvmCross/MvvmCross/pull/2523) ([nickrandolph](https://github.com/nickrandolph))
- Fix native pages not loading without attribute [\#2522](https://github.com/MvvmCross/MvvmCross/pull/2522) ([martijn00](https://github.com/martijn00))
- Update mvvmcross-overview.md [\#2521](https://github.com/MvvmCross/MvvmCross/pull/2521) ([asterixorobelix](https://github.com/asterixorobelix))
- Fix minor typos [\#2519](https://github.com/MvvmCross/MvvmCross/pull/2519) ([programmation](https://github.com/programmation))
- MvxAndroidViewsContainer: Remove NewTask flag as default [\#2516](https://github.com/MvvmCross/MvvmCross/pull/2516) ([nmilcoff](https://github.com/nmilcoff))
- Add LogLevel availability check to IMvxLog [\#2514](https://github.com/MvvmCross/MvvmCross/pull/2514) ([willsb](https://github.com/willsb))
- Update message displayed for OnViewNewIntent [\#2510](https://github.com/MvvmCross/MvvmCross/pull/2510) ([nmilcoff](https://github.com/nmilcoff))
- Added reference to a newer blog post on resx localization by Daniel Krzyczkowski [\#2509](https://github.com/MvvmCross/MvvmCross/pull/2509) ([mellson](https://github.com/mellson))
- Checks fragment type when showing a DialogFragment and throws MvxException instead of InvalidCastException [\#2507](https://github.com/MvvmCross/MvvmCross/pull/2507) ([MKuckert](https://github.com/MKuckert))
- IoC Child Containers: Merge \#2438 into develop [\#2505](https://github.com/MvvmCross/MvvmCross/pull/2505) ([martijn00](https://github.com/martijn00))
- Avoid the Foundation.You\_Should\_Not\_Call\_base\_In\_This\_Method exception [\#2499](https://github.com/MvvmCross/MvvmCross/pull/2499) ([flyingxu](https://github.com/flyingxu))
- Making it easier to override default IMvxViewModelTypeFinder implementation [\#2498](https://github.com/MvvmCross/MvvmCross/pull/2498) ([nickrandolph](https://github.com/nickrandolph))
- Upgrading iOS csproj to use Visible element instead of InProject [\#2493](https://github.com/MvvmCross/MvvmCross/pull/2493) ([nickrandolph](https://github.com/nickrandolph))
- New events binding [\#2490](https://github.com/MvvmCross/MvvmCross/pull/2490) ([Saratsin](https://github.com/Saratsin))
- Fix for Expand/Colapse problem when GroupClick is bound. [\#2489](https://github.com/MvvmCross/MvvmCross/pull/2489) ([AlexStefan](https://github.com/AlexStefan))
- Improve IMvxOverridePresentationAttribute by providing the MvxViewModelRequest as parameter [\#2483](https://github.com/MvvmCross/MvvmCross/pull/2483) ([nmilcoff](https://github.com/nmilcoff))
- Added support for nested fragments \(ChildFragmentManager\) [\#2482](https://github.com/MvvmCross/MvvmCross/pull/2482) ([Qwin](https://github.com/Qwin))
- Update WithDictionaryConversion binding to have optional fallback [\#2480](https://github.com/MvvmCross/MvvmCross/pull/2480) ([Plac3hold3r](https://github.com/Plac3hold3r))
- Webbrowser plugin for mac [\#2464](https://github.com/MvvmCross/MvvmCross/pull/2464) ([tofutim](https://github.com/tofutim))
- mvxforms/droid-resources [\#2461](https://github.com/MvvmCross/MvvmCross/pull/2461) ([johnnywebb](https://github.com/johnnywebb))
