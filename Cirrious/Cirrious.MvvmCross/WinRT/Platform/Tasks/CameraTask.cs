using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Platform.Tasks;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.Views;
using Windows.Foundation;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;

namespace Cirrious.MvvmCross.WinRT.Platform.Tasks
{
    public class CameraTask : IMvxPictureChooserTask,
        IMvxServiceConsumer<IMvxViewDispatcherProvider>
    {
        public void ChoosePictureFromLibrary(int maxPixelDimension, int percentQuality, Action<Stream> pictureAvailable, Action assumeCancelled)
        {
            throw new NotImplementedException();
        }

        public void TakePicture(int maxPixelDimension, int percentQuality, Action<Stream> pictureAvailable, Action assumeCancelled)
        {
            var dispatcher = this.GetService<IMvxViewDispatcherProvider>();

            dispatcher.Dispatcher.RequestMainThreadAction(
                async () =>
                          {
                                // Using Windows.Media.Capture.CameraCaptureUI API to capture a photo
                                var dialog = new CameraCaptureUI();
                                var aspectRatio = new Size(16, 9);
                              
                                dialog.PhotoSettings.CroppedAspectRatio = aspectRatio;
                              // HACK HACK! 
                              dialog.PhotoSettings.MaxResolution = CameraCaptureUIMaxPhotoResolution.SmallVga;
                              dialog.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;

                              var file = await dialog.CaptureFileAsync(CameraCaptureUIMode.Photo);
                                if (file != null)
                                {
                                    BitmapImage bitmapImage = new BitmapImage();
                                    var memoryStream = new MemoryStream();
                                    using (IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read))
                                    {
                                        var conventional = fileStream.AsStreamForRead();
                                        conventional.CopyTo(memoryStream);
                                    }
                                    pictureAvailable(memoryStream);
                                }
                                else
                                {
                                    assumeCancelled();
                                }
                          });
            
        }
    }
   
}
