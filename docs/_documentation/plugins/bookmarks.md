---
layout: documentation
title: Bookmarks
category: Plugins
---
The `Bookmarks` plugin provides a simple 'live tile' service for WindowsPhone only.
```c#
public interface IMvxBookmarkLibrarian
{
  bool HasBookmark(string uniqueName);

  bool AddBookmark(Type viewModelType, string uniqueName, MvxBookmarkMetadata metadata,
                   IDictionary<string, string> navigationArgs);

  bool UpdateBookmark(string uniqueName, MvxBookmarkMetadata metadata);
}
```
Current advice (August 2013): if your app requires cross-platform live-tile support, consider this plugin as an open source example only.
