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

If you are using a version of MvvmCross less than Mvx 6.0, you will need to either: Add the bootstrap file or use Mvx 6.0.
The bootstrap file is available [here](https://github.com/MvvmCross/MvvmCross/blob/develop/nuspec/BootstrapContent/WebBrowserPluginBootstrap.cs.pp)

An example bootstrap class would look like:

```
using System;
using System.Collections.Generic;
using System.Text;
using MvvmCross.Platform.Plugins;

namespace afrikaburn.iOS.Bootstrap
{
    public class WebBrowserPluginBootstrap
        : MvxPluginBootstrapAction<MvvmCross.Plugins.WebBrowser.PluginLoader>
    {
    }
}
```
Pull request REVIEWERS - this still does not make the plugin functional on ios. Perhaps one of the reviewers can advise and modify the PR accordingly? 

