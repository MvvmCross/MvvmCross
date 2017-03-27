---
layout: documentation
title: Android support library
category: Platform
---
To get the bindings working from the support libraries (for example if you want to use `MvxAutoCompleteTextView`) you'll need to do the following:
In Setup.cs override `FillTargetFactories`.
```csharp
protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
{
    MvxAppCompatSetupHelper.FillTargetFactories(registry);
    base.FillTargetFactories(registry);
}
```

## Samples
A sample project can be found on github - [MvvmCross\MvvmCross-AndroidSupport](https://github.com/MvvmCross/MvvmCross-AndroidSupport)
