---
layout: post
title: Moving towards MvvmCross 5.0
date:   2016-12-14 11:37:35 +0100
categories: mvvmcross
---
We have started the work on the next major release of MvvmCross. A lot has been planned for this release and all the tasks can be seen on GitHub https://github.com/MvvmCross/MvvmCross/issues/1415 Some of these tasks have already been completed and work has started on many of the others. In this blog post I'd like to talk a bit about  some of the tasks and what is going on in the MvvmCross world.

## Merging sub-repositories back into the main

Some of the recent changes to MvvmCross have been that we have moved back the sub repositories into the main MvvmCross repository. Why, you may ask. This is due to the tremendous amount of time it currently takes to make a MvvmCross release. Currently the time spent on a release is somewhere around 2 hours! We want to automate this and make it so that I am not the only person able to make a release and that I can spend my time elsewhere than on making releases.
The first step towards automating the release is to merge back the other repositories into the main repository. This is due to currently the sub repositories depend on NuGet packages to be created from the main repository, then upgrade to these NuGets before a release can be made. For the MvvmCross-Plugins repository, where we have over 100 projects, the upgrade time for NuGets takes tremendously long time. Instead we just want to build everything at once and make all NuGet packages at once.

## Removal of Windows Phone 8.1 and Windows 8.1 Store support

To prepare for switching to .NET Standard 2.0, we are slowly moving away from supporting obsolete platforms. This includes removing support for Windows Phone 8.1 and Windows 8.1. We have already removed all Silverlight support. Please refer to the compatibility matrix here: https://github.com/dotnet/standard/blob/master/docs/versions.md
What .NET Standard 2.0 provides for is a cleaner and more unified API to work with. It dictates a minimal API all platforms have to implement. MvvmCross currently depends heavily on Portable Class Libraries, where features such as reflection differ a lot from profile to profile. With PCL it is not very predictable what API surface you get. .NET Standard solves this problem.
In general we are very much for the switch to .NET Standard. However, this means we need to sacrifice some of our supported platforms. Looking at it another way Visual Studio 2017 will not come with tools to load and build Windows Phone 8.1 and Windows 8.1 Store projects either and UWP is what .NET Standard is to the Windows platform. It unifies the API and makes it much easier to code against, not having to maintain two separate projects for your phone and tablet App. These are just some of the reasons to remove Windows Phone 8.1 and Windows 8.1 Store support.
This means that MvvmCross 4.4.0 will be the last version to support Windows Phone 8.1 and Windows 8.1 Store projects.
Just to clarify this does _not_ concern WPF or .NET45 projects at all.

## More power to the developer

A lot of the other tasks for MvvmCross 5.0 aim to make it much easier for the developer to use MvvmCross. Some of the proposed changes are to make unified lifecycle events which can be used throughout the App. We want to integrate runtime permissions as part of a plug in. We want to use bait and switch for plug ins and potentially get rid of bootstrap files floating around. Add support for no splash screens. We want to try make MvvmCross start faster by making the startup bits async. Last but not least we want to add URL/deeplink navigation support to the presenters, such that MvvmCross apps easily can be launched from notifications or URLs they respond to and launch the correct screen, but also have the correct back navigation.

Follow the discussion on GitHub and add your opinion, grab a task and help out. Write documentation or just file issues and bugs.