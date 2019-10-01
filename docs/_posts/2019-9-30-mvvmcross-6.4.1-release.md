---
layout: post
title: MvvmCross 6.4.1
date:   2019-09-30 20:00:00 +0200
categories: mvvmcross
---

A new MvvmCross version is available on [NuGet](https://www.nuget.org/packages/MvvmCross/6.4.1)! You can always find the latest [changelog in the root of the repository](https://github.com/MvvmCross/MvvmCross/blob/develop/CHANGELOG.md) to see what has changed between versions.

This release mainly contains servicing of some of our packages and the addition of `monoandroid10.0` as a target framework and bug fixes to support Android Q.

We have switched away from using the dynamic keyword to get rid of the dependency on the `Microsoft.CSharp` NuGet package. The only place `dynamic` was used in the log4net logger. This should not be a breaking change.

We would like to thank all the people involved in making all the changes for MvvmCross 6.4.1, all changes from small documentation changes to bigger feature Pull Requests are much appreciated.

# Change Log

## [6.4.1](https://github.com/MvvmCross/MvvmCross/tree/6.4.1) (2019-09-30)
[Full Changelog](https://github.com/MvvmCross/MvvmCross/compare/6.4.0...6.4.1)

**Fixed bugs:**

- MvxExpandableTableViewSource doesn't let touches through [\#3574](https://github.com/MvvmCross/MvvmCross/issues/3574)
- MvxLayoutInflater CreateCustomViewInternal fails on Android 10 [\#3550](https://github.com/MvvmCross/MvvmCross/issues/3550)
- NullReferenceException when initializing fragment in ViewPager [\#3535](https://github.com/MvvmCross/MvvmCross/issues/3535)

**Closed issues:**

- Update Fused Location plugin to latest version of Play Services [\#3580](https://github.com/MvvmCross/MvvmCross/issues/3580)
- Nested fragment inside viewpager caused error “Fragment host not found” [\#3380](https://github.com/MvvmCross/MvvmCross/issues/3380)

**Merged pull requests:**

- Fix header cells in MvxExpandableTableViewSource not passing touches to subviews [\#3575](https://github.com/MvvmCross/MvvmCross/pull/3575) ([Cheesebaron](https://github.com/Cheesebaron))
- Fix MvxLayoutInflater throwing NoSuchFieldException on Android Q [\#3573](https://github.com/MvvmCross/MvvmCross/pull/3573) ([Cheesebaron](https://github.com/Cheesebaron))
- Update Play Services in Location Fused plugin [\#3583](https://github.com/MvvmCross/MvvmCross/pull/3583) ([Cheesebaron](https://github.com/Cheesebaron))
- Update nuget packages [\#3582](https://github.com/MvvmCross/MvvmCross/pull/3582) ([martijn00](https://github.com/martijn00))
- Bump to latest version of MSBuild SDK Extras [\#3581](https://github.com/MvvmCross/MvvmCross/pull/3581) ([Cheesebaron](https://github.com/Cheesebaron))
- Fix some comments and obsolete warning [\#3557](https://github.com/MvvmCross/MvvmCross/pull/3557) ([martijn00](https://github.com/martijn00))
- Stop using dynamic [\#3556](https://github.com/MvvmCross/MvvmCross/pull/3556) ([martijn00](https://github.com/martijn00))
- Update Plugins getting-started.md to proper load non explicitly referenced plugins [\#3555](https://github.com/MvvmCross/MvvmCross/pull/3555) ([fedemkr](https://github.com/fedemkr))
- Ci/build script updates [\#3553](https://github.com/MvvmCross/MvvmCross/pull/3553) ([Cheesebaron](https://github.com/Cheesebaron))
- Update android-recyclerview.md [\#3552](https://github.com/MvvmCross/MvvmCross/pull/3552) ([pearsonallen](https://github.com/pearsonallen))
- NullReferenceException when initializing fragment in ViewPager [\#3549](https://github.com/MvvmCross/MvvmCross/pull/3549) ([Prin53](https://github.com/Prin53))
- Views filtering in MvxViewModelViewLookupBuilder [\#3539](https://github.com/MvvmCross/MvvmCross/pull/3539) ([orzech85](https://github.com/orzech85))
- Update github-pages gem and Jekyll [\#3536](https://github.com/MvvmCross/MvvmCross/pull/3536) ([Cheesebaron](https://github.com/Cheesebaron))
- Bump MSBuild.Sdk.Extras from 2.0.41 to 2.0.43 [\#3529](https://github.com/MvvmCross/MvvmCross/pull/3529) ([dependabot-preview[bot]](https://github.com/apps/dependabot-preview))