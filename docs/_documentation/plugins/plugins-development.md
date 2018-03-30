---
layout: documentation
title: Plugins development
category: Plugins
---

Starting from version 6.0, plugin development has become much simpler. All you have to do in order to create your own plugins is: 

- Creating a library that references [the main MvvmCross package](https://www.nuget.org/packages/MvvmCross/)
- Adding a type that inherits from `IMvxPlugin` (we usually call those simply `Plugin`)
- Annotate the aforementioned type with the `MvxPluginAttribute`

That's all! In your Plugin's `Load` method you should include all IoC registration/initialization. This method will be called automatically during the app's Setup.

If your service has platform independent and platform specific services that need to be registered, you can have multiple classes annotated with the `MvxPluginAttribute`. There's no restriction on how many classes can have the attribute on a given assembly.

Please note that the `Load` method should care *exclusively* about registering the appropriate classes and initializing the adequate services. It should not hold any state nor try to be smart about initialization/caching; This is the job of the `IMvxPluginManager`. It's very important to follow this rule because when testing code, one can reliably use the `IMvxPluginManager` methods to reload whatever plugins the tested class depends on (using `forceLoad = true`).