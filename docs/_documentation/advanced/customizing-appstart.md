---
layout: documentation
title: Customizing AppStart
category: Advanced
order: 6
---

# Custom IMvxAppStart

When an MvvmCross application starts by default it shows the View associated with a single ViewModel type.

This default behaviour is configured in Initialize in App.cs using:
```c#
RegisterAppStart<FirstViewModel>();
```

Sometimes you want to do custom checks or logic when starting up your app. To do that you need to change a couple of things.

## A typical App.cs file would look like:

```c#
public class App : MvxApplication
{
	public override void Initialize()
	{
		CreatableTypes()
			.EndingWith("Service")
			.AsInterfaces()
			.RegisterAsLazySingleton();
			
		RegisterAppStart<LoginViewModel>();
	}
}
```

## To use a custom AppStart class change it to:

```c#
public class App : MvxApplication
{
	public override void Initialize()
	{
		CreatableTypes()
			.EndingWith("Service")
			.AsInterfaces()
			.RegisterAsLazySingleton();
			
		RegisterCustomAppStart<AppStart>();
	}
}
```

## Now create an `AppStart.cs` file and add the following code:

```c#
public class AppStart : MvxAppStart
{
	private readonly IAuthenticationService _authenticationService;

	public MvxAppStart(IMvxApplication application, IMvxNavigationService navigationService, IAuthenticationService authenticationService) : base(application, navigationService)
	{
		_authenticationService = authenticationService;
	}
	
	protected override void NavigateToFirstViewModel(object hint)
	{
		try
		{
			// You need to run Task sync otherwise code would continue before completing.
			var tcs = new TaskCompletionSource<bool>();
			Task.Run(async () => tcs.SetResult(await _authenticationService.IsAuthenticated()));
			var isAuthenticated = tcs.Task.Result;

			if (isAuthenticated)
			{
				NavigationService.Navigate<HomeViewModel>().GetAwaiter().GetResult();
			}
			else
			{
				NavigationService.Navigate<LoginViewModel>().GetAwaiter().GetResult();
			}
		}
		catch (System.Exception exception)
		{
			throw exception.MvxWrap("Problem navigating to ViewModel {0}", typeof(TViewModel).Name);
		}
	}
}
```

## Launching using a protocol - e.g. from a Push notification or from an email link

`MvxAppStart` will pass through any object received on startup to the `MvxApplication` class. There you can manipulate it or take any actions you like. It will not send the hint to the ViewModel, so use the typed `MvxAppStart<TParameter>` for that.

```c#
public class App : MvxApplication
{
	public override object Startup(object hint)
	{
		var parameter = base.Startup(hint);
		//Do custom logic with the hint received.
		return parameter;
	}
}
```

There is also a typed version of the `Startup` available. To use that your App class needs to extend `MvxApplication<TParameter>` where `TParameter` is the type you expect to receive from the operating system. Note that the ViewModel you navigate to needs to extend `IMvxViewModel<TParameter>`

```c#
public class App : MvxApplication<TParameter>
{
	public override void Initialize()
	{
		RegisterAppStart<SomeViewModel, TParameter>();
	}

	public override TParameter Startup(TParameter parameter)
	{
		//Do custom logic with the hint received.
		return parameter;
	}
}
```
