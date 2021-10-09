---
layout: post
title: MvvmCross 7.0.0
date:   2020-08-14 20:00:00 +0200
categories: mvvmcross
---

A new MvvmCross version is available on [NuGet](https://www.nuget.org/packages/MvvmCross/7.0.0)! You can always find the latest [changelog in the root of the repository](https://github.com/MvvmCross/MvvmCross/blob/develop/CHANGELOG.md) to see what has changed between versions.

This major release is due to switching out a lot of the inner workings on the Android platform with AndroidX packages. We have removed the support for using Fragments that come with Android and all types are now using Activities and Fragments from AndroidX. This means, that we have breaking changes.

## Package changes

Some packages are deprecated and have been moved into the main MvvmCross package. See the overview in the table below.

| Old Package                                             | New Package                             |
|---------------------------------------------------------|-----------------------------------------|
| MvvmCross.Android.Support.Core.UI                       | MvvmCross.DroidX.SwipeRefreshLayout     |
| MvvmCross.Android.Support.Design                        | MvvmCross.DroidX.Material               |
| MvvmCross.Android.Support.Fragment                      | MvvmCross                               |
| MvvmCross.Android.Support.V17.Leanback                  | MvvmCross.DroidX.Leanback               |
| MvvmCross.Android.Support.V7.AppCompat                  | MvvmCross                               |
| MvvmCross.Android.Support.V7.Preference                 | MvvmCross                               |
| MvvmCross.Android.Support.V7.RecyclerView               | MvvmCross.DroidX.RecyclerView           |

## Type and namespace changes

| Old type                                                                     | New type                                                                   |
|------------------------------------------------------------------------------|----------------------------------------------------------------------------|
| MvvmCross.Droid.Support.V4.MvxFragment                                       | MvvmCross.Platforms.Android.Views.Fragments.MvxFragment                    |
| MvvmCross.Droid.Support.V4.MvxDialogFragment                                 | MvvmCross.Platforms.Android.Views.Fragments.MvxDialogFragment              |
| MvvmCross.Droid.Support.V4.MvxSwipeRefreshLayout                             | MvvmCross.DroidX.MvxSwipeRefreshLayout                                     |
| MvvmCross.Droid.Support.V7.RecyclerView                                      | MvvmCross.DroidX.RecyclerView                                              |
| MvvmCross.Droid.Support.V7.RecyclerViewAdapter                               | MvvmCross.DroidX.RecyclerViewAdapter                                       |
| MvvmCross.Droid.Support.V7.RecyclerViewHolder                                | MvvmCross.DroidX.RecyclerViewHolder                                        |
| MvvmCross.Droid.Support.V7.AppCompat.MvxAppCompatActivity                    | MvvmCross.Platforms.Android.Views.MvxActivity                              |
| MvvmCross.Droid.Support.V7.AppCompat.MvxAppCompatDialogFragment              | MvvmCross.Platforms.Android.Views.Fragments.MvxDialogFragment              |
| MvvmCross.Droid.Support.V7.AppCompat.MvxSplashScreenAppCompatActivity        | MvvmCross.Platforms.Android.Views.MvxSplashScreenActivity                  |
| MvvmCross.Droid.Support.V7.AppCompat.MvxAppCompatSetup                       | MvvmCross.Platforms.Android.Core.MvxAndroidSetup                           |
| MvvmCross.Droid.Support.V7.AppCompat.MvxAppCompatViewPresenter               | MvvmCross.Platforms.Android.Presenters.MvxAndroidViewPresenter             |
| MvvmCross.Droid.Support.V7.AppCompat.Widget.*                                | MvvmCross.Platforms.Android.Binding.Views.*                                |
| MvvmCross.Droid.Support.V7.AppCompat.MvxActionBarDrawerToggle                | MvvmCross.Platforms.Android.Views.AppCompat.MvxActionBarDrawerToggle       |
| MvvmCross.Droid.Support.Design.MvxBottomSheetDialogFragment                  | MvvmCross.DroidX.Material.MvxBottomSheetDialogFragment                     |
| MvvmCross.Droid.Support.V14.Preference.MvxPreferenceFragment                 | MvvmCross.Platforms.Android.Views.Fragments.MvxPreferenceFragment          |
| MvvmCross.Android.Support.V17.Leanback.*                                     | MvvmCross.DroidX.Leanback.*                                                |

## Removal of monodroid9.0 TFM

All packages now target `monodroid10.0`, if you have issues updating, make sure you are targeting Android 10 or newer.

## Thanks

We would like to thank all the people involved in making all the changes for MvvmCross 7.0.0, all changes from small documentation changes to bigger feature Pull Requests are much appreciated.

# Change Log

## [7.0.0](https://github.com/MvvmCross/MvvmCross/tree/7.0.0) (2020-08-14)

[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/6.4.2...7.0.0)

**Breaking changes:**

- Remove support for Framework Fragments. Everything now uses AndroidX Fragments! [\#3750](https://github.com/MvvmCross/MvvmCross/pull/3750) ([Cheesebaron](https://github.com/Cheesebaron))
- Remove monodroid9.0 TFM and update packages [\#3741](https://github.com/MvvmCross/MvvmCross/pull/3741) ([martijn00](https://github.com/martijn00))
- Preliminary Android X support [\#3709](https://github.com/MvvmCross/MvvmCross/pull/3709) ([Cheesebaron](https://github.com/Cheesebaron))
- Remove obsolete platform services method [\#3674](https://github.com/MvvmCross/MvvmCross/pull/3674) ([Strifex](https://github.com/Strifex))

**Implemented enhancements:**

- Binding : Binding Set Creation [\#3693](https://github.com/MvvmCross/MvvmCross/issues/3693)

**Fixed bugs:**

- MvxIosViewPresenter fails to close all modal ViewControllers [\#3826](https://github.com/MvvmCross/MvvmCross/issues/3826)
- EXC\_BAD\_ACCESS on MvxPathSourceStep.ClearPathSourceBinding [\#3784](https://github.com/MvvmCross/MvvmCross/issues/3784)
- Conflict calls to SaveAsync\(\) in MvxSuspensionManager [\#3772](https://github.com/MvvmCross/MvvmCross/issues/3772)
- Exception Java.Lang.NoSuchMethodError: no method with name='isDestroyed' occurs on API 16 [\#3727](https://github.com/MvvmCross/MvvmCross/issues/3727)
- Use dedicated object to acquire lock [\#3810](https://github.com/MvvmCross/MvvmCross/pull/3810) ([Cheesebaron](https://github.com/Cheesebaron))
- Fix NoSuchMethodError exception when calling IsDestroyed [\#3729](https://github.com/MvvmCross/MvvmCross/pull/3729) ([Cheesebaron](https://github.com/Cheesebaron))
- Fix rethrowing exceptions [\#3683](https://github.com/MvvmCross/MvvmCross/pull/3683) ([sm-g](https://github.com/sm-g))

**Closed issues:**

- Mistake in documentation [\#3690](https://github.com/MvvmCross/MvvmCross/issues/3690)

**Merged pull requests:**

- Bump Microsoft.CodeAnalysis.FxCopAnalyzers from 3.0.0 to 3.3.0 [\#3864](https://github.com/MvvmCross/MvvmCross/pull/3864) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Bump Microsoft.NET.Test.Sdk from 16.6.1 to 16.7.0 [\#3860](https://github.com/MvvmCross/MvvmCross/pull/3860) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Bump Microsoft.CodeAnalysis from 3.6.0 to 3.7.0 [\#3859](https://github.com/MvvmCross/MvvmCross/pull/3859) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Bump xunit.runner.visualstudio from 2.4.2 to 2.4.3 [\#3856](https://github.com/MvvmCross/MvvmCross/pull/3856) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Bump MSBuild.Sdk.Extras from 2.0.54 to 2.1.2 [\#3848](https://github.com/MvvmCross/MvvmCross/pull/3848) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Bump Xamarin.GooglePlayServices.Basement from 71.1620.2 to 71.1620.4 [\#3843](https://github.com/MvvmCross/MvvmCross/pull/3843) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Bump Xamarin.GooglePlayServices.Location from 71.1600.1 to 71.1600.4 [\#3842](https://github.com/MvvmCross/MvvmCross/pull/3842) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Fix iOS Modal ViewController closing issue [\#3827](https://github.com/MvvmCross/MvvmCross/pull/3827) ([Gaisuru](https://github.com/Gaisuru))
- Fix SonarCube failing PR builds [\#3823](https://github.com/MvvmCross/MvvmCross/pull/3823) ([Cheesebaron](https://github.com/Cheesebaron))
- Bump Moq from 4.14.4 to 4.14.5 [\#3818](https://github.com/MvvmCross/MvvmCross/pull/3818) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Bump Moq from 4.14.3 to 4.14.4 [\#3815](https://github.com/MvvmCross/MvvmCross/pull/3815) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Bump Moq from 4.14.2 to 4.14.3 [\#3813](https://github.com/MvvmCross/MvvmCross/pull/3813) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Bump Xamarin.AndroidX.MediaRouter from 1.1.0 to 1.1.0.1 [\#3812](https://github.com/MvvmCross/MvvmCross/pull/3812) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Bump Moq from 4.14.1 to 4.14.2 [\#3809](https://github.com/MvvmCross/MvvmCross/pull/3809) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Bump Xamarin.AndroidX.Preference from 1.1.1 to 1.1.1.1 [\#3808](https://github.com/MvvmCross/MvvmCross/pull/3808) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Bump Xamarin.AndroidX.Fragment from 1.2.4 to 1.2.4.1 [\#3807](https://github.com/MvvmCross/MvvmCross/pull/3807) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Fix Playground.Forms.Droid sample [\#3806](https://github.com/MvvmCross/MvvmCross/pull/3806) ([Cheesebaron](https://github.com/Cheesebaron))
- Bump Xamarin.AndroidX.ViewPager from 1.0.0 to 1.0.0.1 [\#3800](https://github.com/MvvmCross/MvvmCross/pull/3800) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Bump Xamarin.AndroidX.ExifInterface from 1.1.0 to 1.1.0.1 [\#3799](https://github.com/MvvmCross/MvvmCross/pull/3799) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Bump Xamarin.AndroidX.RecyclerView from 1.1.0 to 1.1.0.1 [\#3798](https://github.com/MvvmCross/MvvmCross/pull/3798) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Bump Xamarin.AndroidX.Leanback from 1.0.0 to 1.0.0.1 [\#3797](https://github.com/MvvmCross/MvvmCross/pull/3797) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Bump Xamarin.AndroidX.CardView from 1.0.0 to 1.0.0.1 [\#3796](https://github.com/MvvmCross/MvvmCross/pull/3796) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Bump Xamarin.Google.Android.Material from 1.0.0 to 1.0.0.1 [\#3795](https://github.com/MvvmCross/MvvmCross/pull/3795) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Bump Xamarin.AndroidX.Legacy.Support.V4 from 1.0.0 to 1.0.0.1 [\#3794](https://github.com/MvvmCross/MvvmCross/pull/3794) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Bump Xamarin.AndroidX.SwipeRefreshLayout from 1.0.0 to 1.0.0.1 [\#3793](https://github.com/MvvmCross/MvvmCross/pull/3793) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Bump Xamarin.AndroidX.AppCompat from 1.1.0 to 1.1.0.1 [\#3792](https://github.com/MvvmCross/MvvmCross/pull/3792) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Bump Xamarin.AndroidX.Lifecycle.LiveData from 2.2.0 to 2.2.0.1 [\#3791](https://github.com/MvvmCross/MvvmCross/pull/3791) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Bump xunit.runner.visualstudio from 2.4.1 to 2.4.2 [\#3788](https://github.com/MvvmCross/MvvmCross/pull/3788) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Bump AsyncFixer from 1.1.6 to 1.3.0 [\#3786](https://github.com/MvvmCross/MvvmCross/pull/3786) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Bump Xamarin.AndroidX.Lifecycle.LiveData from 2.1.0 to 2.2.0 [\#3782](https://github.com/MvvmCross/MvvmCross/pull/3782) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Fix Android Playground after migration to AndroidX [\#3781](https://github.com/MvvmCross/MvvmCross/pull/3781) ([Cheesebaron](https://github.com/Cheesebaron))
- Bump Microsoft.CodeAnalysis from 3.5.0 to 3.6.0 [\#3776](https://github.com/MvvmCross/MvvmCross/pull/3776) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Bump Xamarin.GooglePlayServices.Location from 71.1600.0 to 71.1600.1 [\#3771](https://github.com/MvvmCross/MvvmCross/pull/3771) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Bump Xamarin.GooglePlayServices.Basement from 71.1620.0 to 71.1620.2 [\#3770](https://github.com/MvvmCross/MvvmCross/pull/3770) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Bump Moq from 4.14.0 to 4.14.1 [\#3765](https://github.com/MvvmCross/MvvmCross/pull/3765) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Bump Microsoft.CodeAnalysis.FxCopAnalyzers from 2.9.8 to 3.0.0 [\#3764](https://github.com/MvvmCross/MvvmCross/pull/3764) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Bump Xamarin.AndroidX.Fragment from 1.2.2 to 1.2.4 [\#3763](https://github.com/MvvmCross/MvvmCross/pull/3763) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Bump Microsoft.NET.Test.Sdk from 16.6.0 to 16.6.1 [\#3762](https://github.com/MvvmCross/MvvmCross/pull/3762) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Bump Xamarin.AndroidX.Preference from 1.1.0 to 1.1.1 [\#3761](https://github.com/MvvmCross/MvvmCross/pull/3761) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Bump Moq from 4.13.1 to 4.14.0 [\#3760](https://github.com/MvvmCross/MvvmCross/pull/3760) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Bump Microsoft.NET.Test.Sdk from 16.5.0 to 16.6.0 [\#3756](https://github.com/MvvmCross/MvvmCross/pull/3756) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Bump Serilog.Sinks.Xamarin from 0.1.37 to 0.2.0.64 [\#3755](https://github.com/MvvmCross/MvvmCross/pull/3755) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Bump Plugin.Permissions from 6.0.0-beta to 6.0.1 [\#3749](https://github.com/MvvmCross/MvvmCross/pull/3749) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Bump Xamarin.AndroidX.Fragment from 1.2.2 to 1.2.3 [\#3744](https://github.com/MvvmCross/MvvmCross/pull/3744) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Bump Xamarin.AndroidX.Fragment from 1.1.0 to 1.2.2 [\#3738](https://github.com/MvvmCross/MvvmCross/pull/3738) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Bump Microsoft.NETCore.UniversalWindowsPlatform from 6.2.9 to 6.2.10 [\#3737](https://github.com/MvvmCross/MvvmCross/pull/3737) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Bump Microsoft.CodeAnalysis from 3.4.0 to 3.5.0 [\#3736](https://github.com/MvvmCross/MvvmCross/pull/3736) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Bump Xamarin.Build.Download from 0.9.0 to 0.10.0 [\#3732](https://github.com/MvvmCross/MvvmCross/pull/3732) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Bump SidebarNavigation from 2.0.0 to 2.1.0 [\#3731](https://github.com/MvvmCross/MvvmCross/pull/3731) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Modify docs. Typo. [\#3703](https://github.com/MvvmCross/MvvmCross/pull/3703) ([jz5](https://github.com/jz5))
- \#3690: Fixed class name [\#3696](https://github.com/MvvmCross/MvvmCross/pull/3696) ([markuspalme](https://github.com/markuspalme))
- Fix MvxLayoutInflater nullref [\#3688](https://github.com/MvvmCross/MvvmCross/pull/3688) ([Cheesebaron](https://github.com/Cheesebaron))
- Update singleton registering sample in the docs [\#3687](https://github.com/MvvmCross/MvvmCross/pull/3687) ([PoLaKoSz](https://github.com/PoLaKoSz))
- Bump Xamarin.Build.Download from 0.4.11 to 0.8.0 [\#3681](https://github.com/MvvmCross/MvvmCross/pull/3681) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Downgrade Xamarin.Build.Download to 0.4.11 as latest has a bug [\#3679](https://github.com/MvvmCross/MvvmCross/pull/3679) ([Cheesebaron](https://github.com/Cheesebaron))