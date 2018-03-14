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

As part of the netstandard migration we took the chance to align some names and namespaces. We have simplified the solution structure to make it easier to get started with, and also changed some names in order to improve consistency across all platforms.

We know this (sadly) means breaking changes, so here you have a small summary which should help in the process of upgrading:

### Changed names
MvxSimpleIoCContainer -> MvxIoCContainer.
Android color plugin implementation: ToAndroidColor -> ToNativeColor
ReflectionExtensions -> MvxReflectionExtensions

#### Extensions vs ExtensionMethods
In the past we had a mix for extension methods between `Extensions` and `ExtensionMethods`. All extension classes are now called `Extensions`.

### Changed namespaces

#### iOS note
Previously we had a mix between `iOS` and `Ios` namespaces. We have unified them all to `Ios`.

#### Android note
As we now don't have separate projects for each platform's implementations, we got rid of the Xamarin limitation for naming android projects as "Android" (that's why the convention of `Droid` exists!). In summary, we have changed most namespaces that included `Droid` to `Android`.

#### UWP note
Previously we had a mix between `Uwp` and `Uap` namespaces. We have unified them all to `Uap`.

### Core changes
- `MvvmCross.Platform` is now just `MvvmCross`.
- Most framework core classes are now under `MvvmCross.Base`.
- `MvvmCross.{Platform}.Views` is now `MvvmCross.Platform.{Platform}.Views`.
- `MvvmCross.Platform.Converters` is now `MvvmCross.Converters`.
- `MvvmCross.Core.Platform` is now `MvvmCross.Core`.
- Platform initialization code (Setup class, ..) was moved from `MvvmCross.{Platform}.Platform` to `MvvmCross.Platform.{Platform}.Core`.
- `MvvmCross.Platform.{Platform}.Views` is now under `MvvmCross.Platform.{Platform}.Views.Base`.
- `MvvmCross.Platform.UI` is now `MvvmCross.UI`.
- All PresentationHints are now at `MvvmCross.ViewModels.Hints`.
- `MvvmCross.Core.ViewModels` is now `MvvmCross.ViewModels`.
- `MvvmCross.Core.Views` is now `MvvmCross.Views`.

### IoC
- All IoC related code was moved from `MvvmCross.Platform.IoC` to `MvvmCross.IoC`.

### Logging
`MvxTrace` and everything related was removed in v6. Please take a look at the [official documentation](https://www.mvvmcross.com/documentation/fundamentals/logging) to learn about the new logging system.

- `MvvmCross.Platform.Logging` is now `MvvmCross.Logging`.
- `MvvmCross.Core.Platform.LogProviders` is now `MvvmCross.Platform.Logging.LogProviders`.

### Navigation
- `MvvmCross.Core.Navigation` is now `MvvmCross.Navigation`.

### Binding
- `MvvmCross.Binding.{Platform}` is now available at `MvvmCross.Platform.{Platform}.Binding`.

### Commands
All Commands related code was moved from `MvvmCross.Core.ViewModels` to `MvvmCross.Commands`.

### ViewPresenters
ViewPresenters are now under their own folder. Therefore we had to modify namespaces: 
- Presenters code under `MvvmCross.{Platform}.Views` is now at `MvvmCross.Platform.{Platform}.Presenters`
- `MvvmCross.{Platform}.Views.Attributes` to `MvvmCross.Platform.{Platform}.Presenters.Attributes` 

### Plugins
All plugin namespaces have changed, but you shouldn't worry about it unless you are diving deep into their implementations. 
- The `Plugins` keyword is now `Plugin`.
- `MvvmCross.Plugins.{PluginName}.{Platform}` is now `MvvmCross.Plugin.{PluginName}.Platform.{Platform}`
- If using Mvmm versions less than 6.0.0, the resolution of namespaces can be achieved by using a platform specific bootstrap class in the rootnamespace. See the [iOS specific implementation](https://github.com/MvvmCross/MvvmCross/blob/5.6.3/nuspec/iOSBootstrapContent/WebBrowserPluginBootstrap.cs.pp). Note that your bootstrap file must be a class and as such should have the .cs extension, not the .cs.pp extension.

### Plugins removed

`DownloadCache` was removed in v6.0. If you need an alternative, take a look at [FFImageLoading](https://github.com/luberda-molinet/FFImageLoading/wiki/MvvmCross)
## Breaking changes

#### ViewPresenters 
- `IMvxOverridePresentationAttribute.PresentationAttribute` now takes a `MvxViewModelRequest` as parameter. 
If you're using a custom ViewPresenter that extends the default provided by MvvmCross, be aware that `GetPresentationAttribute` and `GetOverridePresentationAttribute` methods signatures have changed.
- `MvxFormsPagePresenter` constructor now takes a single parameter. You will be affected by this change only if you are using Xamarin Forms and you are providing a custom pages presenter for a platform ViewPresenter.

#### Logging

`IMvxLog` has a new method: `bool IsLogLevelEnabled(MvxLogLevel logLevel)`.
