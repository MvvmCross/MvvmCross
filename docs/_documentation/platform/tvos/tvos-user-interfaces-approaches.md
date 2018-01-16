---
layout: documentation
title: Approaches for tvOS UIs
category: Platforms
---

Flexibility is important to us. That is why MvvmCross allows you to create your iOS user interfaces using any of the following approaches:

- Coded interfaces
- XIBs 
- Storyboards

### XIBs

XIB files are XML files that describe visual elements. You can edit them using the Xamarin Designer or XCode Interface Builder. Just remember to set up the Outlets (XCode IB) / Names (X Designer) to be able to bind your controls later!

**Note:** It is very important that you drag and drop the correct ViewController type (for example, if you want to add a TabBarController, you need to drag and drop a UITabBarViewController). Otherwise your app might not crash, but you might encounter some weird situations.

### Storyboards

Storyboards are XML files that can be visually edited using the Xamarin Designer or XCode Interface Builder and are the preferred way to setup tvOS projects. These files are very similar to XIB ones, but they normally contain more than a single user interface, alongisde some "segues" to navigate between them (although is not the recommended approach, you can use segues for navigation in MvvmCross).

Storyboards are definitely a step forward when working with XIBs and are the recommended approach from Apple, but keep in mind that you still need to set up the Outlets (XCode IB) / Names (X Designer) to be able to bind your controls later. And since the generated XML code is not intended to be human readable, it is difficult to resolve conflicts for them in merge situations (you can have multiple storyboards to leverage this).  NOTE:  You must set the StoryboardID to match your class name to properly lookup your views from the Storygboard file if using XCode Interface Builder.  

To use a view generated in a Storyboard, all you have to do is to add an `MvxFromStoryboard` attribute over your View class, indicating the name of the storyboard file:

```c#
    [MvxFromStoryboard("Main")]
    public partial class RootView : MvxViewController<RootViewModel>
    {
        // ...
    }
```

**Note:** It is very important that you drag and drop the correct ViewController type (for example, if you want to add a TabBarController, you need to drag and drop a UITabBarViewController). Otherwise your app might not crash, but you might encounter some weird situations.

