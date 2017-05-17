---
layout: documentation
title: Navigation
category: Fundamentals
order: 3
---
MvvmCross uses `ViewModel first navigation`. Meaning the we navigate from ViewModel to ViewModel and not from View to View. In MvvmCross the ViewModel will lookup its corresponding View. By doing so we don't have to write platform specific navigation and we can manage everything from within our core.

# MvvmCross 5.x and higher navigation

MvvmCross 5 introduces a new NavigationService! The new navigation enables you to inject it into your ViewModels, which makes it more testable, and gives you the ability to implement your own navigation! Other main features are that it is fully async and type safe.
For more details see [#1634](https://github.com/MvvmCross/MvvmCross/issues/1634)

The following Api is available to use:

```c#
public interface IMvxNavigationService
{
    Task Navigate<TViewModel>() where TViewModel : IMvxViewModel;
    Task Navigate<TViewModel, TParameter>(TParameter param) where TViewModel : IMvxViewModel<TParameter> where TParameter : class;
    Task<TResult> Navigate<TViewModel, TParameter, TResult>(TParameter param) where TViewModel : IMvxViewModel<TParameter, TResult> where TParameter : class where TResult : class;
    Task<TResult> Navigate<TViewModel, TResult>() where TViewModel : IMvxViewModelResult<TResult> where TResult : class;
    Task Navigate(string path);
    Task Navigate<TParameter>(string path, TParameter param) where TParameter : class;
    Task<TResult> Navigate<TResult>(string path) where TResult : class;
    Task<TResult> Navigate<TParameter, TResult>(string path, TParameter param) where TParameter : class where TResult : class;
    Task<bool> CanNavigate(string path);
    Task<bool> CanNavigate<TViewModel>() where TViewModel : IMvxViewModel;
    Task<bool> Close(IMvxViewModel viewModel);
}

public static class MvxNavigationExtensions
{
    public static Task<bool> CanNavigate(this IMvxNavigationService navigationService, Uri path)
    public static Task Navigate(this IMvxNavigationService navigationService, Uri path)
    public static Task Navigate<TParameter>(this IMvxNavigationService navigationService, Uri path, TParameter param)
    public static Task<TResult> Navigate<TResult>(this IMvxNavigationService navigationService, Uri path)
    public static Task<TResult> Navigate<TParameter, TResult>(this IMvxNavigationService navigationService, Uri path, TParameter param)
    Task<bool> Close<TViewModel>(this IMvxNavigationService navigationService)
}
```

The Uri navigation will build the navigation stack if required. This will also enable deeplinking and building up the navigationstack for it. Every ViewModel added to the stack can split up into multiple paths of it's own backstack. This will enable all kinds of layout structures as Hamburger, Tab or Top navigation.

In your ViewModel this could look like:

```c#
public class MyViewModel : MvxViewModel
{
    private readonly IMvxNavigationService _navigationService;
    public MyViewModel(IMvxNavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    public async Task SomeMethod()
    {
        _navigationService.Navigate<NextViewModel, MyObject>(new MyObject());
    }
}

public class NextViewModel : MvxViewModel<MyObject>
{
    public async Task Initialize(MyObject parameter)
    {
        //Do something with parameter
    }
}
```

When you want to return a result to the place where you navigated from you can do:

```c#
public class MyViewModel : MvxViewModel
{
    private readonly IMvxNavigationService _navigationService;
    public MyViewModel(IMvxNavigationService navigation)
    {
        _navigationService = navigationService;
    }

    public async Task SomeMethod()
    {
        var result = await _navigationService.Navigate<NextViewModel, MyObject, MyReturnObject>(new MyObject());
        //Do something with the result MyReturnObject that you get back
    }
}

public class NextViewModel : MvxViewModel<MyObject, MyReturnObject>
{
    public async Task Initialize(MyObject parameter)
    {
        //Do something with parameter
    }
    
    public async Task SomeMethod()
    {
        await Close(new MyObject());
    }
}
```

To check if you are able to navigate to a certain ViewModel you can use the `CanNavigate` method.

```c#
if (Mvx.Resolve<IMvxNavigationService>().CanNavigate<NextViewModel>())
{
    //Do something
}
```

If you want to intercept ViewModel navigation changes you can hook into the events of the NavigationService.

```c#
Mvx.Resolve<IMvxNavigationService>().AfterClose += (object sender, NavigateEventArgs e) => {
    //Do something with e.ViewModelType or e.Url
};
```

The events available are:
* BeforeNavigate
* AfterNavigate
* BeforeClose
* AfterClose

# MvvmCross 4.x navigation

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

If you have a BaseViewModel you might not be able to inherit `MvxViewModel<TParameter>` because you already have the BaseViewModel as base class. In this case you can implement the following interface:

```c#
IMvxViewModelInitializer<TInit>
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
