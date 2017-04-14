---
layout: documentation
title: Android support library
category: Platform specifics
---

To get bindings working when using the support libraries (for example if you want to use `MvxAutoCompleteTextView`) you will need to do the following.

In Setup.cs override `FillTargetFactories`.

```c#
protected override void FillTargetFactories(
    IMvxTargetBindingFactoryRegistry registry)
{
    MvxAppCompatSetupHelper.FillTargetFactories(registry);
    base.FillTargetFactories(registry);
}
```

## Samples
A sample project can be found on github - [MvvmCross-AndroidSupport](https://github.com/MvvmCross/MvvmCross/tree/develop/MvvmCross-AndroidSupport)

