---
layout: documentation
title: PictureChooser
category: Plugins
---
The `PictureChooser` plugin provides implementations of:
```C# public interface IMvxPictureChooserTask\n{\n  void ChoosePictureFromLibrary(int maxPixelDimension, int percentQuality, Action<Stream> pictureAvailable,\n                                Action assumeCancelled);\n\n  void TakePicture(int maxPixelDimension, int percentQuality, Action<Stream> pictureAvailable,\n                   Action assumeCancelled);\n}",
```
This is available on Android, iOS, WindowsPhone and WindowsStore. 

This interface is designed for single use only - i.e. each time you require a picture you should request a new `IMvxPictureChooserTask` instance.

The interface can be used as:
```C# var task = Mvx.Resolve<IMvxPictureChooserTask>();\ntask.ChoosePictureFromLibrary(500, 90,\n                              stream => {\n                                // use the stream\n                                // expect the stream to be disposed after immediately this method returns.\n                              },\n                              () => {\n                                // perform any cancelled operation\n                              });",
```
**Note:** Using this interface well on Android is very difficult.

The reason for this is because of Android's Activity lifecyle. The Android lifecycle means that the image that may be returned to a different View and ViewModel than the one that requested it. This is partly because camera apps generally use a lot of RAM (raw camera images are large files) - so while th camera app is capturing you image, then Android may look to free up additional RAM by killing your app's Activity.

If you want to use this `IMvxPictureChooserTask` effectively and reliably on Android then you really need to call this API via a service class, to use Messaging to pass the returned message back to a ViewModel and to implement 'tombstoning' support for that ViewModel.

There is a simple demo for `IMvxPictureChooserTask` in [PictureTaking](https://github.com/slodge/MvvmCross-Tutorials/tree/master/PictureTaking) - however, this simple demo doesn't currently show this full Android technique. 

**Note:** On Windows Phone 8.0, an additional implementation is available:
```C# public interface IMvxCombinedPictureChooserTask\n{\n  void ChooseOrTakePicture(int maxPixelDimension, int percentQuality, Action<Stream> pictureAvailable,\n                           Action assumeCancelled);\n}\n\nClient code can test for the availability of this interface using:\n\nvar isAvailable = Mvx.CanResolve<IMvxCombinedPictureChooserTask>();\n\nor:\n\nIMvxCombinedPictureChooserTask combined;\nvar isAvailable = Mvx.TryResolve(out combined);",
```
Finally, the `PictureChooser` plugin also provides an "InMemoryImage" ValueConverter - `MvxInMemoryImageValueConverter`. This value converter allows images to be decoded from byte arrays for use on-screen.

The "InMemoryImage" ValueConverter can be seen in use in the PictureTaking sample - see https://github.com/slodge/MvvmCross-Tutorials/tree/master/PictureTaking.

#### Windows Phone 8.1 <a name="picturechooserwp81" />
Windows Phone 8.1 and Windows 8.1 API hasn't converged entirely and there are differences in how pictures are handled. If you want to choose pictures from the gallery, you need to implement a `EventHandler` on your `Page`, which listens to `Activated` events. This is due to the picture data is returned on that event on Windows Phone 8.1.

Hence in your constructor of your page add:

```
var view = CoreApplication.GetCurrentView();
view.Activated += ViewOnActivated;
```

The `ViewOnActivated` `EventHandler` will look something like this:

```
private void ViewOnActivated(CoreApplicationView sender, IActivatedEventArgs args)
{
    var continuationArgs = args as FileOpenPickerContinuationEventArgs;
    if (continuationArgs == null) return;

    ViewModel.ContinueFileOpenPicker(args);
}
```

The `ContinueFileOpenPicker()` method in the `ViewModel` simply calls the same method on the `IMvxPictureChooserTask`:

```
public void ContinueFilePicker(object args)
{
    if (_currentChooserTask == null) return;
            
    _currentChooserTask.ContinueFileOpenPicker(args);
}
```

If you don't want to expose this method in your `ViewModel`, the alternative is to pass a message using `IMvxMessenger` instead.