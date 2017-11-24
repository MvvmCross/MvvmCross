---
layout: post
title: MvvmCross 5.5
date:   2017-11-23 14:00:00 +0100
categories: mvvmcross
---

# Announcing MvvmCross 5.5!

A new MvvmCross version is available on [NuGet](https://www.nuget.org/packages/MvvmCross/5.5.0)! You can always find the latest [changelog in the root of the repository](https://github.com/MvvmCross/MvvmCross/blob/develop/CHANGELOG.md) to see what has changed.

## New UWP ViewPresenter

This time it's UWP turn! We have a brand new ViewPresenter that uses the same attribute system as the others ViewPresenters. It support the following presentation modes:

- Page (default)
- SplitView (pane and content)
- Region

Note that as part of this change the old ViewPresenter has been deprecated. 

In order to use the new attributes, all you need to do is to place any of them over your view class:

```c#
[MvxPagePresentation]
public sealed partial class RootView : MvxWindowsPage
{
    public RootView()
    {
        this.InitializeComponent();
    }
}
```

Check out the [Playground.UWP](https://github.com/MvvmCross/MvvmCross/tree/develop/TestProjects/Playground/Playground.Uwp) sample to see it in action!

## ViewModels lifecycle

We are very happy to announce that we have fixed several issues related to navigation and we have finally stabilized the new ViewModel Lifecycle! This is what it looks like:

1. Construction
2. Prepare
3. Initialize

There is also a stable mechanism to handle tombstoning situations: SaveState and ReloadState issues are solved!

Some other highlights include:
- You can watch the state (and even bind views) of Initialize using the property `ViewModel.InitializeTask`.
- ViewModels loaded manually (using MvxViewModelLoader) do call Prepare and Initialize as expected.

We highly recommend you to read the [ViewModel Lifecycle document](https://www.mvvmcross.com/documentation/fundamentals/viewmodel-lifecycle) to understand how everything works.

## Async operations with MvxNotifyTask

We have added a super useful helper when it comes to async/await: [MvxNotifyTask](https://github.com/MvvmCross/MvvmCross/blob/develop/MvvmCross/Core/Core/ViewModels/MvxNotifyTask.cs).

MvxNotifyTask provides you with an object that can watch for different Task states and raise property-changed notifications that you can subscribe to: This means you can bind any Task properties in your Views.

Other than that, this class acts as a sandbox for async operations: If a Task fails and raises an exception, your app won’t crash, and the exception will be available for you through `MvxNotifyTask.Exception`.

If this looks interesting to you, don't hesitate to read [the official documentation for it](https://www.mvvmcross.com/documentation/fundamentals/mvxnotifytask?scroll=225). And of course, you can start using it in your apps right now!



Changelog and more details are coming soon!