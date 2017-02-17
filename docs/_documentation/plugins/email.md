---
layout: documentation
title: Email
category: Plugins
---
The `Email` plugin provides a cross-platform implementation for sending emails.
```c# 
public interface IMvxComposeEmailTask\n{\nvoid ComposeEmail(string to, string cc, string subject, string body, bool isHtml);\n}",
```
The Email plugins is supported on all platforms.

The implementation on Windows Store and Wpf is very simplistic - using only 'mailto:' url-open requests.

To send an email you can use:
```c# 
Mvx.Resolve<IMvxComposeEmailTask>()\n  .ComposeEmail(\"me@slodge.com\", \n                string.Empty, \n                \"MvvmCross Email\",\n                \"I <3 MvvmCross\",\n                false);",
```