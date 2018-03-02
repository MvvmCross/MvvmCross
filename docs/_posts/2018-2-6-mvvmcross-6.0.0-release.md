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

### .NET Standard

TBA

### ViewPresenters

TBA
General alignment: https://github.com/MvvmCross/MvvmCross/pull/2558
OverrideAttribute: https://github.com/MvvmCross/MvvmCross/pull/2483

### Navigation

TBA
Removed ShowViewModel: https://github.com/MvvmCross/MvvmCross/pull/2559

### IoC

TBA
IoC Child containers: https://github.com/MvvmCross/MvvmCross/pull/2438

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

Color plugin: https://github.com/MvvmCross/MvvmCross/pull/2557
Removed some layouts: https://github.com/MvvmCross/MvvmCross/pull/2561

### macOS

Our `WebBrowser` plugin now has support for macOS! All thanks to [@tofutim](https://github.com/tofutim).

### UWP
[kipters](https://github.com/kipters) made a great job adding StarterPack content for UWP! But unfortunately NuGet doesn't like nuspec content anymore. We are actively looking for a way to improve the installation experience.

### Others

#### Improved Setup

MvvmCross has always been easy to extend and customize, but we never stop improving! Starting with 6.0, [@nickrandolph](https://github.com/nickrandolph) has made it much easier to provide your own implementations for `IMvxViewModelByNameLookup`, `IMvxViewModelByNameRegistry` and `IMvxViewModelTypeFinder`. 

#### Commands
`MvxAsyncCommand<T>` now implements `IMvxCommand`, same as others. Thanks to [@kipters](https://github.com/kipters), [@softlion](https://github.com/softlion) and [@nickrandolph](https://github.com/nickrandolph) for making our lives easier!

TBA
Bindings: https://github.com/MvvmCross/MvvmCross/pull/2490
Dictionary Conversion: https://github.com/MvvmCross/MvvmCross/pull/2480
Improved localization customization: https://github.com/MvvmCross/MvvmCross/pull/2579



# Change Log

Coming soon!
## [6.0.0](https://github.com/MvvmCross/MvvmCross/tree/6.0.0) (2018-2-6)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/5.6.3...6.0.0)
