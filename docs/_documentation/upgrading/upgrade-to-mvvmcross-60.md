---
layout: documentation
title: Upgrade from 5 to MvvmCross 6
category: Upgrading
Order: 3
---

This guide assumes you are using MvvmCross 5.6.x. If you are updating your app from a previous version, please look at the appropiate blog posts.

The very first thing you will notice when upgrading / installing v6 nugets, is that a readme file will open automatically. 
Please read the readme instructions carefully, and don't hesitate to jump in and make a PR to improve it if you think anything is missing!

## .NET Standard

MvvmCross 6 requires your app to use .NET Standard 2.0 as its base library now. Please make sure you make that upgrade before proceeding!

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
- `MvvmCross.Forms.{Platform}` is now `MvvmCross.Forms.Platforms.{Platform}`.

#### Core and all platforms
- `MvvmCross.Platform` namespace is now `MvvmCross`.
- Most framework core classes are now under the namespace `MvvmCross.Base`.
- `MvvmCross.Platform.{Platform}.Platform` is now `MvvmCross.Platforms.{Platform}.Base`.
- `MvvmCross.Binding.{Platform}` namespace is now `MvvmCross.Platforms.{Platform}.Binding`.
- `MvvmCross.Core.Platform.Converters`namespace is now `MvvmCross.Converters`
- `MvvmCross.Core.Platform` namespace is now `MvvmCross.Core`.
- Platform initialization code (Setup classes, ..) was moved from `MvvmCross.{Platform}.Platform` to the namespace `MvvmCross.Platforms.{Platform}.Core`.
- `MvvmCross.Platform.{Platform}.Views` is now under the namespace namespace `MvvmCross.Platforms.{Platform}.Views.Base`.
- `MvvmCross.Platform.UI` namespace is now `MvvmCross.UI`.
- `MvvmCross.Platform.Exceptions` namespace is now `MvvmCross.Exceptions`
- All PresentationHints are now at `MvvmCross.Presenters.Hints`.
- `MvvmCross.Core.ViewModels` namespace is now `MvvmCross.ViewModels`.
- `MvvmCross.{Platform}.Platform` namespace is now `MvvmCross.Platforms.{Platform}.Core`.
- `MvvmCross.{Platform}.ViewModels` namespace is now `MvvmCross.Platforms.{Platform}.ViewModels`.
- `MvvmCross.{Platform}.Views` namespace is now `MvvmCross.Platforms.{Platform}.Views`.
- `MvvmCross.Core.Views` namespace is now `MvvmCross.Views`.
- `MvvmCross.Platform.ExtensionMethods.MvxCrossCoreExtensions` class is now `MvvmCross.Base.MvxCoreExtensions`.
- `MvvmCross.Platform.WeakSubscription` namespace is now `MvvmCross.WeakSubscription`.
- `MvvmCross.Test.Core` namespace is now `MvvmCross.Tests`.
- `JetBrains.Annotations` namespace is now `MvvmCross.Annotations`.

#### IoC
- All IoC related code was moved from `MvvmCross.Platform.IoC` to `MvvmCross.IoC`.
- `MvxSimpleIoCContainer` is now `MvxIoCContainer`.

#### Logging
`MvxTrace` and everything related was removed in v6. Please take a look at the [official documentation](https://www.mvvmcross.com/documentation/fundamentals/logging) to learn about the new logging system.

- `MvvmCross.Platform.Logging` is now `MvvmCross.Logging`.
- `MvvmCross.Core.Platform.LogProviders` namespace is now `MvvmCross.Platforms.Logging.LogProviders`.

#### Navigation
- `MvvmCross.Core.Navigation` is now `MvvmCross.Navigation`.
- `ShowViewModel` and `MvxNavigatingObject` were removed. `MvxNavigationService` should be used instead. Please take a look at the [official documentation](https://www.mvvmcross.com/documentation/fundamentals/navigation) to learn about it.
- `MvxNavigationServiceAppStart` was removed and `MvxAppStart` uses the navigation service internally.

#### Binding
- `MvvmCross.Binding.{Platform}` is now available at `MvvmCross.Platforms.{Platform}.Binding`.

#### Commands
All Commands related code was moved from `MvvmCross.Core.ViewModels` to `MvvmCross.Commands`.

#### ViewPresenters
ViewPresenters are now under their own folder. Therefore we had to modify namespaces: 
- Presenters code under `MvvmCross.{Platform}.Views` is now at `MvvmCross.Platforms.{Platform}.Presenters`
- `MvvmCross.{Platform}.Views.Attributes` to `MvvmCross.Platforms.{Platform}.Presenters.Attributes` 

#### Plugins
All plugin namespaces have changed, but you shouldn't worry about it unless you are diving deep into their implementations. 
- The `Plugins` keyword is now `Plugin`.
- `MvvmCross.Plugins.{PluginName}.{Platform}` is now `MvvmCross.Plugin.{PluginName}.Platforms.{Platform}`
- `MvvmCross.Platform.Plugins` namespace is now `MvvmCross.Plugin`.

## Breaking changes

### Setup
As part of the Setup improvements, we have removed all parameters from constructors and moved them to a new method: `PlatformInitialize`.
The SetupSingleton that existed previously only on Android has been extended and it now exists for all platforms.

### AppStart
- It is no longer necessary that you call AppStart by yourself. All visual initialization code can now be done by MvvmCross automatically. If you need to provide a hint to it (if you are for example using push notifications), then you just need to override the method `GetAppStartHint` on the class where you used to call the code to run the AppStart.
In case you need further control over what happens, you can override `RunAppStart`.
- `IMvxAppStart` has a new method: `ResetStart()` and a new property: `bool IsStarted { get; }`. If you are using a custom AppStart, then it is recommended that you make it inherit from the brand new base class `MvxAppStart`, which implements everything by default.
- The method `Start` is now reserved and managed by the framework. If you are using a custom AppStart you should use the protected method `Startup` from now on.

### Plugins

#### Color
- On Android, the method `ToAndroidColor` was renamed to `ToNativeColor` in order to match all other platforms.

#### DownloadCache
`DownloadCache` was removed in v6.0, as well as `MvxImageView` and all the related code. Reason being is that there were multiple ancient issues around it and its implementation wasn't as clean as we would have liked. There are also multiple more efficient alternatives, like [FFImageLoading](https://github.com/luberda-molinet/FFImageLoading/wiki/MvvmCross).

### ViewPresenters 
- `IMvxOverridePresentationAttribute.PresentationAttribute` now takes a `MvxViewModelRequest` as parameter. 
If you're using a custom ViewPresenter that extends the default provided by MvvmCross, be aware that `GetPresentationAttribute` and `GetOverridePresentationAttribute` methods signatures have changed.
- `MvxFormsPagePresenter` constructor now takes a single parameter. You will be affected by this change only if you are using Xamarin Forms and you are providing a custom pages presenter for a platform ViewPresenter.
- On iOS, macOS and tvOS the Setup method `CreatePresenter` was renamed to `CreateViewPresenter`.
- On iOS and tvOS, the method `CleanupModalViewControllers` was renamed to `CloseModalViewControllers`.
- The method `NativeModalViewControllerDisappearedOnItsOwn` is now called `CloseTopModalViewController`.
- `IMvxTvosModalHost` and `IMvxIosModalHost` were deprecated and removed.
- On iOS, the method `PresentModalViewController` was renamed to `ShowModalViewController`.

### Navigation
- `IMvxNavigationService.ChangePresentation` is now an async method (the method now returns a `Task<bool>`). It was the only "sync" method on IMvxNavigationService, which was odd.
- Two methods were added to the `IMvxNavigationService` interface: `Task<bool> CanNavigate<TViewModel>` and `Task<bool> CanNavigate(Type viewModelType)`.

### IMvxCommand
- The method `RaiseCanExecuteChanged` was added to the `IMvxCommand` interface.

### Logging
`IMvxLog` has a new method: `bool IsLogLevelEnabled(MvxLogLevel logLevel)`.

### Android
- `IMvxAndroidSplashScreenActivity` was removed. It was replaced by `IMvxSetupMonitor`, which is located on the `MvvmCross.Core` namespace.
- On both SplashScreen activities (traditional and AppCompat), the method `TriggerFirstNavigate` was removed. `RunAppStart` is it's replacement. But in case you need to pass a hint to your custom AppStart, the method you should override is `GetAppStartHint`, which will receive the incoming `Bundle` as a parameter.
- `MvxRelativeLayout`, `MvxFrameLayout` and `MvxTableLayout` were removed as they were memory inefficient (nothing we can do to improve that).
- If you were declaring any view on your .axml files which prefix is "Mvx..." using the entire namespace, then you might see your app breaking. This is because namespaces have changed for many views. Just remove the namespace and leave the widget name.
- `IMvxRecyclerViewHolder` now has a new property: `int Id { get; set; }`, which contains the LayoutId being used.
- `MvxWakefulBroadcastReceiver` was removed, as it was deprecated by the platform. 
- The interface `IMvxTemplateSelector` has a new property: `int ItemTemplateId { get; set; }` which will be filled with the default LayoutId or the one you set by .axml using the item template attr.
- On `MvxAndroidViewPresenter`, `ShowIntent` has a new parameter.
- On `MvxAppCompatViewPresenter`, `CreateActivityTransitionOptions` changed its return type.
- As part of the shared elements evolution, `MvxActivityPresentationAttribute` and `MvxFragmentPresentationAttribute` have lost some properties - they were deprecated.
- On both presenters, callback methods like `OnBeforeFragmentChanging` and `OnFragmentChanged` now forward the `MvxViewVodelRequest` object as a parameter.

### WPF
- `MvxBaseWpfViewPresenter` and `MvxSimpleWpfViewPresenter` were removed. It is highly recommended that you migrate to `MvxWpfViewPresenter`.

### iOS Support package
The package iOS-Support has been removed in v6. But this doesn't mean we have deleted it! Most reusable bits are now part of the main lib, and the sidebar support is now a plugin that you can install on your iOS project.

### Others
Some methods and classes that were marked as [Obsolete] previously, have been finally removed on MvvmCross 6. This includes:
- `MvxBaseFluentBindingDescription.Overwrite(...)`
- `MvxFluentBindingDescription.Described(...)`
- `IMvxConsumer` (IoC)
- `IMvxProducer` (IoC)
- `MvxIoCExtensions` (all methods)
- `MvxEventSourceTabActivity` (Android)
- `MvxTabActivity` (Android)
- Some constructos for `MvxCollectionViewCell` (iOS)
