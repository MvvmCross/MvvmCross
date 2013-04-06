// MvxPictureChooserTask.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.IO;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace Cirrious.MvvmCross.Plugins.PictureChooser.WindowsStore
{
    public class MvxPictureChooserTask : IMvxPictureChooserTask
    {
        public void ChoosePictureFromLibrary(int maxPixelDimension, int percentQuality, Action<Stream> pictureAvailable, Action assumeCancelled)
        {
            var filePicker = new FileOpenPicker();
            filePicker.FileTypeFilter.Add(".jpg");
            filePicker.FileTypeFilter.Add(".jpeg");
            filePicker.ViewMode = PickerViewMode.Thumbnail;
            filePicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            filePicker.SettingsIdentifier = "picker1";
            //filePicker.CommitButtonText = "Open";

            var pickTask = filePicker.PickSingleFileAsync();
            pickTask.GetAwaiter().OnCompleted(() =>
                {
                    var file = pickTask.GetResults();
                    ProcessPickedFile(file, pictureAvailable, assumeCancelled);
                });
        }

        public void TakePicture(int maxPixelDimension, int percentQuality, Action<Stream> pictureAvailable, Action assumeCancelled)
        {
               var dialog = new CameraCaptureUI();  
  
  
            var captureTask = dialog.CaptureFileAsync(CameraCaptureUIMode.Photo);  
            
            captureTask.GetAwaiter().OnCompleted(() =>
                {
                    var file = captureTask.GetResults();
                    ProcessPickedFile(file, pictureAvailable, assumeCancelled);
                });
        }

        protected virtual void ProcessPickedFile(StorageFile file, Action<Stream> pictureAvailable, Action assumeCancelled)
        {
            if (file == null)
            {
                assumeCancelled();
                return;
            }

            var fileOpen = file.OpenAsync(FileAccessMode.Read);
            fileOpen.GetAwaiter().OnCompleted(() =>
                {
                    // TODO - we don't currently resize or use picture quality
                    var stream = fileOpen.GetResults().AsStream();
                    pictureAvailable(stream);
                });
        }

        /*
        #region IMvxCombinedPictureChooserTask Members

        public void ChooseOrTakePicture(int maxPixelDimension, int percentQuality, Action<Stream> pictureAvailable,
                                        Action assumeCancelled)
        {
            // note - do not set PixelHeight = maxPixelDimension, PixelWidth = maxPixelDimension here - as that would create square cropping
            var chooser = new PhotoChooserTask {ShowCamera = true};
            ChoosePictureCommon(chooser, maxPixelDimension, percentQuality, pictureAvailable, assumeCancelled);
        }

        #endregion

        #region IMvxPictureChooserTask Members

        public void ChoosePictureFromLibrary(int maxPixelDimension, int percentQuality, Action<Stream> pictureAvailable,
                                             Action assumeCancelled)
        {
            // note - do not set PixelHeight = maxPixelDimension, PixelWidth = maxPixelDimension here - as that would create square cropping
            var chooser = new PhotoChooserTask {ShowCamera = false};
            ChoosePictureCommon(chooser, maxPixelDimension, percentQuality, pictureAvailable, assumeCancelled);
        }

        public void TakePicture(int maxPixelDimension, int percentQuality, Action<Stream> pictureAvailable,
                                Action assumeCancelled)
        {
            var chooser = new CameraCaptureTask {};
            ChoosePictureCommon(chooser, maxPixelDimension, percentQuality, pictureAvailable, assumeCancelled);
        }

        #endregion

        public void ChoosePictureCommon(ChooserBase<PhotoResult> chooser, int maxPixelDimension, int percentQuality,
                                        Action<Stream> pictureAvailable, Action assumeCancelled)
        {
            
            var dialog = new CameraCaptureUI();
            // Define the aspect ratio for the photo
            var aspectRatio = new Size(16, 9);
            dialog.PhotoSettings.CroppedAspectRatio = aspectRatio;

            // Perform a photo capture and return the file object
            var file = dialog.CaptureFileAsync(CameraCaptureUIMode.Photo).Await();

            // Physically save the image to local storage
            var bitmapImage = new BitmapImage();
            using (IRandomAccessStream fileStream = file.OpenAsync(FileAccessMode.Read).Await())
            {
                bitmapImage.SetSource(fileStream);
            }
            chooser.Completed += (sender, args) =>
                {
                    if (args.ChosenPhoto != null)
                    {
                        ResizeThenCallOnMainThread(maxPixelDimension,
                                                   percentQuality,
                                                   args.ChosenPhoto,
                                                   pictureAvailable);
                    }
                    else
                        assumeCancelled();
                };
            DoWithInvalidOperationProtection(chooser.Show);
        }

        private void ResizeThenCallOnMainThread(int maxPixelDimension, int percentQuality, Stream input,
                                                Action<Stream> success)
        {
            ResizeJpegStream(maxPixelDimension, percentQuality, input, (stream) => CallAsync(stream, success));
        }

        private void ResizeJpegStream(int maxPixelDimension, int percentQuality, Stream input, Action<Stream> success)
        {
            var bitmap = new BitmapImage();
            bitmap.SetSource(input);
            var writeable = new WriteableBitmap(bitmap);
            var ratio = 1.0;
            if (writeable.PixelWidth > writeable.PixelHeight)
                ratio = (maxPixelDimension)/((double) writeable.PixelWidth);
            else
                ratio = (maxPixelDimension)/((double) writeable.PixelHeight);

            var targetWidth = (int) Math.Round(ratio*writeable.PixelWidth);
            var targetHeight = (int) Math.Round(ratio*writeable.PixelHeight);

            // not - important - we do *not* use using here - disposing of memoryStream is someone else's problem
            var memoryStream = new MemoryStream();
            writeable.SaveJpeg(memoryStream, targetWidth, targetHeight, 0, percentQuality);
            memoryStream.Seek(0L, SeekOrigin.Begin);
            success(memoryStream);
        }

        private void CallAsync(Stream input, Action<Stream> success)
        {
            Dispatcher.RequestMainThreadAction(() => success(input));
        }
        */
    }
}