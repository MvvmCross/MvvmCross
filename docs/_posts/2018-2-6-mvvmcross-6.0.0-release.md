---
layout: post
title: MvvmCross 6.0
date:   2018-2-6 14:00:00 -0300
categories: mvvmcross
---

# Announcing MvvmCross 6.0!

Yes, you read it correctly! MvvmCross 6 has finally arrived and it is available on [NuGet](https://www.nuget.org/packages/MvvmCross/6.0.0)!

## What's new?!

- Migration to .NET Standard
- Polished support for Xamarin.Forms
- Supercharged `IMvxOverridePresentationAttribute` for ViewPresenters
- Support multiple levels of nested fragments on Android
- Initial support for Tizen 
- Tons of minor improvements and bug fixes!

## Migration guide

MvvmCross 6 comes with quite a lot of improvements, but this also means some things will break. We have prepared a migration guide that will help you do the transition real quick!

### NuGet packages

With MvvmCross 6 there are some changes to the NuGet packages. The following packages are obsolete and included in the main `MvvmCross` package:

- MvvmCross.Core
- MvvmCross.Platform
- MvvmCross.Binding

NuGet packages are now versioned using SemVer 2.0, meaning you need to use Visual Studio 2017 (15.3) and above or NuGet 4.3.0 and above.

The MvvmCross.* namespace has been reserved on NuGet, meaning that plugin authors should move their plugins away from this package namespace. We are also planning on signing future releases.

### Plugins

No more bootstrap file! Yes, you read it correctly. [@willsb](https://github.com/willsb) has worked hard on an easier way to register your MvvmCross plugins, by simply adding the `[MvxPlugin]` attribute to your plugin and inherit from `IMvxPlugin` as usual.

Read more about [how to get started with plugins in our documentation](https://www.mvvmcross.com/documentation/plugins/getting-started-with-plugin-development).

#### Json and Resx plugins

All methods in `MvxResxTextProvider`, `MvxJsonDictionaryTextProvider` and `MvxTextProvider` are now virtual. Customization is now much easier!

### .NET Standard

TBA

### ViewPresenters

`IMvxOverridePresentationAttribute.PresentationAttribute` now takes a `MvxViewModelRequest` as parameter. As a result, when the method `PresentationAttribute` is called, you will be able to make your choice on which attribute to use taking advantage of the ViewModel request. But that's not everything! If you are using the MvxNavigationService, you can cast the arriving parameter of type `MvxViewModelRequest` to be a `MvxViewModelInstanceRequest`, which will allow you to see the ViewModel that is being presented. 
This change was made by [@nmilcoff](https://github.com/nmilcoff).

ViewPresenters registration was aligned and improved on many platforms. You can now obtain the current ViewPresenter from anywhere by resolving the interface `IMvxViewPresenter`. All thanks to the amazing [@martijn00](https://github.com/martijn00)!
OverrideAttribute: https://github.com/MvvmCross/MvvmCross/pull/2483

### Navigation

The brand new MvxNavigationService that was introduced in MvvmCross 5 is now the default. This means `ShowViewModel` has been finally removed, as well as `MvxNavigatingObject`. If you aren't using it yet, it's time you take a look at the [official documentation](https://www.mvvmcross.com/documentation/fundamentals/navigation).

The intermediary helper class `MvxNavigationServiceAppStart` has been removed as well, because the classic `MvxAppStart` now uses MvxNavigationService internally.

### IoC

TBA
IoC Child containers: https://github.com/MvvmCross/MvvmCross/pull/2438

### Logging
`MvxTrace` and everything related was removed. The new (and much improved) logging system was already present since MvvmCross 5.4. If you haven't heard about it, please take a look at the [official documentation](https://www.mvvmcross.com/documentation/fundamentals/logging)

### Xamarin.Forms

#### ViewCells
`MvxViewCell` is now usable! We have [fixed](https://github.com/MvvmCross/MvvmCross/pull/2511) a bug and apps won't crash in runtime anymore. It is recommended that you use `MvxViewCell` in ListViews instead of the default `ViewCell`.

#### Support for "native" views
[@martijn00](https://github.com/martijn00) and [@nickrandolph](https://github.com/nickrandolph) fixed several issues regarding the Forms ViewPresenter not being able to display native views. We now provide a much better support for it!  
 
### iOS

TBA

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

#### Nested fragments
Both versions of our provided ViewPresenters (default and AppCompat) now support nested fragments! To be fair we did support this in the past, but we took it from 1 level indentation to N levels. Quite cool, right? Kudos to [@Qwin](https://github.com/Qwin).

#### Removed layouts
`MvxRelativeLayout`, `MvxFrameLayout` and `MvxTableLayout` were removed as they were memory inefficient (nothing we can do to improve that).

Color plugin: https://github.com/MvvmCross/MvvmCross/pull/2557
Removed some layouts: https://github.com/MvvmCross/MvvmCross/pull/2561

### macOS

Our `WebBrowser` plugin now has support for macOS! All thanks to [@tofutim](https://github.com/tofutim).

### UWP
We now cover scenarios where apps are launched from file associations, URIs and many more! At code level this means `MvxFormsWindowsSetup` now expects a parameter of type `IActivatedEventArgs` instead of `LaunchActivatedEventArgs`. Thanks for this [@MartinZikmund](https://github.com/MartinZikmund)!

[kipters](https://github.com/kipters) made a great job adding StarterPack content for UWP! But unfortunately NuGet doesn't like nuspec content anymore. We are actively looking for a way to improve the installation experience.

### Tizen

Although the status is not yet PRD Ready, initial support for the platform was already added. We look forward too see what developers will build with MvvmCross & Tizen!

### Others

#### Improved Setup

MvvmCross has always been easy to extend and customize, but we never stop improving! Starting with 6.0, [@nickrandolph](https://github.com/nickrandolph) has made it much easier to provide your own implementations for `IMvxViewModelByNameLookup`, `IMvxViewModelByNameRegistry` and `IMvxViewModelTypeFinder`. 

#### Commands
`MvxAsyncCommand<T>` now implements `IMvxCommand`, same as others. Thanks to [@kipters](https://github.com/kipters), [@softlion](https://github.com/softlion) and [@nickrandolph](https://github.com/nickrandolph) for making our lives easier!

#### Framework Unit Testing

[@Cheesebaron](https://github.com/Cheesebaron) took the chance and converted all our Unit Tests to XUnit, which works better for some platforms. After that he didn't stop there and he added a bunch more of tests. Let's help him and improve our coverage for the next version!

TBA
Bindings: https://github.com/MvvmCross/MvvmCross/pull/2490
Dictionary Conversion: https://github.com/MvvmCross/MvvmCross/pull/2480


# Change Log

Coming soon!
## [6.0.0](https://github.com/MvvmCross/MvvmCross/tree/6.0.0) (2018-2-6)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.6.3...6.0.0)
