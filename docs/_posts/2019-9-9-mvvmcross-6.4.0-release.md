---
layout: post
title: MvvmCross 6.4.0
date:   2019-09-9 10:00:00 -0200
categories: mvvmcross
---

A new MvvmCross version is available on [NuGet](https://www.nuget.org/packages/MvvmCross/6.4.0)! You can always find the latest [changelog in the root of the repository](https://github.com/MvvmCross/MvvmCross/blob/develop/CHANGELOG.md) to see what has changed between versions.

We are very happy to announce this release includes [41](https://github.com/MvvmCross/MvvmCross/milestone/52?closed=1) issues / pull requests. Some of these changes are breaking changes, which we will attempt to describe in this blog post.

# Breaking changes

An overview of all the [breaking changes in 6.4.0 can be found in GitHub Pull Requests.](https://github.com/MvvmCross/MvvmCross/pulls?q=is%3Apr+is%3Aclosed+milestone%3A6.4.0+label%3At%2Fbreaking)

## PR#3372 - Tidying up and making startup more consistent

[Link to Pull Request](https://github.com/MvvmCross/MvvmCross/pull/3372)

This PR primarily introduces changes to `MvxSetup` and inheriting platform specific versions, which are breaking.

A new method `RegisterDefaultSetupDependencies` has been added, which creates and registers most types needed for startup. Previously this was spread over multiple method calls different places in `MvxSetup`. This method is marked virtual so you can change the behavior of what is happening inside of it. There is also an event you can listen to called `RegisterSetupDependencies` if you want to know when setup dependencies have been registered.

Some other methods have had their signatures changed from `void` to returning a type. Most of these methods are usually not overriden by Apps.

## PR#3461 - Breaking up logic for creating view lookup

[Link to Pull Request](https://github.com/MvvmCross/MvvmCross/pull/3461)

This PR introduces a breaking change by adding a new virtual method called `InitializeLookupDictionary` which should be used instead of overriding `InitializeViewLookup` going forward. This breaks up creating the Views container and creating the dictionary for mapping View to ViewModels.

Apps using `InitializeViewLookup` can with advantange start using `InitializeLookupDictionary` to change their View to ViewModel mappings.

## PR#3456 - Replace MvxColor with System.Drawing.Color

[Link to Pull Request](https://github.com/MvvmCross/MvvmCross/pull/3456)

Since we changed to .NET Standard 2.0 for our core libraries, we now have access to namespaces such as `System.Drawing.Color`. Meaning we do not need to have our own color class anymore. For the platform specific implementations of `System.Drawing.Color` for Xamarin target frameworks, we use OpenTK which is bundled with Xamarin.

This means if you are using `MvxColor`, you should move to use `System.Drawing.Color`. We still provide the `MvxColor` plugin, which contains converters for bindings and methods to transform `System.Drawing.Color` into native `UIColor`, `Color` and other platform specific colors.

## PR#3487 - Fixing RequestTranslator ignores Presentation/Parameter values (Android)

[Link to Pull Request](https://github.com/MvvmCross/MvvmCross/pull/3487)

The method signature in the interface `IMvxAndroidViewModelRequestTranslator` for the method `GetIntentWithKeyFor` has been changed from returning a `Tuple<Intent, int>` to returning a `ValueTuple (Intent intent, int key)`.

The method in the Android View Presenter `CreateIntentForRequest` was also changed to take into account the instance of ViewModel passed in as the property `ViewModelInstance` on a `MvxViewModelInstanceRequest`, instead of trying to create a new one every time.

## PR#3501 - Fixing ViewPager ignores Presentation Values

[Link to Pull Request](https://github.com/MvvmCross/MvvmCross/pull/3501)

Similar as [PR#3487](https://github.com/MvvmCross/MvvmCross/pull/3487), the Adapters for ViewPager ignores the values in `MvxViewModelRequest`. This has been fixed and when Fragments are created for the ViewPager, this should be taken into account.
This was marked breaking as it changes the behavior of ViewPager slightly.

# Other highlighs

A couple of other highlights includes:

- [PR#3431](https://github.com/MvvmCross/MvvmCross/pull/3431) you can now use a Binding set in the `using` pattern and the disposal of the binding set will call `Apply()` on the set.
- [PR#3484](https://github.com/MvvmCross/MvvmCross/pull/3484) which prepares `MvxIoCTest` to allow other IoC providers.
- [PR#3510](https://github.com/MvvmCross/MvvmCross/pull/3510) adds additional Android Target Bindings for `VideoView`, `WebView` and `View`.
- [PR#3510](https://github.com/MvvmCross/MvvmCross/pull/3491) our IoCConstruct method now tries harder to find a best-matching constructor instead of using `FirstOrDefault()` and too quickly trying to fall back to defaul constructor.
- Moving from AppVeyor for building packages to using Azure DevOps. This is mainly due to the big amount of time it took AppVeyor to prepare working VS2019 images, while Azure DevOps have had them for a very long time.
- Pushing NuGet packages to [Github Package Registry](https://github.com/MvvmCross/MvvmCross/packages), you can add `https://nuget.pkg.github.com/MvvmCross/index.json` as your NuGet package source to try out builds from the develop branch. For more information on how to set up this look at [GitHub's Configuring NuGet for use with GitHub Package Registry documentation](https://help.github.com/en/articles/configuring-nuget-for-use-with-github-package-registry), right now it needs authentication to consume it.
- We've added DependaBot to automate updating of dependencies in MvvmCross.

We would like to thank all the people involved in making all the changes for MvvmCross 6.4.0, all changes from small documentation changes to bigger feature Pull Requests are much appreciated.

# Changelog

You can always find the latest [changelog in the root of the repository](https://github.com/MvvmCross/MvvmCross/blob/develop/CHANGELOG.md) to see what has changed between versions.

## [6.4.0](https://github.com/MvvmCross/MvvmCross/tree/6.4.0) (2019-09-09)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/6.3.1...6.4.0)

**Implemented enhancements:**

- Prepare MvxIoCTest to allow other IoC providers [\#3484](https://github.com/MvvmCross/MvvmCross/pull/3484) ([SamuelDebruyn](https://github.com/SamuelDebruyn))
- Impliment apply\(\) on dispose of clear binding set [\#3431](https://github.com/MvvmCross/MvvmCross/pull/3431) ([Tyron18](https://github.com/Tyron18))

**Fixed bugs:**

- ViewPager ignores Presentation Values [\#3497](https://github.com/MvvmCross/MvvmCross/issues/3497)
- Prevent null reference when trying to look up latest binding context [\#3518](https://github.com/MvvmCross/MvvmCross/pull/3518) ([Cheesebaron](https://github.com/Cheesebaron))
- Fix ctor name in target binding [\#3511](https://github.com/MvvmCross/MvvmCross/pull/3511) ([Cheesebaron](https://github.com/Cheesebaron))
- RequestTranslator ignores Presentation/Parameter values [\#3487](https://github.com/MvvmCross/MvvmCross/pull/3487) ([Prin53](https://github.com/Prin53))

**Closed issues:**

- Is MVVMCross Compatible with Xamarin.Forms Version 4.0 Shell? [\#3523](https://github.com/MvvmCross/MvvmCross/issues/3523)
- ContentPage decorated with MvxTabbedPagePresentation not wrapped in NavigationPage [\#3513](https://github.com/MvvmCross/MvvmCross/issues/3513)
- RequestTranslator ignores Presentation/Parameter values [\#3482](https://github.com/MvvmCross/MvvmCross/issues/3482)
- LinkageError: No implementation found for void mono.android.text.TextWatcherImplementor.n\_beforeTextChanged [\#3478](https://github.com/MvvmCross/MvvmCross/issues/3478)
- Mvx.IoCProvider.CallbackWhenRegistered's action is called BEFORE the actual singleton is registered. [\#3472](https://github.com/MvvmCross/MvvmCross/issues/3472)
- MvxFormsAppCompatActivity is unavailable in Android project [\#3460](https://github.com/MvvmCross/MvvmCross/issues/3460)
- MvxSpinner classNotFoundException inside fragment [\#3454](https://github.com/MvvmCross/MvvmCross/issues/3454)
- Bindings not attempted in MvxRecycler item template layout when given item in ItemsSource is null  [\#3424](https://github.com/MvvmCross/MvvmCross/issues/3424)

**Merged pull requests:**

- Update android-spinner.md [\#3530](https://github.com/MvvmCross/MvvmCross/pull/3530) ([SebastienForay](https://github.com/SebastienForay))
- Don't try to sign packages for Pull Requests [\#3525](https://github.com/MvvmCross/MvvmCross/pull/3525) ([Cheesebaron](https://github.com/Cheesebaron))
- Update ios-uirefreshcontrol.md [\#3524](https://github.com/MvvmCross/MvvmCross/pull/3524) ([Nerves82](https://github.com/Nerves82))
- Fix presentation hint for non-forms views in forms presenter [\#3522](https://github.com/MvvmCross/MvvmCross/pull/3522) ([orzech85](https://github.com/orzech85))
- Example of Android Native View for MvvmCross.Forms [\#3521](https://github.com/MvvmCross/MvvmCross/pull/3521) ([orzech85](https://github.com/orzech85))
- Bump Moq from 4.12.0 to 4.13.0 [\#3519](https://github.com/MvvmCross/MvvmCross/pull/3519) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Modal Popover Presentation Crash [\#3516](https://github.com/MvvmCross/MvvmCross/pull/3516) ([Prin53](https://github.com/Prin53))
- Cleanup csproj files [\#3512](https://github.com/MvvmCross/MvvmCross/pull/3512) ([martijn00](https://github.com/martijn00))
- Add more Android Target bindings [\#3510](https://github.com/MvvmCross/MvvmCross/pull/3510) ([Cheesebaron](https://github.com/Cheesebaron))
- Update github-pages gem [\#3509](https://github.com/MvvmCross/MvvmCross/pull/3509) ([Cheesebaron](https://github.com/Cheesebaron))
- ViewPager ignores Presentation Values [\#3501](https://github.com/MvvmCross/MvvmCross/pull/3501) ([Prin53](https://github.com/Prin53))
- Bump MSBuild.Sdk.Extras from 2.0.31 to 2.0.41 [\#3496](https://github.com/MvvmCross/MvvmCross/pull/3496) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Bump Microsoft.CodeAnalysis from 3.1.0 to 3.2.1 [\#3493](https://github.com/MvvmCross/MvvmCross/pull/3493) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- IocConstruct constructor lookup & constructor injection [\#3491](https://github.com/MvvmCross/MvvmCross/pull/3491) ([SamuelDebruyn](https://github.com/SamuelDebruyn))
- Bump Xamarin.Forms.Platform.WPF from 3.6.0.220655 to 4.1.0.673156 [\#3490](https://github.com/MvvmCross/MvvmCross/pull/3490) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Bump Xamarin.Forms from 3.6.0.220655 to 4.1.0.673156 [\#3489](https://github.com/MvvmCross/MvvmCross/pull/3489) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Update tizen backend implementation [\#3477](https://github.com/MvvmCross/MvvmCross/pull/3477) ([rookiejava](https://github.com/rookiejava))
- Minor typo [\#3473](https://github.com/MvvmCross/MvvmCross/pull/3473) ([garyng](https://github.com/garyng))
- Bump Microsoft.SourceLink.GitHub from 1.0.0-beta2-19351-01 to 1.0.0-beta2-19367-01 [\#3471](https://github.com/MvvmCross/MvvmCross/pull/3471) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Minor typo in docs [\#3469](https://github.com/MvvmCross/MvvmCross/pull/3469) ([garyng](https://github.com/garyng))
- Bump MSBuild.Sdk.Extras from 2.0.29 to 2.0.31 [\#3468](https://github.com/MvvmCross/MvvmCross/pull/3468) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Breaking up logic for creating view lookup [\#3461](https://github.com/MvvmCross/MvvmCross/pull/3461) ([nickrandolph](https://github.com/nickrandolph))
- Increment Min Android Version [\#3458](https://github.com/MvvmCross/MvvmCross/pull/3458) ([nickrandolph](https://github.com/nickrandolph))
- Replace MvxColor with System.Drawing.Color [\#3456](https://github.com/MvvmCross/MvvmCross/pull/3456) ([Strifex](https://github.com/Strifex))
- Bump Microsoft.SourceLink.GitHub from 1.0.0-beta2-18618-05 to 1.0.0-beta2-19351-01 [\#3451](https://github.com/MvvmCross/MvvmCross/pull/3451) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Bump Microsoft.NETCore.UniversalWindowsPlatform from 6.2.3 to 6.2.8 [\#3448](https://github.com/MvvmCross/MvvmCross/pull/3448) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Bump MSBuild.Sdk.Extras from 2.0.24 to 2.0.29 [\#3447](https://github.com/MvvmCross/MvvmCross/pull/3447) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Bump Xamarin.FFImageLoading from 2.4.9.961 to 2.4.11.982 [\#3446](https://github.com/MvvmCross/MvvmCross/pull/3446) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Bump Microsoft.CodeAnalysis from 2.10.0 to 3.1.0 [\#3444](https://github.com/MvvmCross/MvvmCross/pull/3444) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Bump Microsoft.NET.Test.Sdk from 15.9.0 to 16.2.0 [\#3443](https://github.com/MvvmCross/MvvmCross/pull/3443) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Bump Moq from 4.10.1 to 4.12.0 [\#3441](https://github.com/MvvmCross/MvvmCross/pull/3441) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Bump Newtonsoft.Json from 12.0.1 to 12.0.2 [\#3440](https://github.com/MvvmCross/MvvmCross/pull/3440) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))
- Update value-converters.md [\#3437](https://github.com/MvvmCross/MvvmCross/pull/3437) ([An0d](https://github.com/An0d))
- Switch to Azure Pipelines [\#3397](https://github.com/MvvmCross/MvvmCross/pull/3397) ([Cheesebaron](https://github.com/Cheesebaron))
- Tidying up and making startup more consistent [\#3372](https://github.com/MvvmCross/MvvmCross/pull/3372) ([nickrandolph](https://github.com/nickrandolph))