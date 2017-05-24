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
`MvvmCross.Forms.Presenter`            | `MvvmCross.Forms`

## Core

To make sure your navigation stays up-to-date change all your `ShowViewModel<>()` calls to the new navigation explained in the [documentation](https://www.mvvmcross.com/documentation/fundamentals/navigation#mvvmcross-5x-and-higher-navigation)

Example before:

```c#
private IMvxCommand _navigateCommand;
public IMvxCommand NavigateCommand
{
    get
    {
        _navigateCommand = _navigateCommand ?? new MvxCommand(() => ShowViewModel<TViewModel>());
        return _navigateCommand;
    }
}
```

After:

```c#
private IMvxAsyncCommand _navigateCommand;
public IMvxAsyncCommand NavigateCommand
{
    get
    {
        _navigateCommand = _navigateCommand ?? new MvxAsyncCommand(() => _navigationService.Navigate<TViewModel>());
        return _navigateCommand;
    }
}
```

Also your `public void Init()` won't be called anymore. This is because this was done using reflection. With the new navigation a method called `public override async Task Initialize()` will be called. This method is typed and async.

## iOS

### iOS View Presenter and Tab bar control

With version 5 of MvvmCross the iOS View Presenter received a major overhaul. This results in a few changes especially when using the tab bar control.

* The `IMvxTabBarPresenter` (and its implementation `MvxTabsViewPresenter`) have been removed and are now fully integrated with the `MvxIosViewPresenter` class. When you are using the tab bar control, please replace the `MvxTabsViewPresenter` with the `MvxIosViewPresenter`;
* Like in the previous versions you would still decorate your view controllers with the `MvxTabPresentationAttribute` attribute, however it has now moved to a new namespace. Namely from `MvvmCross.iOS.Support.Presenters` to `MvvmCross.iOS.Views.Presenters.Attributes`.
* The `MvxTabPresentationAttribute` no longer accepts the `MvxTabPresentationMode` enum as a parameter. These options have now been replaced by their own attributes. See following table (all attributes are located in the `MvvmCross.iOS.Views.Presenters.Attributes` namespace):

MxvTabPresentationMode | new Attribute
---------------------- | -------------
Root                   | `MvxRootPresentationAttribute`
Tab                    | `MvxTabPresentationAttribute`
Child                  | `MvxChildPresentationAttribute`
Modal                  | `MvxModalPresentationAttribute`

Detailed information regarding the new iOS View Presenter and the above attributes can be found in the [iOS View Presenter]({{"/documentation/platform/ios-view-presenter" | relative_url}}) section of the documentation.

## Xamarin.Forms

Update your code to use the new base classes, support for MvvmCross bindings and improved presenters. Information about this can be found in: [Xamarin.Forms](https://www.mvvmcross.com/documentation/platform/xamarin-forms?scroll=551)

> `MvxFormsApp` has changed to `MvxFormsApplication`

## Changes to test

The following things are recommended to test

* iOS Presenters and navigation
* Plugins you used that are removed now
