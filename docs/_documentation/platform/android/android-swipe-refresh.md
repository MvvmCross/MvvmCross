---
layout: documentation
title: Android Swipe to Refresh
category: Platforms
---

Available from Android API 4 (Support package), MvvmCross 4.4.0.

To use this install the [MvvmCross.Droid.Support.Core.UI][nugetpackage] NuGet package.

MvvmCross has a extended version of SwipeRefreshLayout from Android Support Core UI (previously Support v4) which exposes `RefreshCommand`. This control is useful when you have a
scrollable View such as a RecyclerView, ScrollView or ListView you want to refresh, when scrolling it to the top and pulling it down. It is also known as pull to refresh.

## RefreshCommand
MvxSwipeRefreshLayout internally listens to the `Refresh` event on the SwipeRefreshLayout, to fire the `RefreshCommand` command when the event is triggered.
This allows you to bind an Command from your ViewModel to this event to refresh or load more data.

## Sample usage

### ViewModel

```csharp
private MvxCommand _refreshCommand;
public ICommand RefreshCommand => _refreshCommand = _refreshCommand ?? new MvxCommand(DoRefreshCommand);

private DoRefreshCommand()
{
    // load stuff
}
```

### View

```xml
<MvxSwipeRefreshLayout
    android:layout_height="match_parent"
    android:layout_width="match_parent"
    local:MvxBind="RefreshCommand RefreshCommand">
    <ScrollView />
    <!-- or -->
    <RecyclerView />
    <!-- or -->
    <ListView />
</MvxSwipeRefreshLayout>
```

[nugetpackage]: https://www.nuget.org/packages/MvvmCross.Droid.Support.Core.UI/