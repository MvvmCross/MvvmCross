---
layout: documentation
title: Getting Started
category: Plugins
---

In order to help reusability and reduce weight and complexity, MvvmCross introduces the concept of plugins.

Each plugin is suposed to provide abstractions of native functionalities such as camera, geolocation, accelerometer or files. Using plugins is extremely easy! You just need to install a nuget package on your _Core_ project and your _platform_ projects.

### A note about MvvmCross 5 and below

If you are using a version of MvvmCross less than Mvx 6.0.0, you will need to add the bootstrap file yourself.
The bootstrap file is available [here](https://github.com/MvvmCross/MvvmCross/blob/develop/nuspec/BootstrapContent/WebBrowserPluginBootstrap.cs.pp)

An example bootstrap class would look like:

```csharp
using System;
using System.Collections.Generic;
using System.Text;
using MvvmCross.Platform.Plugins;

namespace my.namespace.iOS.Bootstrap
{
    public class WebBrowserPluginBootstrap
        : MvxPluginBootstrapAction<MvvmCross.Plugins.WebBrowser.PluginLoader>
    {
    }
}
```
