---
layout: documentation
title: ViewModel Lifecycle
category: Fundamentals
order: 5
---
Starting from MvvmCross 5.0 ViewModels will be coupled to the lifecycle of the view. This means that the ViewModel has the following methods available:

```c#
    void Appearing();

    void Appeared();

    void Disappearing();

    void Disappeared();
```

The MvxViewController, MvxFragment(s), MvxActivity and the UWP views will call those methods open the platform specific events that are fired. This will give us a more refined control of the ViewModel and the state of its lifecycle. There may be binding that you want to update or resources to clean up, these lifecycle events can help with that.

It should be noted however that it is not 100% reliable but it should work for most of the apps. We don't know what you do in the lifecycle of your app and what could interfere with the called order of the viewmodel lifecycle events.

## Mapping view event to viewmodel events

There has been a thread going on on the [Xamarin forums](https://forums.xamarin.com/discussion/comment/240043/) where the implementation is discussed of this functionality. MvvmCross has based its lifecycle support on this thread and those events. 

|           | Appearing             | Appeared       | Disappearing         | Disappeared | 
| iOS       | ViewWillAppear        | ViewDidAppear  | ViewWillDisappear    | ViewDidDisappear | 
| Android   | OnAttachedToWindow    | OnGlobalLayout | OnPause              | OnDetachedToWindow | 
| UWP       | Loading               | OnLoaded       | Unloaded             | OnUnloaded |    


For more information on the implementation of this functionality please see [Github](https://github.com/MvvmCross/MvvmCross/pull/1601)