---
layout: documentation
title: Android MvxSpinner
category: Platforms
---

Available in MvvmCross 3.

MvxSpinner is a wrapper around Android's Spinner View, which provides a drop down menu kind of view, to quickly select one value.

Currently `MvxSpinner` supports binding to the following properties:
- ItemsSource
- HandleItemSelected

## Getting started

Adding a `MvxSpinner` is fairly simple. You add the widget in your layout like so.

```xml
<mvvmcross.platforms.android.binding.views.MvxSpinner
    ...
    local:MvxItemTemplate="@layout/item_template"
    local:MvxDropDownItemTemplate="@layout/drop_down_item_template"
    local:MvxBind="ItemsSource Items; HandleItemSelected ItemSelectedCommand"
    />
```

You will need to bind a `ItemsSource` and provide a item template and drop down item template for it.

> Note: if you do not provide a Item Template or Drop Down Item Template, the default adapter will use `Android.Resource.Layout.SimpleSpinnerItem` as Item Template and `Android.Resource.Layout.SimpleSpinnerDropDownItem` for the Drop Down Item Template.

### Using an Item Template and Drop Down Item Template

If you do not want the default look of the spinner or you want to bind multiple properties in your ViewModel, you will need to provide a custom Item Template and Drop Down Template.

The Item Template is for the selected item. The Drop Down Item Template is for the items, when the Spinner is expanded and shows the list of spinner choices.

For this example let us assume you want to fill your `MvxSpinner` with countries and you want to shown the country name and country flag in your template.

The ViewModel for the items in the Spinner could simply consist of a couple of properties, for briefness assume they are `CountryName` and `CountryCode`.

```xml
<?xml version="1.0" encoding="utf-8"?>
<LinearLayout xmlns:android="http://schemas.android.com/apk/res/android"
    xmlns:local="http://schemas.android.com/apk/res-auto"
    android:layout_width="match_parent"
    android:layout_height="wrap_content"
    android:orientation="horizontal">

    <ImageView
        android:layout_width="50dp"
        android:layout_height="50dp"
        android:padding="5dp"
        local:MvxBind="Drawable CountryCodeToDrawable(CountryCode)" />

    <TextView
        android:layout_width="fill_parent"
        android:layout_height="wrap_content"
        android:layout_gravity="center"
        android:padding="@dimen/activity_horizontal_margin"
        local:MvxBind="Text CountryName" />
</LinearLayout>
```

> Note: it is up to the reader to create the `CountryCodeToDrawableValueConverter`.

This layout would be possible to use both for Item Template and for Drop Down Item Template, making these look the same. If you want different appearances, adjust your layouts accordingly.

Given that we called the Item Template above `country_item_template.axml` it will be accessible as `@layout/country_item_template` for the view attributes.

```xml
<mvvmcross.platforms.android.binding.views.MvxSpinner
    ...
    local:MvxItemTemplate="@layout/country_item_template"
    local:MvxDropDownItemTemplate="@layout/country_item_template"
    />
```

### HandlItemSelected command

The `HandleItemSelected` command, can be bound to a command in your ViewModel. When executed this will provide the `ViewModel` bound to the selected row as the command parameter.

```csharp
private MvxCommand<CountryViewModel> _countrySelectedCommand;
public MvxCommand<DogViewModel> CountrySelectedCommand => _countrySelectedCommand = 
    _countrySelectedCommand ?? new MvxCommand<DogViewModel>(OnCountrySelectedCommand);

private void OnCountrySelectedCommand(CountryViewModel country)
{
    // write on country selected logic here
}