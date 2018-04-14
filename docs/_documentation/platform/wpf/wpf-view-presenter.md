---
layout: documentation
title: WPF View Presenter
category: Platforms
---

## View Presenter Overview

The default presenter for WPF named `MvxWpfViewPresenter` offers out of the box support for the following navigation patterns / strategies:

- Single window and Multiple windows app
- Content inside window with stack/non-stack navigation
- Modal and modal-less window

Also if your app needs another kind of presentation mode, you can easily extend it!

## Presentation Attributes

The presenter uses a set of `MvxBasePresentationAttribute` to define how a view will be displayed. The existing attributes are:

### MvxWindowPresentationAttribute

Used to a _Window_. 

The view can be one of the following types:

- `MvxWindowView` (that inherits `Window` and implements `IMvxWindowView`)
- `MvxWpfView` (that inherits `UserControl` and implements `IMvxWpfView`)


If the view class isn't `Window`, wrap in a window by the presenter. 

You can customize how the window will look through the following properties:

| Name | Type | Description |
| ---- | ---- | ----------- |
| Identifier | `string` | Window identifier, used to identify the window for other attributes. If an identifier is not provided by the developer, it will be set to the name of the view class. |
| Modal | `bool` | How to show window, if true the window will show by `ShowDialog` method. If false the window will show by `Show` method. |

This view with this attribute like this:
```c#
[MvxWindowPresentation(Identifier = nameof(MainWindow), Modal = false)]
public partial class MainWindow : MvxWindow<MainWindow>
{
    public MainWindow() => InitializeComponent();
}
```

### MvxContentPresentationAttribute

Used to set a view as a _content_. The view that will be displayed as a `Content` object in a `Window`. The view class that will be displayed with stack navigation by default.

The view can be one of the following types:

- `MvxWpfView` (that inherits `UserControl` and implements `IMvxWpfView`)

You can customize how the content will look through the following properties:

| Name | Type | Description |
| ---- | ---- | ----------- |
| WindowIdentifier | `string` | You can choose in which window should this view be displayed by using this property.If the identifier is not provided, the view will be displayed in the last opened window. |
| StackNavigation | `bool` | The view class can decide if wants to be displayed with stack navigation or non-stack navigation. The default is true. |


The view with this attribute like this:
```c#
[MvxContentPresentation(WindowIdentifier = nameof(MainWindow), StackNavigation = false)]
public partial class ChildView : MvxWpfView<ChildViewModel>
{
    public ChildView() => InitializeComponent();
}
```


## Views without attributes: Default values

If a `Window` class has no attribute over it, the presenter will assume _Window_ presentation.  Other classes are assumed _Content_ presentation.


## Override a presentation attribute at runtime

To override a presentation attribute at runtime you can implement the `IMvxOverridePresentationAttribute` in your view and determine the presentation attribute in the `PresentationAttribute` method like this:

```c#
public MvxBasePresentationAttribute PresentationAttribute(MvxViewModelRequest request)
{
    return new MvxWindowPresentationAttribute
    {
        Identifier = $"{nameof(WindowView)}.{ViewModel.Count}"
    };
}
```

As you can see in the code snippet, you will be able to make your choice using a `MvxViewModelRequest`. This object will contain the `PresentationValues` dictionary alongside other properties. 

If you return `null` from the `PresentationAttribute` method, the ViewPresenter will fallback to the attribute used to decorate the view. If the view is not decorated with any presentation attribute, then it will use the default attribute instead.

__Hint:__ Be aware that `this.ViewModel` property will be null during `PresentationAttribute`. If you want to have the ViewModel instance available, you need to use the `MvxNavigationService` and cast the `request` parameter to `MvxViewModelInstanceRequest`.


## Extensibility
The presenter is completely extensible! You can override any attribute and customize attribute members.

You can also define new attributes to satisfy your needs. The steps to do so are:

1. Add a new attribute that extends `MvxBasePresentationAttribute`
    ```c#
    public class MyCustomModePresentationAttribute : MvxBasePresentationAttribute
    {
        public int MyProperty { get; set; }
    }
    ```
2. Subclass `MvxWpfViewPresenter` and make it the presenter of your application in Setup.cs (by overriding the method `CreateViewPresenter`).
    ```c#
    public class Setup : MvxWpfSetup
    {
        public Setup(Dispatcher uiThreadDispatcher, ContentControl root) : base(uiThreadDispatcher, root)
        {
        }

        protected override IMvxWpfViewPresenter CreateViewPresenter(ContentControl root)
        {
            return new MyCustomViewPresenter(root);
        }
    }
    ```
3. Override the method `RegisterAttributeTypes` and add a registry to the dictionary like this:
    ```c#
    public class MyCustomViewPresenter
            : MvxWpfViewPresenter
    {
        protected virtual void RegisterAttributeTypes()
        {
			AttributeTypesToActionsDictionary.Add(
                typeof(MyCustomModePresentationAttribute),
                new MyCustomModePresentationAttribute
                {
                    ShowAction = (viewType, attribute, request) =>
                    {
                        var view = WpfViewLoader.CreateView(request);
                        ShowWindow(view, (MyCustomModePresentationAttribute)attribute, request);
                    },
                    CloseAction = (viewModel, attribute) => CloseWindow(viewModel)
                });
        }
    }
    ```
4. Implement a method that takes care of the presentation mode (in the example above, `ShowMyCustomModeView`).
5. Use your attribute over a view class. Ready!



## Sample please!
You can browse the code of the [Playground](https://github.com/MvvmCross/MvvmCross/tree/master/TestProjects/Playground) WPF project to see this presenter in action.
