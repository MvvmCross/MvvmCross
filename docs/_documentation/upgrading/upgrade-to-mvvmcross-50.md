---
layout: documentation
title: Upgrade from 4 to MvvmCross 5
category: Upgrading
---

## NuGet package changes

Since MvvmCross 5.0 some packages have been changed or moved.

old NuGet package                      | new NuGet package
-------------------------------------- | -----------------
`MvvmCross.Droid.Support.V4`           | `MvvmCross.Droid.Support.Core.UI, MvvmCross.Droid.Support.Core.Utils, MvvmCross.Droid.Support.Fragment`
`MvvmCross.Droid.Support.V7.Fragging`  | `MvvmCross.Droid.Support.Fragment`
`MvvmCross.Forms.Presenters`           | `MvvmCross.Forms`

## Changes to test

The following things are recommended to test

* iOS Presenters and navigation
* Plugins you used that are removed now