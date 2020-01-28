---
layout: documentation
title: Android support library
category: Platforms
---

If your app will target the Android platform, you will probably want to make use of the Android Support libraries (because of many reasons).

If you don't really know what Android Support packages are about, we recommend that you read the [official documentation](https://developer.android.com/topic/libraries/support-library/).

When using MvvmCross, all you need to do is to install the MvvmCross support packages. And then you will have access to the "app compat" objects / widgets, like:
- AppCompatActivity -> MvxAppCompatActivity
- Fragment -> MvxFragment

We highly recommend you to take a look at some samples, like [StarWarsSample](https://github.com/MvvmCross/MvvmCross-Samples/tree/master/StarWarsSample) or the [Playground.Droid](https://github.com/MvvmCross/MvvmCross/tree/master/Projects/Playground/Playground.Droid) project, which is located in the main repository.


To get bindings working when using the support libraries (for example if you want to use `MvxAutoCompleteTextView`) you should ensure your `Setup` class inherits from `MvxAppCompatSetup`:

```c#
namespace MyAwesomeApp.Droid
{
    public class Setup : MvxAppCompatSetup
    {
        ...
    }
}
```
