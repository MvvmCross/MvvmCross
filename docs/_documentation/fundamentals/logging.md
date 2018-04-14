---
layout: documentation
title: Diagnostic & Logging
category: Fundamentals
order: 7
---

A great option to log messages, info and errors to the console or other outputs is MvxLog! It is very extendable and easy to use.

You just need to inject it in your ViewModel, something like:

```c#
public class MyViewModel : MvxViewModel
{
    private readonly IMvxLog _log;
    public MyViewModel(IMvxLog log)
    {
        _log = log;
    }

    private void SomeMethod()
    {
            _log.Trace("Some message");
    }
}
```

A more advanced case would be:

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

This makes the log context aware.

# Default logging with MvxLog

MvxLog is based on the Console output. If you want to use more advanced functionalities use one of the other frameworks by simply installing them from nuget.

# Log levels

The log levels that are available are:

```c#
public enum MvxLogLevel
{
	Trace,
	Debug,
	Info,
	Warn,
	Error,
	Fatal
}
```

# Extend it with custom log providers

There is built-in support for quite some popular logging frameworks. These ones are included:

- EntLib
- Log4Net
- Loupe
- NLog
- Serilog

To enable any of them override the `GetDefaultLogProviderType` in your platform projects `Setup.cs`.

```c#
protected override MvxLogProviderType GetDefaultLogProviderType() => MvxLogProviderType.Serilog;
```

If you have your own logging provider or want to implement one, you need to override this method on Setup: `protected override IMvxLogProvider CreateLogProvider()` and return your own implementation for it.

# Customize the log provider

In the case of Serilog you could do the following:

```c#
protected override IMvxLogProvider CreateLogProvider()
{
	Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.LiterateConsole()
                .WriteTo.AndroidLog()
                .CreateLogger();
	return base.CreateLogProvider();
}
```

This would install a sink for the Serilog Xamarin extension.

Similar customizations can be done for other providers as well.

# Methods overview

The base interface for all the logging system is:

```c#
public interface IMvxLog
{
	bool Log(MvxLogLevel logLevel, Func<string> messageFunc, Exception exception = null, params object[] formatParameters);
}
```

But we have you covered with some extension methods which are ready to use:

```c#
public static class MvxLogExtensions
{
	public static bool IsDebugEnabled(this IMvxLog logger)

	public static bool IsErrorEnabled(this IMvxLog logger)

	public static bool IsFatalEnabled(this IMvxLog logger)

	public static bool IsInfoEnabled(this IMvxLog logger)

	public static bool IsTraceEnabled(this IMvxLog logger)

	public static bool IsWarnEnabled(this IMvxLog logger)

	public static void Debug(this IMvxLog logger, Func<string> messageFunc)

	public static void Debug(this IMvxLog logger, string message)

	public static void Debug(this IMvxLog logger, string message, params object[] args)

	public static void Debug(this IMvxLog logger, Exception exception, string message, params object[] args)

	public static void DebugFormat(this IMvxLog logger, string message, params object[] args)

	public static void DebugException(this IMvxLog logger, string message, Exception exception)

	public static void DebugException(this IMvxLog logger, string message, Exception exception, params object[] formatParams)

	public static void Error(this IMvxLog logger, Func<string> messageFunc)

	public static void Error(this IMvxLog logger, string message)

	public static void Error(this IMvxLog logger, string message, params object[] args)

	public static void Error(this IMvxLog logger, Exception exception, string message, params object[] args)

	public static void ErrorFormat(this IMvxLog logger, string message, params object[] args)

	public static void ErrorException(this IMvxLog logger, string message, Exception exception, params object[] formatParams)

	public static void Fatal(this IMvxLog logger, Func<string> messageFunc)

	public static void Fatal(this IMvxLog logger, string message)

	public static void Fatal(this IMvxLog logger, string message, params object[] args)

	public static void Fatal(this IMvxLog logger, Exception exception, string message, params object[] args)

	public static void FatalFormat(this IMvxLog logger, string message, params object[] args)

	public static void FatalException(this IMvxLog logger, string message, Exception exception, params object[] formatParams)

	public static void Info(this IMvxLog logger, Func<string> messageFunc)

	public static void Info(this IMvxLog logger, string message)

	public static void Info(this IMvxLog logger, string message, params object[] args)

	public static void Info(this IMvxLog logger, Exception exception, string message, params object[] args)

	public static void InfoFormat(this IMvxLog logger, string message, params object[] args)

	public static void InfoException(this IMvxLog logger, string message, Exception exception, params object[] formatParams)

	public static void Trace(this IMvxLog logger, Func<string> messageFunc)

	public static void Trace(this IMvxLog logger, string message)

	public static void Trace(this IMvxLog logger, string message, params object[] args)

	public static void Trace(this IMvxLog logger, Exception exception, string message, params object[] args)

	public static void TraceFormat(this IMvxLog logger, string message, params object[] args)

	public static void TraceException(this IMvxLog logger, string message, Exception exception, params object[] formatParams)

	public static void Warn(this IMvxLog logger, Func<string> messageFunc)

	public static void Warn(this IMvxLog logger, string message)

	public static void Warn(this IMvxLog logger, string message, params object[] args)

	public static void Warn(this IMvxLog logger, Exception exception, string message, params object[] args)

	public static void WarnFormat(this IMvxLog logger, string message, params object[] args)
}
```

All log providers implement `IMvxLogProvider`:

```c#
public interface IMvxLogProvider
{
	IMvxLog GetLogFor<T>();

	IMvxLog GetLogFor(string name);

	IDisposable OpenNestedContext(string message);

	IDisposable OpenMappedContext(string key, string value);
}
```
