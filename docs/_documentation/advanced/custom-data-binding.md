---
layout: documentation
title: Custom Data Binding
category: Advanced
order: 3
---

This article descibes how to add your own definitions for binding custom views or views, which MvvmCross does not support out of the box in `TwoWay` mode. Or for views that require extra parameters when needing to set them, or if you want to add bindings for views that expose no properties etc.
In order to do so, you will need to write a Target Binding class which descibes how to bind a specific property and in case of `TwoWay` bindings, which event to listen to value changes on.

Typically on iOS and Android a property will have an event indicating it changed. This is a bit different from Windows and Xamarin.Forms, where you normally will have a `DependencyProperty` or `BindableProperty` which describes how to both get and set and how to react to updates to that property. These properties and the responsibility to implement these on those platforms are up to the View itself. In MvvmCross, we have these descriptions outside of the View, meaning in a lot of cases, no modifications are needed in order to add descriptions on how to bind a View.

### Adding A Target Binding

Let us start by making a couple of assumptions. We are binding a View called `MyView`, we want to bind to `MyView`'s public property `MyProperty`, of type `string` which has both a getter and a setter. The View also has a `MyPropertyChanged` event, which fires when someone sets the `MyProperty`. With this information we can now define a Target Binding.

```c#
public class MyViewMyPropertyTargetBinding
    : MvxPropertyInfoTargetBinding<MyView>
{
    // used to figure out whether a subscription to MyPropertyChanged
    // has been made
    private bool _subscribed;

    public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

    public MyViewMyPropertyTargetBinding(object target, PropertyInfo targetPropertyInfo)
        : base(target, targetPropertyInfo)
    {
    }

    // describes how to set MyProperty on MyView
    protected override void SetValueImpl(object target, object value)
    {
        var view = target as MyView;
        if (view == null) return;

        view.MyProperty = (string)value;
    }

    // is called when we are ready to listen for change events
    public override void SubscribeToEvents()
    {
        var myView = View;
        if (myView == null)
        {
            MvxBindingTrace.Trace(MvxTraceLevel.Error, "Error - MyView is null in MyViewMyPropertyTargetBinding");
            return;
        }

        _subscribed = true;
        myView.MyPropertyChanged += HandleMyPropertyChanged;
    }

    private void HandleMyPropertyChanged(object sender, EventArgs e)
    {
        var myView = View;
        if (myView == null) return;

        FireValueChanged(myView.MyProperty);
    }

    // clean up
    protected override void Dispose(bool isDisposing)
    {
        base.Dispose(isDisposing);

        if (isDisposing)
        {
            var myView = View;
            if (myView != null && _subscribed)
            {
                myView.MyPropertyChanged -= HandleMyPropertyChanged;
                _subscribed = false;
            }
        }
    }
}
```

As you can see it is fairly simple to define these classes and for each one you define you have an additional step where you will have to add this Target Binding definition in your `Setup.cs` file by overriding `FillTargetFactories`.

```c#
protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
{
    base.FillTargetFactories(registry);

    registry.RegisterPropertyInfoBindingFactory(
        typeof(MyViewMyPropertyTargetBinding),
        typeof(MyView), "MyProperty");
}
```

### Internals Of A Target Binding

All target bindings are a subclass of `MvxTargetBinding`, there are a couple of subclasses of this, which MvvmCross provides, which contain some additions to help prevent issues such as infinte call loops. Adding support for converters, adding platform specifics and overcoming View quirks.

This section goes a little bit deeper into what each of these variants provide.

#### MvxTargetBinding

This is the base for all target bindings, it implements `IMvxTargetBinding`, which exposes:

- `TargetType` property describing the `type` of the target to be bound
- `Mode` property describing the mode of the binding (OneWay, TwoWay, OneWayToSouce etc.)
- `SetValue()` method to describe how to set a value from the source
- `ValueChanged` event to notify the binding engine that the target change its value
- `SubscribeToEvents()` method called when we are ready to hook up event handlers

The `MvxTargetBinding` class additionally adds `FireValueChanged()`, which is a helper method to raise the `ValueChanged` event, allowing the developer to just pass along the new value to the method.

It also makes a `WeakReference` to the target and exposes that reference as the `Target` property.

#### MvxConvertingTargetBinding

This is the base that most target bindings use. A lot of target bindings will have to support using converters and MvvmCross combiners. What is special about this class is that it prevents feedback loops, when updating a target with a new value, and the `ValueChanged` event fires.
This class also provides a couple of virtual methods, to provide the developer means of skipping calling `SetValue` for platform or view specific reasons:

- `ShouldSkipSetValueForViewSpecificReasons()`
- `ShouldSkipSetValueForPlatformSpecificReasons()`

Both are called right before attempting to set the value.

#### MvxPropertyInfoTargetBinding

This class is a subclass of [`MvxConvertingTargetBinding`](#mvxconvertingtargetbinding), which prepopulates the `TargetType` and automatically implements the `SetValue()` method, based on `PropertyInfo`. Using this is the shortest path to add a simple `OneWay` binding for a view, where you simply provide the instance of `MvxPropertyInfoTargetBinding` with the `PropertyInfo` of the target you want to bind, when you register the target binding.

#### MvxWithEventPropertyInfoTargetBinding

This class is a subclass of [`MvxPropertyInfoTargetBinding`](#mvxpropertyinfotargetbinding), which is a shortcut to adding `TwoWay` bindings based on a specific event. Similarly to `MvxPropertyInfoTargetBinding` it uses the `PropertyInfo` to implement the `SetValue()` method. Additionally it implements the `SubscribeToEvents()` method, based on the assumption that there is an event which is called the same as the name of the property, postfixed with `Changed`. So if your property is called `MyProperty` it assumes that the corresponding event is called `MyPropertChanged`.

#### MvxEventNameTargetBinding

This class is a subclass of [`MvxTargetBinding`](#mvxtargetbinding), which is a shortcut to adding `OneWay` binding to a command based on a specific event. Acceptable event delegates are `EventHandler` and `EventHandler<TEventArgs>` where `TEventArgs` can be any class or structure (with `TEventArgs` you have to use the generic version of this binding). All you have to pass for this binding is the target object and the name of the target event. By default it also passes event handler's `EventArgs` (or `TEventArgs`) as a command parameter, but you can disable it in the constructor.

#### MvxAndroidTargetBinding

This class is a subclass of [`MvxConvertingTargetBinding`](#mvxconvertingtargetbinding), which provides the current `IMvxAndroidGlobals`, to be able to get the current `ApplicationContext` for stuff like getting resources from the Android Resources and other operations which require the `ApplicationContext`.

### Getting Inspiration

To get some inspiration on how to create your own target bindings, you can take a look at the ones that come out of the box with MvvmCross.

You can find them in the [Bindings code](https://github.com/MvvmCross/MvvmCross/tree/develop/MvvmCross/Binding). There are a lot of target bindings you can look at for different kinds of behavior and requirements.