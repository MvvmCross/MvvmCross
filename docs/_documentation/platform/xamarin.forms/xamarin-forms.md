---
layout: documentation
title: MvvmCross for Xamarin.Forms
category: Platforms
---

MvvmCross tries to align Forms as much as possible with the native Xamarin features. You'll be able to use almost any of them, but some implementations have Forms specific implementations.

# Startup

You need to inherit the Forms variant of the startup point for the platform. For example: `MvxFormsAppdelegate`.

# Setup

Use the Forms specific Setup for each platform, and we will load up the neccesary bases. You can do this by extending from example `MvxFormsAndroidSetup`.

# Presenters

On Forms every platform has it's own presenter that inherits from the native platform presenter. This enables us to navigate between native and Xamarin.Forms views. On top of that we have the `MvxFormsPagePresenter` which handles all the comon logic for Forms related navigation.

For more information about the Forms presenter see: [Xamarin.Forms view presenter](xamarin-forms-view-presenter.html)

# Bindings

There are some specific Forms bindings but in general you can use any binding that you would in Xamarin native.

# Views

Every Forms view has a `Mvx` version that handles setting up MvvmCross things like ViewModels. To use this simply add `Mvx` in front of a type. For example: `MvxContentPage`
