---
layout: documentation
title: iOS UIRefreshControl
category: Platforms
---

Available from iOS 6, MvvmCross 5.0.0.

MvvmCross has a extended version of UIRefreshControl on iOS which exposes `RefreshCommand`, `IsRefreshing`, and `Message`. This control is useful when you have a
scrollable View such as a UITableView, UICollectionView or a UIScrollView, you want to refresh, when scrolling it to the top and pulling it down. It is also known as pull to refresh.

## RefreshCommand
MvxUIRefreshControl internally listens to the `ValueChanged` event on the UIRefreshControl, to fire the `RefreshCommand` command when the event is triggered.
This allows you to bind an Command from your ViewModel to this event to refresh or load more data.

## IsRefreshing
This property is used to toggle the visual representation of the UIRefreshControl. If set to `true` the UIRefreshControl will show a loading indicator along with
the set message. If set to `false` no loading indicator will be shown. This should always be set from the UI Thread.

## Message
This property is the message to be shown when `IsRefreshing` is `true`.

## Sample usage

### ViewModel

```csharp
private bool _isLoading;
public bool IsLoading
{
    get => _isLoading;
    set => SetProperty(ref _isLoading, value);
}

private string _loadingMessage;
public string LoadingMessage
{
    get => _loadingMessage;
    set => SetProperty(ref _loadingMessage, value);
}

private MvxCommand _refreshCommand;
public ICommand RefreshCommand => _refreshCommand = _refreshCommand ?? new MvxCommand(DoRefreshCommand);

private DoRefreshCommand()
{
    IsLoading = true;
    LoadingMessage = "Loading dogs and kittens...";

    // load stuff

    IsLoading = false;
}
```

### View

```csharp
var refreshControl = new MvxUIRefreshControl();

// RefreshControl is a property on a `UIViewController`
RefreshControl = refreshControl;

var set = this.CreateBindingSet<MyView, MyViewModel>();
set.Bind(refreshControl).For(r => r.IsRefreshing).To(vm => vm.IsLoading);
set.Bind(refreshControl).For(r => r.RefreshCommand).To(vm => vm.ReloadCommand);
set.Bind(refreshControl).For(r => r.Message).To(vm => vm.LoadingMessage);
set.Apply();
```

## Resources
* https://montemagno.com/mvxuirefreshcontrol-for-mvvmcross/