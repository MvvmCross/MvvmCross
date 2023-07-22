---
layout: documentation
title: ResxLocalization
category: Plugins
---
## Introduction

The `ResxLocalization` plugin provides a support class to help use RESX files for internationalization (i18n).

The `ResxLocalization` plugin is a single .NET Standard Assembly and isn't really a typical plugin - it doesn't itself register any singletons or services with the MvvmCross IoC container.

## Setup

* Add the [MvvmCross.Plugin.ResxLocalization](https://www.nuget.org/packages/MvvmCross.Plugin.ResxLocalization) nuget to your core project.

* Create your resource files (.resx). The [Microsoft documentation](https://docs.microsoft.com/en-us/xamarin/xamarin-forms/app-fundamentals/localization/text?pivots=macos) covers this in the `Create Resx files` section.

* Register your Resx file(s) in your MvxApplication class:
```C#
public override void Initialize()
{
   Mvx.IoCProvider.RegisterSingleton<IMvxTextProvider>(new MvxResxTextProvider(Strings.ResourceManager));

   // Note: The MvxResxText Provider can also accept an array of ResourceManagers

   //additional service registrations

   RegisterAppStart<MainViewModel>();
}
```

* Add the `IMvxLocalizedTextSourceOwner` interface to your ViewModels. We recommend adding this to a base ViewModel class.
```C#
public abstract class BaseViewModel : MvxViewModel, IMvxLocalizedTextSourceOwner
{
   public IMvxLanguageBinder LocalizedTextSource => new MvxLanguageBinder("", GetType().Name);
}
```

## Binding

As long as your ViewModel implements the `IMvxLocalizedTextSourceOwner` you can use the extension method `ToLocalizationId()` in fluent data binding.

```c#
bindingSet.Bind(TextBox).For(v => v.Text).ToLocalizationId("Description");
```

## Additional Reading
For more advice on using the Localization library, see the [blog post](https://mobileprogrammerblog.wordpress.com/2017/12/30/mvvm-cross-with-xamarin-platform-resx-localization) by [@DKrzyczkowski](https://twitter.com/@DKrzyczkowski/).

There is also this [blog post](http://opendix.blogspot.ch/2013/05/using-resx-files-for-localization-in.html) by [@stefanschoeb](https://twitter.com/stefanschoeb). The `ResxTextProvider` he describes is now contained in the `ResxLocalization` plugin as `MvxResxTextProvider`.
