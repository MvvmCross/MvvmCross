---
layout: documentation
title: Upgrade from MvvmCross 7 to MvvmCross 8
category: Upgrading
Order: 4
---

> If you have other migration issues than described here. Please chime in on [this issue on GitHub][gh-docs-issue].

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

[gh-docs-issue]: https://github.com/MvvmCross/MvvmCross/issues/4201
[upgrade-pr]: https://github.com/MvvmCross/MvvmCross-Samples/pull/159
[logging-doc]: [old-doc]: {{ site.baseurl }}{% link _documentation/fundamentals/logging-new.md %}