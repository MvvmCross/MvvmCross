---
layout: documentation
title: MvvmCross Overview
category: Getting-started
order: 2
---
Deployed MvvmCross applications consist of two parts:

- the "Core" - containing all the ViewModels, Services, Models and 'business' code
- the "UI" - containing the Views and platform specific code for interacting with the "Core"

Optionally, you can split your code into more projects / assemblies to improve reusability and decouple layers.

For a multi-platform application, it's typical for there to be:

- A "Core" project, written as a .NET Standard library
- A "UI" project per platform, written as a native project for the current target platform. 
- Optionally some other projects that could be .NET Standard or platform libraries, which provide reusable abstractions or specific functionalities.

This is the way that MvvmCross encourages people to write their applications, and this guide will.

## Some MvvmCross key objects

There are a few key objects within the framework which are expected to be found on almost every application:

- In the "Core" part:
    - An `App` class, which is responsible for registering custom objects on the IoC container and starting your ViewModels and your business logic.
    - Optionally an `AppStart` object - responsible for making the decision of which `ViewModel` to present first.
    - One or more `ViewModel` objects - each one responsible for a piece of user interaction.
    - Services, Models, ...

- In each application platform project:
    - A native global `Application` handler object - responsible for native lifecycle events -. For example, on Android it would be a MainActivity / MainApplication class, while on iOS it woulde be an AppDelegate class. 
    - Optionally a `Setup` class - responsible for bootstrapping MvvmCross and registering platform services.
    - One or more `Views` - where generally each one is responsible for presenting one of your `ViewModels`.
    - Optionally a custom `ViewPresenter` - responsible for deciding how `Views` are shown.
    - UI code for controls, gestures, events.
    - Platform services for providing some very specialized features like Accelerometer or Camera.

## How an MvvmCross application starts

When an MvvmCross app starts, this is what actually happens:

1. The platform startup process fires.
2. Within the construction of the platform Application, the MvvmCross `Setup` is created.
3. The `Setup` performs the framework initialization in two steps: 
    - `InitializePrimary`: Runs on the main sync context (aka main thread). Initializes the IoC, Logging mechanism and other core parts.
    - `InitializeSecondary`: Runs on the background (never on the main thread). Constructs some other platform services like bindings, the `App` class and calls `Initialize` on it. It finally registers Views / ViewModels lookups.
4. When `App.Initialize` is called, your app is expected to provide an `AppStart` object, which is responsible for managing the first navigation step. The last step of `Setup` initialization consist on calling `AppStart.Startup(object hint)`.
5. `AppStart.Startup(object hint)` runs and the first ViewModel / View of your app is shown.

Note: In case you are wondering about the `hint` parameter on the `Startup` method, it's something you can use to pass initial parameters from your platform project to your Core layer. Super useful when implemeting push notifications, for example.

## The "Core" project

An MvvmCross Core project is supposed to include:

- An application object - typically called `App.cs`
- Optionally, a custom `AppStart` object which manages first navigation.
- One or more ViewModels - which are expected to be found in a folder called `ViewModels`.
- Your code: Services, Models, Repositories, ...
 
### The "App" class

The MvvmCross `App` class shouldn't be confused with the `ApplicationDelegate` in iOS, or with the `Application` object in Android or Windows. Those are native, SDK provided objects, while this one class is meant to be located on the common part of your code.

`App` is there to register an `IMvxAppStart` object and also to register your own bits to the IoC. This is what it would typically look like:

```c#
using MvvmCross.Ioc;

namespace MyName.Core
{
    public class App : MvvmCross.Core.ViewModels.MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            RegisterAppStart<ViewModels.MainViewModel>();
            // if you want to use a custom AppStart, you should replace the previous line with this one:
            // RegisterCustomAppStart<MyCustomAppStart>();
        }
    }
}
```

In this code snippet, the first line does a bulk registration to the IoC container. It looks within the current `Assembly` (the "Core" Assembly) and uses Reflection to register all classes ending in `Service` as lazily-constructed singletons.

If you want to know more about customizing this part of the MvvmCross initialization, it's highly recommended that you take a look at this document: [Customizing App and Setup](https://www.mvvmcross.com/documentation/advanced/customizing-using-App-and-Setup).

### ViewModels

`ViewModels` are key objects of the MVVM pattern. These should typically contain code for managing state and operations. As the name implies, ViewModels are View abstractions which provide properties, and commands to be consumed.

When using MvvmCross, all your ViewModels should inherit from `MvxViewModel`. These should typically contain:

- C# Properties which raise changes
- Commands
- Private methods for managing operations
    
This is how a typical `ViewModel` might look like:

```c#
public class MainViewModel : MvxViewModel
{
    public MainViewModel()
    {
    }

    private void Prepare()
    {
        // This is the first method to be called after construction
    }
        
    public override Task Initialize()
    {
        // Async initialization, YEY!
            
        return base.Initialize();
    }
        
    public IMvxCommand ResetTextCommand => new MvxCommand(ResetText);

    private void ResetText()
    {
        Text = "Hello MvvmCross";
    }

    private string _text = "Hello MvvmCross";
    public string Text
    {
        get { return _text; }
        set { SetProperty(ref _text, value); }
    }
}
```

This `MainViewModel` has:

- A `Text` property which raises a `PropertyChanged` notification when it changes
- A `ResetTextCommand` command which will call `ResetText()` whenever the command is executed.
 
Beyond this super simple example, `ViewModels` may also:

- Contain lists
- Perform navigation operations to other ViewModels
- Contain child ViewModels
- Be constructed from IoC. [Reference](https://www.mvvmcross.com/documentation/fundamentals/inversion-of-control-ioc)
- Injected dependencies on the Constructor / Properties. [Reference](https://www.mvvmcross.com/documentation/fundamentals/dependency-injection)
- Fody.PropertyChanged to remove some of the boilerplate code. [Reference](https://github.com/Fody/PropertyChanged) 

If you want to learn more about MvvmCross ViewModels, take a look at the documentation for [ViewModels Lifecycle](https://www.mvvmcross.com/documentation/fundamentals/viewmodel-lifecycle)

## Platform projects

When using MvvmCross, a typical platform project would contain:

- The native platform-specific initialization code - e.g `Main.cs` and `AppDelegate.cs` on Xamarin.iOS
- Optionally, a custom `Setup.cs` class
- One or more `Views` - each one responsible for presenting one of your `ViewModels`
- Optionally, a custom `ViewPresenter` - responsible for deciding how `Views` are shown
- Custom SDK dependant code - custom controls, gestures, background services, ...

### MvvmCross Platform specific initilization

#### iOS

On iOS, we need to replace the normal `AppDelegate.cs` class with an `MvxApplicationDelegate` one.

An initial replacement looks like:

```c#
namespace MyAwesomeApp.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : MvxApplicationDelegate<MvxIosSetup<App>, App>
    {
        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            var result = base.FinishedLaunching(application, launchOptions);

            // here is where your custom code should be placed

            return result;
        }
    }
}
```

The code snippet assumes your MvvmCross App class is called `App`. It also registers the default `MvxIosSetup` as your app's setup. If you want to use a custom one, you just need to modify the class constraint.

#### Android

On Android, the easiest way to declare initialization is by adding a custom `Application` class:

```c#
namespace MyAwesomeApp.Droid
{
    [Application]
    public class MainApplication : MvxAndroidApplication<MvxAndroidSetup<App>, App>
    {
        public MainApplication(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }
    }
}
```

The code snippet assumes your MvvmCross App class is called `App`. It also registers the default `MvxAndroidSetup` as your app's setup. If you want to use a custom one, you just need to modify the class constraint.

A final note: If your app uses Android Support packages, you should replace `MvxAndroidApplication` for `MvxAppCompatApplication` and `MvxAndroidSetup` for `MvxAppCompatSetup`.

#### WPF

On WPF, a new project will contain a native `App.xaml.cs`. You should edit it to make it look like this:

```c#
namespace MyAwesomeApp.Wpf
{
    public partial class App : MvxApplication
    {
        protected override void RegisterSetup()
        {
            this.RegisterSetupType<MvxWpfSetup<Core.App>>();
        }
    }
}
```

The code snippet assumes your MvvmCross App class is called `App`. If you want or need to use a custom Setup class, then you only need to modify the class constraint for `RegisterSetupType`.

#### UWP

On UWP, a new project will again contain a native `App.xaml.cs`

```c#
namespace MyAwesomeApp.Uwp
{
    public partial class App : MvxApplication
    {
        public App()
        {
            InitializeComponent();
        }

        protected override void RegisterSetup()
        {
            this.RegisterSetupType<MvxWindowsSetup<Core.App>>();
        }
    }
}
```

The code snippet assumes your MvvmCross App class is called `App`. If you want or need to use a custom Setup class, then you only need to modify the class constraint for `RegisterSetupType`.

#### macOS

On macOS, we need to replace the normal `AppDelegate.cs` class with an `MvxApplicationDelegate` one.

An initial replacement looks like:

```c#
namespace MyAwesomeApp.Mac
{
    [Register("AppDelegate")]
    public partial class AppDelegate : MvxApplicationDelegate<MvxMacSetup<App>, App>
    {    
    }
}
```

The code snippet assumes your MvvmCross App class is called `App`. It also registers the default `MvxMacSetup` as your app's setup. If you want to use a custom one, you just need to modify the class constraint.

#### tvOS

On macOS, we need to replace the normal `AppDelegate.cs` class with an `MvxApplicationDelegate` one.

An initial replacement looks like:

```c#
namespace MyAwesomeApp.TvOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : MvxApplicationDelegate<MvxTvosSetup<App>, App>
    {    
    }
}
```

The code snippet assumes your MvvmCross App class is called `App`. It also registers the default `MvxTvosSetup` as your app's setup. If you want to use a custom one, you just need to modify the class constraint.

### Setup Singleton

The Setup Singleton is a very special object within the framework, which can be used to ensure the framework is up and running at any time.

This class is accessible through platform static objects, and provides you with a method called `EnsureInitialized`. Just be sure not to abuse from it, as it may block your UI.

#### Android
The setup singleton is called `MvxAndroidSetupSingleton`. You would use it this way:

```c#
var setup = MvxAndroidSetupSingleton.EnsureSingletonAvailable(context);
setup.EnsureInitialized();
```

#### iOS
The setup singleton is called `MvxIosSetupSingleton`. You would use it this way:

```c#
var setup = MvxIosSetupSingleton.EnsureSingletonAvailable(yourAppDelegate, Window);
setup.EnsureInitialized();
```

#### macOS
The setup singleton is called `MvxMacSetupSingleton`. You would use it this way:

```c#
var setup = MvxMacSetupSingleton.EnsureSingletonAvailable(yourAppDelegate, Window);
setup.EnsureInitialized();
```

#### tvOS
The setup singleton is called `MvxTvosSetupSingleton`. You would use it this way:

```c#
var setup = MvxTvosSetupSingleton.EnsureSingletonAvailable(yourAppDelegate, Window);
setup.EnsureInitialized();
```

#### UWP
The setup singleton is called `MvxWindowsSetupSingleton`. You would use it this way:

```c#
var setup = MvxWindowsSetupSingleton.EnsureSingletonAvailable(rootFrame, activationArguments, nameof(Suspend))
setup.EnsureInitialized();
```

#### WPF
The setup singleton is called `MvxWpfSetupSingleton`. You would use it this way:

```c#
var setup = MvxWpfSetupSingleton.EnsureSingletonAvailable(Dispatcher, MainWindow)
setup.EnsureInitialized();
```

### Views

On your platform projects, it is highly expected to find one or more Views, where each one is normally - data bound - to a ViewModel.

A view is typically represented as a platform "View" screen. It could be an Android Activity/Fragment, or an iOS ViewController.

One important thing to note, is that by default `View`s are associated with `ViewModel`s using a naming convention. But using generic is the preferred way. On iOS for example, this is what a View class declaration would look like:

```c#
public class MyView : MvxViewController<MyViewModel>
```

### View Presenters

One of the main benefits MvvmCross provides you with is a super powerful, ViewModel first Navigation system. ViewPresenters are an important part of it, and they are highly customizable.

MvvmCross contains default ViewPresenters for all platforms, but they're highly customizable. If you want to learn more about ViewPresenters, please read the following document [ViewPresenters](https://www.mvvmcross.com/documentation/fundamentals/view-presenters).
