---
layout: documentation
title: Upgrade from 7 to 8
category: Upgrading
Order: 4
---

> If you have other migration issues than described here. Please chime in on [this issue on GitHub][gh-docs-issue].

## Explicit Setup.cs needed

A breaking change in MvvmCross 8 is that you now explicitly have to implement your own MvxSetup derivative and implement two logging related methods. MvvmCross doesn't provide any defaults for this.

> An example of changes necessary to upgrade the TipCalc project from 7.1.2 to 8.0.1 can be found [here in this Pull Request][upgrade-pr].

Here is a non-exhaustive list of platform specific `MvxSetup` classes you can use

| Platform      | Class Name           |
|---------------|----------------------|
| iOS           | MvxIosSetup          |
| Android       | MvxAndroidSetup      |
| WPF           | MvxWpfSetup          |
| UWP           | MvxWindowsSetup      |
| Mac           | MvxMacSetup          |
| iOS Forms     | MvxFormsIosSetup     |
| Android Forms | MvxFormsAndroidSetup |

You will need to create your own `Setup.cs` class per platform as described in the [logging documentation][logging-doc]. If you don't care about logging, you _should_ be able to just return `null` in the two methods. However, it hasn't been tested thoroughly.

```csharp
public class Setup : MvxAndroidSetup<App>
{
    protected override ILoggerProvider CreateLogProvider()
    {
        return new SerilogLoggerProvider();
    }

    protected override ILoggerFactory CreateLogFactory()
    {
        // serilog configuration
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            // add more sinks here
            .CreateLogger();

        return new SerilogLoggerFactory();
    }
}
```

Then instead of registering which setup class to use in your `Application` class you specify your new `Setup` class instead:

```csharp
public class MainApplication : MvxAndroidApplication<Setup, App>
```

## Changes to method signatures in Setup.cs

If you have overridden some of the methods in a `Setup.cs` file already, you might notice that some methods have changed signature. This is intentionally to ease work in the future, if we decide to switch out the `Mvx.IoCProvider` static or switch to a different IoC Container. So now a lot of methods pass in `IMvxIoCProvider` as argument.

An example of what changes you can expect:

```diff
- protected virtual IMvxChildViewModelCache CreateViewModelCache()
+ protected virtual IMvxChildViewModelCache CreateViewModelCache(IMvxIoCProvider iocProvider)
```

## MvxNavigationService constructor requires IMvxIoCProvider

`MvxNavigationService` now requires an instance of `IMvxIoCProvider` as argument. It was previously abusing `Mvx.IoCProvider` to acquire an instance of `IMvxViewsContainer` lazily. This is a bit better now, we might consider changing this even further in the future simply requiring as an argument in the constructor as well. This eases testing significantly as `IMvxIoCProvider` now can be mocked:

```diff
- public MvxNavigationService(
-    IMvxViewModelLoader viewModelLoader,
-    IMvxViewDispatcher viewDispatcher)
+ public MvxNavigationService(
+    IMvxViewModelLoader viewModelLoader,
+    IMvxViewDispatcher viewDispatcher,
+    IMvxIoCProvider iocProvider)
```

## MvxBindingBuilder methods now pass in IMvxIoCProvider

`MvxBindingBuilder` and subclasses have changed some method signatures to now pass along `IMvxIoCProvider`.

```diff
- public virtual void DoRegistration()
+ public virtual void DoRegistration(IMvxIoCProvider iocProvider)

- protected virtual void InitializeLayoutInflation()
+ protected virtual void InitializeLayoutInflation(IMvxIoCProvider iocProvider)

- protected virtual void InitializeBindingResources()
+ protected virtual void InitializeBindingResources(IMvxIoCProvider iocProvider)

- protected override void RegisterPlatformSpecificComponents()
+ protected override void RegisterPlatformSpecificComponents(IMvxIoCProvider iocProvider)

- protected virtual void InitializeContextStack()
+ protected virtual void InitializeContextStack(IMvxIoCProvider iocProvider)

- protected virtual void InitializeViewTypeResolver()
+ protected virtual void InitializeViewTypeResolver(IMvxIoCProvider iocProvider)
```

[gh-docs-issue]: https://github.com/MvvmCross/MvvmCross/issues/4201
[upgrade-pr]: https://github.com/MvvmCross/MvvmCross-Samples/pull/159
[logging-doc]: {{ site.baseurl }}{% link _documentation/fundamentals/logging-new.md %}
[old-doc]: {{ site.baseurl }}{% link _documentation/fundamentals/logging.md %}