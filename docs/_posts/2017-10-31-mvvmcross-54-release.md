---
layout: post
title: MvvmCross 5.4
date:   2017-10-31 14:00:00 +0100
categories: mvvmcross
---

# Announcing MvvmCross 5.4!

A new MvvmCross version is available on [NuGet](https://www.nuget.org/packages/MvvmCross/5.4.0)! You can always find the latest [changelog in the root of the repository](https://github.com/MvvmCross/MvvmCross/blob/develop/CHANGELOG.md) to see what has changed.

## Forms
We have had a couple of fixes for our Xamarin.Forms support
- navigation between native and Forms views has been improved
- fixes crashes when navigating between Pages with `WrapInNavigationPage` set
- when `WrapInNavigationPage` was set, title was missing
- binding markup extensions have been fixed so that you can use `Target="mvx:MvxBind Source` and `Target="mvx:MvxLang LocalizationKey"`
- design time checker added for MvvmCross bindings
- on Android you can now provide your own Resource Assembly if the one we guess for you is wrong. See the [documentation for more information on how to override this behavior](https://www.mvvmcross.com/documentation/platform/forms/xamarin-forms-customization)

## iOS
- Fixes in Sidebar navigation initialization where hamburger icon sometimes disappeared

## Android
- Added missing constructor in generic version of `MvxDialogFragment`

# New Logging interface

The reason to bump minor semver this time was that we have added a new Logging interface deprecating the old `IMvxTrace`. Upgrading to MvvmCross 5.4.0 you will start getting warnings in your project that `Mvx.Trace` and the likes are deprecated.

We now provide `IMvxLog` and `IMvxLogProvider` interfaces. You can inject them in your classes where needed and use this for logging. 

```c#
public class MyViewModel : MvxViewModel
{
    private readonly IMvxLog _log;
    public MyViewModel(IMvxLogProvider logProvider)
    {
        _log = logProvider.GetLogFor<MyViewModel>();
    }
	
	private void SomeMethod()
	{
		_log.ErrorException("Some message", new Exception());
	}
}
```

We also provide a bunch of custom log providers for common logging libraries, such as:

- EntLib
- Log4Net
- Loupe
- NLog
- Serilog

You can provide your log provider by overriding `GetDefaultLogProviderType()` in `Setup.cs`.

You can read more about [how to use these new interfaces in our documentation](https://www.mvvmcross.com/documentation/fundamentals/logging).