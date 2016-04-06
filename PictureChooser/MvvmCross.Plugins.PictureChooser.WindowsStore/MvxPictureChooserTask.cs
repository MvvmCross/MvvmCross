// MvxPictureChooserTask.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Core;

namespace MvvmCross.Plugins.PictureChooser.WindowsStore
{
    public class MvxPictureChooserTask : IMvxPictureChooserTask
    {
        public void ChoosePictureFromLibrary(int maxPixelDimension, int percentQuality, Action<Stream, string> pictureAvailable, Action assumeCancelled)
        {
            TakePictureCommon(StorageFileFromDisk, maxPixelDimension, percentQuality, pictureAvailable, assumeCancelled);
        }

        public void ChoosePictureFromLibrary(int maxPixelDimension, int percentQuality, Action<Stream> pictureAvailable, Action assumeCancelled)
        {
            TakePictureCommon(StorageFileFromDisk, maxPixelDimension, percentQuality, (stream, name) => pictureAvailable(stream), assumeCancelled);
        }

        public void TakePicture(int maxPixelDimension, int percentQuality, Action<Stream> pictureAvailable, Action assumeCancelled)
        {
            TakePictureCommon(StorageFileFromCamera, maxPixelDimension, percentQuality, (stream, name) => pictureAvailable(stream), assumeCancelled);
        }

        public Task<Stream> ChoosePictureFromLibrary(int maxPixelDimension, int percentQuality)
        {
            var task = new TaskCompletionSource<Stream>();
            ChoosePictureFromLibrary(maxPixelDimension, percentQuality, task.SetResult, () => task.SetResult(null));
            return task.Task;
        }

        public Task<Stream> TakePicture(int maxPixelDimension, int percentQuality)
        {
            var task = new TaskCompletionSource<Stream>();
            TakePicture(maxPixelDimension, percentQuality, task.SetResult, () => task.SetResult(null));
            return task.Task;
        }

        public void ContinueFileOpenPicker(object args)
        {
        }

        private void TakePictureCommon(Func<Task<StorageFile>> storageFile, int maxPixelDimension, int percentQuality, Action<Stream, string> pictureAvailable,
                                             Action assumeCancelled)
        {
            var dispatcher = Windows.UI.Core.CoreWindow.GetForCurrentThread().Dispatcher;
            dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                                async () =>
                                    {
                                        await
                                            Process(storageFile, maxPixelDimension, percentQuality, pictureAvailable,
                                                    assumeCancelled);
                                    });
        }

        private async Task Process(Func<Task<StorageFile>> storageFile, int maxPixelDimension, int percentQuality, Action<Stream, string> pictureAvailable, Action assumeCancelled)
        {
            var file = await storageFile();
            if (file == null)
            {
                assumeCancelled();
                return;
            }

            var rawFileStream = await file.OpenAsync(FileAccessMode.Read);
            var resizedStream = await ResizeJpegStreamAsync(maxPixelDimension, percentQuality, rawFileStream);

            pictureAvailable(resizedStream.AsStreamForRead(), file.DisplayName);
        }

        private static async Task<StorageFile> StorageFileFromCamera()
        {
            var dialog = new CameraCaptureUI();
            var file = await dialog.CaptureFileAsync(CameraCaptureUIMode.Photo);
            return file;
        }

        private static async Task<StorageFile> StorageFileFromDisk()
        {
            var filePicker = new FileOpenPicker();
            filePicker.FileTypeFilter.Add(".jpg");
            filePicker.FileTypeFilter.Add(".jpeg");
            filePicker.ViewMode = PickerViewMode.Thumbnail;
            filePicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            //filePicker.SettingsIdentifier = "picker1";
            //filePicker.CommitButtonText = "Open";

            return await filePicker.PickSingleFileAsync();
        }

        private async Task<IRandomAccessStream> ResizeJpegStreamAsyncRubbish(int maxPixelDimension, int percentQuality, IRandomAccessStream input)
        {
            BitmapDecoder decoder = await BitmapDecoder.CreateAsync(input);

            // create a new stream and encoder for the new image
            var ras = new InMemoryRandomAccessStream();
            var enc = await BitmapEncoder.CreateForTranscodingAsync(ras, decoder);

            int targetHeight;
            int targetWidth;
            MvxPictureDimensionHelper.TargetWidthAndHeight(maxPixelDimension, (int)decoder.PixelWidth, (int)decoder.PixelHeight, out targetWidth, out targetHeight);

            enc.BitmapTransform.ScaledHeight = (uint)targetHeight;
            enc.BitmapTransform.ScaledWidth = (uint)targetWidth;

            // write out to the stream
            await enc.FlushAsync();

            return ras;
        }

        private async Task<IRandomAccessStream> ResizeJpegStreamAsync(int maxPixelDimension, int percentQuality, IRandomAccessStream input)
        {
            var decoder = await BitmapDecoder.CreateAsync(input);

            int targetHeight;
            int targetWidth;
            MvxPictureDimensionHelper.TargetWidthAndHeight(maxPixelDimension, (int)decoder.PixelWidth, (int)decoder.PixelHeight, out targetWidth, out targetHeight);

            var transform = new BitmapTransform() { ScaledHeight = (uint)targetHeight, ScaledWidth = (uint)targetWidth };
            var pixelData = await decoder.GetPixelDataAsync(
                BitmapPixelFormat.Rgba8,
                BitmapAlphaMode.Straight,
                transform,
                ExifOrientationMode.RespectExifOrientation,
                ColorManagementMode.DoNotColorManage);

            var destinationStream = new InMemoryRandomAccessStream();
            var bitmapPropertiesSet = new BitmapPropertySet();
            bitmapPropertiesSet.Add("ImageQuality", new BitmapTypedValue(((double)percentQuality) / 100.0, PropertyType.Single));
            var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, destinationStream, bitmapPropertiesSet);
            encoder.SetPixelData(BitmapPixelFormat.Rgba8, BitmapAlphaMode.Premultiplied, (uint)targetWidth, (uint)targetHeight, decoder.DpiX, decoder.DpiY, pixelData.DetachPixelData());
            await encoder.FlushAsync();
            destinationStream.Seek(0L);
            return destinationStream;
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

        #endregion IMvxCombinedPictureChooserTask Members

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

        #endregion IMvxPictureChooserTask Members

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