---
layout: documentation
title: Android MvxLinearLayout
category: Platforms
---

Available from Android API 1, MvvmCross 3.

MvvmCross has a special implementation of Android's LinearLayout, which works kind of like `DataTemplate` in XAML. This is useful when wanting to repeat a Android layout with MvvmCross bindings multiple times.
This is very similar to `MvxListView` and `MvxRecyclerView`. However, without the memory management they provide, even though it uses the same `MvxAdapter` as `MvxListView` uses.

In order to use a `MvxLinearLayout` you need to provide a `ItemTemplateId` and a `ItemsSource`. Where the `ItemTemplateId` is the id of the Android layout you want to repeat and the `ItemsSource` being a collection of ViewModels you want to be bound to the layouts.

## ItemTemplateId
This is the id of the Android layout you want to repeat. This will typically contain binding expressions that match the ViewModel contained in the ItemsSource you are binding.

> Note: if `ItemTemplateId` is not set when binding, `Android.Resource.Layout.SimpleListItem` will be used and `ToString()` will be used from the ViewModel to bind this.

## ItemsSource
This is the collection of ViewModels you want repeated in the `MvxLinearLayout`.

## Misuse
Consider not misusing `MvxLinearLayout` for tasks `MvxRecyclerView` and `MvxListView` are better suited for. A good rule is, if the items you want to use `MvxLinearLayout` for do not all fit in the view bounds of the screen, then you should consider not using `MvxLinearLayout` as you can run into memory issues. These scenarios are handled better with proper Adapter Views.

## Sample Use
This is a small sample that repeats buttons in a `MvxLinearLayout`.

### ViewModel

#### ButtonViewModel
```csharp
public class ButtonViewModel : MvxNotifyPropertyChanged
{
    private string _title;
    private ICommand _clickCommand;

    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    public MvxCommand ClickCommand
    {
        get => _clickCommand;
        set => SetProperty(ref _clickCommand, value);
    }
}
```

#### RepeatButtonViewModel
```csharp
public class RepeatButtonViewModel : MvxViewModel
{
    public ObservableCollection<ButtonViewModel> Buttons
        = new ObservableCollection<ButtonViewModel>();

    public RepeatButtonViewModel()
    {
        Buttons.Add(new ButtonViewModel {
            Title = "Do stuff",
            ClickCommand = new MvxCommand(DoStuff)
        });

        Buttons.Add(new ButtonViewModel {
            Title = "Do other stuff",
            ClickCommand = new MvxCommand(DoOtherStuff)
        });
    }
}
```

### View

#### item_button.axml
```xml
<?xml version="1.0" encoding="utf-8"?>
<Button
    xmlns:android="http://schemas.android.com/apk/res/android"
    xmlns:local="http://schemas.android.com/apk/res-auto"
    android:layout_width="match_parent"
    android:layout_height="wrap_content"
    local:MvxBind="Text Title; Click ClickCommand"/>
````

#### activity_repeatbutton.axml
```xml
<?xml version="1.0" encoding="utf-8"?>
<MvxLinearLayout
    xmlns:android="http://schemas.android.com/apk/res/android"
    xmlns:local="http://schemas.android.com/apk/res-auto"
    android:layout_width="match_parent"
    android:layout_height="match_parent"
    android:orientation="vertical"
    local:MvxItemTemplate="@layout/item_button"
    local:MvxBind="ItemsSource Buttons" />
```

#### RepeatButtonView
```csharp
public class RepeatButtonView : MvxActivity<RepeatButtonViewModel>
{
    protected override void OnCreate(Bundle b)
    {
        base.OnCreat(b);

        SetContentView(Resource.Layout.activity_repeatbutton);
    }
}
```
