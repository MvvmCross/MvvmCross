---
layout: documentation
title: ThreadUtils
category: Plugins
---
The ThreadUtils plugin provides a trivial cross-platform access to a `Thread.Sleep` implementation.

This is really only used for demos.

Current advice (August 2013): there's no real reason to use this plugin in production apps. In demo apps you can use `Task.Delay(100).Wait()` which is already available in PCL for all platforms.

