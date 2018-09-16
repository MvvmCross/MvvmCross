---
layout: post
title: MvvmCross 6.2
date:   2018-4-14 14:00:00 -0300
categories: mvvmcross
---

# Announcing MvvmCross 6.2!

A new MvvmCross version is available on [NuGet](https://www.nuget.org/packages/MvvmCross/6.2.0)! You can always find the latest [changelog in the root of the repository](https://github.com/MvvmCross/MvvmCross/blob/develop/CHANGELOG.md) to see what has changed.

We are very happy to announce this release includes [101](https://github.com/MvvmCross/MvvmCross/milestone/42?closed=1) issues / pull requests. Here is a quick overview:

## About the MvvmCross project

### Signed NuGet packages

You might have already noticed, but we are pleased to announce that our NuGet packages are now signed! You can now verify their origin.

### Improving our community 

[@nickrandolph](https://github.com/nickrandolph) has started a great discussion about building a more active community of contributors. Please feel free to jump [into the conversation](https://github.com/MvvmCross/MvvmCross/issues/3034) and help us take MvvmCross to the next level!

We are currently working on making use of GitHub teams to delegate and distribute responsibilities. As part of this work a codeowners file has been [added](https://github.com/MvvmCross/MvvmCross/pull/3061) (huge thanks to [vatsalyagoel](https://github.com/vatsalyagoel)!).

### Issue templates

[@willsb](https://github.com/willsb) introduced different type of issue templates. When you try to submit a new issue, you will be prompted to choose one among these categories: regression, bug report, feature request, enhancement proposal and question/help.

### Speeding up build times while working in our source code

Thanks to [@nickrandolph](https://github.com/nickrandolph)'s work, you can now set a conditional flag to build a single platform. Please look at [this PR](https://github.com/MvvmCross/MvvmCross/pull/3015) to see the details.

## Core / Shared code

### Breaking change in `MvxViewModel`

Previously, `MvxViewModel` had properties for `IMvxNavigationService` and `IMvxLogProvider` which were resolved internally. After a long discussion we decided it to remove them and add a new ViewModel type: `MvxNavigationViewModel`, which has them as dependencies in the constructor. The interfaces `IMvxNavigationViewModel` and `IMvxLogViewModel` have been removed as well. Please look at [this PR](https://github.com/MvvmCross/MvvmCross/pull/3044) for the details.

### IMvxNavigationService methods now return `Task<bool>` and events like `BeforeNavigateEventHandler` changed a parameter type

- The boolean result indicates success / failure for the navigation action. 
- The parameter of type `NavigateEventArgs` is now `IMvxNavigateEventArgs`. You can now trigger cancellation using that parameter from within your event handler.

### Support for async Startup

[@nickrandolph](https://github.com/nickrandolph) made it possible to perform async operations during the `Initialize` method of the first shown ViewModel. Please just be aware that this could have a direct impact on the UX of your app (SplashScreen could take longer to dissapear). 

__Note:__ This change introduced a few breaking changes: Some methods now return `Task` instead of void.

### ViewPresenter methods are now async

Async operations in ViewPresenter have always been fire and forget. But thanks to [@nickrandolph](https://github.com/nickrandolph) this has changed and now all methods return `Task<bool>` and async operations are `awaited`. This change prevents some race conditions and weird issues that you might have had while using MvvmCross.

### Changes to `Mvx`

As part of a code improvement, `Mvx` methods have been marked as obsolete and will be removed probably for v7. You should update your app and replace calls like `Mvx.Resolve` for `Mvx.IoCProvider.Resolve`.

## Xamarin.Forms

- Compatibility with Xamarin.Forms 3.1 has been added. Please note that your app needs to be updated to that version as well.
- On Android, the method `MvxFormsAppCompatActivity.OnBackPressed` assumed the app was using the default `MvxFormsPagePresenter` and this could cause your app to crash in some cases. The problem has been fixed.
- Do you use the XAML Designer in your MvvmCross app? Yes? Then good news! [@cheesebaron](https://github.com/cheesebaron) [fixed](https://github.com/MvvmCross/MvvmCross/pull/3094) a problem which caused our controls not to be rendered correctly.
- If you tried to resolve `IMvxFormsPagePresenter` using the IoC you could see your app crash. Thanks to [vatsalyagoel](https://github.com/vatsalyagoel) this has been [fixed](https://github.com/MvvmCross/MvvmCross/pull/2972).
- If a binding had something else than a page as a root, your app could crash. Thanks to [@martijn00](https://github.com/martijn00) this has been [fixed](https://github.com/MvvmCross/MvvmCross/pull/3002).
- UWP apps were [crashing](https://github.com/MvvmCross/MvvmCross/pull/3023) when being ressumed. Thanks to [@nickrandolph](https://github.com/nickrandolph) this doesn't happen anymore.
- You can now reuse you ValueConverters within Xamarin.Forms XAML, following the same strategy that makes them usable in UWP (`MvxNativeValueConverter` has been added). If you want to see this in detail, please look at the awesome [PR](https://github.com/MvvmCross/MvvmCross/pull/3047) raised by [@MartinZikmund](https://github.com/MartinZikmund).
- Using `mvx:MvxLang` or `mvx:Bind` in a DataTrigger could cause your app to immediately crash. Thanks to [@cheesebaron](https://github.com/cheesebaron) this doesn't happen anymore.

## Android

- We have improved our support for [nested fragments](https://github.com/MvvmCross/MvvmCross/pull/3001) overall. ViewPager Fragments are now correctly managed when the ViewPager's host is a Fragment and not an Activity. Thanks to [@nmilcoff](https://github.com/nmilcoff) for working on that.
- `MvxAutoCompleteTextView` binding for PartialText was not working correctly. Thanks to [@cheesebaron](https://github.com/cheesebaron) this has been [fixed](https://github.com/MvvmCross/MvvmCross/pull/3027).
- We have fixed a memory leak related to our ViewModel caching mechanism. If you want to see the details please look at the [PR](https://github.com/MvvmCross/MvvmCross/pull/3055) raised by [@nmilcoff](https://github.com/nmilcoff).
- In certain scenarios, the first shown Activity was navigated to twice. Thanks to [@tbalcom](https://github.com/tbalcom) for detecting and fixing this problem!
- If you repeatedly pressed the back button while the SplashScreen was being shown in your app, you might have noticed that it crashed. Thanks to [@tbalcom](https://github.com/tbalcom) for applying a [fix](https://github.com/MvvmCross/MvvmCross/pull/3085).
- The default `DropDownItemTemplate` for `MvxAppCompatSpinner` now displays a string formed by `yourModel.ToString()` instead of nothing. This improvement has been done by [@tbalcom](https://github.com/tbalcom).
- You can now bind `Android.Support.V7.Preferences.Preference.PreferenceClick` to an `ICommand`. This addition was made by [@tbalcom](https://github.com/tbalcom).
- `MvxApplicationCallbacksCurrentTopActivity.Activities` is now protected, which makes it easier to customize. This improvement has been made by [@daividssilverio](https://github.com/daividssilverio).
- `MvxAppCompatViewPresenter` now supports `MvxPagePresentationHint`, which in other words means that you can now change the selected tab of a ViewPager by using `navigationService.ChangePresentation`. This implementation was introduced by [@markuspalme](https://github.com/markuspalme).

## iOS
 
- When binding the ItemsSource property of `MvxExpandableTableViewSource`, a `AmbiguousMatchException` was thrown. Thanks to [@cheesebaron](https://github.com/cheesebaron) this has been [fixed](https://github.com/MvvmCross/MvvmCross/pull/3024).
- We found out that our default LinkerPleaseInclude was missing some methods for `UITextField`. Please make sure to grab the latest version from [here](https://github.com/MvvmCross/MvvmCross/blob/master/ContentFiles/iOS/LinkerPleaseInclude.cs).
- You can now use extension methods in Fluent bindings to bind `Preference.PreferenceClick` and `UIBarButtonItem.Clicked`. Huge thanks to [@Plac3hold3r](https://github.com/Plac3hold3r).
- `MvxIosViewPresenter` now supports `MvxPagePresentationHint`, which in other words means that you can now change the selected tab by using `navigationService.ChangePresentation`. This implementation was introduced by [@markuspalme](https://github.com/markuspalme).
- `MvxTabBarViewController.SelectedViewController` now returns null instead of throwing a `NullReferenceException`. Kudos to [@andrechi1](https://github.com/andrechi1)!

## macOS

- `MvxTableViewSource.GetOrCreateViewFor` is now virtual, and you can provide your own View. This improvement has been made by [@cheesebaron](https://github.com/cheesebaron).

## WPF

- `MvxWpfViewPresenter` used to assume your app was using `MvxWindow`s instead of `IMvxWindow`s. This isn't the case anymore thanks to [@cheesebaron](https://github.com/cheesebaron).
- `ViewModel.ViewDestroy` is now called from `MvxWpfView.Unloaded`, which makes it much more reliable. Thanks to [@thongdoan](https://github.com/thongdoan) for this fix.

## Plugins

- `PictureChooser` was not working properly on iOS because of a crash when being injected. The creation of some components has been delayed so that this doesn't happen. Just make sure you make the plugin calls from the UI thread (iOS needs this!). Kudos to [@cheesebaron](https://github.com/cheesebaron)!
- Our `Network` plugin has received a big update! It now throws exceptions where it should, instead of making your app crash. Also the code is now more async/await friendly. Thanks to [@nmilcoff](https://github.com/nmilcoff) for working on this.

## Docs

- `MvxRecyclerView` is now properly documented thanks to [@cheesebaron](https://github.com/cheesebaron)'s work.
- `MvxSpinner` is now properly documented. Kudos to [@cheesebaron](https://github.com/cheesebaron)!
- Links to code were fixed by [@nmilcoff](https://github.com/nmilcoff). 

## Others

- The framework now supports raising `PropertyChanging` events. This is enabled by default. Thanks to [@nickrandolph](https://github.com/nickrandolph) for implementing [this](https://github.com/MvvmCross/MvvmCross/pull/2943).
- If you are using `Fody.PropertyChanged` please be sure you upgrade your app to the latest version of that package. [@borbmizzet](https://github.com/borbmizzet) has made [a PR](https://github.com/Fody/PropertyChanged/pull/346) fixing our compatibility with it.
- Exceptions raised in event handlers for `PropertyChanged` won't make your app crash anymore. Thanks to [@nickrandolph](https://github.com/nickrandolph) for this improvement.
- When cancelling a navigation to a ViewModel, the action wasn't always being stopped. This has been [fixed](https://github.com/MvvmCross/MvvmCross/pull/3006) by [@martijn00](https://github.com/martijn00).
- Exceptions raised during the initialization of Setup are no longer swallowed. This fix was done by [@nickrandolph](https://github.com/nickrandolph).
- When using Fluent bindings, you can now take advantage of the newly introduced method `ApplyWithClearBindingKey`. You can of course clear the entire binding set later on. This addition was done by [@Plac3hold3r](https://github.com/Plac3hold3r).


# Change Log

## [6.2.0](https://github.com/MvvmCross/MvvmCross/tree/6.2.0) (2018-09-13)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/6.2.0-beta4...6.2.0)

**Fixed bugs:**

- Cannot use any MainWindow type other than MvxWindow [\#3080](https://github.com/MvvmCross/MvvmCross/issues/3080)
- Playground.Droid crashes in nav stack [\#2931](https://github.com/MvvmCross/MvvmCross/issues/2931)
- Few of the examples compile on develop [\#2930](https://github.com/MvvmCross/MvvmCross/issues/2930)
- IMvxNavigationService.Navigate\<TViewModel, TParam, TResult\> deadlock if the back button is used [\#2924](https://github.com/MvvmCross/MvvmCross/issues/2924)
- Exceptions are swallowed during Android setup [\#2903](https://github.com/MvvmCross/MvvmCross/issues/2903)
- Memory leak on opening browser and returning back on droid [\#2884](https://github.com/MvvmCross/MvvmCross/issues/2884)
- Master Detail never cancel CloseCompletionSource [\#2833](https://github.com/MvvmCross/MvvmCross/issues/2833)
- MvxNavigationService.Navigate\(Type\) returns before completing [\#2827](https://github.com/MvvmCross/MvvmCross/issues/2827)
- RunAppStart isn't called in Xamarin Form - Android project [\#2813](https://github.com/MvvmCross/MvvmCross/issues/2813)
- Failed to resolve type MvvmCross.ViewModels.IMvxAppStart [\#2810](https://github.com/MvvmCross/MvvmCross/issues/2810)
- mvx:Lang and mvx:Bind crashes in Setter Value [\#3096](https://github.com/MvvmCross/MvvmCross/pull/3096) ([Cheesebaron](https://github.com/Cheesebaron))
- Fix bug preventing Playground.Droid from starting [\#3084](https://github.com/MvvmCross/MvvmCross/pull/3084) ([tbalcom](https://github.com/tbalcom))
-  Move ViewModel?.ViewDestroy\(\) to MvxWpfView\_Unloaded \(MvxWpfView.cs\) [\#3078](https://github.com/MvvmCross/MvvmCross/pull/3078) ([thongdoan](https://github.com/thongdoan))
- Give some love to our Network plugin [\#3056](https://github.com/MvvmCross/MvvmCross/pull/3056) ([nmilcoff](https://github.com/nmilcoff))
- Fix memory leaks on IMvxMultipleViewModelCache [\#3055](https://github.com/MvvmCross/MvvmCross/pull/3055) ([nmilcoff](https://github.com/nmilcoff))
- Repair NullReferenceException with SelectedViewController is null. [\#3054](https://github.com/MvvmCross/MvvmCross/pull/3054) ([andrechi1](https://github.com/andrechi1))
- Delay creation of UIImagePickerController [\#3038](https://github.com/MvvmCross/MvvmCross/pull/3038) ([Cheesebaron](https://github.com/Cheesebaron))
- Fix crash when switching back to the app after Permission change [\#3032](https://github.com/MvvmCross/MvvmCross/pull/3032) ([vatsalyagoel](https://github.com/vatsalyagoel))
- Android: Add support for ViewPagers inside Fragments [\#3001](https://github.com/MvvmCross/MvvmCross/pull/3001) ([nmilcoff](https://github.com/nmilcoff))

**Closed issues:**

- Make MvxApplicationCallbacksCurrentTopActivity.cs:\_Activities protected to facilitate extension [\#3048](https://github.com/MvvmCross/MvvmCross/issues/3048)
- Build error in VS on Windows: The target "GetBuiltProjectOutputRecursive" does not exist in the project. [\#3043](https://github.com/MvvmCross/MvvmCross/issues/3043)
- MvxIoCResolveException Exception when back button clicked [\#2984](https://github.com/MvvmCross/MvvmCross/issues/2984)
- Custom Presentation Hint Handler is still being ignored [\#2950](https://github.com/MvvmCross/MvvmCross/issues/2950)
- What should come after The Core Project in the TipCalc tutorial? It seems wrong. [\#2920](https://github.com/MvvmCross/MvvmCross/issues/2920)
- Address "RequestMainThreadAction is obsolete" build warnings [\#2859](https://github.com/MvvmCross/MvvmCross/issues/2859)
- Converters for Xamarin.Forms [\#2847](https://github.com/MvvmCross/MvvmCross/issues/2847)
- Update documentation based on new namespaces [\#2621](https://github.com/MvvmCross/MvvmCross/issues/2621)

**Merged pull requests:**

- Attempt to fix failing navigation service test [\#3100](https://github.com/MvvmCross/MvvmCross/pull/3100) ([Cheesebaron](https://github.com/Cheesebaron))
- Playground.Droid: Remove incorrect button on SplitDetailNavView [\#3097](https://github.com/MvvmCross/MvvmCross/pull/3097) ([nmilcoff](https://github.com/nmilcoff))
- Fixed links to code and documentation [\#3088](https://github.com/MvvmCross/MvvmCross/pull/3088) ([markuspalme](https://github.com/markuspalme))
- Android support for the MvxPagePresentationHint. [\#3086](https://github.com/MvvmCross/MvvmCross/pull/3086) ([markuspalme](https://github.com/markuspalme))
- Fix TipCalc Core navigation link [\#3082](https://github.com/MvvmCross/MvvmCross/pull/3082) ([nmilcoff](https://github.com/nmilcoff))
- Check for IMvxWindow instead of MvxWindow on WPF [\#3081](https://github.com/MvvmCross/MvvmCross/pull/3081) ([Cheesebaron](https://github.com/Cheesebaron))
- Update comments in MvxWpfLocationWatcher [\#3079](https://github.com/MvvmCross/MvvmCross/pull/3079) ([fredeil](https://github.com/fredeil))
- Add ApplyWithClearBindingKey to MvxFluentBindingDescriptionSet [\#3073](https://github.com/MvvmCross/MvvmCross/pull/3073) ([Plac3hold3r](https://github.com/Plac3hold3r))
- Support for MvxPagePresentationHint in MvxIosViewPresenter \(\#2518\). [\#3071](https://github.com/MvvmCross/MvvmCross/pull/3071) ([markuspalme](https://github.com/markuspalme))
- Improving documentation of the usage scenario of Presentation Attribute Overriding for iOS and XF [\#3062](https://github.com/MvvmCross/MvvmCross/pull/3062) ([agat366](https://github.com/agat366))
- Documentation/android spinner [\#3060](https://github.com/MvvmCross/MvvmCross/pull/3060) ([Cheesebaron](https://github.com/Cheesebaron))
- Using ExceptionDispatchInfo to capture exception [\#3058](https://github.com/MvvmCross/MvvmCross/pull/3058) ([nickrandolph](https://github.com/nickrandolph))
- Move VisibleUIViewController to MvxTabBarViewController [\#3057](https://github.com/MvvmCross/MvvmCross/pull/3057) ([andrechi1](https://github.com/andrechi1))
- should in lower case [\#3053](https://github.com/MvvmCross/MvvmCross/pull/3053) ([JTOne123](https://github.com/JTOne123))
- was updated my template version [\#3052](https://github.com/MvvmCross/MvvmCross/pull/3052) ([JTOne123](https://github.com/JTOne123))
- Propagating exceptions out of setup [\#3051](https://github.com/MvvmCross/MvvmCross/pull/3051) ([nickrandolph](https://github.com/nickrandolph))
- Make activities dictionary protected to ease extension [\#3049](https://github.com/MvvmCross/MvvmCross/pull/3049) ([daividssilverio](https://github.com/daividssilverio))
- MvxNativeValueConverter and MvxFormsValueConverter for MvvmCross.Forms [\#3047](https://github.com/MvvmCross/MvvmCross/pull/3047) ([MartinZikmund](https://github.com/MartinZikmund))
- Adding cancel support through navigation [\#3046](https://github.com/MvvmCross/MvvmCross/pull/3046) ([nickrandolph](https://github.com/nickrandolph))
- documentation: MvxRecyclerView [\#3045](https://github.com/MvvmCross/MvvmCross/pull/3045) ([Cheesebaron](https://github.com/Cheesebaron))
- Adding MvxNavigationViewModel to remove injected NavigationService and LogProvider [\#3044](https://github.com/MvvmCross/MvvmCross/pull/3044) ([nickrandolph](https://github.com/nickrandolph))
- Catch exceptions in ProcessMvxIntentResult and log [\#3039](https://github.com/MvvmCross/MvvmCross/pull/3039) ([Cheesebaron](https://github.com/Cheesebaron))
- Fixes \#3028. [\#3036](https://github.com/MvvmCross/MvvmCross/pull/3036) ([tbalcom](https://github.com/tbalcom))
- Fix MvxAutoCompleteTextView not returning results [\#3027](https://github.com/MvvmCross/MvvmCross/pull/3027) ([Cheesebaron](https://github.com/Cheesebaron))
- Fix docs style link on pull request template [\#3026](https://github.com/MvvmCross/MvvmCross/pull/3026) ([borbmizzet](https://github.com/borbmizzet))
- \#3000 Fix ambiguous reference when binding MvxExpandableTableViewSource [\#3024](https://github.com/MvvmCross/MvvmCross/pull/3024) ([Cheesebaron](https://github.com/Cheesebaron))
- Fixing Forms reload issue [\#3023](https://github.com/MvvmCross/MvvmCross/pull/3023) ([nickrandolph](https://github.com/nickrandolph))
- Update missing Fluent binding extensions method and doc entries [\#3020](https://github.com/MvvmCross/MvvmCross/pull/3020) ([Plac3hold3r](https://github.com/Plac3hold3r))
- Adding conditional flag to allow switching between different TFMs [\#3015](https://github.com/MvvmCross/MvvmCross/pull/3015) ([nickrandolph](https://github.com/nickrandolph))
- Adds Android.Support.V7.Preferences.Preference.PreferenceClick target binding [\#3009](https://github.com/MvvmCross/MvvmCross/pull/3009) ([tbalcom](https://github.com/tbalcom))
- Update links in docs [\#3007](https://github.com/MvvmCross/MvvmCross/pull/3007) ([nmilcoff](https://github.com/nmilcoff))
- Make sure navigation is cancel when requested [\#3006](https://github.com/MvvmCross/MvvmCross/pull/3006) ([martijn00](https://github.com/martijn00))
- Update nuget packages [\#3005](https://github.com/MvvmCross/MvvmCross/pull/3005) ([martijn00](https://github.com/martijn00))
- Unit Test Update: Async Dispatcher [\#3003](https://github.com/MvvmCross/MvvmCross/pull/3003) ([johnnywebb](https://github.com/johnnywebb))
- Don't throw when there is no root [\#3002](https://github.com/MvvmCross/MvvmCross/pull/3002) ([martijn00](https://github.com/martijn00))
- Tidying up more obsolete method calls and adding await as appropriate [\#2998](https://github.com/MvvmCross/MvvmCross/pull/2998) ([nickrandolph](https://github.com/nickrandolph))
- Tidying up dispatcher code [\#2997](https://github.com/MvvmCross/MvvmCross/pull/2997) ([nickrandolph](https://github.com/nickrandolph))
- Tidying up references and removing build warnings [\#2994](https://github.com/MvvmCross/MvvmCross/pull/2994) ([nickrandolph](https://github.com/nickrandolph))
- Removing unnecessary sdk library references [\#2992](https://github.com/MvvmCross/MvvmCross/pull/2992) ([nickrandolph](https://github.com/nickrandolph))
- Bugfix/issue templates [\#2989](https://github.com/MvvmCross/MvvmCross/pull/2989) ([Cheesebaron](https://github.com/Cheesebaron))
- Make GetOrCreateViewFor virtual [\#2986](https://github.com/MvvmCross/MvvmCross/pull/2986) ([Cheesebaron](https://github.com/Cheesebaron))
- Update data-binding.md: fixed typo [\#2982](https://github.com/MvvmCross/MvvmCross/pull/2982) ([AndreKraemer](https://github.com/AndreKraemer))
- Upgrading UWP target platform version and support library version [\#2978](https://github.com/MvvmCross/MvvmCross/pull/2978) ([nickrandolph](https://github.com/nickrandolph))
- Version bump to Xamarin.Forms v3.1 [\#2976](https://github.com/MvvmCross/MvvmCross/pull/2976) ([nickrandolph](https://github.com/nickrandolph))
- Default MvxAppCompatSpinner DropDownItemTemplate doesn't display strings or use ToString on models [\#2975](https://github.com/MvvmCross/MvvmCross/pull/2975) ([tbalcom](https://github.com/tbalcom))
- App crashes on resolving IMvxFormsPagePresenter [\#2972](https://github.com/MvvmCross/MvvmCross/pull/2972) ([vatsalyagoel](https://github.com/vatsalyagoel))
- Move Mvx class into IoC [\#2964](https://github.com/MvvmCross/MvvmCross/pull/2964) ([martijn00](https://github.com/martijn00))
- Adding SignClient, updating unit test runner + security fixes [\#2949](https://github.com/MvvmCross/MvvmCross/pull/2949) ([Cheesebaron](https://github.com/Cheesebaron))
- Implementing INotifyPropertyChanging [\#2943](https://github.com/MvvmCross/MvvmCross/pull/2943) ([nickrandolph](https://github.com/nickrandolph))
- Improve issue templates [\#2940](https://github.com/MvvmCross/MvvmCross/pull/2940) ([willsb](https://github.com/willsb))
- Making IMvxViewPresenter methods async [\#2868](https://github.com/MvvmCross/MvvmCross/pull/2868) ([nickrandolph](https://github.com/nickrandolph))
- Add support for async startup [\#2866](https://github.com/MvvmCross/MvvmCross/pull/2866) ([nickrandolph](https://github.com/nickrandolph))

## [6.2.0-beta4](https://github.com/MvvmCross/MvvmCross/tree/6.2.0-beta4) (2018-09-13)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/6.2.0-beta3...6.2.0-beta4)

**Fixed bugs:**

- \[6.2.0-beta2\] FrameLayout to show Fragment not found [\#3059](https://github.com/MvvmCross/MvvmCross/issues/3059)
- Splash Screen Crashes on Android when Hard Back or Hard Home button hit  [\#3017](https://github.com/MvvmCross/MvvmCross/issues/3017)
- MvxTaskBasedBindingContext creates timing issues with autosizing cells [\#2898](https://github.com/MvvmCross/MvvmCross/issues/2898)

**Closed issues:**

- Playground.Droid can't navigate to RootViewModel [\#3083](https://github.com/MvvmCross/MvvmCross/issues/3083)
- Fix comments in MvxLocationWatcher WPF [\#2911](https://github.com/MvvmCross/MvvmCross/issues/2911)

**Merged pull requests:**

- Fix forms xaml preview on android [\#3094](https://github.com/MvvmCross/MvvmCross/pull/3094) ([Cheesebaron](https://github.com/Cheesebaron))
- Update mvvmcross-overview.md [\#3087](https://github.com/MvvmCross/MvvmCross/pull/3087) ([yehorhromadskyi](https://github.com/yehorhromadskyi))
- Fix issues when pressing back button on splash screen [\#3085](https://github.com/MvvmCross/MvvmCross/pull/3085) ([tbalcom](https://github.com/tbalcom))
- It seems like the code owners file is case sensitive [\#3076](https://github.com/MvvmCross/MvvmCross/pull/3076) ([vatsalyagoel](https://github.com/vatsalyagoel))
- Add Codeowners [\#3061](https://github.com/MvvmCross/MvvmCross/pull/3061) ([vatsalyagoel](https://github.com/vatsalyagoel))

## [6.2.0-beta3](https://github.com/MvvmCross/MvvmCross/tree/6.2.0-beta3) (2018-08-17)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/6.2.0-beta2...6.2.0-beta3)

**Fixed bugs:**

- \[Android\] 6.2.0-beta2 Playground.Droid shows RootViewModel twice [\#3028](https://github.com/MvvmCross/MvvmCross/issues/3028)
- MvxAutoCompleteTextView PartialText never changes after initial setting [\#3008](https://github.com/MvvmCross/MvvmCross/issues/3008)
- MvxExpandableTableViewSource issue [\#3000](https://github.com/MvvmCross/MvvmCross/issues/3000)
- PictureChooser can't be injected in ViewModel [\#2886](https://github.com/MvvmCross/MvvmCross/issues/2886)
- HTML email body doesn't show properly in Android [\#2572](https://github.com/MvvmCross/MvvmCross/issues/2572)

**Closed issues:**

- MvxTabbedPagePresentation with WrapInNavigationPage = false crashes when closing a modal view [\#3050](https://github.com/MvvmCross/MvvmCross/issues/3050)
- Google Sign in IdToken null with MvxAppCompatActivity [\#3040](https://github.com/MvvmCross/MvvmCross/issues/3040)
- Error building MvvmCross with Cake build script [\#3022](https://github.com/MvvmCross/MvvmCross/issues/3022)
- Commit d2a7fb2d on June 15 breaks compatibility with PropertyChanged.Fody [\#3016](https://github.com/MvvmCross/MvvmCross/issues/3016)
- `\[MvxContentPagePresentation\(WrapInNavigationPage = true, NoHistory = true\)\] causes crash when NavigationPage.HasNavigationBar="True" [\#2812](https://github.com/MvvmCross/MvvmCross/issues/2812)

**Merged pull requests:**

- Fix failing MvxIocPropertyInjectionTest [\#3069](https://github.com/MvvmCross/MvvmCross/pull/3069) ([Cheesebaron](https://github.com/Cheesebaron))

## [6.2.0-beta2](https://github.com/MvvmCross/MvvmCross/tree/6.2.0-beta2) (2018-07-12)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/6.2.0-beta1...6.2.0-beta2)

**Closed issues:**

- Fragments crash on rotate, splitview \(2 fragments\) for landscape, one fragment for landscape. [\#3004](https://github.com/MvvmCross/MvvmCross/issues/3004)
- Getting Exception System.ArgumentNullException. [\#2952](https://github.com/MvvmCross/MvvmCross/issues/2952)

## [6.2.0-beta1](https://github.com/MvvmCross/MvvmCross/tree/6.2.0-beta1) (2018-07-10)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/6.1.2...6.2.0-beta1)

**Fixed bugs:**

- TabLayout inside a nested layout adds the tabs' fragments to the host FragmentManager  [\#2550](https://github.com/MvvmCross/MvvmCross/issues/2550)

**Closed issues:**

- Exception When  [\#2987](https://github.com/MvvmCross/MvvmCross/issues/2987)
- MvxObservableCollection AddRange [\#2983](https://github.com/MvvmCross/MvvmCross/issues/2983)
- Mvvmcross v6.1.2 Android - recyclerview custom adapter throwing error on scroll [\#2981](https://github.com/MvvmCross/MvvmCross/issues/2981)
- Question: Place to Init nuget packages [\#2980](https://github.com/MvvmCross/MvvmCross/issues/2980)
- Xamarin Forms: Android white screen [\#2974](https://github.com/MvvmCross/MvvmCross/issues/2974)
- MvvmCross 6.1.2: Xamarin Forms Instalation  [\#2973](https://github.com/MvvmCross/MvvmCross/issues/2973)
- Android: Registering MvxLanguageConverter for Resx localization broke [\#2967](https://github.com/MvvmCross/MvvmCross/issues/2967)
- MvxFormsAppCompatActivity.OnBackPressed assumes the standard Forms page presenter is being used [\#2965](https://github.com/MvvmCross/MvvmCross/issues/2965)
- Binding stop working after upgrade from 6.0.1 to 6.1.1 on Xamarin.Forms [\#2960](https://github.com/MvvmCross/MvvmCross/issues/2960)
- Working with Xamarin.ios using MvvmCross Framework, getting System.ArgumentNullException. [\#2954](https://github.com/MvvmCross/MvvmCross/issues/2954)
- Mvvmcross Android - getting error for custom adapter listview in alertdialog using dialogfragment [\#2846](https://github.com/MvvmCross/MvvmCross/issues/2846)