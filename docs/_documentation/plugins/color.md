---
title: "Color"
excerpt: ""
---
The `Color` plugin provides native implementations for conversion from the cross-platform `MvxColor` to platform-specific `Color` implementations.
[block:code]
{
  "codes": [
    {
      "code": "public interface IMvxNativeColor\n{\n  object ToNative(MvxColor mvxColor);\n}",
      "language": "csharp"
    }
  ]
}
[/block]
This plugin is available on all platforms.

This plugin also provides a number of useful Color-outputting ValueConverters - see [Mvx Color Value Conversion](https://github.com/slodge/MvvmCross/wiki/Value-Converters#the-mvx-color-valueconverters) for information on these. If you wish to create your own Color ValueConverters, then this plugin provides the base classes `MvxColorValueConverter` and `MvxColorValueConverter<T>`.

The Android version of this plugin also registers some Binding Targets for use with Color 

- `BackgroundColor` for any Android `View` 
- `TextColor` for any Android `TextView`