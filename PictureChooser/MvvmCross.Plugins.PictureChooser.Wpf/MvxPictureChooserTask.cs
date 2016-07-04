using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;


namespace MvvmCross.Plugins.PictureChooser.Wpf
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
            this.ChoosePictureFromLibrary(maxPixelDimension, percentQuality, (stream, name) => pictureAvailable(stream), assumeCancelled);
        }

        public void ChoosePictureFromLibrary(int maxPixelDimension, int percentQuality, Action<Stream, string> pictureAvailable, Action assumeCancelled)
        {
            var filePicker = new OpenFileDialog();
            filePicker.Filter = "Image Files (*.jpg, *.jpeg)|*.jpg;*.jpeg";
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

                        JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                        encoder.QualityLevel = percentQuality;

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
                System.Drawing.Imaging.ImageLockMode.ReadOnly, bitmap.PixelFormat);


            var bitmapSource = BitmapSource.Create(
                 bitmapData.Width, bitmapData.Height, 96, 96, PixelFormats.Bgr24, null,
                 bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride);

            bitmap.UnlockBits(bitmapData);
            return bitmapSource;
        }
    }
}
