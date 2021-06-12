---
layout: documentation
title: Diagnostic & Logging
category: Fundamentals
permaling: logging-8
order: 7
---

> If you are looking for the documentation for MvvmCross 7 and lower, go to the [Diagnostics & Logging MvvmCross 7 documentation][old-doc].

Since MvvmCross 8, we have switched to using [Microsoft.Extensions.Logging][dotnet-logging] instead of rolling our own interface for logging.
This is a nice and stable API and has support for multiple third party logging providers. Similarly to what we had before with `IMvxLog` and `IMvxLogProvider`, just without the reflection to not directly depend on the third party providers.

You will be able to inject either a `ILogger<T>` or `ILoggerFactory` into your classes resolved through the MvvmCross IoC provider. This could be for ViewModels, Services, Repositories etc. Normal usage would look something like:

```csharp
public class MyViewModel : MvxViewModel
{
    private ILogger<MyViewModel> _logger;

    public MyViewModel(ILogger<MyViewModel> logger)
    {
        _logger = logger;

        _logger.Log(LogLevel.Trace, "Hello, World");
    }
}
```

This will automatically scope the logging to the specific ViewModel such that it is easier to filter etc. If you are not interested in scoping your log, you can use `ILoggerFactory` to create a non-scoped log or to create multiple loggers as needed.

As a minimum you will need to provide implementations for two interfaces in your `MvxSetup` class, `ILoggerProvider` and `ILoggerFactory`, these are necessary to plumb the logging
infrastuctore Microsoft.Extensions.Logging provides.

## Serilog example

You can install third party providers such as [Serilog][serilog], to customize your logs and support structured logging and more.

Install the [`Serilog`][serilog-nuget] and [`Serilog.Extensions.Logging`][serilog-mel] NuGet packages. Then implement `CreateLogProvider` and `CreateLogFactory` in your `Setup.cs`:

```csharp
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
```

You can add Serilog sinks to log to the Android, iOS, Debug, Files and many more places to log to.

[serilog]: https://serilog.net/
[serilog-nuget]: https://www.nuget.org/packages/Serilog
[serilog-mel]: https://www.nuget.org/packages/Serilog.Extensions.Logging
[old-doc]: {{ site.baseurl }}{% link _documentation/fundamentals/logging.md %}
[dotnet-logging]: https://docs.microsoft.com/en-us/dotnet/core/extensions/logging?tabs=command-line