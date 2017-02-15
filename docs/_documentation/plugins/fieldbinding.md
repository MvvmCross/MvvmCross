---
title: "FieldBinding"
excerpt: ""
---
The `FieldBinding` plugin is part of the `Rio` binding extensions for MvvmCross. The plugin is a pure PCL plugin - it contains only PCL assemblies.

When FieldBinding is loaded, then MvvmCross data-binding:

- can use fields as well as properties for binding.
- can use `INotifyChanged` for dynamic fields.

An example, Rio-based ViewModel using both FieldBinding and MethodBinding is:
[block:code]
{
  "codes": [
    {
      "code": "public class FirstViewModel\n       : MvxViewModel\n    {\n        private readonly IDataStore _dataStore;\n        \n        public FirstViewModel(IDataStore dataStore)\n        {\n            _dataStore = dataStore;\n        }\n        \n        public void Init(int id)\n        {\n            var person = _dataStore.Get<Person>(id);\n            Id.Value = id;\n            FirstName.Value = person.FirstName;\n            LastName.Value = person.LastName;\n        }\n        \n        public readonly INC<int> Id = new NC<int>();\n        public readonly INC<string> FirstName = new NC<string>();\n        public readonly INC<string> LastName = new NC<string>();\n        \n        public void Save()\n        {\n            var person = _dataStore.Get<Person>(id);\n            person.FirstName = FirstName.Value;\n            person.LastName = LastName.Value;\n            _dataStore.Update(person);\n            Close(this);\n        }\n    }",
      "language": "csharp"
    }
  ]
}
[/block]
The field in this class could be accessed using Android syntax:
[block:code]
{
  "codes": [
    {
      "code": "<TextView\n\tandroid:layout_width='fill_parent'\n  android:layout_height='wrap_content'\n  local:MvxBind='Text FirstName' />\n\n<TextView\n  android:layout_width='fill_parent'\n  android:layout_height='wrap_content'\n  local:MvxBind='Text LastName' />",
      "language": "xml"
    }
  ]
}
[/block]
For more on Rio FieldBinding see the [N=36 video](http://slodge.blogspot.co.uk/2013/07/n36-rio-binding-carnival.html)