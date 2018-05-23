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

- A typical App.cs file would look like:

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

- To use a custom AppStart class change it to:

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

- Now create an `AppStart.cs` file and add the following code:

```c#
public class AppStart<TViewModel> : MvxAppStart
	where TViewModel : IMvxViewModel
{
	private readonly IAuthenticationService _authenticationService;

	public MvxAppStart(IMvxApplication application, IMvxNavigationService navigationService, IAuthenticationService authenticationService) : base(application, navigationService)
	{
		_authenticationService = authenticationService;
	}
	
	protected override void Startup(object hint = null)
	{
		// You need to run Task sync otherwise code would continue before completing.
		var tcs = new TaskCompletionSource<bool>();
		Task.Run(async () => tcs.SetResult(await _authenticationService.IsAuthenticated()));
		var isAuthenticated = tcs.Task.Result;

		if (isAuthenticated)
		{
			TViewModel = typeof(HomeViewModel);
		}
		else
		{
			TViewModel = typeof(LoginViewModel);
		}
		base.Startup(hint);
	}
}
```

**Note:** For situations where the app is launched using a protocol - e.g. from a Push notification or from an email link - then the object hint parameter start can be used to transfer a hint from the UI to the start object. Currently, it's up to you - the app developer - to write the UI side code to do this.
