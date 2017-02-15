---
layout: documentation
title: Get MvvmCross
category: Getting-started
---
[block:parameters]
{
  "data": {
    "0-2": "[Nuget](https://www.nuget.org/packages/MvvmCross.StarterPack/)",
    "0-1": "When starting a fresh project you should install this nuget. This will add all files to get you started. Afterwards you need to remove the nuget reference again manually from all your `packages.config` files (not through Nuget as this will delete the source files).",
    "0-0": "`MvvmCross.StarterPack`",
    "h-0": "Name",
    "h-1": "Description",
    "h-2": "Link",
    "1-0": "`MvvmCross`",
    "2-0": "`MvvmCross.Platform`",
    "3-0": "`MvvmCross.Core`",
    "5-0": "`MvvmCross.Tests`",
    "6-0": "`MvvmCross.Droid.FullFragging`",
    "7-0": "`MvvmCross.Dialog.iOS`",
    "8-0": "`MvvmCross.Dialog.Droid`",
    "9-0": "`MvvmCross.Console.Platform`",
    "10-0": "`MvvmCross.CodeAnalysis`",
    "4-0": "`MvvmCross.Binding`",
    "11-0": "`MvvmCross.BindingEx`",
    "12-0": "`MvvmCross.AutoView`",
    "13-0": "`MvvmCross.AutoView.iOS`",
    "14-0": "`MvvmCross.AutoView.Droid`",
    "1-2": "[Nuget](https://www.nuget.org/packages/MvvmCross/)",
    "2-2": "[Nuget](https://www.nuget.org/packages/MvvmCross.Platform/)",
    "3-2": "[Nuget](https://www.nuget.org/packages/MvvmCross.Core/)",
    "4-2": "[Nuget](https://www.nuget.org/packages/MvvmCross.Binding/)",
    "5-2": "[Nuget](https://www.nuget.org/packages/MvvmCross.Tests/)",
    "6-2": "[Nuget](https://www.nuget.org/packages/MvvmCross.Droid.FullFragging/)",
    "7-2": "[Nuget](https://www.nuget.org/packages/MvvmCross.Dialog.iOS/)",
    "8-2": "[Nuget](https://www.nuget.org/packages/MvvmCross.StarterPack/)",
    "9-2": "[Nuget](https://www.nuget.org/packages/MvvmCross.Console.Platform/)",
    "10-2": "[Nuget](https://www.nuget.org/packages/MvvmCross.CodeAnalysis/)",
    "11-2": "[Nuget](https://www.nuget.org/packages/MvvmCross.BindingEx/)",
    "12-2": "[Nuget](https://www.nuget.org/packages/MvvmCross.AutoView/)",
    "13-2": "[Nuget](https://www.nuget.org/packages/MvvmCross.AutoView.iOS/)",
    "14-2": "[Nuget](https://www.nuget.org/packages/MvvmCross.AutoView.Droid/)",
    "1-1": "Main nuget. This will add Core and Binding.",
    "2-1": "Contains the libraries for all the separate platforms.",
    "3-1": "Core libraries for the base of your app.",
    "4-1": "Bindings system for all platforms",
    "5-1": "Test helpers",
    "6-1": "Support for native (Android > 4.0) fragments, and more Android helpers",
    "10-1": "Code fixes for MvvmCross"
  },
  "cols": 3,
  "rows": 15
}
[/block]

[block:api-header]
{
  "type": "basic",
  "title": "Android Support"
}
[/block]

[block:parameters]
{
  "data": {
    "7-0": "`MvvmCross.Droid.Support.Design`",
    "0-0": "`MvvmCross.Droid.Support.V4`",
    "1-0": "`MvvmCross.Droid.Support.V7.AppCompat`",
    "2-0": "`MvvmCross.Droid.Support.V7.Fragging`",
    "3-0": "`MvvmCross.Droid.Support.V7.Preference`",
    "4-0": "`MvvmCross.Droid.Support.V7.RecyclerView`",
    "5-0": "`MvvmCross.Droid.Support.V14.Preference`",
    "6-0": "`MvvmCross.Droid.Support.V17.Leanback`",
    "0-2": "[Nuget](https://www.nuget.org/packages/MvvmCross.Droid.Support.V4/)",
    "1-2": "[Nuget](https://www.nuget.org/packages/MvvmCross.Droid.Support.V7.AppCompat/)",
    "2-2": "[Nuget](https://www.nuget.org/packages/MvvmCross.Droid.Support.V7.Fragging/)",
    "3-2": "[Nuget](https://www.nuget.org/packages/MvvmCross.Droid.Support.V7.Preference/)",
    "4-2": "[Nuget](https://www.nuget.org/packages/MvvmCross.Droid.Support.V7.RecyclerView/)",
    "5-2": "[Nuget](https://www.nuget.org/packages/MvvmCross.Droid.Support.V14.Preference/)",
    "6-2": "[Nuget](https://www.nuget.org/packages/MvvmCross.Droid.Support.V17.Leanback/)",
    "7-2": "[Nuget](https://www.nuget.org/packages/MvvmCross.Droid.Support.Design/)",
    "2-1": "Android support fragments, caching and helpers",
    "1-1": "AppCompat for MvvmCross",
    "h-1": "Description",
    "h-0": "Name",
    "h-2": "Link"
  },
  "cols": 3,
  "rows": 8
}
[/block]

[block:api-header]
{
  "type": "basic",
  "title": "iOS Support"
}
[/block]

[block:parameters]
{
  "data": {
    "0-0": "`MvvmCross.iOS.Support`",
    "0-2": "[Nuget](https://www.nuget.org/packages/MvvmCross.iOS.Support/)",
    "0-1": "Helpers, featured ViewPresenters and Base classes for iOS",
    "1-2": "[Nuget](https://www.nuget.org/packages/MvvmCross.iOS.Support.JASidePanels/)",
    "1-0": "`MvvmCross.iOS.Support.JASidePanels`",
    "1-1": "Implementation for JASidePanels library",
    "2-0": "`MvvmCross.iOS.Support.XamarinSidebar`",
    "2-2": "[Nuget](https://www.nuget.org/packages/MvvmCross.iOS.Support.XamarinSidebar/)",
    "2-1": "Implementation for XamarinSidebar library"
  },
  "cols": 3,
  "rows": 3
}
[/block]

[block:api-header]
{
  "type": "basic",
  "title": "Plugins"
}
[/block]

[block:parameters]
{
  "data": {
    "h-1": "Description",
    "h-2": "Link",
    "h-0": "Name",
    "0-0": "`MvvmCross.Plugin.All`",
    "24-0": "`MvvmCross.Plugin.WebBrowser`",
    "23-0": "`MvvmCross.Plugin.Visibility`",
    "22-0": "`MvvmCross.Plugin.ThreadUtils`",
    "21-0": "`MvvmCross.Plugin.SQLitePCL`",
    "20-0": "`MvvmCross.Plugin.SQLite`",
    "19-0": "`MvvmCross.Plugin.SoundEffects`",
    "18-0": "`MvvmCross.Plugin.Share`",
    "17-0": "`MvvmCross.Plugin.ResourceLoader`",
    "16-0": "`MvvmCross.Plugin.ReflectionEx`",
    "15-0": "`MvvmCross.Plugin.PictureChooser`",
    "14-0": "`MvvmCross.Plugin.PhoneCall`",
    "13-0": "`MvvmCross.Plugin.Network`",
    "12-0": "`MvvmCross.Plugin.MethodBinding`",
    "11-0": "`MvvmCross.Plugin.Messenger`",
    "10-0": "`MvvmCross.Plugin.Location`",
    "9-0": "`MvvmCross.Plugin.JsonLocalization`",
    "8-0": "`MvvmCross.Plugin.Json`",
    "7-0": "`MvvmCross.Plugin.File`",
    "6-0": "`MvvmCross.Plugin.FieldBinding`",
    "5-0": "`MvvmCross.Plugin.Email`",
    "4-0": "`MvvmCross.Plugin.DownloadCache`",
    "3-0": "`MvvmCross.Plugin.Color`",
    "2-0": "`MvvmCross.Plugin.Bookmarks`",
    "1-0": "`MvvmCross.Plugin.Accelerometer`",
    "0-2": "[Nuget](https://www.nuget.org/packages/MvvmCross.Plugin.All/)",
    "1-2": "[Nuget](https://www.nuget.org/packages/MvvmCross.Plugin.Accelerometer/)",
    "2-2": "[Nuget](https://www.nuget.org/packages/MvvmCross.Plugin.Bookmarks/)",
    "3-2": "[Nuget](https://www.nuget.org/packages/MvvmCross.Plugin.Color/)",
    "4-2": "[Nuget](https://www.nuget.org/packages/MvvmCross.Plugin.DownloadCache/)",
    "5-2": "[Nuget](https://www.nuget.org/packages/MvvmCross.Plugin.Email/)",
    "6-2": "[Nuget](https://www.nuget.org/packages/MvvmCross.Plugin.FieldBinding/)",
    "7-2": "[Nuget](https://www.nuget.org/packages/MvvmCross.Plugin.File/)",
    "8-2": "[Nuget](https://www.nuget.org/packages/MvvmCross.Plugin.Json/)",
    "9-2": "[Nuget](https://www.nuget.org/packages/MvvmCross.Plugin.JsonLocalization/)",
    "10-2": "[Nuget](https://www.nuget.org/packages/MvvmCross.Plugin.Location/)",
    "11-2": "[Nuget](https://www.nuget.org/packages/MvvmCross.Plugin.Messenger/)",
    "12-2": "[Nuget](https://www.nuget.org/packages/MvvmCross.Plugin.MethodBinding/)",
    "13-2": "[Nuget](https://www.nuget.org/packages/MvvmCross.Plugin.Network/)",
    "14-2": "[Nuget](https://www.nuget.org/packages/MvvmCross.Plugin.PhoneCall/)",
    "15-2": "[Nuget](https://www.nuget.org/packages/MvvmCross.Plugin.PictureChooser/)",
    "16-2": "[Nuget](https://www.nuget.org/packages/MvvmCross.Plugin.ReflectionEx/)",
    "17-2": "[Nuget](https://www.nuget.org/packages/MvvmCross.Plugin.ResourceLoader/)",
    "18-2": "[Nuget](https://www.nuget.org/packages/MvvmCross.Plugin.Share/)",
    "19-2": "[Nuget](https://www.nuget.org/packages/MvvmCross.Plugin.SoundEffects/)",
    "20-2": "[Nuget](https://www.nuget.org/packages/MvvmCross.Plugin.SQLite/)",
    "21-2": "[Nuget](https://www.nuget.org/packages/MvvmCross.Plugin.SQLitePCL/)",
    "22-2": "[Nuget](https://www.nuget.org/packages/MvvmCross.Plugin.ThreadUtils/)",
    "23-2": "[Nuget](https://www.nuget.org/packages/MvvmCross.Plugin.Visibility/)",
    "24-2": "[Nuget](https://www.nuget.org/packages/MvvmCross.Plugin.WebBrowser/)"
  },
  "cols": 3,
  "rows": 25
}
[/block]