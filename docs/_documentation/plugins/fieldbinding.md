---
layout: documentation
title: FieldBinding
category: Plugins
---
The `FieldBinding` plugin is part of the `Rio` binding extensions for MvvmCross. The plugin is a pure PCL plugin - it contains only PCL assemblies.

When FieldBinding is loaded, then MvvmCross data-binding:

- can use fields as well as properties for binding.
- can use `INotifyChanged` for dynamic fields.

An example, Rio-based ViewModel using both FieldBinding and MethodBinding is:

```c#
public class FirstViewModel
    : MvxViewModel
{
    private readonly IDataStore _dataStore;

    public FirstViewModel(IDataStore dataStore)
    {
        _dataStore = dataStore;
    }

    public void Init(int id)
    {
        var person = _dataStore.Get<Person>(id);
        Id.Value = id;
        FirstName.Value = person.FirstName;
        LastName.Value = person.LastName;
    }

    public readonly INC<int> Id = new NC<int>();
    public readonly INC<string> FirstName = new NC<string>();
    public readonly INC<string> LastName = new NC<string>();

    public void Save()
    {
        var person = _dataStore.Get<Person>(id);
        person.FirstName = FirstName.Value;
        person.LastName = LastName.Value;
        _dataStore.Update(person);
        Close(this);
    }
}
```

The field in this class could be accessed using Android syntax:

```xml
<TextView
android:layout_width='fill_parent'
                     android:layout_height='wrap_content'
                             local:MvxBind='Text FirstName' />

                                     <TextView
                                     android:layout_width='fill_parent'
                                             android:layout_height='wrap_content'
                                                     local:MvxBind='Text LastName' />
```

For more on Rio FieldBinding see the [N=36 video](http://slodge.blogspot.co.uk/2013/07/n36-rio-binding-carnival.html)

