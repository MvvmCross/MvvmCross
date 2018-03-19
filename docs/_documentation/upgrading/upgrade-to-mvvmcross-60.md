---
layout: documentation
title: Upgrade from 5 to MvvmCross 6
category: Upgrading
Order: 3
---

This guide assumes you are using MvvmCross 5.6.x. If you are updating your app from a previous version, please look at the appropiate blog posts.

## .NET Standard

TBA

## Names and namespaces changes

As part of the netstandard migration we took the chance to align some names and namespaces. We have massively simplified the solution structure to make it easier to get started with, and also changed some names in order to improve consistency across all platforms.

We know this (sadly) means breaking changes, so here you have a small summary which should help in the process of upgrading:

#### Extensions vs ExtensionMethods
In the past we had a mix for extension methods between `Extensions` and `ExtensionMethods`. All extension classes are now called `Extensions`.

#### iOS
Previously we had a mix between `iOS` and `Ios` namespaces. We have unified them all to `Ios`.

#### Android 
As we now don't have separate projects for each platform's implementations, we got rid of the Xamarin limitation for naming android projects as "Android" (that's why the convention of `Droid` exists!). In summary, we have changed most namespaces that included `Droid` to `Android`.

#### tvOS
Previously we had a mix between `tvOS` and `Tvos` namespaces. We have unified them all to `Tvos`.

#### UWP
Previously we had a mix between `Uwp` and `Uap` namespaces. We have unified them all to `Uap`.

#### Xamarin.Forms
- `MvvmCross.Forms.Platform` is now `MvvmCross.Forms.Core`.
- `MvvmCross.Forms.{Platform}` is now `MvvmCross.Forms.Platform.{Platform}`.

#### Core and all platforms
- `MvvmCross.Platform` namespace is now `MvvmCross`.
- Most framework core classes are now under the namespace `MvvmCross.Base`.
- `MvvmCross.Platform.{Platform}.Platform` is now `MvvmCross.Platform.{Platform}.Base`.
- `MvvmCross.Binding.{Platform}` namespace is now `MvvmCross.Platform.{Platform}.Binding`.
- `MvvmCross.Core.Platform.Converters`namespace is now `MvvmCross.Converters`
- `MvvmCross.Core.Platform` namespace is now `MvvmCross.Core`.
- Platform initialization code (Setup classes, ..) was moved from `MvvmCross.{Platform}.Platform` to the namespace `MvvmCross.Platform.{Platform}.Core`.
- `MvvmCross.Platform.{Platform}.Views` is now under the namespace namespace `MvvmCross.Platform.{Platform}.Views.Base`.
- `MvvmCross.Platform.UI` namespace is now `MvvmCross.UI`.
- `MvvmCross.Platform.Exceptions` namespace is now `MvvmCross.Exceptions`
- All PresentationHints are now at `MvvmCross.ViewModels.Hints`.
- `MvvmCross.Core.ViewModels` namespace is now `MvvmCross.ViewModels`.
- `MvvmCross.{Platform}.Platform` namespace is now `MvvmCross.Platform.{Platform}.Core`.
- `MvvmCross.{Platform}.ViewModels` namespace is now `MvvmCross.Platform.{Platform}.ViewModels`.
- `MvvmCross.{Platform}.Views` namespace is now `MvvmCross.Platform.{Platform}.Views`.
- `MvvmCross.Core.Views` namespace is now `MvvmCross.Views`.
- `MvvmCross.Platform.ExtensionMethods.MvxCrossCoreExtensions` class is now `MvvmCross.Base.MvxCoreExtensions`.
- `MvvmCross.Platform.WeakSubscription` namespace is now `MvvmCross.WeakSubscription`.
- `MvvmCross.Test.Core` namespace is now `MvvmCross.Test`.
- `JetBrains.Annotations` namespace is now `MvvmCross.Annotations`.

#### IoC
- All IoC related code was moved from `MvvmCross.Platform.IoC` to `MvvmCross.IoC`.
- `MvxSimpleIoCContainer` is now `MvxIoCContainer`.

#### Logging
`MvxTrace` and everything related was removed in v6. Please take a look at the [official documentation](https://www.mvvmcross.com/documentation/fundamentals/logging) to learn about the new logging system.

- `MvvmCross.Platform.Logging` is now `MvvmCross.Logging`.
- `MvvmCross.Core.Platform.LogProviders` namespace is now `MvvmCross.Platform.Logging.LogProviders`.

#### Navigation
- `MvvmCross.Core.Navigation` is now `MvvmCross.Navigation`.
- `ShowViewModel` and `MvxNavigatingObject` were removed. `MvxNavigationService` should be used instead. Please take a look at the [official documentation](https://www.mvvmcross.com/documentation/fundamentals/navigation) to learn about it.
- `MvxNavigationServiceAppStart` was removed and `MvxAppStart` uses the navigation service internally.

#### Binding
- `MvvmCross.Binding.{Platform}` is now available at `MvvmCross.Platform.{Platform}.Binding`.

#### Commands
All Commands related code was moved from `MvvmCross.Core.ViewModels` to `MvvmCross.Commands`.

#### ViewPresenters
ViewPresenters are now under their own folder. Therefore we had to modify namespaces: 
- Presenters code under `MvvmCross.{Platform}.Views` is now at `MvvmCross.Platform.{Platform}.Presenters`
- `MvvmCross.{Platform}.Views.Attributes` to `MvvmCross.Platform.{Platform}.Presenters.Attributes` 

#### Plugins
All plugin namespaces have changed, but you shouldn't worry about it unless you are diving deep into their implementations. 
- The `Plugins` keyword is now `Plugin`.
- `MvvmCross.Plugins.{PluginName}.{Platform}` is now `MvvmCross.Plugin.{PluginName}.Platform.{Platform}`
- `MvvmCross.Platform.Plugins` namespace is now `MvvmCross.Plugin`.
- Android color plugin implementation: `ToAndroidColor` was renamed to `ToNativeColor`.

## Breaking changes

#### ViewPresenters 
- `IMvxOverridePresentationAttribute.PresentationAttribute` now takes a `MvxViewModelRequest` as parameter. 
If you're using a custom ViewPresenter that extends the default provided by MvvmCross, be aware that `GetPresentationAttribute` and `GetOverridePresentationAttribute` methods signatures have changed.
- `MvxFormsPagePresenter` constructor now takes a single parameter. You will be affected by this change only if you are using Xamarin Forms and you are providing a custom pages presenter for a platform ViewPresenter.
- On iOS, macOS and tvOS the Setup method `CreatePresenter` was renamed to `CreateViewPresenter`.
- On iOS and tvOS, the method `CleanupModalViewControllers` was renamed to `CloseModalViewControllers`.
- The method `NativeModalViewControllerDisappearedOnItsOwn` is now called `CloseTopModalViewController`.
- `IMvxTvosModalHost` and `IMvxIosModalHost` were deprecated and removed.
- On iOS, the method `PresentModalViewController` was renamed to `ShowModalViewController`.

#### Navigation
`IMvxNavigationService.ChangePresentation` has been marked async (the method now returns a `Task<bool>`).

#### Logging

`IMvxLog` has a new method: `bool IsLogLevelEnabled(MvxLogLevel logLevel)`.

#### Plugins removed

`DownloadCache` was removed in v6.0. If you need an alternative, take a look at [FFImageLoading](https://github.com/luberda-molinet/FFImageLoading/wiki/MvvmCross)

#### Android

- `MvxRelativeLayout`, `MvxFrameLayout` and `MvxTableLayout` were removed as they were memory inefficient (nothing we can do to improve that).
