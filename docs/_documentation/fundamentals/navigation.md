---
layout: documentation
title: Navigation
category: Fundamentals
order: 3
---
MvvmCross uses `ViewModel first navigation`. Meaning the we navigate from ViewModel to ViewModel and not from View to View. In MvvmCross the ViewModel will lookup its corresponding View. By doing so we don't have to write platform specific navigation and we can manage everything from within our core.

## Simple ViewModel navigation

To navigate from a ViewModel to another ViewModel you can use `ShowViewModel` command.
The `ShowViewModel` command will take a `Generic` type which should represent the ViewModel that you want to navigate to.

```c#
ShowViewModel<TViewModel>();
```

If we want to for example navigate to the DetailViewModel we only have to call the command from within another ViewModel.

```c#
ShowViewModel<DetailViewModel>();
```

To move back to the previous ViewModel we can call `Close(this);` on the ViewModel that we want to close.

## Navigation with parameters - using a complex parameter object

Every object can be passed to another ViewModel as a parameter. If its is desired to pass data along in the form of a more complex parameter it can be done like so:

```c#
ShowViewModel<TViewModel, TParameter>(new TParameter());
```

To be able to retrieve this the receiving class should implement the expected parameter on a class level and implement the correct init:

```c#
public class MyViewModel : MvxViewModel<TParameter>
{
    protected override Task Init(TParameter parameter)
    {
        // use the parameters here
    }
}
```

MvvmCross uses JSON to serialize the object and to use complex parameters you should have the MvvmCross Json plugin installed or register your own IMvxJsonConverter.

## Navigation with parameters - using a simple parameter object

As you write apps, you may frequently find that you want to parameterize a `ViewModel` navigation.

For example, you may encounter List-Detail situations - where:

- The Master view shows a list of items.
- When the user selects one of these, then the app will navigate to a Detail view
- The Detail view will then shows that specific selected item.

To achieve this, the navigation from `MasterViewModel` to `DetailViewModel` will normally be achieved by:

- we declare a class `DetailParameters` for the navigation:

```c#
public class DetailParameters
{
    public int Index {
        get;
        set;
    }
}
```

- the `MasterViewModel` makes `ShowViewModel` a call like:

`ShowViewModel<DetailViewModel>(new DetailParameters() { Index = 2 });`

- the `DetailViewModel` declares an `Init` method in order to receive this `DetailParameters`:

```c#
public void Init(DetailParameters parameters)
{
    // use the parameters here
}
```

**Note** that the `DetailParameters` class used here must be a 'simple' class used only for these navigations:

- it must contain a parameterless constructor
- it should contain only public properties with both `get` and `set` access
- these properties should be only of types:
- `int`
- `long`
- `double`
- `string`
- `Guid`
- enumeration values

## Navigation with parameters - using an anonymous parameter object

For simple navigations, declaring a formal `Parameters` object can feel like 'overkill' - like 'hard work'.

In these situations you can instead use anonymous classes and named method arguments.

For example, you can:

- use a call to `ShowViewModel` like:

`ShowViewModel<DetailViewModel>(new { index = 2 });`

- in the `DetailViewModel` declare an `Init` method in order to receive this `index` as:

```c#
public void Init(int index)
{
// use the index here
}
```

**Note** that due to serialization requirements, the only available parameter types used within this technique are only:

- `int`
- `long`
- `double`
- `string`
- `Guid`
- enumeration values