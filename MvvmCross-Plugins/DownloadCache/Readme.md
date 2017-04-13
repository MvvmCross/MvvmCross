### DownloadCache

The `DownloadCache` plugin provides a managed disk and memory cache for downloaded files - especially images.

The DownloadCache plugin is only available for iOS and Android, although others have ported this to Windows platforms too.

For the DownloadCache plugin to fully work, implementations must be registered for:

- `IMvxTextSerializer`
- `IMvxFileStore`

One relatively easiest way to register implementations for these interfaces is to load the Json and File plugins.

The most common use of the DownloadCache plugin is for downloading images using in `MvxImageView` in Android and iOS. This is managed automatically using the `IMvxImageHelper<Bitmap>` and `<IMvxImageHelper<UIImage>` helpers.

The caches used by these helpers are configured `MvxDownloadCacheConfiguration` configuration classes. The default configurations store up to 500 files for up to 7 days, and will maintain up to 4MB of up to 30 images in RAM. To override the default values, provide custom settings in `GetPluginConfiguration` in your `Setup` class.

Known issues:

- this cache is a complicated implementation, has been well tested in apps, but is poorly unit tested currently
- one user has reported MonoTouch download issues in certain network conditions - these problems seem to be related to known MonoTouch issues - see [StackOverflow Q&A on this](http://stackoverflow.com/questions/17238809/mvxdynamicimagehelper-unreliable). That first StackOverflow post has a suggested workaround - overriding the `IMvxHttpFileDownloader` registration with an implementation which uses the `UIImage.LoadFromData` method.