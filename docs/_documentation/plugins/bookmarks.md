---
layout: documentation
title: Bookmarks
category: Plugins
---
The `Bookmarks` plugin provides a simple 'live tile' service for WindowsPhone only.
```c# 

public interface IMvxBookmarkLibrarian\n{\n  bool HasBookmark(string uniqueName);\n\n  bool AddBookmark(Type viewModelType, string uniqueName, MvxBookmarkMetadata metadata,\n                   IDictionary<string, string> navigationArgs);\n\n  bool UpdateBookmark(string uniqueName, MvxBookmarkMetadata metadata);\n}",
```
Current advice (August 2013): if your app requires cross-platform live-tile support, consider this plugin as an open source example only.