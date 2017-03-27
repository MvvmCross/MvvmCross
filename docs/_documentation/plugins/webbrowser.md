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
This plugin is available on all of Android, iOS, WindowsPhone and WindowsStore.