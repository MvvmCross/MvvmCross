---
layout: documentation
title: MvvmCross Overview
category: Fundamentals
order: 1
---
Deployed MvvmCross applications consist of two parts:

- the "core" - containing all the ViewModels, Services, Models and 'business' code
- the "ui" - containing the Views and platform specific code for interacting with the "core"

For a multi-platform application, it's typical for there to be:

- a single "core" project, written as a PCL (Portable Class Library)
- a "ui" project per platform written as a native project for the current target platform.
- optionally some "plugins" - each one containing a PCL part and native parts - each one providing reusable abstractions of native functionality such as camera, geolocation, accelerometer, files, etc

This is the way that MvvmCross encourages people to write their applications, and this guide will. However, other approaches are possible - e.g. a single project can include both "core" and "ui", or multiple "core" projects can be written using copy-and-paste or using a technique such as file-linking

##Some key MvvmCross objects

There are a few key objects within an MvvmCross application:

- in the "core", there are:
 - an `App` - responsible for starting your ViewModels and your business logic
 - a `Start` object - responsible for deciding the first `ViewModel` or `ViewModels` which should be presented
 - one or more `ViewModels` - each one responsible for a piece of user interaction
 - your services, models, etc

- in each "ui", there are:
 - the native `Application` object - responsible for native lifecycle events - on each platform this object is a platform-specific class
 - an MvvmCross `Setup` class - responsible for 'bootstraping' MvvmCross, your 'core' and your 'ui'
 - one or more `Views` - each one responsible for presenting one of your `ViewModels`
 - a `Presenter` - responsible for deciding how `Views` are shown
 - custom UI code - for controls, gestures, events, etc

##How an MvvmCross application starts

When an MvvmCross app starts on a native project, then:

- the native Application will 'be created' first
- within the construction of the native Application, a `Setup` will be created
 - the `Setup` will perform very basic tasks - e.g. initialisation of the IoC system (see https://github.com/slodge/MvvmCross/wiki/Service-Location-and-Inversion-of-Control)
 - then the `Setup` will call into the core project, construct an `App` and will call `Initialize` on it.
 - during the `Initialize` your App will typically: 
    - register your app-specific services with the IoC system
    - create and register a `Start` object
 - the `Setup` will then configure the UI side of the project - especially things like lookup tables for views
 - finally the `Setup` will start the MvvmCross binding engine (if needed)
- with `Setup` complete, your native Application can then actually start.
- to do this, it requests a reference to the `Start` object and calls `Start()` on it 
- after this, the app will start presenting `ViewModels` using databound `Views`

##The MvvmCross Core

An MvvmCross Core project provides:

- an application object - typically in `App.cs`
- one or more ViewModels - normally in a folder called `ViewModels`
- your code: services, models, repositories, engines, units of work, etc - whatever your app needs to work
 
### App.cs

In each MvvmCross application there should be one and only one `App`.

This `App` is not to be confused with the `ApplicationDelegate` in iOS, or with the `Application` objects in Android or Windows. Those native objects are there to provide the lifecycle of the native platform-specific code.

Instead, this `App` in MvvmCross is there to assist with the lifecycle of your ViewModels and your services, models, etc

The key methods within an `App` are:

- `Initialize` - called on start up
- `FindViewModelLocator(MvxViewModelRequest request)` - used to find the object which provides `ViewModels` during navigation

The specific jobs that your `App` should do during its `Initialize` are:

- to construct and/or IoC-register any objects specific to your applications - services, models, etc
- to register an `IMvxAppStart` object

A default `App` supplied via nuget, looks like:

```c#
using Cirrious.CrossCore.IoC;

namespace MyName.Core
{
public class App : Cirrious.MvvmCross.ViewModels.MvxApplication
{
    public override void Initialize()
    {
        CreatableTypes()
        .EndingWith("Service")
        .AsInterfaces()
        .RegisterAsLazySingleton();

        RegisterAppStart<ViewModels.FirstViewModel>();
    }
}
}
```

This `App`:

- within Initialize
 - looks within the current `Assembly` (the "core" Assembly) and uses Reflection to register all classes ending in `Service` as lazily-constructed singletons
 - calls `RegisterAppStart<TViewModel>` to create and register a very simple `IMvxAppStart` implementation - an implementation which always shows a single `FirstViewModel` when `Start()` is called
- uses the default `ViewModelLocator` - this default uses naming conventions to locate and construct `ViewModels` and creates a new `ViewModel` for each and every request from a `View`

If you wanted to use a custom `IMvxAppStart` object, see https://github.com/slodge/MvvmCross/wiki/Customising-using-App-and-Setup.

### ViewModels

In each MvvmCross 'core' application your `ViewModels` provide containers for the state and the behaviour for your User Interface.

Typically they do this using:

- C# Properties
- the `INotifyPropertyChanged` and `INotifyCollectionChanged` interfaces to send notifications when properties change
- special `ICommand` properties which can allow View events (e.g. button taps) to call actions within the `ViewModel`
    
For MvvmCross, `ViewModels` normally inherit from `MvxViewModel`

A typical `ViewModel` might look like:

```c#
public class FirstViewModel
    : MvxViewModel
{
    private string _name;
    public string Name
    {
        get {
            return _name;
        }
        set {
            _name = value;
            RaisePropertyChanged(() => Name);
        }
    }

    private MvxCommand _resetCommand;
    public ICommand ResetCommand
    {
        get
        {
            _resetCommand = _resetCommand ?? new MvxCommand(() => Reset());
            return _resetCommand;
        }
    }

    private void Reset()
    {
        Name = string.Empty;
    }
}
```

This `FirstViewModel` has:

- a single `Name` property which raises a `PropertyChanged` notification when it changes
- a single `ResetCommand` command which will call the `Reset()` method whenever the command is executed.
 
Beyond this simple example, `ViewModels` can also:

- contain dynamic lists (see https://github.com/slodge/MvvmCross/wiki/MvvmCross-Tutorials#working-with-collections)
- be constructed from IoC (https://github.com/slodge/MvvmCross/wiki/Service-Location-and-Inversion-of-Control)
- use 'techniques' like:
- `MvxCommandCollection` (see http://slodge.blogspot.co.uk/2013/03/fixing-mvvm-commands-making-hot-tuna.html), 
- `IMvxINPCInterceptor` (see http://slodge.blogspot.co.uk/2013/07/intercepting-raisepropertychanged.html)
- Fody to remove some of the boilerplate code (http://slodge.blogspot.co.uk/2013/07/awesome-clean-viewmodels-via-fody.html)
- Rio binding (see http://slodge.blogspot.co.uk/2013/07/n36-rio-binding-carnival.html)

##The MvvmCross UI

An MvvmCross 'ui' project provides:

- the native platform-specific application code - e.g `Main.cs` and `AppDelegate.cs` on Xamarin.iOS
- a `Setup.cs` class
- one or more `Views` - each one responsible for presenting one of your `ViewModels`
- a `Presenter` - responsible for deciding how `Views` are shown
- custom UI code - for controls, gestures, events, etc

###Platform specific application code

####iOS

On iOS, we need to replace the normal `AppDelegate.cs` class with an `MvxApplicationDelegate`

An initial replacement looks like:

```c#
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Touch.Platform;
using Cirrious.MvvmCross.ViewModels;

namespace MyName.Touch
{
[Register ("AppDelegate")]
public partial class AppDelegate : MvxApplicationDelegate
{
    UIWindow _window;

    public override bool FinishedLaunching (UIApplication app, NSDictionary options)
    {
        _window = new UIWindow (UIScreen.MainScreen.Bounds);

        var setup = new Setup(this, _window);
        setup.Initialize();

        var startup = Mvx.Resolve<IMvxAppStart>();
        startup.Start();

        _window.MakeKeyAndVisible ();

        return true;
    }
}
}
```
  
####Android

On Android, we don't normally have any `Application` to override. Instead of this, MvvmCross by default provides a `SplashScreen` - this typically looks like:

```c#
using Android.App;
using Android.Content.PM;
using Cirrious.MvvmCross.Droid.Views;

namespace MyName.Droid
{
[Activity(
     Label = "CustomBinding.Droid"
             , MainLauncher = true
     , Icon = "@drawable/icon"
     , Theme = "@style/Theme.Splash"
     , NoHistory = true
     , ScreenOrientation = ScreenOrientation.Portrait)]
public class SplashScreen : MvxSplashScreenActivity
{
    public SplashScreen()
    : base(Resource.Layout.SplashScreen)
    {
    }
}
}
```
    
Importantly, please note that this class is marked with `MainLauncher = true` to ensure that this is the first thing created when the native platform starts.

####WindowsPhone

On WindowsPhone, a new project will contain a native `App.xaml.cs`

To adapt this for MvvmCross, we simply:

1. add a line to the constructor:

            var setup = new Setup(RootFrame);
            setup.Initialize();

2. add a block to `Application_Launching` to force the native app to defer the start actions to `IMvxAppStart`
 
```c#
private void Application_Launching(object sender, LaunchingEventArgs e)
{
    RootFrame.Navigating += RootFrameOnNavigating;
}

private void RootFrameOnNavigating(object sender, NavigatingCancelEventArgs args)
{
    args.Cancel = true;
    RootFrame.Navigating -= RootFrameOnNavigating;
    RootFrame.Dispatcher.BeginInvoke(() => {
        Cirrious.CrossCore.Mvx.Resolve<Cirrious.MvvmCross.ViewModels.IMvxAppStart>().Start();
    });
}
```

####Wpf

On Wpf, a new project will contain a native `App.xaml.cs`.  After adding the MvvmCross libraries via Nuget a new file is added called 'App.Xam.Mvx.cs'.  This file contains -

   using System;
   using System.Windows;
   using Cirrious.CrossCore;
   using Cirrious.MvvmCross.ViewModels;
  `using Cirrious.MvvmCross.Wpf.Views;`

  namespace MyName.Wpf
  {
      public partial class App : Application
      {
          private bool _setupComplete;

          private void DoSetup()
          {
              LoadMvxAssemblyResources();
			
              var presenter = new MvxSimpleWpfViewPresenter(MainWindow);

              var setup = new Setup(Dispatcher, presenter);
              setup.Initialize();

              var start = Mvx.Resolve<IMvxAppStart>();
              start.Start();

              _setupComplete = true;
          }

          protected override void OnActivated(EventArgs e)
          {
              if (!_setupComplete)
                  DoSetup();

              base.OnActivated(e);
          }
		
          private void LoadMvxAssemblyResources()
          {
              for (var i = 0;; i++)
              {
                  string key = "MvxAssemblyImport" + i;
                  var data = TryFindResource(key);
                  if (data == null)
                      return;
              }
          }
      }
  }

A default FirstView should also exist.

####WindowsStore

On WindowsStore, a new project will again contain a native `App.xaml.cs`

To adapt this for MvvmCross, we simply find the method `OnLaunched` and replace the `if (rootFrame.Content == null)` block with:

```c#
var setup = new Setup(rootFrame);
setup.Initialize();

var start = Cirrious.CrossCore.Mvx.Resolve<Cirrious.MvvmCross.ViewModels.IMvxAppStart>();
start.Start();
```

###Setup.cs

The Setup class is the bootstrapper for the MvvmCross system.

This bootstrapper goes through a lot of steps, and almost all of these are `virtual` allowing you to customise MvvmCross. 

Some key ones you should be aware of are:

- `CreateApplication` - your Setup **must** override this one in order to provide a new instance of your `App` object from your core project
- `InitializeFirstChance` - a "first blood" placeholder for any steps you want to take before any of the later steps happen
- `CreateDebugTrace` - a chance to customise where application trace is placed - see http://stackoverflow.com/a/17234083/373321 for an example
- `InitializeLastChance` - a "last ditch" placeholder for any steps you want to take after all of earlier steps have happened.  Note that the Android and iOS base Setup classes use 'last chance' for initializing the UI data-binding system, so it's important to always call `base.InitializeLastChance()` in your override.

Beyond this, a larger list of Setup customisation options is discussed in https://github.com/slodge/MvvmCross/wiki/Customising-using-App-and-Setup

####Minimal Setup - Android

```c#
using Android.Content;
using Cirrious.MvvmCross.Droid.Platform;
using Cirrious.MvvmCross.ViewModels;

namespace MyName.Droid
{
public class Setup : MvxAndroidSetup
{
    public Setup(Context applicationContext) : base(applicationContext)
    {
    }

    protected override IMvxApplication CreateApp()
    {
        return new Core.App();
    }
}
}
```

####Minimal Setup - iOS

```c#
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Touch.Platform;

namespace MyName.Touch
{
public class Setup : MvxTouchSetup
{
    public Setup(MvxApplicationDelegate applicationDelegate, UIWindow window)
    : base(applicationDelegate, window)
    {
    }

    protected override Cirrious.MvvmCross.ViewModels.IMvxApplication CreateApp ()
    {
        return new Core.App();
    }
}
}
```

####Minimal Setup - WindowsPhone

```c#
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.WindowsPhone.Platform;
using Microsoft.Phone.Controls;

namespace MyName.Phone
{
public class Setup : MvxPhoneSetup
{
    public Setup(PhoneApplicationFrame rootFrame) : base(rootFrame)
    {
    }

    protected override IMvxApplication CreateApp()
    {
        return new Core.App();
    }
}
}
```
    
####Minimal Setup - Wpf

```c#
using System.Windows.Threading;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Wpf.Platform;
using Cirrious.MvvmCross.Wpf.Views;

namespace MyName.Wpf
{
public class Setup : MvxWpfSetup
{
    public Setup(Dispatcher dispatcher, IMvxWpfViewPresenter presenter)
    : base(dispatcher, presenter)
    {
    }

    protected override IMvxApplication CreateApp()
    {
        return new Core.App();
    }

    protected override IMvxTrace CreateDebugTrace()
    {
        return new DebugTrace();
    }
}
}
```
    
####Setup - WindowsStore

```c#
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.WindowsStore.Platform;
using Windows.UI.Xaml.Controls;

namespace MyName.Store
{
public class Setup : MvxStoreSetup
{
    public Setup(Frame rootFrame) : base(rootFrame)
    {
    }

    protected override IMvxApplication CreateApp()
    {
        return new Core.App();
    }
}
}
```

###Views

Each UI Platform needs a set of Views

Each View is normally databound to a single ViewModel for its entire lifetime.

On each platform, Views in the Mvvm sense are typically implemented using data-bound versions of:

- On Windows platforms a `UserControl` - for WindowsStore and WindowsPhone, this is very often specialised into a `Page`
- On Android, an `Activity` or `Fragment`
- On iOS, a `UIViewController`

Within this introduction we won't go further into how these Views are actually written - instead see the introductions to data-binding on each platform within the TipCalc tutorial.

One important thing to note, is that by default `View`s are associated with `ViewModel`s using a naming convention in MvvmCross. This can be overridden if required (see the https://github.com/slodge/MvvmCross/wiki/Customising-using-App-and-Setup#overriding-view-viewmodel-associations) - but by default the MvvmCross system links a View called `FooView` to a ViewModel called `FooViewModel`

###A Presenter

Each UI Platform provides a `Presenter` which implements `IMvxViewPresenter`.

In default applications, the `Presenter` used normally fills the entire screen with a `Page` and allows back button navigation to previous pages.

When more advanced screen layouts are needed - e.g. flyouts, tabs, pivots, split-screens, etc - then these can be supplied by using a custom presenter. For more on this, see http://slodge.blogspot.co.uk/2013/06/presenter-roundup.html

