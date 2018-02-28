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

As part of the netstandard migration we took the chance to align some names and namespaces. We have simplified the solution structure to make it easier to get started with, and also changed some names to improve consistency across all platforms.

We know this (sadly) means breaking changes, so here you have a small summary that should help in the process of upgrading:

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
As we now don't have separate projects for each platform implementations, we got rid of the Xamarin limitation for naming android projects as "Android" (that's why the convention of `Droid` exists!). In summary, we have changed most namespaces that included `Droid` to `Android`.

#### UWP note
Previously we had a mix between `Uwp` and `Uap` namespaces. We have unified them all to `Uap`.

### Core changes
- `MvvmCross.Platform` is now just `MvvmCross`.
- Most framework core clases are now under `MvvmCross.Base`.
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
- `MvvmCross.Platform.Logging` is now `MvvmCross.Logging`.

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
All plugin namespaces changed, but you shouldn't worry about it unless you are diving deep into their implementations. 
- The `Plugins` keyword is now `Plugin`.
- `MvvmCross.Plugins.{PluginName}.{Platform}` is now `MvvmCross.Plugin.{PluginName}.Platform.{Platform}`

## Breaking changes

#### ViewPresenters 
- `IMvxOverridePresentationAttribute.PresentationAttribute` now takes a `MvxViewModelRequest` as parameter. 
If you're using a custom ViewPresenter that extends the default provided by MvvmCross, be aware that `GetPresentationAttribute` and `GetOverridePresentationAttribute` methods signatures have changed.
- `MvxFormsPagePresenter` constructor now takes a single parameter. You will be affected by this change only if you are using Xamarin Forms and you are providing a custom pages presenter for a platform ViewPresenter.

#### Logging

`IMvxLog` has a new method: `bool IsLogLevelEnabled(MvxLogLevel logLevel)`.

TBA

## Plugins

`DownloadCache` was removed in v6.0. If you need an alternative, take a look at [FFImageLoading](https://github.com/luberda-molinet/FFImageLoading/wiki/MvvmCross)

