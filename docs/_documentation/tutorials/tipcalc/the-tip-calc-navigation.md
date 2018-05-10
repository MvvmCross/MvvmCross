---
layout: documentation
title: Extra - Navigation
category: Tutorials
order: 10
---

This article will cover some of the techniques available within MvvmCross for navigating between Page-level **ViewModels**.

### What do we mean by Page?

MvvmCross was born for making modern Mobile apps - for building apps for iPhone, Android, and windows. Over the years the community expanded the framework to make it usable on many more platforms like macOS, watchOS or even Xamarin.Forms!

Since usually Apps have more than one screen - pages - there is normally a need for navigation between them, which often involves the user experience moving forwards and backwards through the application workflow.

The most common scenario for UIs consist on a single page at a time. But there are variations on this, especially for tabbed or splitted user interfaces. There are also dialogs and other several approaches.

## The initial navigation

On the TipCalc tutorial, we added a very special line to our `App.cs`, which indicates which `ViewModel` should be the very first to appear on the screen:

```c#
RegisterAppStart<TipViewModel>();
```

That line can be also replaced by this code, since that's what it does internally:

```c#
Mvx.RegisterSingleton<IMvxAppStart>(new MvxAppStart<TipViewModel>());
```

The only exception to the rule is, in terms of AppStart, Android. This is because Android requires the application to indicate which Activity will be the first to show up. We are doing so through the `MainLauncher=true` property value within the `[Activity]` attribute.

### How to add a splashscreen to the Android app

In case you want to display something fancy while MvvmCross and your app is loaded, you can follow these steps:

1. Remove the `MainLauncher=true` property from any existing `Activity` attributes.

2. Add some simple axml for a splashscreen. For example, a very simple screen might be:

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

Note that this splashscreen will be displayed before the MvvmCross system is fully booted - so you **cannot** use data-binding within the splashscreen axml.

3. Add a simple Activity for the splashscreen. This will contain C# like:

```c#
using Android.App;
using MvvmCross.Droid.Views;

namespace CalcApp.UI.Droid
{
    [Activity(Label = "My Tip Calc", MainLauncher = true, NoHistory = true, Icon = "@drawable/icon")]
    public class SplashScreenActivity : MvxSplashScreenActivity
    {
        public SplashScreenActivity()
            : base(Resource.Layout.SplashScreen)
        {
        }
    }
}
```

This `SplashScreenActivity` uses the base `MvxSplashScreenActivity` which implements `IMvxSetupMonitor` to start the MvvmCross framework and call `IMvxAppStart` when initialization is complete.

### Supporting a more advanced startup 

The `TipCalc` app has a very simple startup instruction, which doesn't handle any logic.

This was an instruction to: **always start the app with a `TipViewModel`**.

If you wanted to start with some logic - suppose you need to display a `LoginViewModel` or a `MainViewModel` in different situations - then you can do this by providing a custom `IMvxAppStart` implementation - e.g.:

```c#
public class CustomAppStart : MvxAppStart
{
    private readonly IMvxNavigationService _mvxNavigationService;
    private readonly ILoginService _loginService;

    public CustomAppStart(IMvxApplication app, 
                    IMvxNavigationService mvxNavigationService,
                    ILoginService loginService)
        : base(app)
    {
        _mvxNavigationService = mvxNavigationService;
        _loginService = loginService;
    }

    protected override void Startup(object hint = null)
    {
        if(_loginService.IsLoggedIn)
        {
            _mvxNavigationService.Navigate<MainViewModel>();
        }
        else
        {
            _mvxNavigationService.Navigate<LoginViewModel>();
        }  
    }
}
```

Notice that to request this initial navigation, the `CustomAppStart` uses the `IMvxNavigationService` to navigate through the pages. We will see more about this throughout this article - `IMvxNavigationService` is the core of the MvvmCross navigation mechanism.

If you wanted to do even more here, e.g. if you wanted to use parameters passed to the app from the operating system, then this is also possible - these can be passed from the platform-specific startup code to the `IMvxAppStart.Startup(object hint)`.

_Hint: There is a method called `GetAppStartHint(object hint = null)` on every platform specific 'App' class which can be used to set the hint parameter.

## Simple Navigation between Page-level ViewModels

When your app is displaying a `ViewModel` page, say `FirstViewModel`, then that first page can request that the display is moved forwards to a new `ViewModel` page, say `SecondViewModel` by using a call like:

```c#
_mvxNavigationService.Navigate<SecondViewModel>();
```

When the `FirstViewModel` makes this request, then the MvvmCross framework will:

- Locate a `View` to use as a page for `SecondViewModel` within the app - normally this will be `SecondView`.
- Create a new instance of this `SecondView`.
- Create a `SecondViewModel` and provide it as the `DataContext` for the new `SecondView`.
- Ask the platform ViewPresenter to display the `SecondView` in the most appropiate way.
- Make the ViewPresenter do the actual navigation.

### In action - an Android app

To see an example of this, let's set up a simple Android application.

1. Create a new _Core_, .NET Standard Library - exactly as we did in the `TipCalc` example.

2. Within this _Core_ application add two `ViewModel`s:

```c#
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace TipCalc.Core.ViewModels
{
    public class FirstViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        public FirstViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

            NavigateCommand = new MvxAsyncCommand(() => _navigationService.Navigate<SecondViewModel>());
        }

        public IMvxAsyncCommand NavigateCommand { get; private set; }
    }

    public class SecondViewModel : MvxViewModel
    {
    }
}
```

3. For `IMvxAppStart` choose to always show the `FirstViewModel` adding this line on the `App` class:

```c#
RegisterAppStart<FirstViewModel>();
```

4. Create an Android UI for this app - just as we did in the `TipCalc` sample.

5. Add simple Android views for both `FirstView` and `SecondView`.

6. For `FirstView` include a button - and bind it's `Click` event to the `NavigateCommand` within the `FirstViewModel`

```xml
    <?xml version="1.0" encoding="utf-8"?>
    <LinearLayout
      xmlns:android="http://schemas.android.com/apk/res/android"
      xmlns:local="http://schemas.android.com/apk/res/MyApp.UI.Droid"
      android:layout_width="match_parent"
      android:layout_height="match_parent">
    	<Button
    	  android:layout_width="match_parent"
    	  android:layout_height="wrap_content"
    	  android:text="Go"
    	  android:textSize="40dp"
    	  local:MvxBind="Click NavigateCommand"/>
    </LinearLayout>
```
 
7. For `SecondView` include only some 'simple' text:

```xml
	<?xml version="1.0" encoding="utf-8"?>
	<LinearLayout
	  xmlns:android="http://schemas.android.com/apk/res/android"
	  xmlns:local="http://schemas.android.com/apk/res/MyApp.UI.Droid"
	  android:layout_width="match_parent"
	  android:layout_height="match_parent">
		<TextView
		  android:layout_width="match_parent"
		  android:layout_height="wrap_content"
		  android:text="This is the Second View"
		  android:textSize="40dp"/>
	</LinearLayout>
```
 
8. As discussed above in 'The initial navigation' add a SplashScreen for this Droid app.

When this application runs, you should see a simple UI for `FirstView` with a `FirstViewModel` data context, and when you press the 'Go' button, you should see the display shift to a `SecondView` with a `SecondViewModel` data context.

## Navigation with parameters - using a parameter object

As you write apps, you may frequently find that you want to parameterize a `ViewModel` navigation.

For example, you may encounter List-Detail situations - where:

- The Master view shows a list of items. 
- When the user selects one of these, then the app will navigate to a Detail view 
- The Detail view will then show that specific selected item.

To achieve this, the navigation from `MasterViewModel` to `DetailViewModel` will normally be achieved by:

- We declare a class named `DetailNavigationArgs` for the navigation:

```c#
public class DetailNavigationArgs
{
    public int Index { get; set; }
}
```

- the `MasterViewModel` performs a navigaton with a call like this:

```c#
_navigationService.Navigate<DetailViewModel, DetailNavigationArgs>(new DetailNavigationArgs { Index = 2});
```

- The `DetailViewModel` class declaration should then be:

```c#
public class DetailViewModel : MvxViewModel<DetailNavigationArgs>
```

And you should override a method named `Prepare` to receive the parameter:

```c#
public void Prepare(DetailNavigationArgs parameter)
{
    // use the parameters here!
}
```

## Navigating for result and more advanced scenarios

If you want to learn more about how navigation in MvvmCross works, we highly recommend you to read the [official article](https://www.mvvmcross.com/documentation/fundamentals/navigation) which covers the main aspects (async/await, navigating for result, url navigation, ...).
