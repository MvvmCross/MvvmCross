#region Copyright

// <copyright file="MvxPictureChooserTask.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using System;
using System.IO;
using System.Windows.Media.Imaging;
using Cirrious.MvvmCross.WindowsPhone.Platform.Tasks;
using Microsoft.Phone.Tasks;

namespace Cirrious.MvvmCross.Plugins.PictureChooser.WindowsPhone
{
    public class MvxPictureChooserTask : MvxWindowsPhoneTask, IMvxPictureChooserTask, IMvxCombinedPictureChooserTask
    {
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
            ViewDispatcher.RequestMainThreadAction(() => success(input));
        }
    }
}