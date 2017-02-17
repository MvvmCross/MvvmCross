---
layout: documentation
title: WebBrowser
category: Plugins
---
The `WebBrowser` plugin provides cross platform support for showing web pages using the external web browser using:
```c# 
private void MySampleCommand()\n{\n   PluginLoader.Instance.EnsureLoaded();\n   var task = Mvx.Resolve<IMvxWebBrowserTask>();\n   task.ShowWebPage(\"http://www.xamarin.com\");\n}",
```
This plugin is available on all of Android, iOS, WindowsPhone and WindowsStore.