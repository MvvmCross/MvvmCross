---
layout: documentation
title: Android View Presenter
category: Platforms
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

| Name | Type | Description |
| ---- | ---- | ----------- |
| Extras | `Bundle` | Use this `Bundle` to add any extra parameters to the Activity Intent. |

### MvxFragmentPresentationAttribute

A Fragment is hosted inside an Activity (or a fragment). By using this ViewPresenter, you can decide whether to make all of your screens Activities, or to use an Activity host and many Fragments inside of it. MvvmCross can handle both scenarios smoothly.

The ViewPresenter also supports nested fragments in one level: This means you can show fragments inside of a Fragment without extending any code!

Use this attribute over a Fragment view class and customize its presentation by using these properties:

| Name | Type | Description |
| ---- | ---- | ----------- |
|  ActivityHostViewModelType | `Type` | The __ViewModel type__ of the Activity that will be the host of your fragment. In case that Activity is not the current one in foreground, the ViewPresenter will show it before showing the fragment. Can be left empty only in case of fragments nesting. |
| FragmentHostViewType | `Type` | The __View type__ of the Fragment that will be the host of your fragment. Use this property only in case you want a fragment to be shown as nested. |
| FragmentContentId | `int` | Resource id where your fragment will be presented. |
| AddToBackStack | `bool` | Default value is `false`. If you set it to `true` the FragmentTransaction will be added to the backstack. |
| EnterAnimation | `int` | Resource id for the animation that will be run when the fragment is shown. |
| ExitAnimation | `int` | Resource id for the animation that will be run when the fragment is closed. |
| PopEnterAnimation | `int` | Resource id for the animation that will be run when the fragment comes back to foreground. |
| PopExitAnimation | `int` | Resource id for the animation that will be run when the fragment is retrieved from foreground. |
| TransitionStyle | `int` | In case you want to use a Transition Style, use this property by setting its resource id. |
| IsCacheableFragment | `bool` | Default value is false. You should leave it that way unless you really want/need to reuse a fragment view (for example, in case you are displaying a WebView, you might want to cache the already loaded URL). If it is set to `true`, the ViewPresenter will try to find a Fragment instance already present in the FragmentManager object before instantiating a new one and will reuse that object. |

When providing a value for EnterAnimation you need to provide one for ExitAnimation as well, otherwise the animation won't work (same applies in the other way around). 

Same as above, if you want to set a Pop animation, you will need to set four animation resources: EnterAnimation, ExitAnimation, PopEnterAnimation and PopExitAnimation. Otherwise the animation won't work.

### MvxDialogFragmentPresentationAttribute

This attribute extends `MvxFragmentPresentationAttribute`, which means you can use all the properties it provides to customize the presentation. Use this attribute over a FragmentDialog view class to display a dialog and take advantage of even more customization with this property:

| Name | Type | Description |
| ---- | ---- | ----------- |
| Cancelable | `bool` | Default value is `true`. This property indicates if the dialog can be canceled. |

### MvxViewPagerFragmentPresentationAttribute (AppCompat only)

This attribute extends `MvxFragmentPresentationAttribute`, which means you can use all the properties it provides to customize the presentation. use this attribute over a Fragment view class to display a fragment inside of a ViewPager and take advantage of even more customization with these properties:

| Name | Type | Description |
| ---- | ---- | ----------- |
| Title | `string` | Title for the ViewPager. It will also be used as Title for the TabLayout when using MvxTabLayoutPresentationAttribute. |
| ViewPagerResourceId | `int` | The resource id for the ViewPager that will be used as host. |

Note: If you intend to display your fragment in more than one host activity, please remember to set the property ActivityHostViewModelType on each attribute!

### MvxTabLayoutPresentationAttribute (AppCompat only)

This attribute extends `MvxViewPagerFragmentPresentationAttribute`, which means you can use all the properties it provides to customize the presentation. use this attribute over a Fragment view class to display a fragment inside of a ViewPager with TabLayout and take advantage of even more customization with this property:

| Name | Type | Description |
| ---- | ---- | ----------- |
| TabLayoutResourceId | `int` | The resource id for the TabLayout that will be used. |

Note: If you intend to display your fragment in more than one host activity, please remember to set the property ActivityHostViewModelType on each attribute!

## Views without attributes: Default values

- If a view class has no attribute over it, the presenter will check the type and try to create the correct attribute for it:

- Activity -> `MvxActivityPresentationAttribute`
- Fragment -> `MvxFragmentPresentationAttribute`
- DialogFragment -> `MvxDialogFragmentPresentationAttribute`

## Override a presentation attribute at runtime

To override a presentation attribute at runtime you can implement the `IMvxOverridePresentationAttribute` in your view and determine the presentation attribute in the `PresentationAttribute` method like this:

```c#
public MvxBasePresentationAttribute PresentationAttribute(MvxViewModelRequest request)
{
    return new MvxFragmentPresentationAttribute()
    {
        
    };
}
```

As you can see in the code snippet, you will be able to make your choice using a `MvxViewModelRequest`. This object will contain the `PresentationValues` dictionary alongside other properties. 

If you return `null` from the `PresentationAttribute` method, the ViewPresenter will fallback to the attribute used to decorate the view. If the view is not decorated with any presentation attribute, then it will use the default attribute instead.

__Hint:__ Be aware that `this.ViewModel` property will be null during `PresentationAttribute`. If you want to have the ViewModel instance available, you need to use the `MvxNavigationService` and cast the `request` parameter to `MvxViewModelInstanceRequest`.

## Shared Element transitions

To add a Shared Element transition navigating to an Activity or Fragment implement the `IMvxAndroidSharedElements` in your hosting Activity. You can then use `FetchSharedElementsToAnimate` to assign the view elements you want to transition.

```C#
public IDictionary<string, View> FetchSharedElementsToAnimate(MvxBasePresentationAttribute attribute, MvxViewModelRequest request)
{
    IDictionary<string, View> sharedElements = new Dictionary<string, View>();

    if (request.ViewModelType == typeof(SharedElementSecondChildViewModel))
    {
        var selectedLogo = FindViewById<ImageView>(Resource.Id.img_logo);
        sharedElements.Add("identifier", selectedLogo);
    }

    return sharedElements;
}
```

`FetchSharedElementsToAnimate` provides access to the `MvxViewModelRequest` used for the navigation as well as the attributed used by the view navigating to. The dictionary that is returned must contain an `identifier` as well as the view element to transition.

__Hint:__ Be aware that you must have already specified the transition name on the view elements you add to the dictionary.

The view you are navigating to will receive a `Bundle` that contains additional metadata. Being the identifier as well as the transition name. MvvmCross provides an extension method to assist with extracting this metadata, `GetSharedElementTransitionNames` , which can be used to get identifier and transition name pairs.

```C#
static IDictionary<string, string> GetSharedElementTransitionNames(this Bundle bundle)
```

Furthermore, if the identifier you passed through is the Android resource id name or Android resource tag. You can make use of the following extension method to find the corresponding view element and apply the transition name if required.

```C#
static void SetSharedElementsByTag(this Bundle bundle, View view)

static void SetSharedElementsById(this Bundle bundle, View view)
```

You can also check out the Playground Shared Elements sample.

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

To get more control over your Fragment lifecycle (or activity) and transitions, you can override the following methods. You can also modify your fragment transitions :

__MvxAndroidViewPresenter__

```c#
void OnBeforeFragmentChanging(FragmentTransaction ft, Fragment fragment, MvxFragmentPresentationAttribute attribute, MvxViewModelRequest request)

void OnFragmentChanged(FragmentTransaction ft, Fragment fragment, MvxFragmentPresentationAttribute attribute, MvxViewModelRequest request)

void OnFragmentChanging(FragmentTransaction ft, Fragment fragment, MvxFragmentPresentationAttribute attribute, MvxViewModelRequest request)

void OnFragmentPopped(FragmentTransaction ft, Fragment fragment, MvxFragmentPresentationAttribute attribute)
```

__MvxAppCompatViewPresenter__

```c#
void OnBeforeFragmentChanging(FragmentTransaction ft, Fragment fragment, MvxFragmentPresentationAttribute attribute, MvxViewModelRequest request)

void OnFragmentChanged(FragmentTransaction ft, Fragment fragment, MvxFragmentPresentationAttribute attribute, MvxViewModelRequest request)
        
void OnFragmentChanging(FragmentTransaction ft, Fragment fragment, MvxFragmentPresentationAttribute attribute, MvxViewModelRequest request)
		
void OnFragmentPopped(FragmentTransaction ft, Fragment fragment, MvxFragmentPresentationAttribute attribute)

```

### Activity Transitions

In case you want to override the transition options bundle pasted between 2 activities you can override the following method :

```c#

Bundle CreateActivityTransitionOptions(Intent intent, MvxActivityPresentationAttribute attribute, MvxViewModelRequest request)
```

## Sample please!
You can browse the code of the [Playground](https://github.com/MvvmCross/MvvmCross/tree/master/TestProjects/Playground) Android project to see this presenter in action.
