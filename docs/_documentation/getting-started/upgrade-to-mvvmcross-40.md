---
layout: documentation
title: Upgrade to MvvmCross 4.0
category: Getting-started
---
[block:api-header]
{
  "type": "basic",
  "title": "Nuget package changes"
}
[/block]
Since MvvmCross beta8, the Nuget package names have changed. All packages containing the name `HotTuna` or `Cirrious` have been renamed. If you update the packages using Nuget to a version greater than beta7 or to the stable, the new Nuget packages will be automatically installed. However, you should delete the old `HotTuna` or `Cirrious` packages from your solution after the update.
[block:parameters]
{
  "data": {
    "0-0": "`MvvmCross.HotTuna.AutoViews`",
    "h-0": "Old Nuget package",
    "h-1": "New Nuget package",
    "0-1": "`MvvmCross.AutoView`",
    "1-0": "`MvvmCross.HotTuna.Binding`",
    "1-1": "`MvvmCross.Binding`",
    "2-0": "`MvvmCross.HotTuna.CrossCore`",
    "2-1": "`MvvmCross.Platform`",
    "3-0": "`MvvmCross.HotTuna.Droid.AutoViews`",
    "3-1": "`MvvmCross.AutoView.Droid`",
    "4-0": "`MvvmCross.HotTuna.Droid.Dialog`",
    "4-1": "`MvvmCross.Dialog.Droid`",
    "5-0": "`MvvmCross.HotTuna.MvvmCrossLibraries`",
    "5-1": "`MvvmCross.Core`",
    "10-0": "`MvvmCross`",
    "10-1": "`MvvmCross`",
    "6-0": "`MvvmCross.HotTuna.StarterPack`",
    "6-1": "`MvvmCross.StarterPack`",
    "7-0": "`MvvmCross.HotTuna.Tests`",
    "7-1": "`MvvmCross.Tests`",
    "8-0": "`MvvmCross.HotTuna.Touch.AutoViews`",
    "8-1": "`MvvmCross.AutoView.iOS`",
    "9-0": "`MvvmCross.HotTuna.Touch.Dialog`",
    "9-1": "`MvvmCross.Dialog.iOS`"
  },
  "cols": 2,
  "rows": 11
}
[/block]
Additionally, the `JsonLocalisation` plugin has been renamed to `JsonLocalization`.
The package `MvvmCross.StarterPack` contains sample code and should be removed manually from the `packages.config` files after adding it to your project (its dependencies will remain there). For an empty project, it is suggested you install `MvvmCross.StarterPack`. For an existing project or if you don't want the sample code, it is suggested you install `MvvmCross`.
[block:api-header]
{
  "type": "basic",
  "title": "Namespace changes"
}
[/block]
The namespaces beginning with `Cirrious` have had the `Cirrious` part removed from them. `Cirrious.CrossCore` has been moved to `MvvmCross.Platform` and `Cirrious.MvvmCross` has moved to `MvvmCross.Core`. All namespaces named `Touch` have been renamed to `iOS` since MonoTouch has since been renamed to Xamarin.iOS. If you are maintaining a plugin and have a project ending in `.Touch` you should also rename it to `.iOS`.
[block:api-header]
{
  "type": "basic",
  "title": "Type changes"
}
[/block]
All classes, methods, and properties containing the term `Touch` were replaced with `Ios`. For example, the class `MvxTouchSetup` is now called `MvxIosSetup`.
[block:api-header]
{
  "type": "basic",
  "title": "Mac support"
}
[/block]
All Nugets now support Xamarin.Mac ([Unified API](https://developer.xamarin.com/guides/cross-platform/macios/unified/)). Add the `MvvmCross.StarterPack` Nuget to an empty Xamarin.Mac project in Xamarin Studio to help you get started with sample code.