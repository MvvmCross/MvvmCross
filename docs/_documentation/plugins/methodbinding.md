---
layout: documentation
title: MethodBinding
category: Plugins
---
The `MethodBinding` plugin is part of the `Rio` binding extensions for MvvmCross.

The MethodBinding plugin is a pure .NET Standard plugin - it contains only .NET Standard assemblies.

When MethodBinding is loaded, then MvvmCross data-binding:

- can use public methods as well as `ICommand` properties for action/command binding.

## Setup

Since this plugin is typically not explicitly referenced in a MvvmCross project, it is necessary to reference it to ensure that MvvmCross loads it at startup. 

Placing a reference in your `LinkerPleaseInclude.cs` file is a good place to do this.

```C#
public void Include(MvvmCross.Plugin.MethodBinding.Plugin p)
{
    var _ = p;
}
```

Alternatively you can ensure that plugin is loaded in your MvxApplication class.

```C#
public override void LoadPlugins(IMvxPluginManager pluginManager)
{
    pluginManager.EnsurePluginLoaded<MvvmCross.Plugin.MethodBinding.Plugin>();
}
```

## Example

An example of a Rio-based ViewModel using MethodBinding:

```c#
public class FirstViewModel
    : MvxViewModel
{
    public FirstViewModel()
    {
        
    }

    public void Save()
    {
        ... your code here
    }
}
```

The `Save` method in this class could be accessed using Android syntax:

```xml
    <Button
        android:layout_width='fill_parent'
        android:layout_height='wrap_content'
        android:text='Save'
        local:MvxBind='Click Save' />
```

And as follows for FluentBinding in iOS or Android.

```C#
var set = CreateBindingSet();
set.Bind(_button).To(nameof(ViewModel.Save));
set.Apply();
```

Note the use of `nameof`. You cannot use `vm => vm.Save` for fluent method binding. Using nameof will ensure that the compiler will catch errors if you rename the method at a later date.

## References

For more on Rio MethodBinding see N=36 on http://slodge.blogspot.co.uk/2013/07/n36-rio-binding-carnival.html

