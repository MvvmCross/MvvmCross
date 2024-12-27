---
layout: documentation
title: Approaches for iOS UIs
category: Platforms
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

Storyboards are XML files that can be visually edited using the Xamarin Designer or XCode Interface Builder. These files are very similar to XIB ones, but they normally contain more than a single user interface, alongside some "segues" to navigate between them (although is not the recommended approach, you can use segues for navigation in MvvmCross).

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

#### Storyboard caveats

- MvvmCross does not support defining main storyboard in Info.plist using the `UIMainStoryboardFile` key and this might be ignored
- Remove `initiaViewController` attribute in the root `document` node to prevent issues with iOS instantiating ViewControllers twice
- You can omit the name in the `MvxFromStoryboard` attribute, in such cases make sure that you ViewController and `storyboardIdentifier="{ViewName}"` have the same names otherwise you will get a crash 

## MvxBaseViewController

In addition to the core MvvmCross view controller classes (MvxViewController) we have added a slightly expanded feature set to a new abstract base class called MvxBaseViewController.

The idea behind this class is to gradually build up some "extended core" features that most developers will use without over-burdening  the class with too much extraneous stuff.

At the moment this class is a generic view controller that has currently only one major feature over and above the core "basic" view controller - automatic keyboard handling.

The feature requires there to be a UIScrollView in the view hierarchy in order to function.  It will detect a touch on a UIView that also expands the keyboard, it will then ensure that the view with focus is not obscured by the keyboard and is centered in the applications UI.  It can also optionally hide the keyboard when the user makes any further touches that moves focus away from the edit view.

You can make use of this class using the following standard inheritance syntax:

```c#
public class KeyboardHandlingView
    : MvxBaseViewController<KeyboardHandlingViewModel>
{
}
```

In order to enable the keyboard handing features you need to first call the initializing method during view initialization, such as:

```c#
public override void ViewDidLoad()
{
    base.ViewDidLoad();
    // setup the keyboard handling
    InitKeyboardHandling();

    var scrollView = new UIScrollView();

    Add(scrollView);
    View.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
    View.AddConstraints(
        scrollView.AtTopOf(View),
        scrollView.AtLeftOf(View),
        scrollView.WithSameWidth(View),
        scrollView.WithSameHeight(View)
    );
}
```

In addition to calling this initialization method you also need to override the following method and ensure that it returns true:

```c#
public override bool HandlesKeyboardNotifications()
{
    return true;
}
```
