// MvxImagePickerTask.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Touch.Platform;
using Cirrious.CrossCore.Touch.Views;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Plugins.PictureChooser.Touch
{
    public class MvxImagePickerTask
        : MvxTouchTask
          , IMvxPictureChooserTask
    {
        private readonly UIImagePickerController _picker;
        private readonly IMvxTouchModalHost _modalHost;
        private bool _currentlyActive;
        private int _maxPixelDimension;
        private int _percentQuality;
        private Action<Stream> _pictureAvailable;
        private Action _assumeCancelled;

        public MvxImagePickerTask()
        {
            _modalHost = Mvx.Resolve<IMvxTouchModalHost>();
            _picker = new UIImagePickerController();
            _picker.FinishedPickingMedia += Picker_FinishedPickingMedia;
            _picker.FinishedPickingImage += Picker_FinishedPickingImage;
            _picker.Canceled += Picker_Canceled;
        }

        public void ChoosePictureFromLibrary(int maxPixelDimension, int percentQuality, Action<Stream> pictureAvailable,
                                             Action assumeCancelled)
        {
            _picker.SourceType = UIImagePickerControllerSourceType.PhotoLibrary;
            ChoosePictureCommon(maxPixelDimension, percentQuality, pictureAvailable, assumeCancelled);
        }

        public void TakePicture(int maxPixelDimension, int percentQuality, Action<Stream> pictureAvailable,
                                Action assumeCancelled)
        {
            _picker.SourceType = UIImagePickerControllerSourceType.Camera;
            ChoosePictureCommon(maxPixelDimension, percentQuality, pictureAvailable, assumeCancelled);
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

        private void ChoosePictureCommon(int maxPixelDimension, int percentQuality,
                                         Action<Stream> pictureAvailable, Action assumeCancelled)
        {
            SetCurrentlyActive();
            _maxPixelDimension = maxPixelDimension;
            _percentQuality = percentQuality;
            _pictureAvailable = pictureAvailable;
            _assumeCancelled = assumeCancelled;

            _modalHost.PresentModalViewController(_picker, true);
        }

        private void HandleImagePick(UIImage image)
        {
            ClearCurrentlyActive();
            if (image != null)
            {
				if (image.Size.Height > _maxPixelDimension || image.Size.Width > _maxPixelDimension)
				{
					// resize the image
					image = image.ImageToFitSize(new SizeF(_maxPixelDimension, _maxPixelDimension));
				}

                using (NSData data = image.AsJPEG((float)(_percentQuality / 100.0)))
                {
                    var byteArray = new byte[data.Length];
                    Marshal.Copy(data.Bytes, byteArray, 0, Convert.ToInt32(data.Length));

                    var imageStream = new MemoryStream();
                    imageStream.Write(byteArray, 0, Convert.ToInt32(data.Length));
                    imageStream.Seek(0, SeekOrigin.Begin);
                    if (_pictureAvailable != null)
                        _pictureAvailable(imageStream);
                }
            }
            else
            {
                if (_assumeCancelled != null)
                    _assumeCancelled();
            }

            _picker.DismissViewController(true, () => { });
            _modalHost.NativeModalViewControllerDisappearedOnItsOwn();
        }

        void Picker_FinishedPickingMedia(object sender, UIImagePickerMediaPickedEventArgs e)
        {
            var image = e.EditedImage ?? e.OriginalImage;
            HandleImagePick(image);

        }

        void Picker_FinishedPickingImage(object sender, UIImagePickerImagePickedEventArgs e)
        {
            var image = e.Image;
            HandleImagePick(image);
        }

        void Picker_Canceled(object sender, EventArgs e)
        {
            ClearCurrentlyActive();
            if (_assumeCancelled != null)
                _assumeCancelled();
            _picker.DismissViewController(true, () => { });
            _modalHost.NativeModalViewControllerDisappearedOnItsOwn();
        }

        private void SetCurrentlyActive()
        {
            if (_currentlyActive)
                Mvx.Warning("MvxImagePickerTask called when task already active");
            _currentlyActive = true;
        }

        private void ClearCurrentlyActive()
        {
            if (!_currentlyActive)
                Mvx.Warning("Tried to clear currently active - but already cleared");
            _currentlyActive = false;
        }

        #region Nested type: CameraDelegate

        private class CameraDelegate : UIImagePickerControllerDelegate
        {
            public Action<UIImage, NSDictionary> Callback { get; set; }

            public override void FinishedPickingImage(UIImagePickerController picker, UIImage image,
                                                      NSDictionary editingInfo)
            {
                if (Callback != null)
                    Callback(image, editingInfo);
            }

            public override void Canceled(UIImagePickerController picker)
            {
                if (Callback != null)
                    Callback(null, null);
            }
        }

        #endregion
    }
}