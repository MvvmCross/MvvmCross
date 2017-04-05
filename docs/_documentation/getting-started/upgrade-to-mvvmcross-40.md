---
layout: documentation
title: Upgrade from 3 to MvvmCross 4
category: Getting-started
---
## NuGet package changes

Since MvvmCross 4, the NuGet package names have changed. All packages containing the name `HotTuna` or `Cirrious` have been renamed. If you update the packages using Nuget to a version greater than 4, the new NuGet packages will be automatically installed. However, you should delete the old `HotTuna` or `Cirrious` packages from your solution after the update.

old NuGet package                      | new NuGet package
-------------------------------------- | -----------------
`MvvmCross.HotTuna.AutoViews`          | `MvvmCross.AutoView`
`MvvmCross.HotTuna.Binding`            | `MvvmCross.Binding`
`MvvmCross.HotTuna.CrossCore`          | `MvvmCross.Platform`
`MvvmCross.HotTuna.Droid.AutoViews`    | `MvvmCross.AutoView.Droid`
`MvvmCross.HotTuna.Droid.Dialog`       | `MvvmCross.Dialog.Droid`
`MvvmCross.HotTuna.MvvmCrossLibraries` | `MvvmCross.Core`
`MvvmCross.HotTuna.StarterPack`        | `MvvmCross.StarterPack`
`MvvmCross.HotTuna.Tests`              | `MvvmCross.Tests`
`MvvmCross.HotTuna.Touch.AutoViews`    | `MvvmCross.AutoView.iOS`
`MvvmCross.HotTuna.Touch.Dialog`       | `MvvmCross.Dialog.iOS`
`MvvmCross`                            | `MvvmCross`

Additionally, the `JsonLocalisation` plugin has been renamed to `JsonLocalization`.
The package `MvvmCross.StarterPack` contains sample code and should be removed manually from the `packages.config` files after adding it to your project (its dependencies will remain there). For an empty project, it is suggested you install `MvvmCross.StarterPack`. For an existing project or if you don't want the sample code, it is suggested you install `MvvmCross`.

## Namespace changes

The namespaces beginning with `Cirrious` have had the `Cirrious` part removed from them. `Cirrious.CrossCore` has been moved to `MvvmCross.Platform` and `Cirrious.MvvmCross` has moved to `MvvmCross.Core`. All namespaces named `Touch` have been renamed to `iOS` since MonoTouch has since been renamed to Xamarin.iOS. If you are maintaining a plugin and have a project ending in `.Touch` you should also rename it to `.iOS`.

## Type changes

All classes, methods, and properties containing the term `Touch` were replaced with `Ios`. For example, the class `MvxTouchSetup` is now called `MvxIosSetup`.

## Mac support

All Nugets now support Xamarin.Mac ([Unified API](https://developer.xamarin.com/guides/cross-platform/macios/unified/)). Add the `MvvmCross.StarterPack` NuGet to an empty Xamarin.Mac project in Xamarin Studio to help you get started with sample code.

