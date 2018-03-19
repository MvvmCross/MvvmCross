---
layout: documentation
title: WebBrowser
category: Plugins
---
The `WebBrowser` plugin provides cross platform support for showing web pages using the external web browser using:

```c#
private void MySampleCommand()
{
    PluginLoader.Instance.EnsureLoaded();
    var task = Mvx.Resolve<IMvxWebBrowserTask>();
    task.ShowWebPage("http://www.xamarin.com");
}
```

This plugin is available on all of Android, iOS and Windows Uwp.

> If you are using a version of MvvmCross less than Mvx 6.0.0, you will need to add the bootstrap file.
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
