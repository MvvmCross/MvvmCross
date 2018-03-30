---
layout: post
title: MvvmCross 6.0
date:   2018-2-6 14:00:00 -0300
categories: mvvmcross
---

# Announcing MvvmCross 6.0!

Yes, you read it correctly! MvvmCross 6 has finally arrived and it is available on [NuGet](https://www.nuget.org/packages/MvvmCross/6.0.0)!

## What's new?!

- Migration to .NET Standard 2
- Polished support for Xamarin.Forms
- Brand new framework initialization (Setup & AppStart)
- New register process for plugins
- Supercharged `IMvxOverridePresentationAttribute` for ViewPresenters
- Initial support for Tizen
- Tons of improvements and bug fixes!

## Migration guide

MvvmCross 6 comes with quite a lot of improvements, but this also means some things will break. We have prepared a migration guide that will help you do the transition real quick! You can find it [here](https://www.mvvmcross.com/documentation/upgrading/upgrade-to-mvvmcross-60).

### NuGet packages

With MvvmCross 6 there are some changes to the NuGet packages. The following packages are obsolete and included in the main `MvvmCross` package:

- MvvmCross.Core
- MvvmCross.Platform
- MvvmCross.Binding

NuGet packages are now versioned using SemVer 2.0, meaning you need to use Visual Studio 2017 (15.3) and above or NuGet 4.3.0 and above.

The MvvmCross.* namespace has been reserved on NuGet, meaning that plugin authors should move their plugins away from this package namespace. We are also planning on signing future releases.

### .NET Standard

MvvmCross uses .NET Standard 2.0 as its base library now. This ensures compability on all platforms, and helps us develop MvvmCross faster!

For a explanation about .NET Standard see: https://blogs.msdn.microsoft.com/dotnet/2016/09/26/introducing-net-standard/

### Setup & platforms initialization

We've changed the way platforms are loaded. Previously you had to create the `Setup` class in every platform yourself (except for Android if you were using a Splashscreen).
Starting with v6 the Setup class hs received a lot of improvements! Also if you are starting with a brand new application, you might not even need to write your own!

In order to avoid having to create and initialize the Setup class yourself, you can now use generic versions of some classes:
- On iOS, there's a version of `MvxApplicationDelegate` which takes a `IMvxIosSetup` and a `IMvxApplication` constraints
- On Android, there's a version of `MvxAndroidApplication` which takes a `MvxAndroidSetup` and a `IMvxApplication` constraints
- On Android (support packages), there's a version of `MvxAppCompatApplication` which takes a `MvxAppCompatSetup` and a `IMvxApplication` constraints
- On UWP, there's a version of `MvxApplication` which takes a `MvxWindowsSetup` and a `IMvxApplication` constraints
- On WPF, there's a version of `MvxApplication` which takes a `MvxWpfSetup` and a `IMvxApplication` constraints
- On macOS, there's a version of `MvxApplicationDelegate` which takes a `MvxMacSetup` and a `IMvxApplication` constraints
- On tvOS, there's a version of `MvxApplicationDelegate` which takes a `MvxTvosSetup` and a `IMvxApplication` constraints

A few importatant notes on this are:

* On iOS, tvOS and macOS please make sure you are calling `var result = base.FinishedLaunching(app, options);` and returning the result at the end of the method.
* Remove custom App.cs code from your UWP and WPF projects
* On Android this initialization also works for apps without splash screens.
* Of course you can keep your Setup class if you want (and it is still encouraged to initialize everything there)!
* There is now also a singleton for Setup on all platforms, which you can use to ensure MvvmCross is running!

The main work on all this changes was done by [@nickrandolph](https://github.com/nickrandolph) and [@martijn00](https://github.com/martijn00). Well done guys!

### AppStart

The way apps start with MvvmCross has now become much cleaner. MvxAppStart is now called automatically by the framework uniformly. This means you can safely delete your initialization code on platforms like iOS (the framework now will also create the key window for you).

In case you are using a custom AppStart, it is recommended that you make it inherit from the newly added `MvxAppStart` class.

If you are wondering now whether it's possible to add some customization to that, the answer is YES. In the same class where you used to run your own AppStart, there is now a virtual method called `RunAppStart` that you can override. And going further on that direction, if what you need is to make sure you provide a correct hint to your AppStart, then you only need to override the new method called `GetAppStartHint`. Sweet, ah? All thanks to the MvvmCross Core Team.

_Note:_ In case you have a custom AppStart, watch out! The method `Start` has been made protected and it's now called `Startup`.

### Plugins

No more bootstrap files! Yes, you read it correctly. [@willsb](https://github.com/willsb) has worked hard on plugins during some time and he came out with an easier way to register your MvvmCross plugins, by simply adding the `[MvxPlugin]` attribute to your plugin and inheriting from `IMvxPlugin` - as usual -. 

The way plugins are loaded has improved as well, and now you can force them to be loaded again and even see which ones are loaded at a given time through `IMvxPluginManager.LoadedPlugins`.

Read more about how to get started with plugins [in our documentation](https://www.mvvmcross.com/documentation/plugins/plugins-development).

#### Json and Resx plugins

All methods in `MvxResxTextProvider`, `MvxJsonDictionaryTextProvider` and `MvxTextProvider` are now virtual. Customization is now much easier!

#### DownloadCache
`DownloadCache` was removed in v6.0, as well as `MvxImageView` and all the related code.

### ViewPresenters

`IMvxOverridePresentationAttribute.PresentationAttribute` now takes a `MvxViewModelRequest` as parameter. As a result, when the method `PresentationAttribute` is called, you will be able to make your choice on which attribute to use taking advantage of the ViewModel request. But that's not everything! If you are using the MvxNavigationService, you can cast the arriving parameter of type `MvxViewModelRequest` to be a `MvxViewModelInstanceRequest`, which will allow you to see the ViewModel that is being presented. 
This change was made by [@nmilcoff](https://github.com/nmilcoff).

ViewPresenters registration was aligned and improved on many platforms. You can now obtain the current ViewPresenter from anywhere by resolving the interface `IMvxViewPresenter`. All thanks to the amazing [@martijn00](https://github.com/martijn00)!

### Navigation

The brand new MvxNavigationService that was introduced in MvvmCross 5 is now the default. This means `ShowViewModel` has been finally removed, as well as `MvxNavigatingObject`. If you aren't using it yet, it's time you take a look at the [official documentation](https://www.mvvmcross.com/documentation/fundamentals/navigation).

On this release we have added support for checking whether it's possible or not to navigate to a specific ViewModel. The default implementation will return `true` if the View for that ViewModel is reachable from the platform ViewContainer.

Also please note that the intermediary helper class `MvxNavigationServiceAppStart` has been removed as well, because the classic `MvxAppStart` now uses MvxNavigationService internally.

### IoC

Sometimes you'd like to add some instances or types to an IoC Container for a specific purpose and not to the app-wide container. You can use Child Containers for that:

```
var container = Mvx.Resolve<IMvxIoCProvider>();
var childContainer = container.CreateChildContainer():
childContainer.RegisterType<IFoo, Foo>(); // Is only registered in Child Container scope
childContainer.Create<IFoo>();
```

You can create as many and as deeply nested Child Containers as you want - each container inherits all dependencies registered on it's parent container.

### Logging

`MvxTrace` and everything related was removed. The new (and much improved) logging system was already present since MvvmCross 5.4. If you haven't heard about it, please take a look at the [official documentation](https://www.mvvmcross.com/documentation/fundamentals/logging)

### Xamarin.Forms

#### General stability and bugs fixes

We've put a lot of effort in to make sure MvvmCross works with Forms as good as with traditional Xamarin apps! A whole bunch of bugs have been fixed (and we've added test scenarios in our playground project to make sure we keep it this way!). 

#### MvxFormsApplication
MvvmCross became much more flexible and it doesn't require your app to use our own `MvxFormsApplication` class anymore. 

#### ViewCells

`MvxViewCell` is now usable! We have [fixed](https://github.com/MvvmCross/MvvmCross/pull/2511) a bug and apps won't crash in runtime anymore. It is recommended that you use `MvxViewCell` in ListViews instead of the default `ViewCell`.

#### Support for "native" views

[@martijn00](https://github.com/martijn00) and [@nickrandolph](https://github.com/nickrandolph) fixed several issues regarding the Forms ViewPresenter not being able to display native views. We now provide a much better support for it!  
 
#### Xamarin.Forms UWP

You are now free to place your custom renderers in a different assembly. All you have to do to make it work is to add your assembly to the `Setup.ViewAssemblies` collection.

### iOS

[@thefex](https://github.com/thefex) has made it possible to easily navigate to child ViewControllers when using more than 5 bottom tabs. Our default ViewPresenter now supports it out of the box!

### Android

#### Current top activity

Starting with v6, there is a new implementation you can use for `IMvxAndroidCurrentTopActivity`.

This new implentation uses the Android provided interface `Application.IActivityLifecycleCallbacks`, while the old one fires all changes monitoring each activity.

The default implementation remains the same, unless you make your Application class inherit from `MvxAndroidApplication`. In that case, the new version will be used:

```c#
[Application]
public class MainApplication : MvxAndroidApplication
{
    public MainApplication(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
    {
    }
}
```

Kudos to [@nmilcoff](https://github.com/nmilcoff) and [@dazinator](https://github.com/dazinator).

#### Activities and backstack 

`MvxAndroidViewsContainer` will (finally!) no longer force every activity to run on a new task (`ActivityFlags.NewTask` won't be added anymore by default).

#### Shared element transitions

Because everyone loves animations, we are lucky that [@Plac3hold3r](https://github.com/Plac3hold3r) did a great job and improved our support for shared element transitions. As it is a very Android specific topic, you might need to read a bit about how it works before getting your hands into it. 

Once you are ready to start implementing it, take a look at our [official documentation](https://www.mvvmcross.com/documentation/presenters/android-view-presenter) and the Playground sample if you want to see some code.

#### Nested fragments

Both versions of our provided ViewPresenters (default and AppCompat) now support nested fragments! To be fair we did support this in the past, but we took it from 1 level indentation to N levels. Quite cool, right? Kudos to [@Qwin](https://github.com/Qwin).

#### Removed layouts

`MvxRelativeLayout`, `MvxFrameLayout` and `MvxTableLayout` were removed as they were memory inefficient (nothing we can do to improve that).

### macOS

Our `WebBrowser` plugin now has support for macOS! All thanks to [@tofutim](https://github.com/tofutim).

### UWP

We now cover scenarios where apps are launched from file associations, URIs and many more! At code level this means `MvxFormsWindowsSetup` now expects a parameter of type `IActivatedEventArgs` instead of `LaunchActivatedEventArgs`. Thanks for this [@MartinZikmund](https://github.com/MartinZikmund)!

[kipters](https://github.com/kipters) made a great job adding StarterPack content for UWP! But unfortunately NuGet doesn't like nuspec content anymore. We are actively looking for a way to improve the installation experience.

### Tizen

Although the status is not yet PRD Ready, initial support for the platform was already added. We look forward too see what developers will build with MvvmCross & Tizen!

### Others

#### iOS Support

iOS Support has been redesigned. Most pieces are now part of the main lib, while the sidebar support has become now a plugin that you can install on your iOS project.

#### MvxNotifyTask

`MvxNotifyTask` now has an optional callback to set an action to be run when an exception happens.

#### Improved flexibility Setup

MvvmCross has always been easy to extend and customize, but we never stop improving! Starting with 6.0, [@nickrandolph](https://github.com/nickrandolph) has made it much easier to provide your own implementations for `IMvxViewModelByNameLookup`, `IMvxViewModelByNameRegistry` and `IMvxViewModelTypeFinder`. 

#### Commands

`MvxAsyncCommand<T>` now implements `IMvxCommand`, same as others. Thanks to [@kipters](https://github.com/kipters), [@softlion](https://github.com/softlion) and [@nickrandolph](https://github.com/nickrandolph) for making our lives easier!

#### Framework Unit Testing

[@Cheesebaron](https://github.com/Cheesebaron) took the chance and converted all our Unit Tests to XUnit, which works better for some platforms. After that he didn't stop there and he added a bunch more of tests. Let's help him and improve our coverage for the next version!

`RaiseCanExecuteChanged` is now much easier to test, since [@jacobduijzer](https://github.com/jacobduijzer) has added some helpers and extension methods to ensure whether `CanExecuteChanged` has been raised or not. You can read more about this on the [official documentation](https://www.mvvmcross.com/documentation/fundamentals/testing).

#### Bindings

[@Saratsin](https://github.com/Saratsin) added a new type of binding: `MvxEventNameTargetBinding` (and `MvxEventNameTargetBinding<TEventArgs>`). This new type is a shortcut for adding `OneWay` bindings to commands, based on specific events. By default it also passes event handler's `EventArgs` (or `TEventArgs`) as a command parameter (but you can disable it!).

`WithDictionaryConversion` won't require you to specify values for every key that you want to use. To be more specific, it will now accept a fallback value. All thanks to [@Plac3hold3r](https://github.com/Plac3hold3r).
If you don't know what this is, then maybe it's time for you to read our [official documentation](https://www.mvvmcross.com/documentation/fundamentals/data-binding#dictionary-conversion).


# Change Log

## [6.0.0-beta.5](https://github.com/MvvmCross/MvvmCross/tree/6.0.0-beta.5) (2018-03-09)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/6.0.0-beta4...6.0.0-beta.5)

**Fixed bugs:**

- OnStart is not called for a Xamarin Forms iOS application [\#2223](https://github.com/MvvmCross/MvvmCross/issues/2223)
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
- Binding types fix [\#2632](https://github.com/MvvmCross/MvvmCross/pull/2632) ([Saratsin](https://github.com/Saratsin))
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
- MvxWindowsPage cannot navigate to MvxContentPage [\#2466](https://github.com/MvvmCross/MvvmCross/issues/2466)
- Allow more than 5 children in MoreNavigationController [\#2610](https://github.com/MvvmCross/MvvmCross/pull/2610) ([thefex](https://github.com/thefex))

**Closed issues:**

- MvxObservableCollection reports wrong index when doing AddRange [\#2515](https://github.com/MvvmCross/MvvmCross/issues/2515)

**Merged pull requests:**

- Improve the IMvxPluginManager interface [\#2616](https://github.com/MvvmCross/MvvmCross/pull/2616) ([willsb](https://github.com/willsb))
- Fixed issue \#2515 where MvxObservableCollection.AddRange\(\) reports wrong index [\#2614](https://github.com/MvvmCross/MvvmCross/pull/2614) ([Strifex](https://github.com/Strifex))
- Fix a bunch of Warnings [\#2613](https://github.com/MvvmCross/MvvmCross/pull/2613) ([Cheesebaron](https://github.com/Cheesebaron))
- Update Forms namespaces to match traditional Xamarin  [\#2612](https://github.com/MvvmCross/MvvmCross/pull/2612) ([nmilcoff](https://github.com/nmilcoff))
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
- 'System.TypeInitializationException' In 'MvvmCross.Core.Platform.LogProviders.ConsoleLogProvider' On UWP Projects [\#2333](https://github.com/MvvmCross/MvvmCross/issues/2333)
- Inconsistent PCL profile for PictureChooser [\#2295](https://github.com/MvvmCross/MvvmCross/issues/2295)
- Align nuspec profiles with plugin PCL profiles [\#2031](https://github.com/MvvmCross/MvvmCross/issues/2031)
-  \#2600 no backstack fragment close does not work hotfix [\#2601](https://github.com/MvvmCross/MvvmCross/pull/2601) ([thefex](https://github.com/thefex))
- Fix inheritance for MvxBaseSplitViewController class with constraint [\#2564](https://github.com/MvvmCross/MvvmCross/pull/2564) ([nmilcoff](https://github.com/nmilcoff))
- MvxBottomSheetDialogFragment: Fix OnDestroy [\#2526](https://github.com/MvvmCross/MvvmCross/pull/2526) ([nmilcoff](https://github.com/nmilcoff))
- Improve implementation of IMvxAndroidCurrentTopActivity [\#2513](https://github.com/MvvmCross/MvvmCross/pull/2513) ([nmilcoff](https://github.com/nmilcoff))
- Fix MvxTabBarViewController.CloseTab: Pick correct ViewController [\#2506](https://github.com/MvvmCross/MvvmCross/pull/2506) ([nmilcoff](https://github.com/nmilcoff))
- Switch parameters in MvxException so that first exception is InnerException [\#2504](https://github.com/MvvmCross/MvvmCross/pull/2504) ([mubold](https://github.com/mubold))
- MvxNavigationServiceAppStart: Don't swallow exceptions [\#2471](https://github.com/MvvmCross/MvvmCross/pull/2471) ([nmilcoff](https://github.com/nmilcoff))

**Closed issues:**

- Correct way to register navigation service [\#2571](https://github.com/MvvmCross/MvvmCross/issues/2571)
- \[Question\] - thoughts on routing for navigation [\#2563](https://github.com/MvvmCross/MvvmCross/issues/2563)
- Remove usage of MvxTrace from code [\#2541](https://github.com/MvvmCross/MvvmCross/issues/2541)
- System.Reflection GetCustomAttributes [\#2535](https://github.com/MvvmCross/MvvmCross/issues/2535)
- Inconsistency between MvxCommand\<T\> and MvxAsyncCommand\<T\> implementing IMvxCommand [\#2520](https://github.com/MvvmCross/MvvmCross/issues/2520)
- Remove IMvxIosModalHost [\#2475](https://github.com/MvvmCross/MvvmCross/issues/2475)
- WithConversion\<...\> does not appear do the same as WithConversion\(new ...\) [\#2449](https://github.com/MvvmCross/MvvmCross/issues/2449)
- The new logger infrastructure should not send null messages to IMvxLog [\#2437](https://github.com/MvvmCross/MvvmCross/issues/2437)
- MvxExpandableListView GroupClick binding replaces group expanding functionality [\#2408](https://github.com/MvvmCross/MvvmCross/issues/2408)
- \[Android\] MvxSplashScreenActivity should not have to be the first Activity [\#2261](https://github.com/MvvmCross/MvvmCross/issues/2261)
- MvxViewModel\<TParameter\>.Init should be removed [\#2257](https://github.com/MvvmCross/MvvmCross/issues/2257)
- MvxTabLayoutPresentationAttribute title localization [\#2211](https://github.com/MvvmCross/MvvmCross/issues/2211)
- Android RecyclerView.ViewHolder DataContext property change view updates lost depending on RecyclerView item position on screen [\#2142](https://github.com/MvvmCross/MvvmCross/issues/2142)
- MvxObservableCollection cause Adapter to show wrong cells data. [\#2130](https://github.com/MvvmCross/MvvmCross/issues/2130)
- .NET Standard support [\#2059](https://github.com/MvvmCross/MvvmCross/issues/2059)

**Merged pull requests:**

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
- Fixing Android attributes [\#2529](https://github.com/MvvmCross/MvvmCross/pull/2529) ([nickrandolph](https://github.com/nickrandolph))
- Fixing layout of files for plugins for multi-targetting [\#2528](https://github.com/MvvmCross/MvvmCross/pull/2528) ([nickrandolph](https://github.com/nickrandolph))
- Update navigation.md [\#2524](https://github.com/MvvmCross/MvvmCross/pull/2524) ([asterixorobelix](https://github.com/asterixorobelix))
- Adding NativePage to Playground sample for UWP [\#2523](https://github.com/MvvmCross/MvvmCross/pull/2523) ([nickrandolph](https://github.com/nickrandolph))
- Fix native pages not loading without attribute [\#2522](https://github.com/MvvmCross/MvvmCross/pull/2522) ([martijn00](https://github.com/martijn00))
- Update mvvmcross-overview.md [\#2521](https://github.com/MvvmCross/MvvmCross/pull/2521) ([asterixorobelix](https://github.com/asterixorobelix))
- Fix minor typos [\#2519](https://github.com/MvvmCross/MvvmCross/pull/2519) ([programmation](https://github.com/programmation))
- MvxAndroidViewsContainer: Remove NewTask flag as default [\#2516](https://github.com/MvvmCross/MvvmCross/pull/2516) ([nmilcoff](https://github.com/nmilcoff))
- Add LogLevel availability check to IMvxLog [\#2514](https://github.com/MvvmCross/MvvmCross/pull/2514) ([willsb](https://github.com/willsb))
-  Make Mvx...Cell inherit from IMvxCell instead of IMvxElement [\#2511](https://github.com/MvvmCross/MvvmCross/pull/2511) ([mubold](https://github.com/mubold))
- Update message displayed for OnViewNewIntent [\#2510](https://github.com/MvvmCross/MvvmCross/pull/2510) ([nmilcoff](https://github.com/nmilcoff))
- Added reference to a newer blog post on resx localization by Daniel Krzyczkowski [\#2509](https://github.com/MvvmCross/MvvmCross/pull/2509) ([mellson](https://github.com/mellson))
- Checks fragment type when showing a DialogFragment and throws MvxException instead of InvalidCastException [\#2507](https://github.com/MvvmCross/MvvmCross/pull/2507) ([MKuckert](https://github.com/MKuckert))
- Merge \#2438 into develop [\#2505](https://github.com/MvvmCross/MvvmCross/pull/2505) ([martijn00](https://github.com/martijn00))
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
