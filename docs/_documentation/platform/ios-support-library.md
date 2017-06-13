---
layout: documentation
title: iOS Support Library
category: Platform specifics
---

In version 4.0.0 we have started work on a new library dedicated to providing further classes and extended functionality targeting the iOS platform.

Currently the classes available in this library consists of:

 * MvxSidebarPresenter
 * MvxBaseViewController
 * MvxExpandableTableViewSource

## MvxSidebarPresenter

This presenter provides 3 panels as view "targets", a main central panel, a right side panel and a left side panel. Where views appear in the UI and how they are shown is controlled through the decoration of a view controller using  class level attribute.

A view controller class can be decorated with the MvxSidebarPresentationAttribute. The constructor for this attribute is shown below:
```c#
public MvxSidebarPresentationAttribute(
    MvxPanelEnum panel, MvxPanelHintType hintType, bool showPanel,
    MvxSplitViewBehaviour behaviour = MvxSplitViewBehaviour.None)
{
}
```
This attribute is used by the presenter to make decisions about what to do with the view request when showing this view using the syntax shown below:
```c#
[Register("CenterPanelView")]
[MvxSidebarPresentation(
     MvxPanelEnum.Center, MvxPanelHintType.ActivePanel, true)]
public class CenterPanelView
    : BaseViewController<CenterPanelViewModel>
{
}
```

So to explain this example it's telling the MvxSidebarPresenter that this view controller wants to be displayed in the center panel, set as the active panel and it also wants to be shown immediately.

If this was using MvxPanelEnum.Left for instance this would be shown in the left hand panel and would also immediately slide the left panel into view.

If the last value set in the attribute was set to false this would simply add the view to the left hand panel but leave the view hidden until the user performed some action in the UI that would result in that panel being shown.

## MvxSplitViewBehaviour

The MvxSidePanelsPresenter also contains a feature to allow this use of UISplitViewControllers "inline" with normal view controllers.  The presenter will automatically detect if the application is running on a large screen device (iPad) and provided the attribute details some behaviour other than None will respond accordingly.

in a "normal" application view flow where a table of data is being displayed to the user it is fairly normal that there is some form of a child view that would show more details to the user in response to a users UI Touch on the table view.  In these cases on an small screen device (iPhone, iPad etc.) this would often result in a further navigation to a new view that shows the request content.

However on devices with large screens this would often be displayed to the user on the same screen without the need for a navigation to a new view.  The data is then in a form of Master / Detail arrangement where both the Master (often a table view) and the Detail (the child view) will be displayed on the same screen.  A user touch on the master view results in the view hosted in the Detail view is then updated.

The issue this behaviour deals with is that often iOS application will only user split views as the root view controller, this gets around this issue by allowing a split view to be shown as part of the normal flow of views without the need for explicitly setting this view as the root view in the view hierarchy.

You can see an example of this below:

```c#
[Register("MasterView")]
[MvxPanelPresentation(
     MvxPanelEnum.Center, MvxPanelHintType.ActivePanel,
     true, MvxSplitViewBehaviour.Master)]
public class MasterView
    : BaseViewController<MasterViewModel>
{
}

[Register("DetailView")]
[MvxPanelPresentation(
     MvxPanelEnum.Center, MvxPanelHintType.ActivePanel,
     true, MvxSplitViewBehaviour.Detail)]
public class DetailView
    : BaseViewController<DetailViewModel>
{
}
```

So when the presenter receives a request to show the master view **and** the application is running on a large screen device the presenter will respond to that request by constructing a number of controllers to manage the hosting of the split view controller and will then ultimately show the view in the appropriate section of the spit view.

Subsequent touches on the UI master view which result in a request for view controller that requests to be shown as a detail view will then be shown in this same view (with no navigation occurring) but placed in the detail panel of the same instance of the split view created when the user navigated to the master view.

Confused? See the [demo applications](https://github.com/MvvmCross/MvvmCross/tree/develop/TestProjects/iOS-Support)

## MvxBaseViewController

In addition to the core MvvmCross view controller classes (MvxViewController) we have added a slightly expanded feature set to a new abstract base class called MvxBaseViewController.

The idea behind this class is to gradually build up some "extended core" features that most developers will use without over-burdening  the class with too much extraneous stuff.

At the moment this class is a generic view controller that has currently only one major feature over and above the core "basic" view controller - automatic keyboard handling.

The feature requires there to be a UIScrollView in the view hierarchy in order to function.  It will detect a touch on a UIView that also expands the keyboard, it will then ensure that the view with focus is not obscured by the keyboard and is centered in the applications UI.  It can also optionally hide the keyboard when the user makes any further touches that moves focus away from the edit view.

You can make use of this class using the following standard inheritance syntax:

```c#
[Register("KeyboardHandlingView")]
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

