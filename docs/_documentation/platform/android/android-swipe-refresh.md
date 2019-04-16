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
private IMvxAsyncCommand _refreshCommand;
public IMvxAsyncCommand RefreshCommand 
    => _refreshCommand ?? (_refreshCommand = new MvxAsyncCommand(ExecuteRefreshCommand));

private bool _isBusy;
public bool IsBusy
{
    get => _isBusy;
    set => SetProperty(ref _isBusy, value);
}

private async Task ExecuteRefreshCommand()
{
    IsBusy = true;
    // do refresh work here
    IsBusy = false;
}
```

### View

```xml
<MvxSwipeRefreshLayout
    android:layout_height="match_parent"
    android:layout_width="match_parent"
    android:id="@+id/refresher"
    local:MvxBind="Refreshing IsBusy; RefreshCommand RefreshCommand">
    <ScrollView />
    <!-- or -->
    <RecyclerView />
    <!-- or -->
    <ListView />
</MvxSwipeRefreshLayout>
```
### Code in your OnCreate(): for changing the loader color

 public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Android.OS.Bundle savedInstanceState)
 {
    base.OnCreateView(inflater, container, savedInstanceState);
    var view = this.BindingInflate(Resource.Layout.yourView, container, false);
    var refresher = view.FindViewById<MvxSwipeRefreshLayout>(Resource.Id.refresher);
    refresher.SetColorScheme (Resource.Color.blue);
    return view;
  }

[nugetpackage]: https://www.nuget.org/packages/MvvmCross.Droid.Support.Core.UI/
