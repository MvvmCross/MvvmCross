---
layout: documentation
title: Approaches for iOS UIs
category: Platform specifics
---

Flexibility is important to us. That is why MvvmCross allows you to create your iOS user interfaces using any of the following approaches:

- Coded interfaces
- XIBs
- Storyboards

Is it hard to make a decision? Don't worry! You can combine different approaches within your app.


### Coded interfaces

When using this technique, you have to create all your visual elements in C# files. This approach is widely spread around iOS developers, because it gives you full control of what is going on - and it keeps you away from UI designer tools. By coding your interfaces by hand, you can take advantage of the power C# offers, as well as having type-safe UI code.

But watch out! Coded interfaces also mean that you need to create the constraints manually. You might want to take a look at some helper libraries for that (like [FluentLayout](https://github.com/FluentLayout/Cirrious.FluentLayout)).

### XIBs

XIB files are XML files that describe visual elements. You can edit them using the Xamarin Designer or XCode Interface Builder. Just remember to set up the Outlets (XCode IB) / Names (X Designer) to be able to bind your controls later!

**Note:** It is very important that you drag and drop the correct ViewController type (for example, if you want to add a TabBarController, you need to drag and drop a UITabBarViewController). Otherwise your app might not crash, but you might encounter some weird situations.

### Storyboards

Storyboards are XML files that can be visually edited using the Xamarin Designer or XCode Interface Builder. These files are very similar to XIB ones, but they normally contain more than a single user interface, alongisde some "segues" to navigate between them (although is not the recommended approach, you can use segues for navigation in MvvmCross).

Storyboards are definitely a step forward when working with XIBs, but keep in mind that you still need to set up the Outlets (XCode IB) / Names (X Designer) to be able to bind your controls later. And since the generated XML code is not intended to be human readable, it is difficult to resolve conflicts for them in merge situations (you can have multiple storyboards to leverage this).

To use a view generated in a Storyboard, all you have to do is to add an `MvxFromStoryboard` attribute over your View class, indicating the name of the storyboard file:

```c#
    [MvxFromStoryboard("Main")]
    public partial class RootView : MvxViewController<RootViewModel>
    {
        // ...
    }
```

**Note:** It is very important that you drag and drop the correct ViewController type (for example, if you want to add a TabBarController, you need to drag and drop a UITabBarViewController). Otherwise your app might not crash, but you might encounter some weird situations.

