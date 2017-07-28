---
layout: documentation
title: Mac View Presenter
category: Platform specifics
---

Starting with MvvmCross 5, there is a new default Presenter for macOS Views, namely `MvxMacViewPresenter`.

## View Presenter Overview

The default presenter provided by MvvmCross works in a similar way as the default ViewPresenter for iOS does. Together with the ViewsContainer, the presenter supports Storyboards, .xibs and coded interfaces.

It offers out of the box support for the following navigation patterns / strategies:

- Multiple windows
- Content inside windows
- Modals
- Sheets
- Tabs

Also if your app needs another kind of presentation mode, you can easily extend it!

## Presentation Attributes

The presenter uses a set of `PresentationAttributes` to define how a view will be displayed. The existing attributes are:

### MvxWindowPresentationAttribute

Used to initiate a new _Window_. The first view of your app should use this attribute or any attribute (see defaults section).

You can customize how the window will look through the following properties:

- Identifier: Window identifier, used to identify the window for other attributes. If an identifier is not provided by the developer, it will be set to the name of the view class.
- WindowStyle: Used to set the NSWindowStyle. Default value is `NSWindowStyle.Closable | NSWindowStyle.Resizable | NSWindowStyle.Titled`.
- BufferingType: Used to set the NSBackingStore. Default value is `NSBackingStore.Buffered`.
- PositionX: float, default value is 200.
- PositionY: float, default value is 200.
- Width: float, default value is 600.
- Height: float, default value is 400.

### MvxContentPresentationAttribute

Used to set a view as content of a _Window_. Please notice that changing the content of a window does not automatically generate a navigation stack as it would be on iOS. Changing the content of a window dismisses the old content.
You can choose in which window should this view be displayed by using the `WindowIdentifier` property of the attribute. If the identifier is not provided, the view will be displayed in the last opened window.

### MvxModalPresentationAttribute

Used to display a view as _Modal_. Same as with content, you can choose through the `WindowIdentifier` property from which window should the modal be opened. If the identifier is not provided, the view will be displayed in the last opened window.

### MvxSheetPresentationAttribute

MacOS uses the concept of _sheets_ to prompt the user with messages or small forms. If you use this attribute over a view class, it will be displayed as a sheet. You can choose from which window should it be opened through the `WindowIdentifier` property. If the identifier is not provided, the view will be displayed in the last opened window.

### MvxTabPresentationAttribute

This attribute is only useful (and should only be used) when the current _Content_ of a Window is a `IMvxTabViewController`.
By using it over a view class, the presenter will show the view as a _Tab_ inside the TabViewController.

The presentation can be customized through this properties:

- TabTitle: Defines the title of the tab that will be displayed in the segmented control for tabs.
- WindowIdentifier: identifier for the window where the view should be displayed.

## Views without attributes: Default values

- If a view class has no attribute over it, the presenter will assume a _new window_ presentation.

## Override a presentation attribute at runtime

To override a presentation attribute at runtime you can implement the `IMvxOverridePresentationAttribute` in your ViewController and determine the presentation attribute in the `PresentationAttribute` method like this:
```c#
public MvxBasePresentationAttribute PresentationAttribute()
{
    return new MvxModalPresentationAttribute
    {
        WindowIdentifier = "MyWindow"
    };
}
```

If you return `null` from the `PresentationAttribute` the Mac View Presenter will fallback to the attribute used to decorate the view controller. If the view controller is not decorated with a presentation attribute it will use the default presentation attribute (a new window).

__Note:__ Be aware that your ViewModel will be null during `PresentationAttribute`, so the logic you can perform there is limited here. Reason to this limitation is MvvmCross Presenters are stateless, you can't connect an already instantiated ViewModel with a new View.

## Extensibility
The presenter is completely extensible! You can override any attribute and customize attribute members.

You can also define new attributes to satisfy your needs. The steps to do so are:

1. Add a new attribute that extends `MvxBasePresentationAttribute`
2. Subclass MvxMacViewPresenter and make it the presenter of your application in Setup.cs (by overriding the method `CreatePresenter`).
3. Override the method `RegisterAttributeTypes` and add a registry to the dictionary like this:

```c#
_attributeTypesToShowMethodDictionary.Add(
    typeof(MyCustomModePresentationAttribute),
    (vc, attribute, request) => ShowMyCustomModeViewController(vc, (MyCustomPresentationAttribute)attribute, request));
```

4. Implement a method that takes care of the presentation mode (in the example above, `ShowMyCustomModeViewController`).
5. Use your attribute over a view class. Ready!


## Sample please!
You can browse the code of the [Playground](https://github.com/MvvmCross/MvvmCross/tree/master/TestProjects/Playground) macOS project to see this presenter in action.
