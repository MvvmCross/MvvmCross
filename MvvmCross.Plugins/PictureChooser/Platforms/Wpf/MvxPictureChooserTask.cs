// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using Color = System.Windows.Media.Color;

namespace MvvmCross.Plugin.PictureChooser.Platforms.Wpf
{
    public class MvxPictureChooserTask : IMvxPictureChooserTask
    {
        public Task<Stream> ChoosePictureFromLibrary(int maxPixelDimension, int percentQuality)
        {
            var task = new TaskCompletionSource<Stream>();
            ChoosePictureFromLibrary(maxPixelDimension, percentQuality, task.SetResult, () => task.SetResult(null));
            return task.Task;
        }

        public void ChoosePictureFromLibrary(int maxPixelDimension, int percentQuality, Action<Stream> pictureAvailable, Action assumeCancelled)
        {
            ChoosePictureFromLibrary(maxPixelDimension, percentQuality, (stream, name) => pictureAvailable(stream), assumeCancelled);
        }

        public void ChoosePictureFromLibrary(int maxPixelDimension, int percentQuality, Action<Stream, string> pictureAvailable, Action assumeCancelled)
        {
            var filePicker = new OpenFileDialog();
            filePicker.Filter = "Image Files (*.jpg,*.jpeg,*.gif,*.png)|*.jpg;*.jpeg;*.gif;*.png";
            filePicker.Multiselect = false;

            if (filePicker.ShowDialog() == true)
            {
                try
                {
                    var bm = new Bitmap(filePicker.FileName);
                    if (bm != null)
                    {
                        int targetWidth;
                        int targetHeight;

                        MvxPictureDimensionHelper.TargetWidthAndHeight(maxPixelDimension, bm.Width, bm.Height, out targetWidth, out targetHeight);
                        var transformBM = new TransformedBitmap(ConvertBitmapInBitmapSource(bm), new ScaleTransform(targetWidth / (double)bm.Width, targetHeight / (double)bm.Height));

                        var encoder = GetBitmapEncoder(filePicker.FileName, percentQuality);
                        if (encoder == null)
                            throw new NotSupportedException("The image file is invalid. Please, select a jpg, jpeg, gif, png or tiff file.");

                        MemoryStream stream = new MemoryStream();
                        encoder.Frames.Add(BitmapFrame.Create(transformBM));
                        encoder.Save(stream);

                        stream.Position = 0;

                        pictureAvailable(stream, filePicker.FileName);
                    }
                }
                catch (ArgumentException)
                {
                    assumeCancelled();
                }
            }
            else
            {
                assumeCancelled();
            }
        }

        /// <summary>
        /// Get valid bitmap encoder from file extension.
        /// </summary>
        /// <param name="fileName">File name or file path, with extension</param>
        /// <param name="percentQuality">quality level (for jpg files only)</param>
        /// <returns></returns>
        private BitmapEncoder GetBitmapEncoder(string fileName, int percentQuality)
        {
            var lowerName = Path.GetExtension(fileName.ToLower());
            if (lowerName.Equals(".jpg") || lowerName.Equals(".jpeg"))
            {
                return new JpegBitmapEncoder()
                {
                    QualityLevel = percentQuality
                };
            }
            else if (lowerName.Equals(".png"))
            {
                return new PngBitmapEncoder();
            }
            else if (lowerName.Equals(".gif"))
            {
                return new GifBitmapEncoder();
            }
            else if (lowerName.Equals(".tiff"))
            {
                return new TiffBitmapEncoder();
            }

            return null;
        }

        public void ContinueFileOpenPicker(object args)
        {
            throw new NotImplementedException();
        }

        public Task<Stream> TakePicture(int maxPixelDimension, int percentQuality)
        {
            throw new NotImplementedException();
        }

        public void TakePicture(int maxPixelDimension, int percentQuality, Action<Stream> pictureAvailable, Action assumeCancelled)
        {
            throw new NotImplementedException();
        }

        private BitmapSource ConvertBitmapInBitmapSource(Bitmap bitmap)
        {
            var bitmapData = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly, bitmap.PixelFormat);

            BitmapPalette palette = null;
            Color[] pal = null;

            if (bitmap.Palette != null && bitmap.Palette.Entries != null && bitmap.Palette.Entries.Length > 0)
            {
                pal = bitmap.Palette.Entries.Select(e => Color.FromArgb(e.A, e.R, e.G, e.B)).ToArray();
                palette = new BitmapPalette(pal);
            }

            var bitmapSource = BitmapSource.Create(
                 bitmapData.Width, bitmapData.Height, 96, 96, bitmap.PixelFormat.Convert(), palette,
                 bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride);
            bitmap.UnlockBits(bitmapData);

            return bitmapSource;
        }
    }
}
