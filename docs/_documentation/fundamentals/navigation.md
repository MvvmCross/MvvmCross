---
layout: documentation
title: Navigation
category: Fundamentals
order: 2
---

MvvmCross uses `ViewModel first navigation`. Meaning that we navigate from ViewModel to ViewModel and not from View to View. In MvvmCross the ViewModel will lookup its corresponding View. By doing so we don't have to write platform specific navigation and we can manage everything from within our core.

# Introducing the MvxNavigationService

The navigation enables you to inject it into your ViewModels, which makes it more testable, and gives you the ability to implement your own navigation! Other main features are that it is fully async and type safe.
For more details see [#1634](https://github.com/MvvmCross/MvvmCross/issues/1634)

The following Api is available to use. (See [IMvxNavigationService code for latest definition](https://github.com/MvvmCross/MvvmCross/blob/develop/MvvmCross/Navigation/IMvxNavigationService.cs)):

```c#
public interface IMvxNavigationService
{
    event EventHandler<IMvxNavigateEventArgs> WillNavigate;
    event EventHandler<IMvxNavigateEventArgs> DidNavigate;
    event EventHandler<IMvxNavigateEventArgs> WillClose;
    event EventHandler<IMvxNavigateEventArgs> DidClose;
    event EventHandler<ChangePresentationEventArgs> WillChangePresentation;
    event EventHandler<ChangePresentationEventArgs> DidChangePresentation;

    void LoadRoutes(IEnumerable<Assembly> assemblies);

    Task<bool> Navigate(IMvxViewModel viewModel, IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default);

    Task<bool> Navigate<TParameter>(IMvxViewModel<TParameter> viewModel, TParameter param,
        IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default)
        where TParameter : notnull;

    Task<TResult> Navigate<TResult>(IMvxViewModelResult<TResult> viewModel, IMvxBundle? presentationBundle = null,
        CancellationToken cancellationToken = default)
        where TResult : notnull;

    Task<TResult> Navigate<TParameter, TResult>(IMvxViewModel<TParameter, TResult> viewModel, TParameter param,
        IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default)
        where TParameter : notnull
        where TResult : notnull;

    Task<bool> Navigate(Type viewModelType, IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default);

    Task<bool> Navigate<TParameter>(Type viewModelType, TParameter param, IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default)
        where TParameter : notnull;

    Task<TResult> Navigate<TResult>(Type viewModelType, IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default)
        where TResult : notnull;

    Task<TResult> Navigate<TParameter, TResult>(Type viewModelType, TParameter param,
        IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default)
        where TParameter : notnull
        where TResult : notnull;

    Task<bool> Navigate(
        string path, IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default);

    Task<bool> Navigate<TParameter>(string path, TParameter param,
        IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default)
        where TParameter : notnull;

    Task<TResult> Navigate<TResult>(string path, IMvxBundle? presentationBundle = null,
        CancellationToken cancellationToken = default)
        where TResult : notnull;

    Task<TResult> Navigate<TParameter, TResult>(string path, TParameter param,
        IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default)
        where TParameter : notnull
        where TResult : notnull;

    Task<bool> Navigate<TViewModel>(
        IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default)
        where TViewModel : IMvxViewModel;

    Task<bool> Navigate<TViewModel, TParameter>(
        TParameter param, IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default)
        where TViewModel : IMvxViewModel<TParameter>
        where TParameter : notnull;

    Task<TResult> Navigate<TViewModel, TResult>(
        IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default)
        where TViewModel : IMvxViewModelResult<TResult>
        where TResult : notnull;

    Task<TResult> Navigate<TViewModel, TParameter, TResult>(
        TParameter param, IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default)
        where TViewModel : IMvxViewModel<TParameter, TResult>
        where TParameter : notnull
        where TResult : notnull;

    Task<bool> CanNavigate(string path);

    Task<bool> CanNavigate<TViewModel>()
        where TViewModel : IMvxViewModel;

    Task<bool> CanNavigate(Type viewModelType);

    Task<bool> Close(IMvxViewModel viewModel, CancellationToken cancellationToken = default);

    Task<bool> Close<TResult>(IMvxViewModelResult<TResult> viewModel, TResult result, CancellationToken cancellationToken = default)
        where TResult : notnull;

    Task<bool> ChangePresentation(MvxPresentationHint hint, CancellationToken cancellationToken = default);
}
```

Some extension methods make it easier to use your already existing code:

```c#
public static class MvxNavigationExtensions
{
    public static Task<bool> CanNavigate(this IMvxNavigationService navigationService, Uri path)

    public static Task Navigate(this IMvxNavigationService navigationService, Uri path, IMvxBundle presentationBundle = null)
    public static Task Navigate<TParameter>(this IMvxNavigationService navigationService, Uri path, TParameter param, IMvxBundle presentationBundle = null)
    public static Task Navigate<TResult>(this IMvxNavigationService navigationService, Uri path, IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken))
    public static Task Navigate<TParameter, TResult>(this IMvxNavigationService navigationService, Uri path, TParameter param, IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken))

    public static async Task<bool> NavigateRegisteringToResult<TViewModel, TResult>(
        this IMvxNavigationService navigationService,
        IMvxResultAwaitingViewModel<TResult> fromViewModel,
        IMvxResultViewModelManager resultViewModelManager,
        IMvxBundle? presentationBundle = null,
        CancellationToken cancellationToken = default)
    public static async Task<bool> NavigateRegisteringToResult<TViewModel, TParameter, TResult>(
        this IMvxNavigationService navigationService,
        IMvxResultAwaitingViewModel<TResult> fromViewModel,
        IMvxResultViewModelManager resultViewModelManager,
        TParameter parameter,
        IMvxBundle? presentationBundle = null,
        CancellationToken cancellationToken = default)
    public static async Task<bool> CloseSettingResult<TViewModel, TResult>(
        this IMvxNavigationService navigationService,
        TViewModel viewModel,
        TResult result,
        CancellationToken cancellationToken = default)
}
```

In your ViewModel this could look like:

```c#
public class MyViewModel : MvxViewModel
{
    private readonly IMvxNavigationService _navigationService;

    public MyViewModel(IMvxNavigationService navigationService)
    {
        _navigationService = navigationService;
    }
    
    public override void Prepare()
    {
        // first callback. Initialize parameter-agnostic stuff here
    }

    public async Task Initialize()
    {
        await base.Initialize();

        // do the heavy work here
    }

    public async Task SomeMethod()
    {
        await _navigationService.Navigate<NextViewModel, MyObject>(new MyObject());
    }
}

public class NextViewModel : MvxViewModel<MyObject>
{
    private MyObject _myObject;

    public override void Prepare()
    {
        // first callback. Initialize parameter-agnostic stuff here
    }

    public override void Prepare(MyObject parameter)
    {
        // receive and store the parameter here
        _myObject = parameter;
    }
    
    public override async Task Initialize()
    {
        await base.Initialize();

        // do the heavy work here
    }
}
```

When you want to return a result to the place where you navigated from you can do:

```c#
public class MyViewModel : MvxResultAwaitingViewModel<MyReturnObject>
{
    private readonly IMvxNavigationService _navigationService;
    public MyViewModel(IMvxNavigationService navigationService, IMvxResultViewModelManager resultViewModelManager)
        : base(resultViewModelManager)
    {
        _navigationService = navigationService;
    }
    
    public override void Prepare()
    {
        // first callback. Initialize parameter-agnostic stuff here
    }

    public override async Task Initialize()
    {
        await base.Initialize();
        
        // do the heavy work here
    }

    public async Task SomeMethod()
    {
        await _navigationService.NavigateRegisteringToResult<NextViewModel, MyObject, MyReturnObject>(new MyObject(), ResultViewModelManager);

        // handle the result (if any) in ResultSet method
    }

    public override bool ResultSet(IMvxResultSettingViewModel<MyReturnObject> viewModel, MyReturnObject result)
    {
        if (IMvxResultSettingViewModel<MyReturnObject> is NextViewModel)
        {
            // do something with the result
            Debug.WriteLine(result);

            // we expected the result from NextViewModel, return true to mark it handled and unregister
            return true;
        }
        return false;
    }
}

public class NextViewModel : MvxResultSettingViewModel<MyObject, MyReturnObject>
{
    private readonly IMvxNavigationService _navigationService;

    private MyObject _myObject;

    public NextViewModel(IMvxNavigationService navigationService, IMvxResultViewModelManager resultViewModelManager)
        : base(resultViewModelManager)
    {
        _navigationService = navigationService;
    }

    public override void Prepare()
    {
        // first callback. Initialize parameter-agnostic stuff here
    }
    
    public override void Prepare(MyObject parameter)
    {
        // receive and store the parameter here
        _myObject = parameter;
    }
    
    public override async Task Initialize()
    {
        // do the heavy work here
    }
    
    public async Task SomeMethodToClose()
    {
        await _navigationService.CloseSettingResult(this, new MyReturnObject(), ResultViewModelManager);

        // or to set the result without closing the ViewModel call
        // SetResult(this, new MyReturnObject());
    }
}
```

Awaiting the result from the first caller ViewModel is considered a bad practice that can lead to undefined behaviour on some platforms (the most problematic here might be Android after tombstombing) that is why a contract between the caller and the callee ViewModels are required. The contract is implementing `IMvxResultAwaitingViewModel<TResult>` and `IMvxResultSettingViewModel<TResult>`.

If you have a BaseViewModel you might not be able to inherit `MvxViewModel<TParameter>`, `MvxResultAwaitingViewModel<TResult>` or `MvxResultSettingViewModel<TParameter, TResult>` because you already have the BaseViewModel as base class. In this case you can implement the following interface:

`IMvxViewModel<TParameter>`, `IMvxResultAwaitingViewModel<TResult>`, `IMvxResultSettingViewModel<TResult>`

To implement a parameter receiving (Base)ViewModel inherit from `IMvxViewModel<TParameter>` and add the following to your (Base)ViewModel:

```c#
public abstract void Prepare(TParameter parameter);
```

To implement awaiting your own result contract inherit from `IMvxResultAwaitingViewModel<TResult>` and add the following to your (Base)ViewModel:

```c#
protected IMvxResultViewModelManager ResultViewModelManager { get; }

protected BaseViewModel(IMvxResultViewModelManager resultViewModelManager)
{
    ResultViewModelManager = resultViewModelManager;
}

protected override void ReloadFromBundle(IMvxBundle state)
{
    base.ReloadFromBundle(state);
    this.ReloadAndRegisterToResult<TResult>(state, ResultViewModelManager);
}

protected override void SaveStateToBundle(IMvxBundle bundle)
{
    base.SaveStateToBundle(bundle);
    this.SaveRegisterToResult<TResult>(bundle, ResultViewModelManager);
}

public override ViewDestroy(bool viewFinishing = true)
{
    base.ViewDestroy();
    if (viewFinishing)
        this.UnregisterToResult<TResult>(ResultViewModelManager);
}
```

To implement setting your own result contract inherit from `IMvxResultSettingViewModel<TResult>` and add the following to your (Base)ViewModel:

```c#
protected IMvxResultViewModelManager ResultViewModelManager { get; }

protected BaseViewModel(IMvxResultViewModelManager resultViewModelManager)
{
    ResultViewModelManager = resultViewModelManager;
}

// call that method from anywhere to set the result
public virtual void SetResult(TResult result)
{
    this.SetResult<TResult>(result, ResultViewModelManager);
}
```

To check if you are able to navigate to a certain ViewModel you can use the `CanNavigate` method.

```c#
if (Mvx.IoCProvider.Resolve<IMvxNavigationService>().CanNavigate<NextViewModel>())
{
    //Do something
}
```

If you want to intercept ViewModel navigation changes you can hook into the events of the NavigationService.

```c#
Mvx.IoCProvider.Resolve<IMvxNavigationService>().AfterClose += (object sender, IMvxNavigateEventArgs e) =>
 {
    //Do something with e.ViewModel
};
```

The events available are:
* BeforeNavigate
* AfterNavigate
* BeforeClose
* AfterClose
* BeforeChangePresentation
* AfterChangePresentation

You might be using `Init()` or `Start()` methods in your ViewModels when updating from MvvmCross 4.x. These are now deprecated because it was done using reflection and therefore not very safe. When MvxNavigationService is used, a typed method called `Task Initialize()` will be available for you to perform any async heavy operations.

### Uri navigation

The Uri navigation of the NavigationService will build the navigation stack if required. This will also enable deeplinking and building up the navigationstack for it. Every ViewModel added to the stack can split up into multiple paths of it's own backstack. This will enable all kinds of layout structures as Hamburger, Tab or Top navigation.

The NavigationService supports multiple URIs per ViewModel as well as "NavigationFacades" that return the right ViewModel + parameters depending on the URI.

The solution is composed of:

* Navigation Attribute (ViewModel/Facade, URI regex)
* NavigationFacades are constructed via Mvx.IocConstruct to profit from dependency injection
* NavigationService, registered as a singleton, uses IMvxViewDispatcher to show the viewmodels
* Necessary additions to Android (Activity.OnNewIntent) + iOS (AppDelegate.OpenUrl) (look a the example project for more infos)
You can also use this solution for triggering deeplink from outside the app:

Register a custom scheme (i.e. "foo") in our app (look a the example project for me info)
Push-Messages: Depending on the status of the app you can pass a uri as the Notification Parameter, so when the app starts you can deep link directly to the view you want.

Supply your routings as assembly attributes. We would recommend putting them in the same file as the referenced ViewModel.

```c#
[assembly: MvxNavigation(typeof(ViewModelA), @"mvx://test/\?id=(?<id>[A-Z0-9]{32})$")]
namespace *.ViewModels
{
    public class ViewModelA
        : MvxViewModel
    {
    	public void Init(string id) // you can use captured groups defined in the regex as parameters here
        {

        }
    }
}
```

Routing in a ViewModel.

```c#
public class MainViewModel : MvxViewModel
{
    private readonly IMvxNavigationService _navigationService;

    public MainViewModel(IMvxNavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    private IMvxAsyncCommand _showACommand;
    public IMvxAsyncCommand ShowACommand
    {
        get
        {
            return _showACommand ?? (_showACommand = new MvxAsyncCommand(async () =>
            {
                await _navigationService.Navigate("mvx://test/?id=" + Guid.NewGuid().ToString("N"));
            }));
        }
    }
}
```

#### Facades

Say you are building a task app and depending on the type of task you want to show a different view. This is where NavigationFacades come in handy (there is only so much regular expressions can do for you).

mvx://task/?id=00000000000000000000000000000000 <-- this task is done, show read-only view (ViewModelA) mvx://task/?id=00000000000000000000000000000001 <-- this task isn't, go straight to edit view (ViewModelB)

```c#
[assembly: MvxRouting(typeof(SimpleNavigationFacade), @"mvx://task/\?id=(?<id>[A-Z0-9]{32})$")]
namespace *.NavigationFacades
{
	public class SimpleNavigationFacade
	    : IMvxNavigationFacade
	{
	    public Task<MvxViewModelRequest> BuildViewModelRequest(string url,
	        IDictionary<string, string> currentParameters, MvxRequestedBy requestedBy)
	    {
	    	// you can load data from a database etc.
	    	// try not to do a lot of work here, as the user is waiting for the UI to do something ;)
	        var viewModelType = currentParameters["id"] == Guid.Empty.ToString("N") ? typeof(ViewModelA) : typeof(ViewModelB);

	        return Task.FromResult(new MvxViewModelRequest(viewModelType, new MvxBundle(), null, requestedBy));
	    }
	}
}
```

## Upgrading from 4.x to 5.x

To make sure your navigation stays up-to-date change all your `ShowViewModel<>()` calls to the new navigation methods.

Example before:

```c#
private IMvxCommand _navigateCommand;
public IMvxCommand NavigateCommand
{
    get
    {
        _navigateCommand = _navigateCommand ?? new MvxCommand(() => ShowViewModel<TViewModel>());
        return _navigateCommand;
    }
}
```

After:

```c#
private IMvxAsyncCommand _navigateCommand;
public IMvxAsyncCommand NavigateCommand
{
    get
    {
        _navigateCommand = _navigateCommand ?? new MvxAsyncCommand(() => _navigationService.Navigate<TViewModel>());
        return _navigateCommand;
    }
}
```

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
IMvxViewModel<TParameter>
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
    - `bool`
    - Integral types: `sbyte`, `short`, `int`, `long`, `byte`, `ushort`, `uint`, `ulong`
    - Floating-point types: `float`, `double`
    - `decimal`
    - `char`
    - `string`
    - `DateTime`
    - `Guid`
    - Enumeration values

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

- `bool`
- Integral types: `sbyte`, `short`, `int`, `long`, `byte`, `ushort`, `uint`, `ulong`
- Floating-point types: `float`, `double`
- `decimal`
- `char`
- `string`
- `DateTime`
- `Guid`
- Enumeration values
