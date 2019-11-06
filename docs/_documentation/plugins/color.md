---
layout: documentation
title: Color
category: Plugins
---
The `Color` plugin provides native implementations for conversion from `System.Drawing.Color` to platform-specific `Color` implementations.

```c#
public interface IMvxNativeColor
{
    object ToNative(System.Drawing.Color color);
}
```

This plugin is available on all platforms, but not when using Xamarin.Forms, yet. (you can check this [issue](https://github.com/MvvmCross/MvvmCross/issues/3148#issuecomment-428097084) to find a workaround)

This plugin also provides a number of useful Color-outputting ValueConverters - see [Mvx Color Value Conversion](https://www.mvvmcross.com/documentation/fundamentals/value-converters#the-mvx-color-valueconverters) for information on these. If you wish to create your own Color ValueConverters, then this plugin provides the base classes `MvxColorValueConverter` and `MvxColorValueConverter<T>`.

The Android version of this plugin also registers some Binding Targets for use with Color 

- `BackgroundColor` for any Android `View` 
- `TextColor` for any Android `TextView`

