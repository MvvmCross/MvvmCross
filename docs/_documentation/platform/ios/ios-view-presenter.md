---
layout: documentation
title: iOS View Presenter
category: Platforms
---

## View Presenter Overview

The default presenter for iOS named `MvxIosViewPresenter` offers out of the box support for the following navigation patterns / strategies:

- Stack navigation
- Tabs
- SplitView
- Modal
- Modal navigation
- Nested modals

Also if your app needs another kind of presentation mode, you can easily extend it!

## Presentation Attributes

The presenter uses a set of `PresentationAttributes` to define how a view will be displayed. The existing attributes are:

### MvxRootPresentationAttribute
Used to set a view as _Root_. You should use this attribute over the view class that will be the root of your application (your app can have several root views, one at a time).
The view root can be one of the following types:

- `MvxViewController`
- `MvxTabBarViewController` (actually implementing `IMvxTabBarViewController` is enough)
- `MvxSplitViewController` (actually implementing `IMvxSplitViewController` is enough)

If you want to initiate a stack navigation, just set the attribute member `WrapInNavigationController` to true.


### MvxChildPresentationAttribute
Used to set a view as a _child_. You should use this attribute over a view class that will be displayed inside a navigation stack (modal or not).
The view class can decide if wants to be displayed animated or not through the attribute member `Animated` (the default value is `true`).
If your app uses a TabBarController as a child ViewController of a "master" NavigationController, you can decide whether to display a new child ViewController inside a Tab of the TabBarViewController (assuming that Tab  is a NavigationController) or to display it as a child of the "master" NavigationController. You can take control of this behavior by overriding `MvxTabBarController.ShowChildView`.


### MvxModalPresentationAttribute
Used to display a view as _Modal_. You should use this attribute over a view class to present the view as a modal.
There are several attribute members that the view class can customize:

| Name | Type | Description |
| ---- | ---- | ----------- |
| WrapInNavigationController | `bool` | If set to `true`, a modal navigation stack will be initiated (following child presentations will be displayed inside the modal stack). The default value is `false`. |
| ModalPresentationStyle | `UIModalPresentationStyle` | Corresponds to the `ModalPresentationStyle` property of UIViewController. The default value is `UIModalPresentationStyle.FullScreen`. |
| ModalTransitionStyle | `UIModalTransitionStyle` | Corresponds to the `ModalTransitionStyle` property of UIViewController. The default value is `UIModalTransitionStyle.CoverVertical`. |
| PreferredContentSize | `CGSize` | Corresponds to the `PreferredContentSize` property of UIViewController. The property works for iPad only. |
| Animated | `bool` | If set to true, the presentation will be animated. The default value is `true`. |


### MvxTabPresentationAttribute
This attribute is only useful (and should only be used) when the current _Root_ view is a `IMvxTabBarViewController`.
By using it over a view class, the presenter will show the view as a _Tab_ inside the TabBarController.
The presentation can be highly customized through this attribute members:

| Name | Type | Description |
| ---- | ---- | ----------- |
| TabName | `string` | Defines the title of the tab that will be displayed below the tab icon. It has to be a magic string, but it can be for example a key to a localized string that you can grab overriding the method `SetTitleAndTabBarItem` in your TabBarController. |
| TabIconName | `string` | Defines the name of the resource that will be used as icon for the tab. It also has to be a magic string, but same as before, your app can take control of what happens by overriding the method `SetTitleAndTabBarItem` in your TabBarController. |
| TabSelectedIconName | `string` | Defines the name of the resource that will be used as icon for the tab when it becomes selected. It also has to be a magic string, your app can take control of what happens by overriding the method `SetTitleAndTabBarItem` in your TabBarController. |
| WrapInNavigationController | `bool` | If set to `true`, the view will be wrapped in a `MvxNavigationController`, which will allow the tab to have its own navigation stack. **Important note**: When the current _Root_ is a TabBarController and there is no current modal navigation stack, child presentations will be tried to be displayed in the current selected _Tab_. |
| TabAccessibilityIdentifier | `string` |Corresponds to the UIViewController.View `AccessibilityIdentifier` property. |

### MvxSplitViewPresentationAttribute
This attribute is only useful (and should only be used) when the current _Root_ view is a `IMvxSplitViewController`.
The attribute can be used to set the master and detail views of a SplitView.

There is an attribute member that can be used to customize the presentation:

| Name | Type | Description |
| ---- | ---- | ----------- |
| WrapInNavigationController | `bool` | If set to `true`, the view will be displayed wrapped in a `MvxNavigationController`, which will allow you to set a title, which is the most common scenario of SplitView. The default value is therefore `true` on detail and `false` on master. |
| Position | `MasterDetailPosition` | Can be set to `Master` to show as the root of the SplitView. The default value is `Detail` and this will push the viewcontroller onto the detail stack. |

## Views without attributes: Default values

- If the initial view class of your app has no attribute over it, the presenter will assume stack navigation and will wrap your initial view in a `MvxNavigationController`.
- If a view class has no attribute over it, the presenter will assume _animated_ child presentation and will display the view in the current navigation stack (could be modal or not).


## Override a presentation attribute at runtime

To override a presentation attribute at runtime you can implement the `IMvxOverridePresentationAttribute` in your view controller and determine the presentation attribute in the `PresentationAttribute` method like this:

```c#
[MvxFromStoryboard("Main")]
public partial class LoginView : MvxViewController<LoginViewModel>, IMvxOverridePresentationAttribute
{

    public MvxBasePresentationAttribute PresentationAttribute(MvxViewModelRequest request)
    {
        if (request.PresentationValues != null)
        {
            if (request.PresentationValues.ContainsKey("NavigationMode") &&
                request.PresentationValues["NavigationMode"] == "Modal")
            {
                return new MvxModalPresentationAttribute
                {
                    WrapInNavigationController = true,
                    ModalPresentationStyle = UIModalPresentationStyle.OverFullScreen,
                    ModalTransitionStyle = UIModalTransitionStyle.CrossDissolv
                };
            }
        }

        return null;
    }
}
```

As you can see in the code snippet, you will be able to make your choice using a `MvxViewModelRequest`. This object will contain the `PresentationValues` dictionary alongside other properties. This way your ViewModel can let the presentation (the view) know of a custom case in which it should be opened.

If you return `null` from the `PresentationAttribute` method, the ViewPresenter will fallback to the attribute used to decorate the view. If the view is not decorated with any presentation attribute, then it will use the default attribute instead.

__Hint:__ Be aware that `this.ViewModel` property will be null during `PresentationAttribute`. If you want to have the ViewModel instance available, you need to use the `MvxNavigationService` and cast the `request` parameter to `MvxViewModelInstanceRequest`.

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

You can browse the code of the [Playground](https://github.com/MvvmCross/MvvmCross/tree/master/Projects/Playground) (iOS project) to see this presenter in action.
