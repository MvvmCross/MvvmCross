### ResxLocalization

The `ResxLocalization` plugin provides a support class to help use RESX files for internationalization (i18n).

The `ResxLocalization` plugin is a single PCL Assembly and isn't really a typical plugin - it doesn't itself register any singletons or services with the MvvmCross IoC container.

For more advice on using the Localization library, see the [blog post](http://opendix.blogspot.ch/2013/05/using-resx-files-for-localization-in.html) by [@stefanschoeb](https://twitter.com/stefanschoeb). The `ResxTextProvider` he describes is now contained in the `ResxLocalization` plugin as `MvxResxTextProvider`. The language value converter he describes can also be used using the extension method `ToLocalizationId()` in fluent data binding, e.g.:

```csharp
bindingSet.Bind(TextBox).For(v => v.Text).ToLocalizationId("Description");
```

if your ViewModel implements the interface `IMvxLocalizedTextSourceOwner`.
