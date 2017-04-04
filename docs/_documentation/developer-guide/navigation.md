---
layout: documentation
title: Navigation
category: Developer-guide
---
This article will cover some of the techniques available within MvvmCross for navigating between Page-level **ViewModels**.

### What do we mean by Page?

MvvmCross was born for making modern Mobile apps - for building apps for iPhone, Android, and Windows Phone.

These apps are generally Page-based - that is they generally involve User-Interfaces which show a single page at a time and which often involve the user experience moving forwards and backwards through the application workflow.

There are variations on this, especially for Tabbed or Pivoting user interfaces; for Dialogs; and for Split displays. We'll introduce some of these briefly at the end of this article.

## The initial navigation

In the TipCalc walkthough, we built most of our initial MvvmCross applications to use the `IMvxAppStart` interface as a starting mechanism.

An implementation of this interface was registered by the core `App` like:

     Mvx.RegisterSingleton<IMvxAppStart>(new MvxAppStart<TipViewModel>());

This implementation was then used in the `AppDelegate` and `App.Xaml` start sequences within the UI projects.

The exception was Android - where we explicitly specified one of our Activities/Views with the `MainLauncher=true` property value within the `[Activity]` attribute.

To use the `IMvxAppStart` instruction in Android as well, the easiest way is to add a splashscreen which will be displayed briefly while the application starts.

### Adding a splashscreen to an Android app

To add a splashscreen:

1. Remove the `MainLauncher=true` property from any existing `Activity` attributes.

2. Add some simple AXML for a splashscreen. For example, a very simple screen might be:

    ```xml
    <?xml version="1.0" encoding="utf-8"?>
    <FrameLayout xmlns:android="http://schemas.android.com/apk/res/android"
        android:layout_width="fill_parent"
        android:layout_height="fill_parent">
        <TextView
            android:layout_width="wrap_content"
            android:layout_height="wrap_content"
            android:gravity="center"
            android:text="loading..." />
    </FrameLayout>
    ```

   Note that this splashscreen will be displayed before the MvvmCross system is fully booted - so you **cannot** use data-binding within the splashscreen AXML.

3. Add a simple Activity for the splashscreen. This will contain C# like:

    ```cs
    using Android.App;
    using Cirrious.MvvmCross.Droid.Views;

    namespace CalcApp.UI.Droid
    {
        [Activity(Label = "My App", MainLauncher = true, NoHistory = true, Icon = "@drawable/icon")]
        public class SplashScreenActivity
            : MvxSplashScreenActivity
        {
            public SplashScreenActivity()
                : base(Resource.Layout.SplashScreen)
            {
            }
        }
    }
    ```

  This `SplashScreenActivity` uses the base `MvxSplashScreenActivity` which will start the MvvmCross framework and, when initialization is complete, will then use the `IMvxAppStart` interface.

### Supporting more advanced startup 

The `TipCalc` app has a very simple startup instruction:

`Mvx.RegisterSingleton<IMvxAppStart>(new MvxAppStart<TipViewModel>()); `

This was an instruction to: **always start the app with a `TipViewModel`**

If you wanted instead to start with a different `ViewModel` - e.g. with `LoginViewModel` then you'd have to replace this with:

`Mvx.RegisterSingleton<IMvxAppStart>(new MvxAppStart<LoginViewModel>());`

If you wanted instead to start with some logic, then you can do this by providing a custom `IMvxAppStart` implementation - e.g.:

```cs
    public class CustomAppStart 
        : MvxNavigatingObject
        , IMvxAppStart
    {
        private readonly ILoginService _service;

        public CustomAppStart(ILoginService service)
        {
            _service = service;
        }

        public void Start(object hint = null)
        {
            if (!_service.IsLoggedIn)
            {
                ShowViewModel<LoginViewModel>();
            }
            else
            {
                ShowViewModel<TipViewModel>();
            }
        }
    }
```

Notice that to request this initial navigation, the `CustomAppStart` uses the `ShowViewModel<TViewModel>` method on the `MvxNavigatingObject` base class. We'll see this method used throughout this article - it is the core of the MvvmCross navigation mechanism.

If you wanted to do even more here, e.g. if you wanted to use parameters passed to the app from the operating system, then this is also possible - these can be passed from the platform-specific startup code to the `IMvxAppStart.Start(object hint)` method using the `hint` and can then also be passed on to the displayed `ViewModel`.

## Simple Navigation between Page-level ViewModels

When your app is displaying a `ViewModel` page, say `FirstViewModel`, then that first page can request that the display is moved forwards to a new `ViewModel` page, say `SecondViewModel` by using a call like:

     ShowViewModel<SecondViewModel>();

When the `FirstViewModel` makes this request, then the MvvmCross framework will:

- locate a View to use as a page for `SecondViewModel` within the app - normally this will be `SecondView`
- create a new instance of this `SecondView`
- create a `SecondViewModel` and provide it as the `DataContext` for the new `SecondView`
- ask the operating system to display the `SecondView`

### In action - an Android app

To see an example of this, let's set up a simple Android application.

1. Create a Core Portable Class Library application - exactly as we did in the `TipCalc` example.

2. Within this Core application add two `ViewModel`s:

    ```cs
	using System;
	using System.Windows.Input;
	using Cirrious.CrossCore.Platform;
	using Cirrious.MvvmCross.ViewModels;

	namespace MyApp.Core
	{
		public class FirstViewModel : MvxViewModel
		{
			public ICommand GoCommand
			{
				get
				{
					return new MvxCommand(() => ShowViewModel<SecondViewModel>();
				}
			}
		}		

		public class SecondViewModel : MvxViewModel
		{
		}
	}
    ```		

3. For `IMvxAppStart` choose to always show the `FirstViewModel` using:

    `Mvx.RegisterSingleton<IMvxAppStart>(new MvxAppStart<FirstViewModel>());`

4. Create an Android UI for this app - just as we did in the `TipCalc` sample

5. Add simple Android views for both `FirstView` and `SecondView`.

6. For `FirstView` include a button - and bind it's `Click` event to the `GoCommand` within the `FirstViewModel`

    ```xml
    <?xml version="1.0" encoding="utf-8"?>
    <LinearLayout
      xmlns:android="http://schemas.android.com/apk/res/android"
      xmlns:local="http://schemas.android.com/apk/res/MyApp.UI.Droid"
      android:layout_width="fill_parent"
      android:layout_height="fill_parent"
      >
    	<Button
    	  android:layout_width="fill_parent"
    	  android:layout_height="wrap_content"
    	  android:text="Go"
    	  android:textSize="40dp"
    	  local:MvxBind="Click GoCommand"
    	  />
    </LinearLayout>
    ```
 
7. For `SecondView` include only some 'simple' text:

    ```xml
	<?xml version="1.0" encoding="utf-8"?>
	<LinearLayout
	  xmlns:android="http://schemas.android.com/apk/res/android"
	  xmlns:local="http://schemas.android.com/apk/res/MyApp.UI.Droid"
	  android:layout_width="fill_parent"
	  android:layout_height="fill_parent"
	  >
		<TextView
		  android:layout_width="fill_parent"
		  android:layout_height="wrap_content"
		  android:text="This is the Second View"
		  android:textSize="40dp"
		  />
	</LinearLayout>
    ```
 
8. As discussed above in 'The initial navigation' add a SplashScreen for this Droid app.

When this application runs, you should see a simple UI for `FirstView` with a `FirstViewModel` data context, and when you press the 'Go' button, you should see the display shift to a `SecondView` with a `SecondViewModel` data context

## Navigation with parameters - using a parameter object

As you write apps, you may frequently find that you want to parameterize a `ViewModel` navigation.

For example, you may encounter List-Detail situations - where:

- The Master view shows a list of items. 
- When the user selects one of these, then the app will navigate to a Detail view 
- The Detail view will then shows that specific selected item.

To achieve this, the navigation from `MasterViewModel` to `DetailViewModel` will normally be achieved by:

- we declare a class `DetailParameters` for the navigation:

    ```cs
    public class DetailParameters
    {
        public int Index { get; set; }
    }
    ```

- the `MasterViewModel` makes `ShowViewModel` a call like:

    `ShowViewModel<DetailViewModel>(new DetailParameters() { Index = 2 });`

- the `DetailViewModel` declares an `Init` method in order to receive this `DetailParameters`:

    ```cs
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

The reason for this limitations are that the navigation object itself needs to be serialized - it needs to be passed through mechanisms like Xaml urls on WindowsPhone, and like Intent parameter bundles on Android.

If you do ever want to pass more complex objects between ViewModels during navigation, then you will need to find an alternative mechanism - e.g. perhaps caching the object in SQLite and using an index to identify the object.

## In action - an iOS example

TODO

## Navigation with parameters - using an anonymous parameter object

For simple navigations, declaring a formal `Parameters` object can feel like 'overkill' - like 'hard work'.

In these situations you can instead use anonymous classes and named method arguments.

For example, you can:

- use a call to `ShowViewModel` like:

    `ShowViewModel<DetailViewModel>(new { index = 2 });`

- in the `DetailViewModel` declare an `Init` method in order to receive this `index` as:

    ```cs
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

**Note** that in order to use this technique on Windows platforms, you will need to add a `InternalsVisibleTo` line within the `AssemblyInfo.cs` file for the Core project.

    `[assembly: InternalsVisibleTo("Cirrious.MvvmCross")]`

This is because anonymous classes within C# are `internal` by default - so Cirrious.MvvmCross can only use reflection on them if `InternalsVisibleTo` is specified.

##In action - a WindowsPhone example

TODO

##How to move 'back'?

TODO

###How do I remove ViewModels from the back stack?

TODO

##What if I don't want 'Pages'?

TODO

###Tabs
###Navigation within Tabs
###Modal Windows
###Dialogs

TODO
