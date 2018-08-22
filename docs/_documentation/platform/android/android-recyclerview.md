---
layout: documentation
title: Android MvxRecyclerView
category: Platforms
---

Available in Android Support RecyclerView, MvvmCross 4.

MvvmCross has a implementation of Android's RecyclerView, which allows us to bind a collection of ViewModels to the `ItemsSource` property. It works similarly to a ListView. However, a RecyclerView is out of the box a more resource friendly view, due to enforcing the use of the ViewHolder pattern, it also supports refreshing parts of the View rather than the invalidating the entire View. RecyclerView, although very efficient, it does not come with all the blows and whistles that a normal ListView comes with, such as built in item click events, highlighting selected row and more. Some of these we have covered for you and this article serves the purpose of the common uses of a `MvxRecyclerView`.

Currently `MvxRecyclerView` supports binding to the following properties:
- ItemsSource
- ItemClick
- ItemLongClick

## Getting started

First you need to ensure that you have the `MvvmCross.Droid.Support.V7.RecyclerView` NuGet package installed in your Application project.

Then adding your first `MvxRecyclerView` is fairly simple. You add the widget in your layout like so.

```xml
<mvvmcross.droid.support.v7.recyclerview.MvxRecyclerView
    ...
    local:MvxItemTemplate="@layout/item_template"
    local:MvxItemTemplateSelector="Fully.Qualified.Name,Assembly.Name"
    local:MvxBind="ItemsSource Items; ItemClick ItemClickCommand; ItemLongClick ItemLongClickCommand"
    />
```

You will need to bind a `ItemsSource` and provide a item template or item template selector for it.

> Note: If you don't provide a item template or item template selector `MvxRecyclerView` will fall back to using a `SimpleListItem1`, which is a built in Android Resource. It will also just call `ToString()` on your item that you are supplying.

### Using an Item Template

An item template is simply a Android Layout with binding descriptions, which match the type of ViewModel you are providing in the ItemsSource. Using a Item Template is useful if you only have one type of ViewModel in your ItemsSource. An example of this could be a cell showing first and last name of your friends.

The ViewModel could look something as follows.

```csharp
private string _firstName;
public string FirstName
{
    get => _firstName;
    set => SetProperty(ref _firstName, value);
}

private string _lastName;
public string LastName
{
    get => _lastName;
    set => SetProperty(ref _lastName, value);
}
```

The item template layout could then look as follows.

```xml
<LinearLayout
    xmlns:android="http://schemas.android.com/apk/res/android"
	xmlns:local="http://schemas.android.com/apk/res-auto"
	android:layout_width="match_parent"
	android:layout_height="wrap_content"
	android:orientation="vertical">
    <TextView
		android:layout_width="wrap_content"
		android:layout_height="wrap_content"
		local:MvxBind="Text FirstName" />
    <TextView
		android:layout_width="wrap_content"
		android:layout_height="wrap_content"
		local:MvxBind="Text LastName" />
</LinearLayout>
```

If you call your layout `item_contact`, then using it when filling out the `MvxItemTemplate` attribute you will need to prefix it with `@layout/` to indicate it is a Android layout like so.

```xml
<mvvmcross.droid.support.v7.recyclerview.MvxRecyclerView
    local:MvxItemTemplate="@layout/item_contact"
    ... />
```

### Using an Item Template Selector

An `ItemTemplateSelector` is especially useful, when wanting to show different types of ViewModels in the same list and you want to present them with different views for each different type of ViewModel. The idea behind the `ItemTemplateSelector` class is that you then do not have to extend the `MvxRecylcerAdapter`, to provide different Views for different ViewModel types.

So let us say are trying to keep track of animals in a zoo and you want a different View for each group of Animal. So mammals have its own View, reptiles its own View and so on.

Assuming you have a ViewModel type for each of these group like: MammalViewModel, ReptileViewModel or some other way you could uniquely identify which View to present for the ViewModel, you could create a `ItemTemplateSelector` which could help you achieve this.

To create your own `ItemTemplateSelector` you must create a class implementing the `IMvxItemTemplate` interface, which has two very important methods. `GetItemViewType(object forItemObject)` is used for the RecyclerView to determine how to recycle the Views. If you return `0` it will assume there is only one View type. Usually you would just return the layout id here. `GetItemLayoutId(int fromViewType)` this method is used to provide the actual id of the layout you want to use for the View type.

> Ensure you are returning something else than `0` from `GetItemViewType(object)` if you use multiple views in your `ItemTemplateSelector`.

A small example:

```csharp
namespace Zoo.App
{
    public class AnimalTemplateSelector : IMvxItemTemplateSelector
    {
        public int ItemTemplateId { get; set; } // fallback ItemTemplateId 
        
        public int GetItemViewType(object item)
        {
            if (item is MammalViewModel)
                return 1;
            if (item is ReptileViewModel)
                return 2;
            if (item is BirdViewModel)
                return 3;

            return -1;
        }

        public int GetItemLayoutId(int viewType)
        {
            if (viewType == 1)
                return Resource.Layout.item_mammal;
            if (viewType == 2)
                return Resource.Layout.item_reptile;
            if (viewType == 3)
                return Resource.Layout.item_bird;

            return ItemTemplateId;
        }
    }
}
```

To use this `ItemTemplateSelctor` you will need to provide it in the `MvxItemTemplateSelector` attribute on the `MvxRecylcerView`. It must be of the format: `Fully.Qualified.ClassName,Assembly.Name`. Hence, for the example above. Let us say the assembly will be `Zoo.App.Droid` and as you see the namespace is `Zoo.App` then the string will be: `Zoo.App.AnimalTemplateSelector,Zoo.App.Droid`.

```xml
<mvvmcross.droid.support.v7.recyclerview.MvxRecyclerView
    local:MvxItemTemplateSelector="Zoo.App.AnimalTemplateSelector,Zoo.App.Droid"
    ... />
```

The `ItemTemplateId` property in your `ItemTemplateSelector` will get overwritten if you provide both the `MvxItemTemplateSelector` and `MvxItemTemplate`, by the value in `MvxItemTemplate`.

> Note: If you do not provide a `MvxItemTemplateSelector` the `MvxRecyclerAdapter` will fallback to use `MvxDefaultItemTemplateSelector`.

### ItemClick and ItemLongClick commands

`ItemClick` and `ItemLongClick` can be bound on a `MvxRecyclerView` to execute a command when a specific item in the view is either clicked or long clicked.

This is similar to `MvxListView`s `SelectedItem` property you can bind to, although in this case you can also get the long click.

When you create your command, you can optionally get the `ViewModel` bound in the `ItemsSource`. Just be careful when doing so for an `ItemsSource` containing multiple types.

The binding to your command would look as follows.

```xml
<mvvmcross.droid.support.v7.recyclerview.MvxRecyclerView
    local:MvxBind="ItemClick ItemClickCommand; ItemLongClickCommand"
    ... />
```

#### Single ViewModel Items Sources

```csharp
private MvxCommand<DogViewModel> _dogClickCommand;
public MvxCommand<DogViewModel> DogClickCommand => _dogClickCommand = 
    _dogClickCommand ?? new MvxCommand<DogViewModel>(OnDogClickCommand);

private void OnDogClickCommand(DogViewModel dog)
{
    // write on dog clicked logic here
}
```

#### Multiple ViewModel Items Source

Given that `DogViewModel` and `CatViewModel` both derive from `MammalViewModel` you could create your command as follows for a `MvxRecyclerView` bound to an `ItemsSource` containing `DogViewModel` and `CatViewModel` instances.

```csharp
private MvxCommand<MammalViewModel> _itemClickCommand;
public MvxCommand<DogViewModel> ItemClickCommand => _itemClickCommand = 
    _itemClickCommand ?? new MvxCommand<DogViewModel>(OnItemClickCommand);

private void OnItemClickCommand(MammalViewModel animal)
{
    // do common animal stuff here

    if (animal is DogViewModel dog)
    {
        // do dog stuff
    }
    else if (animal is CatViewModel cat)
    {
        // do cat stuff
    }
}
```

Alternatively you could go a level lower and just use `object` instead of `MammalViewModel` if you do not need to do common stuff with the `MammalViewModel`.

