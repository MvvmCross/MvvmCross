---
layout: documentation
title: Android View Presenter
category: Presenters
---

## View Presenter Overview

The default presenter for Android named `MvxAndroidViewPresenter` or `MvxAppCompatViewPresenter` when using the Android AppCompat support library offers out of the box support for the following navigation patterns / strategies:

- Fragments (Nested)
- Activity's
- Dialogs
- Tabs / ViewPager

Navigation patterns that should be easy to implement with this are:

- NavigationDrawer
- BottomNavigationBar
- BottomSheetDialog
- Master/Detail Flows
- Nested navigation

Also if your app needs another kind of presentation mode, you can easily extend it!

## Presentation Attributes

The presenter uses a set of `PresentationAttributes` to define how a view will be displayed. The existing attributes are:

### MvxActivityPresentationAttribute

Used as root of the App and root for other layouts like Fragments.

### MvxDialogFragmentPresentationAttribute

Extends `MvxFragmentPresentationAttribute` and shows it in a `Dialog`

### MvxFragmentPresentationAttribute

Hosted inside an Activity.

### MvxTabLayoutPresentationAttribute

Extends `MvxFragmentPresentationAttribute` and puts the Fragment into a ViewPager with TabLayout.

### MvxViewPagerFragmentPresentationAttribute

Extends `MvxFragmentPresentationAttribute` and puts the Fragment into a ViewPager.

## Views without attributes: Default values

- If a view class has no attribute over it, the presenter will check the type and try to create the correct attribute for it.

## Override a presentation attribute at runtime

To override a presentation attribute at runtime you can implement the `IMvxOverridePresentationAttribute` in your view and determine the presentation attribute in the `PresentationAttribute` method like this:

```c#
public MvxBasePresentationAttribute PresentationAttribute()
{
    return new MvxFragmentPresentationAttribute()
    {
        
    };
}
```

If you return `null` from the `PresentationAttribute` the View Presenter will fallback to the attribute used to decorate the view. If the view is not decorated with a presentation attribute it will use the default presentation attribute.

__Note:__ Be aware that your ViewModel will be null during `PresentationAttribute`, so the logic you can perform there is limited here. Reason to this limitation is MvvmCross Presenters are stateless, you can't connect an already instantiated ViewModel with a new View.

## Extensibility
The presenter is completely extensible! You can override any attribute and customize attribute members.

You can also define new attributes to satisfy your needs. The steps to do so are:

1. Add a new attribute that extends `MvxBasePresentationAttribute`
2. Subclass `MvxAppCompatViewPresenter` and make it the presenter of your application in Setup.cs (by overriding the method `CreatePresenter`).
3. Override the method `RegisterAttributeTypes` and add a registry to the dictionary like this:

```c#
_attributeTypesToShowMethodDictionary.Add(
    typeof(MyCustomModePresentationAttribute),
    (type, attribute, request) => ShowMyCustomModeView(type, (MyCustomPresentationAttribute)attribute, request));
```

4. Implement a method that takes care of the presentation mode (in the example above, `ShowMyCustomModeView`).
5. Use your attribute over a view class. Ready!


## Sample please!
You can browse the code of the [Playground](https://github.com/MvvmCross/MvvmCross/tree/master/TestProjects/Playground) Android project to see this presenter in action.
