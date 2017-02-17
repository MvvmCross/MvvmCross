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
```C# public class FirstViewModel\n  : MvxViewModel\n  {\n    private readonly IDataStore _dataStore;\n\n    public FirstViewModel(IDataStore dataStore)\n    {\n      _dataStore = dataStore;\n    }\n\n    public void Init(int id)\n    {\n      var person = _dataStore.Get<Person>(id);\n      Id.Value = id;\n      FirstName.Value = person.FirstName;\n      LastName.Value = person.LastName;\n    }\n\n    public readonly INC<int> Id = new NC<int>();\n    public readonly INC<string> FirstName = new NC<string>();\n    public readonly INC<string> LastName = new NC<string>();\n\n    public void Save()\n    {\n      var person = _dataStore.Get<Person>(id);\n      person.FirstName = FirstName.Value;\n      person.LastName = LastName.Value;\n      _dataStore.Update(person);\n      Close(this);\n    }\n  }",
```
The `Save` method in this class could be accessed using Android syntax:
```C#     <Button\n        android:layout_width='fill_parent'\n        android:layout_height='wrap_content'\n        android:text='Save'\n        local:MvxBind='Click Save' />",
      "language": "xml"
    }
  ]
}
```
For more on Rio MethodBinding see N=36 on http://slodge.blogspot.co.uk/2013/07/n36-rio-binding-carnival.html