---
title: "WebBrowser"
excerpt: ""
---
The `WebBrowser` plugin provides cross platform support for showing web pages using the external web browser using:
[block:code]
{
  "codes": [
    {
      "code": "private void MySampleCommand()\n{\n   PluginLoader.Instance.EnsureLoaded();\n   var task = Mvx.Resolve<IMvxWebBrowserTask>();\n   task.ShowWebPage(\"http://www.xamarin.com\");\n}",
      "language": "csharp"
    }
  ]
}
[/block]
This plugin is available on all of Android, iOS, WindowsPhone and WindowsStore.