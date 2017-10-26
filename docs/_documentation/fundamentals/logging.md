---
layout: documentation
title: Diagnostic & Logging
category: Fundamentals
order: 10
---

# Diagnostic & logging

A great option to log messages, info and errors to the console or other outputs is MvxLog! It is very extendeble and easy to use.

In your ViewModel this could look like:

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

More advanced would be:

```c#
public class MyViewModel : MvxViewModel
{
    private readonly IMvxLogProvider _logProvider;
    public MyViewModel(IMvxLogProvider logProvider)
    {
        _logProvider = log;
    }
	
	private void SomeMethod()
	{
		_logProvider.GetLogFor<MyViewModel>().ErrorException("Some message", new Exception())
	}
}
```

# Default logging with MvxLog

MvxLog is based on the Console output. If you want to use more advanced functionalities use one of the other frameworks by simply installing them from nuget.

# Extend it with custom log providers

There is built-in support for quite some popular logging frameworks. These are included:

- EntLibLogProvider
- Log4NetLogProvider
- LoupeLogProvider
- NLogLogProvider
- SerilogLogProvider

To enable them override the `GetDefaultLogProviderType` in your platform projects `Setup.cs`.

```c#
protected override MvxLogProviderType GetDefaultLogProviderType() => MvxLogProviderType.Serilog;
```

If you have your own logging provider or want to implement one, you need to override `protected override IMvxLogProvider CreateLogProvider()` and return a custom.

# Customize the log provider

In the case of Serilog you could do:

```c#
Some code
```

Similair customilization can be done for other providers as well.