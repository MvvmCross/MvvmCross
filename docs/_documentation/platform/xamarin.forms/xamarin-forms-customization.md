---
layout: documentation
title: Xamarin.Forms Startup Customization
category: Platforms
---

# Android Resources

Xamarin.Forms relies on you to pass along the `Assembly` where your resources are contained
in its `Init` method. Using the signature where you do not pass on this `Assembly`,
Xamarin.Forms uses `Assembly.GetCallingAssembly()` to guess it. This means if you subclass `MvxFormsApplicationActivity` or 
`MvxFormsAppCompatActivity` in another Assembly than the calling assembly, the wrong assembly
gets passed on to Xamarin.Forms and it cannot find resources. This will result in missing 
icons and more in your App.

You can customize this behavior by overriding `GetResourceAssembly()` in your subclass:

```csharp
protected override Assembly GetResourceAssembly()
{
    // return your Assembly here
}
```

A very simple assumption could be that your `Setup.cs` file is always in your Application
project and you could just write: `return typeof(Setup).Assembly;`.
