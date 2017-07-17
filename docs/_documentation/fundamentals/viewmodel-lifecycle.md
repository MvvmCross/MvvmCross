---
layout: documentation
title: ViewModel Lifecycle
category: Fundamentals
order: 5
---

# MvvmCross 5.x ViewModels lifecycle

Alongside [a new Navigation Service](https://www.mvvmcross.com/documentation/fundamentals/navigation), MvvmCross provides a new lifecycle for ViewModels with many enhancements such as async initialization. The current lifecycle includes:

1. Construction
2. Initialization

Also note that starting from MvvmCross 5.0 ViewModels will be coupled to the lifecycle of the view. This means that the ViewModel has the following methods available:

```c#
    void Appearing();

    void Appeared();

    void Disappearing();

    void Disappeared();
```

The MvxViewController, MvxFragment(s), MvxActivity and the UWP views will call those methods open the platform specific events that are fired. This will give us a more refined control of the ViewModel and the state of its lifecycle. There may be binding that you want to update or resources to clean up, these lifecycle events can help with that.

It should be noted however that it is not 100% reliable but it should work for most of the apps. We don't know what you do in the lifecycle of your app and what could interfere with the called order of the viewmodel lifecycle events.

## Mapping view event to viewmodel events

There has been a thread going on on the [Xamarin forums](https://forums.xamarin.com/discussion/comment/240043/) where the implementation is discussed of this functionality. MvvmCross has based its lifecycle support on this thread and those events. 

|           | Appearing             | Appeared       | Disappearing         | Disappeared | 
| iOS       | ViewWillAppear        | ViewDidAppear  | ViewWillDisappear    | ViewDidDisappear | 
| Android   | OnAttachedToWindow    | OnGlobalLayout | OnPause              | OnDetachedToWindow | 
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

    ShowViewModel<DetailViewModel>( 
      new 
      {
        First="Hello",
        Second="World",
        Answer=42
      });


In older version of MvvmCross, these navigation parameters were passed to the constructor of the `ViewModel`.

However, from v3 moving forwards, these navigation parameters are instead passed to the `Init()` method, and the constructor is now free to be used for Dependency Injection.

This means that, for example, a `DetailViewModel` constructor might now look like:

    public class DetailViewModel : MvxViewModel
    {
      private readonly IDetailRepository _repository;
 
      public DetailViewModel(IDetailRepository repository)
      {
        _repository = repository;
      }
 
      // ...
    }

This Dependency Injection is, of course, optional - your code can instead continue to use ServiceLocation if you prefer:


    public class DetailViewModel : MvxViewModel
    {
      private readonly IDetailRepository _repository;
 
      public DetailViewModel()
      {
        repository = Mvx.Resolve<IDetailRepository>();
      }
 
      // ...
    }


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

    public class DetailViewModel : MvxViewModel
    {
      // ...
    
      public void Init(string First, string Second, int Answer)
      {
        // use the values
      }

      // ...
    }

or:

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

or:

    public class DetailViewModel : MvxViewModel
    {  
      // ...
    
      public override void InitFromBundle(IMvxBundle bundle)
      {
        // use bundle - e.g. bundle.Data["First"]
      }
    
      // ...
    }


Note that multiple calls can be used together if required. This allows for some separation of logic in your code. However, the separate objects cannot share field names and generally this approach is confusing... so is not really recommended:

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


### 3. ReloadState

If the `View`/`ViewModel` is recovering from a Tombstoned state, then `ReloadState` will be called with the data needed for rehydration.

If there is no saved state then no `ReloadState()` methods will be called. 

Exactly as with `Init()`, `ReloadState` can be called in several different ways.

- individual simply-Typed parameters
- a single Typed parameter object with simply-Typed properties
- as `ReloadStateFromBundle()` using an `IMvxBundle` parameter - this last flavor is always supported via the `IMvxViewModel` interface.

Normally, I'd expect this to be called as:

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


#### Aside: where does the SavedState come from?

One of the new ViewModel APIs available in Hot Tuna is a SaveState pattern.

This can be implemented in one of two ways:

- using one or more paremeterless methods that return Typed state objects
- using the override `SavedStateToBundle(IMvxBundle bundle)`

Using a Typed state object:

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

Using `SavedStateToBundle`:

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


### 4. Start()

After all of `Construction`, `Init`, and `ReloadState` is complete, then the `Start()` method will be called.

This method is simply:

    public class DetailViewModel : MvxViewModel
    {
      // ...
    
      public override void Start()
      {
        // do any start
      }
 
      // ...
    }


### Putting it all together

For a real app, I would expect the navigation, construction and state saving/loading code to actually look like:

    ShowViewModel<DetailViewModel>(
      new DetailViewMode.NavObject
      {
        First = "Hello",
        Second = "World",
        Answer = 42
      });


and

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


### Overriding CIRS.

If you don't like this `CIRS` (Construction-Init-ReloadState-Start) flow for building your ViewModels, then the good news is that you can easily override the `ViewModelLocator` within v3, just as you could within earlier MvvmCross versions. For more on this see LINK-TODO

### ViewModel Deactivation, Activation and Destruction

Monitoring other View/ViewModel lifecycle event across multiple platforms is fairly tricky, especially once developers start experimenting beyond the 'basic' presentation models and start using tabs, splitviews, popups, flyouts, etc

For most viewmodels, it's common to not try to monitor other lifecyle events. This is OK since most viewmodels don't perform any actions and don't consume any resources when the view is not present - so these can just be left to be garbage collected when the system needs the memory back.

For ViewModels which consume low-intensity resources - like timer ticks - then these can generally use the `MvxMessenger` to connect the ViewModel to those resources. This messenger uses **weak referencing** by default and itself sends out subscription change messages when clients subscribe/unsubscribe. Using this method, a developer can allow the background resources to monitor whether the viewmodels are in memory (and referenced by views) - and so the background resources can manage themselves.
   
For those rare situations where resource monitoring is actively needed - e.g. for the `SpheroViewModel` which needs to maintain an active BlueTooth SPP channel - then it is possible to implement a custom interface on the ViewModel - e.g. `IActiveViewModel` - and this interface can be called from each of the views on each of the client platforms.

Generally this involves being hooked up from ViewDidAppear/Disappear on iOS, OnNavigatedTo/From on Windows, and OnRestart/Pause on Android, although this may vary depending on the exact presentation of your views (eg whether they are whole pages, tabs, flyouts, etc).
