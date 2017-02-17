---
layout: documentation
title: A note about Views and ViewModels
category: Tutorials
---
This tutorial used MvvmCross's support for naming conventions to associate the View with the ViewModel.  With a view named `TipView`, MvvmCross can locate it's ViewModel as long as it's named `TipViewModel`.  The base classes we used for views (e.g. `MvxViewController`) have `MvxViewModel` properties named `ViewModel`.  

We can however link Views with ViewModels that have different names and have the Views's `ViewModel` property and have the `ViewModel` property be typed with the actual ViewModel's type rather than the `MvxViewModel` base class.  There are actually two approaches to this...


## Generic base classes

Instead of using the normal view base classes, there are generic versions available for use.  For example, we could have used `MvxViewController<TipViewModel>` instead of `MvxViewController`.  Unfortunately on Windows you can't use generic types within XAML.  You therefore need an extra class such as this:


```C# public class TipViewBase : MvxWindowsPage<TipViewModel>\n{\n}",
```
The `TipView` class can then derive from this class and the following XAML can be used:
```C# <views:TipViewBase\n    xmlns:views=\"using:MvvmCross.WindowsUWP.Views\"\n    ...\n</views:TipViewBase>",
      "language": "xml"
    }
  ]
}
```

## Explicit ViewModel properties

The `ViewModel` property of a View can be explicitly defined.  In the case of `TipView`, you would use the following code:
```C# public new TipViewModel ViewModel\n{\n    get { return (TipViewModel)base.ViewModel; }\n    set { base.ViewModel = value; }\n}",
```