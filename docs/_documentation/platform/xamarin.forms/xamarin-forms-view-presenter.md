---
layout: documentation
title: Xamarin.Forms View Presenter
category: Platforms
---

## View Presenter Overview

On Forms every platform has it's own presenter that inherits from the native platform presenter. This enables us to navigate between native and Xamarin.Forms views. On top of that we have the `MvxFormsPagePresenter` which handles all the comon logic for Forms related navigation.

The default presenter supports every navigation pattern that Xamarin.Forms supports itself:

- Tabs
- MasterDetail
- Childs
- Etc

Also if your app needs another kind of presentation mode, you can easily extend it!

## Presentation Attributes

The presenter uses a set of `PresentationAttributes` to define how a view will be displayed. The existing attributes are:

### MvxPagePresentationAttribute

This is the base class for the other Forms presentation attributes and cannot be used directly.

| Name | Type | Description |
| ---- | ---- | ----------- |
| HostViewModelType | `Type` | Can optionally be set to make sure the Page has the correct Host type. |
| WrapInNavigationPage | `bool` | When you want the Page to be shown in a new NavigationPage or when the current Stack already is a NavigationPage push it inside there. This is true by default. |
| NoHistory | `bool` | Clears out the other pages from the navigation stack. False by default. |
| Animated | `bool` | Indicates if the navigation should be animated. This is true by default. |
| Title | `string` | The optional title of the Page |
| Icon | `string` | The optional Icon of a Page |

### MvxCarouselPagePresentationAttribute

Used to navigate forward to one or multipel pages in a Carousel.

| Name | Type | Description |
| ---- | ---- | ----------- |
| Position | `CarouselPosition` | Use this to set the position of the Page. It can either be the `Root` to host Pages, or as a `Carousel` to be a Page inside the carousel. |

### MvxContentPagePresentationAttribute

This is the standard Page attribute.

### MvxMasterDetailPagePresentationAttribute

| Name | Type | Description |
| ---- | ---- | ----------- |
| Position | `MasterDetailPosition` | Use this to set the position of the Page. It can either be the `Root` to host Pages, `Master` to be a Page inside the Master, or `Detail` to be as the content of the Page. |

### MvxModalPresentationAttribute

Will show the Page as a Modal.

### MvxNavigationPagePresentationAttribute

Used to indicate that this is a NavigationPage.

### MvxTabbedPagePresentationAttribute

| Name | Type | Description |
| ---- | ---- | ----------- |
| Position | `TabbedPosition` | Use this to set the position of the Page. It can either be the `Root` to host Pages, or as a `Tab` to be a Page inside the TabbedPage. |


## Views without attributes: Default values

- If a view class has no attribute over it, the presenter will check the type and try to create the correct attribute for it:

- CarouselPage -> `MvxCarouselPagePresentationAttribute`
- ContentPage -> `MvxContentPagePresentationAttribute`
- MasterDetailPage -> `MvxMasterDetailPagePresentationAttribute`
- NavigationPage -> `MvxNavigationPagePresentationAttribute`
- TabbedPage -> `MvxTabbedPagePresentationAttribute`

## Override a presentation attribute at runtime

To override a presentation attribute at runtime you can implement the `IMvxOverridePresentationAttribute` in your view and determine the presentation attribute in the `PresentationAttribute` method like this:

```c#
[MvxContentPagePresentation(WrapInNavigationPage = true, NoHistory = true, Title = "Sign In")]
public partial class LoginPage : MvxContentPage<LoginViewModel>, IMvxOverridePresentationAttribute
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
                    WrapInNavigationPage = true,
                    NoHistory = true
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