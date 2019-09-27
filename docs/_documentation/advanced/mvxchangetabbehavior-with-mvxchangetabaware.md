---
layout: documentation
title: Change Tab Behavior with Change Tab Aware
category: Advanced
order: 8
---

#### This article describes how to aware about tab changes and send objects between tabs.

## When we need it?

1. When you want to share data between tabs without loading in each of them.
2. When you want to load data in tab only when it will be shown at the first time.
3. When you want to apply user modification in couple tabs without using `Messages`.

## How we can achieve it?

We have behavior `MvxChangeTabBehavior` that should be attached to `MvxTabbedPage` in XAML. 

It takes required Type `TParameter`, by this type it will match if your `MvxContentPage` will have `IMvxViewModel` what implements `IMvxChangeTabAware<TParameter>`(with the same type).


Example of adding behavior to your Root Tabbed Page:
```xml
<?xml version="1.0" encoding="utf-8" ?>
<views:MvxTabbedPage ...>
<TabbedPage.Behaviors>        
        <views:MvxChangeTabBehavior x:TypeArguments="models:SomeModel" />
    </TabbedPage.Behaviors>
</views:MvxTabbedPage>
```

Now we should implement `IMvxChangeTabAware<TParameter>` in ViewModel where we want to get or send some data to other Tabs.

```cs
public interface IMvxChangeTabAware<TParameter>
{
    /// <summary>
    /// Get Data After Tab Changed
    /// </summary>
    /// <param name="parameter">data prepared from previous tab</param>
    void OnNavigatedTo(TParameter parameter);
    /// <summary>
    /// Return Data before Changing the Tab
    /// </summary>
    /// <returns>parameter prepared for next tab</returns>
    TParameter OnNavigatedFrom();
}
```

This quite simple interface will provide notification for us when we leave this Tab and when this Tab is shown.

----
## Examples
----

## Example №1

#### Share loaded data between all tabs.

Let's imagine that we have three tabs, that load the same collection of Users from the database. But each shows it in a different way with a different filter or etc. We don't want to load it three times, we want to load it once and share it between tabs. But during the time user can pull to refresh the list of users at currently visible tab.

How we could make it?

```cs
public class FirstViewModel : MvxViewModel, IMvxChangeTabAware<UserCollection>
{
    private UserCollection _userCollection;
    public UserCollection UserCollection
    {
        get => _userCollection;
        set => SetProperty(ref _userCollection, value);
    }
    public void OnNavigatedTo(UserCollection parameter)
    {
        if (parameter != null)
            UserCollection = parameter;
    }
    public UserCollection OnNavigatedFrom() => UserCollection;
}
```
P.S. -> Second View Model & Third View Model will look the same.
```xml
<views:MvxTabbedPage>
<TabbedPage.Behaviors>
        <views:MvxChangeTabBehavior x:TypeArguments="models:User" />
    </TabbedPage.Behaviors>
</views:MvxTabbedPage>
```
That's it. Now when we move from tab 1 to 2 we will pass loaded UserCollection between tabs. And if someone will pull new UserCollection, the new one will be moved.

## Example №2

#### Loading data only when Tab is shown at the first time.

Let's imagine that we have two tabs. The second Tab is rarely used and have command with long time execution. We want to start loading only if user opens this tab at the first time.

How we could make it?

Here we implementing `IMvxChangeTabAware<object>` only on second tab. First Tab we will leave without changes.

```cs
public class SecondViewModel : MvxViewModel, IMvxChangeTabAware<object>
{
    private bool _loaded;
    public void OnNavigatedTo(object parameter)
    {
        if (!_loaded)
        {
            LoadData();
            _loaded = true;   
        }
    }
    public object OnNavigatedFrom() => null;
}
```
```xml
<views:MvxTabbedPage>
<TabbedPage.Behaviors>
        <views:MvxChangeTabBehavior x:TypeArguments="models:User" />
    </TabbedPage.Behaviors>
</views:MvxTabbedPage>
```

