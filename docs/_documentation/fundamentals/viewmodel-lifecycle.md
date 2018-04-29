---
layout: documentation
title: ViewModel Lifecycle
category: Fundamentals
order: 3
---

Alongside the new [MvxNavigationService](https://www.mvvmcross.com/documentation/fundamentals/navigation), MvvmCross provides a new lifecycle for ViewModels with many enhancements such as async initialization. The standard lifecycle is:

1. Construction: Called when the object is instantiated. You can use Dependency Injection here to introduce all dependencies!
2. Prepare: The initial point for your ViewModel. You can use this method to receive and store all parameters (it is your responsibility to handle them).
3. Initialize: All heavy work should be run here. This method returns a Task, which means you can mark it as async and use await safely. If this method fails, the `Navigate` call that you are probably awaiting will fail, so you might want to catch that exception.

### Construction

When you want to navigate to a certain ViewModel, you will typically do it through the MvxNavigationService:

```c#
private async Task MyMethodAsync()
{
    await _navigationService.Navigate<MyViewModel>();
}
```

Inside that call, MvvmCross will instantiate `MyViewModel` using the IoC container and use the Dependency Injection engine to inject all its dependencies. This is how `MyViewModel` could look like:

```c#
public class MyViewModel : MvxViewModel
{
    private readonly IMyService _myService;

    public MyViewModel(IMyService myService)
    {
        _myService = myService;
    }

    // ...
}
```

### Prepare

If you need to send some parameters to a ViewModel, you will want to use this method. MvxViewModel can have two Prepare methods:

- Parameterless `Prepare`: Called in every scenario.
- `Prepare(TParameter parameter)`: Called when you are navigating to a ViewModel with initial parameters. You shouldn't perform any logics on this method more than saving the parameters.

This is how a typical ViewModel with initial parameters look like:

```c#
public class MyViewModel : MvxViewModel<MyParameterModel>
{
    private readonly IMyService _myService;

    public MyViewModel(IMyService myService)
    {
        _myService = myService;
    }

    public void Prepare(MyParameterModel parameter)
    {

    }

    // ...
}
```

And this is how you would navigate to this ViewModel using the MvxNavigationService:

```c#
private async Task MyMethodAsync()
{
    await _navigationService.Navigate<MyViewModel, MyParameterModel>(new MyParameterModel());
}
```

### Initialize

This method is called right after Prepare, and since it returns a Task, you can mark it as async and await operations inside of it. All heavy loading operations should be made inside this method.

When Initialize is fired from MvxViewModelLoader, there is a MvxNotifyTask called InitializeTask that will watch its state and fire property changed events (you can even bind View properties to InitializeTask properties!).

## Tombstoning: Saving and restoring the ViewModel's state

Each platform MvvmCross supports has a different way of handing low memory situations and as you may imagine, each platform has its own View lifecycle (the most problematic here might be Android).

In order to cope with that, the framework provides you with a way to save your ViewModel's state and a way to restore it later, regardless what might occur with the Views.

### Save State

When your app is going through a tombstoning process, MvvmCross will call a specific method from MvxViewModel: SaveStateToBundle, which is protected and receives a `IMvxBundle` as parameter.
You can override SaveStateToBundle and store any string you want on it, writing to its `Data` property. A good advise is to use [JSON serialization](https://www.mvvmcross.com/documentation/plugins/json) for that.

After SaveStateToBundle is returned to the caller, the IMvxBundle will be stored by MvvmCross.

### Restore State

Same as before, MvxViewModel has a protected method named ReloadFromBundle, which will be called when your app is coming back to life. ReloadFromBundle has a parameter of type `IMvxBundle`, and you can override this method and ready any string from the bundle (again, you might want to use [JSON serialization](https://www.mvvmcross.com/documentation/plugins/json) for this).

The bundle you receive in ReloadFromBundle will contain all the information you have stored before.

### Reloading state process

When your app is being restored, this is what the lifecycle of your ViewModel looks like: 

1. `ReloadFromBundle`: Time to get your saved data (more probably the initial parameters) back.
2. `Prepare`: Only the parameterless version of `Prepare` will be called. It is __your__ responsibility to save the used parameters and restore them (using SaveStateToBundle / ReloadFromBundle).
3. `Initialize`: Initialize will be called as normal. `InitializeTask` will watch the task and fire property changes.

This is what a typical ViewModel that handles tombstoning looks like:

```c#
public class MyViewModel : MvxViewModel<MyParameterModel>
{
    private readonly IMyService _myService;
    private readonly IMvxJsonSerializer _jsonSerializer;

    private MyParameterModel _initialParameter;

    public MyViewModel(IMyService myService, IMvxJsonSerializer jsonSerializer)
    {
        _myService = myService;
        _jsonSerializer = jsonSerializer;
    }

    protected override void SaveStateToBundle(IMvxBundle bundle)
    {
        base.SaveStateToBundle(bundle);

        bundle.Data["MyParameter"] = _jsonSerializer.Serialize(_initialParameter);
    }

    protected override void ReloadFromBundle(IMvxBundle state)
    {
        base.ReloadFromBundle(state);

        var serializedParameter = state.Data["MyParameter"];
        _initialParameter = _jsonSerializer.Deserialize<MyParameterModel>(serializedParameter);
    }

    public void Prepare()
    {
    }

    public void Prepare(MyParameterModel parameter)
    {
        _initialParameter = parameter;
    }

    public async Task Initialize()
    {
        await base.Initialize();

        // do something with _initialParameter
    }

    // ...
}
```

## View callbacks

Starting with MvvmCross 5.0, ViewModels will be coupled to the lifecycle of the view. This means that the ViewModel has the following methods available:

```c#
void ViewCreated();

void ViewAppearing();

void ViewAppeared();

void ViewDisappearing();

void ViewDisappeared();

void ViewDestroy();
```

The MvxViewController, MvxFragment(s), MvxActivity and the UWP views will call those methods when the platform specific events are fired. This will give you a more refined control of the ViewModel and its state. There may be certain bindings that you want to update or resources that you want to clean up in these calls.

However, it should be noted that it is not 100% reliable, due to the natural complex process of any View in different contexts. It _will_ work for most of the apps and most of the cases. But we aware that we don't know what you plan to do in the lifecycle of your app!

### Mapping view event to ViewModel events

There has been a thread going on on the [Xamarin forums](https://forums.xamarin.com/discussion/comment/240043/) where the implementation is discussed of this functionality. MvvmCross has based its lifecycle support on this thread and those events. 

|           | Appearing             | Appeared       | Disappearing         | Disappeared | 
| iOS       | ViewWillAppear        | ViewDidAppear  | ViewWillDisappear    | ViewDidDisappear | 
| Android   | OnAttachedToWindow    | OnGlobalLayout | OnPause              | OnDetachedFromWindow | 
| UWP       | Loading               | OnLoaded       | Unloaded             | OnUnloaded |    


For more information on the implementation of this functionality please see [Github](https://github.com/MvvmCross/MvvmCross/pull/1601)





# MvvmCross 4.x and 3.x ViewModels lifecycle

## ViewModel Creation

In MvvmCross v3 - Hot Tuna - the default ViewModel location and construction was overhauled in order to provide 3 new features:

- constructor based Dependency Injection
- navigation using Typed navigation classes
- saving and reloading VM state for 'tombstoning'

These changes were breaking changes for existing v1 and vNext apps, but provide significant testability and usability advantages for MvvmCross developers.

## How ViewModels are Created 

The default ViewModelLocator builds new ViewModel instances using a 4-step process - CIRS:

1. `Construction` - using IoC for Dependency Injection
2. `Init()` - initialization of navigation parameters
3. `ReloadState()` - rehydration after tombstoning
4. `Start()` - called when initialization and rehydration are complete


### 1 Construction

In MvvmCross, you can navigate to a `ViewModel` using parameter like:
```c#
ShowViewModel<DetailViewModel>( 
    new 
    {
        First="Hello",
        Second="World",
        Answer=42
    });
```

In older version of MvvmCross, these navigation parameters were passed to the constructor of the `ViewModel`.

However, from v3 moving forwards, these navigation parameters are instead passed to the `Init()` method, and the constructor is now free to be used for Dependency Injection.

This means that, for example, a `DetailViewModel` constructor might now look like:
```c#
public class DetailViewModel : MvxViewModel
{
    private readonly IDetailRepository _repository;

    public DetailViewModel(IDetailRepository repository)
    {
        _repository = repository;
    }

    // ...
}
```
This Dependency Injection is, of course, optional - your code can instead continue to use ServiceLocation if you prefer:

```c#
public class DetailViewModel : MvxViewModel
{
    private readonly IDetailRepository _repository;

    public DetailViewModel()
    {
        repository = Mvx.Resolve<IDetailRepository>();
    }

    // ...
}
```

### 2. Init()

Now that the construction is used for Dependency Injection, the navigation parameters move to a new method - `Init()`

`Init()` will always be called after construction and before `ReloadState()` and `Start()`

`Init()` can come in several flavors:.

- individual simply-Typed parameters
- a single Typed parameter object with simply-Typed properties
- as `InitFromBundle()` with an `IMvxBundle` parameter - this last flavor is always supported via the `IMvxViewModel` interface.

You can declare zero or more of each of these types, but generally you will probably only want to use one within your application.

So, for example, to support the navigation:

    RequestNavigate<DetailViewModel>(new { First="Hello", Second="World", Answer=42 });

you could implement any of:
```c#
public class DetailViewModel : MvxViewModel
{
    // ...
    
    public void Init(string First, string Second, int Answer)
    {
        // use the values
    }

    // ...
}
```
or:
```c#
public class DetailViewModel : MvxViewModel
{
    // ...
    
    public class NavObject
    {
        public string First {get;set;}
        public string Second {get;set;}
        public int Answer {get;set;}
    }
    
    public void Init(NavObject navObject)
    {
    // use navObject
    }
    
    // ...
}
```
or:
```c#
public class DetailViewModel : MvxViewModel
{  
    // ...
    
    public override void InitFromBundle(IMvxBundle bundle)
    {
        // use bundle - e.g. bundle.Data["First"]
    }
    
    // ...
}
```

Note that multiple calls can be used together if required. This allows for some separation of logic in your code. However, the separate objects cannot share field names and generally this approach is confusing... so is not really recommended:
```c#
public class DetailViewModel : MvxViewModel
{
    // ...
    
    public class FirstNavObject
    {
        public string First {get;set;}
        public string Second {get;set;}
    }

    public class SecondNavObject
    {
        public int Answer {get;set;}
    }

    public void Init(FirstNavObject firstNavObject)
    {
        // use firstNavObject
    }

    public void Init(SecondNavObject secondNavObject)
    {
        // use secondNavObject
    }

    // ...
}
```

### 3. ReloadState

If the `View`/`ViewModel` is recovering from a Tombstoned state, then `ReloadState` will be called with the data needed for rehydration.

If there is no saved state then no `ReloadState()` methods will be called. 

Exactly as with `Init()`, `ReloadState` can be called in several different ways.

- individual simply-Typed parameters
- a single Typed parameter object with simply-Typed properties
- as `ReloadStateFromBundle()` using an `IMvxBundle` parameter - this last flavor is always supported via the `IMvxViewModel` interface.

Normally, I'd expect this to be called as:
```c#
public class DetailViewModel : MvxViewModel
{
    // ...
    
    public class SavedState
    {
        public string Name {get;set;}
        public int Position {get;set;}
    }
    
    public void ReloadState(SavedState savedState)
    {
        // use savedState
    }

    // ...
}
```

#### Aside: where does the SavedState come from?

One of the new ViewModel APIs available in Hot Tuna is a SaveState pattern.

This can be implemented in one of two ways:

- using one or more paremeterless methods that return Typed state objects
- using the override `SavedStateToBundle(IMvxBundle bundle)`

Using a Typed state object:
```c#
public class DetailViewModel : MvxViewModel
{
    // ...
    
    public class SavedState
    {
        public string Name {get;set;}
        public int Position {get;set;}
    }

    public SavedState SaveState()
    {
        return new SavedState()
        {
        Name = _name,
        Position = _position
        };
    }

    // ...
}
```
Using `SavedStateToBundle`:
```c#
public class DetailViewModel : MvxViewModel
{
    // ...

    protected override void SaveStateToBundle(IMvxBundle bundle)
    {
        bundle.Data["Name"] = _name;
        bundle.Data["Position"] = _position.ToString();
    }
    
    // ...
}
```

### 4. Start()

After all of `Construction`, `Init`, and `ReloadState` is complete, then the `Start()` method will be called.

This method is simply:
```c#
public class DetailViewModel : MvxViewModel
{
    // ...
    
    public override void Start()
    {
        // do any start
    }

    // ...
}
```

### Putting it all together

For a real app, I would expect the navigation, construction and state saving/loading code to actually look like:
```c#
ShowViewModel<DetailViewModel>(
    new DetailViewMode.NavObject
    {
        First = "Hello",
        Second = "World",
        Answer = 42
    });
```

and
```c#
public class DetailViewModel : MvxViewModel
{
    public class SavedState
    {
        public string Name {get;set;}
        public int Position {get;set;}
    }
    
    public class NavObject
    {
        public string First {get;set;}
        public string Second {get;set;}
        public int Answer {get;set;}
    }

    private readonly IDetailRepository _repository;
    
    public DetailViewModel(IDetailRepository repository)
    {
        _repository = repository;
    }

    public void Init(NavObject navObject)
    {
        // use navObject
    }
    
    public void ReloadState(SavedState savedState)
    {
        // use savedState
    }
    
    public override void Start()
    {
        // do any start
    }
    
    public SavedState SaveState()
    {
        return new SavedState()
        {
        Name = _name,
        Position = _position
        };
    }
    
    // ...
}
```

### Overriding CIRS.

If you don't like this `CIRS` (Construction-Init-ReloadState-Start) flow for building your ViewModels, then the good news is that you can easily override the `ViewModelLocator` within v3, just as you could within earlier MvvmCross versions. For more on this see LINK-TODO

### ViewModel Deactivation, Activation and Destruction

Monitoring other View/ViewModel lifecycle event across multiple platforms is fairly tricky, especially once developers start experimenting beyond the 'basic' presentation models and start using tabs, splitviews, popups, flyouts, etc

For most viewmodels, it's common to not try to monitor other lifecyle events. This is OK since most viewmodels don't perform any actions and don't consume any resources when the view is not present - so these can just be left to be garbage collected when the system needs the memory back.

For ViewModels which consume low-intensity resources - like timer ticks - then these can generally use the `MvxMessenger` to connect the ViewModel to those resources. This messenger uses **weak referencing** by default and itself sends out subscription change messages when clients subscribe/unsubscribe. Using this method, a developer can allow the background resources to monitor whether the viewmodels are in memory (and referenced by views) - and so the background resources can manage themselves.
   
For those rare situations where resource monitoring is actively needed - e.g. for the `SpheroViewModel` which needs to maintain an active BlueTooth SPP channel - then it is possible to implement a custom interface on the ViewModel - e.g. `IActiveViewModel` - and this interface can be called from each of the views on each of the client platforms.

Generally this involves being hooked up from ViewDidAppear/Disappear on iOS, OnNavigatedTo/From on Windows, and OnRestart/Pause on Android, although this may vary depending on the exact presentation of your views (eg whether they are whole pages, tabs, flyouts, etc).
