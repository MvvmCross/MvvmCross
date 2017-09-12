---
layout: documentation
title: WPF View Presenter
category: Presenters
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

- `Identifier`: Window identifier, used to identify the window for other attributes. If an identifier is not provided by the developer, it will be set to the name of the view class.
- `Modal`: How to show window, if true the window will show by `ShowDialog` method. If false the window will show by `Show` method.

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

- `WindowIdentifier`: You can choose in which window should this view be displayed by using this property.If the identifier is not provided, the view will be displayed in the last opened window.
- `StackNavigation`: The view class can decide if wants to be displayed with stack navigation or non-stack navigation. The default is true.


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
public partial class WindowView :
    MvxWindow<WindowViewModel>,
    IMvxOverridePresentationAttribute
{
    public WindowView() => InitializeComponent();

    // Override a presentation attribute at runtime
    public MvxBasePresentationAttribute PresentationAttribute() =>
        new MvxWindowPresentationAttribute
        {
            Identifier = $"{nameof(WindowView)}.{ViewModel.Count}"
        };
}
```

If you return `null` from the `PresentationAttribute`, the presenter will assume presentation the same way as views without attributes.


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

        protected override IMvxApplication CreateApp()
        {
            return new Core.App();
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
            _attributeTypesToShowMethodDictionary.Add(
                typeof(MyCustomModePresentationAttribute),
                (element, attribute, request) =>
                    ShowMyCustomModeView(element, (MyCustomPresentationAttribute)attribute, request));
        }
    }
    ```
4. Implement a method that takes care of the presentation mode (in the example above, `ShowMyCustomModeView`).
5. Use your attribute over a view class. Ready!



## Sample please!
You can browse the code of the [Playground](https://github.com/MvvmCross/MvvmCross/tree/master/TestProjects/Playground) WPF project to see this presenter in action.
