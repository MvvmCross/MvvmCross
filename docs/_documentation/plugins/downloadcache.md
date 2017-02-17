---
layout: documentation
title: DownloadCache
category: Plugins
---
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


## Download Cache and Android Bitmap

When loading images from remote source, you have to be carefull with the images size. Loading a 2000x2000 pixels bitmap in a 40dp x 40dp ImageView will do no good to your application. You want eventually to scale down the image to a more pragmatic size.

[Xamarin wrote a recipe for this].(https://developer.xamarin.com/recipes/android/resources/general/load_large_bitmaps_efficiently/)

Lucky you, this recipe is implemented in the `MvxAndroidLocalFileImageLoader.cs` file of the `DownloadCache` plugin.

If you look at the source code, and more precisely at `private static async Task<Bitmap> LoadBitmapAsync(string localPath, int maxWidth, int maxHeight)`, you'll see that the bitmap will be resampled accordingly to the given maxWidth and maxHeight.

Climbing up the call ladder, we can see that these parameters are simply coming from the MvxImageView which overrides the properties of Android's ImageView.

So all you have to do in your layout, if per say you want to display a 40x40 avatar image, is this:

```
android:maxHeight="40dp"
android:maxWidth="40dp"
```

To go further, I strongly advise you to create a "avatarStyle", with your custom attributes, and apply it to your MvxImageView.