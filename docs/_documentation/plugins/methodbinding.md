---
layout: documentation
title: MethodBinding
category: Plugins
---
The `MethodBinding` plugin is part of the `Rio` binding extensions for MvvmCross.

The MethodBinding plugin is a pure PCL plugin - it contains only a PCL assembly.

When MethodBinding is loaded, then MvvmCross data-binding:

- can use public methods as well as `ICommand` properties for action/command binding.

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
The `Save` method in this class could be accessed using Android syntax:
```c# 
    <Button
        android:layout_width='fill_parent'
        android:layout_height='wrap_content'
        android:text='Save'
        local:MvxBind='Click Save' />
```
For more on Rio MethodBinding see N=36 on http://slodge.blogspot.co.uk/2013/07/n36-rio-binding-carnival.html