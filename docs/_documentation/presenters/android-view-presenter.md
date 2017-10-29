---
layout: documentation
title: Android View Presenter
category: Presenters
---

## View Presenter Overview

The default presenter for Android named `MvxAndroidViewPresenter` or `MvxAppCompatViewPresenter` when using the Android AppCompat support library offers out of the box support for the following navigation patterns / strategies:

- Activities
- Fragments
- Nested Fragments
- Dialog Fragments
- TabLayout / ViewPager

Navigation patterns that should be easy to implement with this are:

- NavigationDrawer
- BottomNavigationBar (single backstack)
- BottomSheetDialog
- Master/Detail Flows
- Nested navigation

Also if your app needs another kind of presentation mode, you can easily extend it!

## Presentation Attributes

The presenter uses a set of `PresentationAttributes` to define how a view will be displayed. The existing attributes are:

### MvxActivityPresentationAttribute

Use this attribute if you want to display an Activity in your application. An Activity will be the root of your application and can also act as a host for fragments. Alongside the attribute, your view can customize the presentation by using these attribute properties:

- Extras: Use this `Bundle` to add any extra parameters to the Activity Intent.
- SharedElements: Consists on a `IDictionary<string, View>` that you can use to add shared view elements to the transition. When using the AppCompat version, the string keys are not relevant.

### MvxFragmentPresentationAttribute

A Fragment is hosted inside an Activity (or a fragment). By using this ViewPresenter, you can decide whether to make all of your screens Activities, or to use an Activity host and many Fragments inside of it. MvvmCross can handle both scenarios smoothly.

The ViewPresenter supports also nested fragments in one level: This means you can show fragments inside of a Fragment without extending any code!

Use this attribute over a Fragment view class and customize its presentation by using these properties:

-  ActivityHostViewModelType: The __ViewModel type__ of the Activity that will be the host of your fragment. In case that Activity is not the current one in foreground, the ViewPresenter will show it before showing the fragment. Can be left empty only in case of fragments nesting.
- FragmentHostViewType: The __View type__ of the Fragment that will be the host of your fragment. Use this property only in case you want a fragment to be shown as nested.
- FragmentContentId: Resource id where your fragment will be presented.
- AddToBackStack: Default value is `false`. If you set it to `true` the FragmentTransaction will be added to the backstack.
- EnterAnimation: Resource id for the animation that will be run when the fragment is shown.
- ExitAnimation Resource id for the animation that will be run when the fragment is closed.
- PopEnterAnimation: Resource id for the animation that will be run when the fragment comes back to foreground.
- PopExitAnimation: Resource id for the animation that will be run when the fragment is retrieved from foreground.
- TransitionStyle: In case you want to use a Transition Style, use this property by setting its resource id.
- SharedElements: Consists on a `IDictionary<string, View>` that you can use to add shared view elements to the transition. When using the AppCompat version, the string keys are not relevant.
- IsCacheableFragment: Default value is false. You should leave it that way unless you really want/need to reuse a fragment view (for example, in case you are displaying a WebView, you might want to cache the already loaded URL). If it is set to `true`, the ViewPresenter will try to find a Fragment instance already present in the FragmentManager object before instantiating a new one and will reuse that object. 

When providing a value for EnterAnimation you need to provide one for ExitAnimation as well, otherwise the animation won't work (same applies in the other way around). 

Same as above, if you want to set a Pop animation, you will need to set four animation resources: EnterAnimation, ExitAnimation, PopEnterAnimation and PopExitAnimation. Otherwise the animation won't work.

### MvxDialogFragmentPresentationAttribute

This attribute extends `MvxFragmentPresentationAttribute`, which means you can use all the properties it provides to customize the presentation. Use this attribute over a FragmentDialog view class to display a dialog and take advantage of even more customization with this property:

- Cancelable: Default value is `true`. This property indicates if the dialog can be canceled.

### MvxViewPagerFragmentPresentationAttribute (AppCompat only)

This attribute extends `MvxFragmentPresentationAttribute`, which means you can use all the properties it provides to customize the presentation. use this attribute over a Fragment view class to display a fragment inside of a ViewPager and take advantage of even more customization with these properties:

- Title: Title for the ViewPager. It will also be used as Title for the TabLayout when using MvxTabLayoutPresentationAttribute.
- ViewPagerResourceId: The resource id for the ViewPager that will be used as host.

Note: If you intend to display your fragment in more than one host activity, please remember to set the property ActivityHostViewModelType on each attribute!

### MvxTabLayoutPresentationAttribute (AppCompat only)

This attribute extends `MvxViewPagerFragmentPresentationAttribute`, which means you can use all the properties it provides to customize the presentation. use this attribute over a Fragment view class to display a fragment inside of a ViewPager with TabLayout and take advantage of even more customization with this property:

- TabLayoutResourceId: The resource id for the TabLayout that will be used.

Note: If you intend to display your fragment in more than one host activity, please remember to set the property ActivityHostViewModelType on each attribute!

## Views without attributes: Default values

- If a view class has no attribute over it, the presenter will check the type and try to create the correct attribute for it:

- Activity -> `MvxActivityPresentationAttribute`
- Fragment -> `MvxFragmentPresentationAttribute`
- DialogFragment -> `MvxDialogFragmentPresentationAttribute`

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

### Attributes
The presenter is completely extensible! You can override any attribute and customize attribute members.

You can also define new attributes to satisfy your needs. The steps to do so are:

1. Add a new attribute that extends `MvxBasePresentationAttribute`
2. Subclass `MvxAndroidViewPresenter` or `MvxAppCompatViewPresenter` and make it the presenter of your application in Setup.cs (by overriding the method `CreatePresenter`).
3. Override the method `RegisterAttributeTypes` and add a registry to the dictionary like this:

```c#
_attributeTypesToShowMethodDictionary.Add(
    typeof(MyCustomModePresentationAttribute),
    new MvxPresentationAttributeAction
    {
        ShowAction = (view, attribute, request) => ShowMyCustomModeView(view, (MyCustomPresentationAttribute)attribute, request),
        CloseAction = (viewModel, attribute) => CloseMyCustomModeView(viewModel, (MyCustomPresentationAttribute)attribute)
    });
```

4. Implement a method that takes care of the presentation mode (in the example above, `ShowMyCustomModeView`) and a method that takes care of a ViewModel closing (in the example above, `CloseMyCustomModeView`).
5. Use your attribute over a view class. Ready!


###  Fragment Lifecycle

To get more control over your Fragment lifecycle (or activity) and transitions, you can override the folling methods. You can also modify your fragment transitions :

__MvxAndroidViewPresenter__

```c#
void OnFragmentChanged(FragmentTransaction ft, Fragment fragment, MvxFragmentPresentationAttribute attribute)

void OnFragmentChanging(FragmentTransaction ft, Fragment fragment, MvxFragmentPresentationAttribute attribute)

void OnFragmentPopped(FragmentTransaction ft, Fragment fragment, MvxFragmentPresentationAttribute attribute)
```

__MvxAppCompatViewPresenter__

```c#
void OnFragmentChanged(FragmentTransaction ft, Fragment fragment, MvxFragmentPresentationAttribute attribute)
        
void OnFragmentChanging(FragmentTransaction ft, Fragment fragment, MvxFragmentPresentationAttribute attribute)
		
void OnFragmentPopped(FragmentTransaction ft, Fragment fragment, MvxFragmentPresentationAttribute attribute)

```

### Activity Transitions

In case you want to override the transition options between 2 activities you can override the folling method :

__MvxAppCompatViewPresenter__

```c#

ActivityOptionsCompat CreateActivityTransitionOptions(Android.Content.Intent intent,MvxActivityPresentationAttribute attribute)
```

## Sample please!
You can browse the code of the [Playground](https://github.com/MvvmCross/MvvmCross/tree/master/TestProjects/Playground) Android project to see this presenter in action.
