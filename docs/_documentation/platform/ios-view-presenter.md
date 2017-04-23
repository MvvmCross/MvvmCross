---
layout: documentation
title: iOS View Presenter
category: Platform specifics
---

Starting with MvvmCross 5.0, there is a new default Presenter for Views, namely `MvxIosViewPresenter`.

## View Presenter Overview

The default presenter that comes with MvvmCross offers out of the box support for the following navigation patterns / strategies:

- Stack navigation
- Tabs
- SplitView
- Modal
- Modal navigation

Also if your app needs another kind of presentation mode, you can easily extend it!

## Presentation Attributes

The presenter uses a set of `PresentationAttributes` to define how a view will be displayed. The existing attributes are:

### MvxRootPresentationAttribute
Used to set a view as _Root_. You should use this attribute over the view class that will be the root of your application (your app can have several root views, one at a time).
The view root can be one of the following types:

- To use stack navigation, your view can just be a `MvxViewController`, but it needs to set the attribute member `WrapInNavigationController` to true.
- To use Tabs, your view needs to implement `IMvxTabBarViewController` or simply extend `MvxTabBarViewController`, which has all the needed behavior built in.
- To use a SplitView, your view needs to implement `IMvxSplitViewController` or simply extend `MvxSplitViewController`, which has all the needed behavior built in.


### MvxChildPresentationAttribute
Used to set a view as a _child_. You should use this attribute over a view class that will be displayed inside a navigation stack.
The view class can decide if wants to be displayed animated or not through the attribute member `Animated` (the default value is `true`).


### MvxModalPresentationAttribute
Used to display a view as _Modal_. You should use this attribute over a view class to present the view as a modal.
There are several attribute members that the view class can customize:

- WrapInNavigationController: If set to `true`, a modal navigation stack will be initiated (following child presentations will be displayed inside the modal stack). The default value is `false`.
- ModalPresentationStyle: Corresponds to the `ModalPresentationStyle` property of UIViewController. The default value is `UIModalPresentationStyle.FullScreen`.
- ModalTransitionStyle: Corresponds to the `ModalTransitionStyle` property of UIViewController. The default value is `UIModalTransitionStyle.CoverVertical`.
- Animated: If set to true, the presentation will be animated. The default value is `true`.


### MvxTabPresentationAttribute
This attribute is only useful (and should only be used) when the current _Root_ view is a `IMvxTabBarViewController`.
By using it over a view class, the presenter will show the view as a _Tab_ inside the TabBarController.
The presentation can be highly customized through this attribute members:

- TabName: Defines the title of the tab that will be displayed below the tab icon. It has to be a magic string, but it can be for example a key to a localized string that you can grab overriding the method `SetTitleAndTabBarItem` in your TabBarController.
- TabIconName: Defines the name of the resource that will be used as icon for the tab. It also has to be a magic string, but same as before, your app can take control of what happens by overriding the method `SetTitleAndTabBarItem` in your TabBarController.
- WrapInNavigationController: If set to `true`, the view will be wrapped in a `MvxNavigationController`, which will allow the tab to have its own navigation stack. **Important note**: When the current _Root_ is a TabBarController and there is no current modal navigation stack, child presentations will be tried to be displayed in the current selected _Tab_.
- TabAccessibilityIdentifier: Corresponds to the UIViewController.View `AccessibilityIdentifier` property.

### MvxMasterSplitViewPresentationAttribute
This attribute is only useful (and should only be used) when the current _Root_ view is a `IMvxSplitViewController`.
By using it over a view class, the presenter will show the view as _Master_ of the split.

There is an attribute member that can be used to customize the presentation:
- WrapInNavigationController: If set to `true`, the view will be displayed wrapped in a `MvxNavigationController`, which will allow you to set a title, which is the most common scenario of SplitView. The default value is therefore `true`.


### MvxDetailSplitViewPresentationAttribute
This attribute is only useful (and should only be used) when the current _Root_ view is a `IMvxSplitViewController`.
By using it over a view class, the presenter will show the view as _Detail_ of the split.

There is an attribute member that can be used to customize the presentation:
- WrapInNavigationController: If set to `true`, the view will be displayed wrapped in a `MvxNavigationController`,  which will allow the view to have its own navigation stack.


## Views without attributes: Default values

- If the initial view class of your app has no attribute over it, the presenter will assume stack navigation and will wrap your initial view in a `MvxNavigationController`.
- If a view class has no attribute over it, the presenter will assume _animated_ child presentation.



## Extensibility
The presenter is completely extensible! You can override any attribute and customize attribute members.

You can also define new attributes to satisfy your needs. The steps to do so are:

1. Add a new attribute that extends `MvxBasePresentationAttribute`
2. Subclass MvxIosViewPresenter and make it the presenter of your application in Setup.cs (by overriding the method `CreatePresenter`).
3. Override the method `RegisterAttributeTypes` and add a registry to the dictionary like this:

```c#
_attributeTypesToShowMethodDictionary.Add(
    typeof(MyCustomModePresentationAttribute),
    (vc, attribute, request) => ShowMyCustomModeViewController(vc, (MyCustomPresentationAttribute)attribute, request));
```

4. Implement a method that takes care of the presentation mode (in the example above, `ShowMyCustomModeViewController`).
5. Use your attribute over a view class. Ready!


## Sample please!
You can browse the code of the [Playground](https://github.com/MvvmCross/MvvmCross/tree/master/TestProjects/Playground) iOS project to see this presenter in action.
