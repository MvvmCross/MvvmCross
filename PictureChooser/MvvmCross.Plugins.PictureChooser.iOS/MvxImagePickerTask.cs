// MvxImagePickerTask.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using MvvmCross.Platform;
using MvvmCross.Platform.iOS.Platform;
using MvvmCross.Platform.iOS.Views;
using CoreGraphics;
using Foundation;
using UIKit;

namespace MvvmCross.Plugins.PictureChooser.iOS
{
    public class MvxImagePickerTask
        : MvxIosTask
          , IMvxPictureChooserTask
    {
        private readonly UIImagePickerController _picker;
        private readonly IMvxIosModalHost _modalHost;
        private bool _currentlyActive;
        private int _maxPixelDimension;
        private int _percentQuality;
        private Action<Stream, string> _pictureAvailable;
        private Action _assumeCancelled;

        public MvxImagePickerTask()
        {
            _modalHost = Mvx.Resolve<IMvxIosModalHost>();
            _picker = new UIImagePickerController
            {
                //CameraCaptureMode = UIImagePickerControllerCameraCaptureMode.Photo,
                //CameraDevice = UIImagePickerControllerCameraDevice.Front
            };
            _picker.FinishedPickingMedia += Picker_FinishedPickingMedia;
            _picker.FinishedPickingImage += Picker_FinishedPickingImage;
            _picker.Canceled += Picker_Canceled;
        }

        public void ChoosePictureFromLibrary(int maxPixelDimension, int percentQuality, Action<Stream, string> pictureAvailable,
                                     Action assumeCancelled)
        {
            _picker.SourceType = UIImagePickerControllerSourceType.PhotoLibrary;
            ChoosePictureCommon(maxPixelDimension, percentQuality, pictureAvailable, assumeCancelled);
        }

        public void ChoosePictureFromLibrary(int maxPixelDimension, int percentQuality, Action<Stream> pictureAvailable,
                                             Action assumeCancelled)
        {
            this.ChoosePictureFromLibrary(maxPixelDimension, percentQuality, (stream, name) => pictureAvailable(stream), assumeCancelled);
        }

        public void TakePicture(int maxPixelDimension, int percentQuality, Action<Stream> pictureAvailable,
                                Action assumeCancelled)
        {
            _picker.SourceType = UIImagePickerControllerSourceType.Camera;
            ChoosePictureCommon(maxPixelDimension, percentQuality, (stream, name) => pictureAvailable(stream), assumeCancelled);
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

        private void ChoosePictureCommon(int maxPixelDimension, int percentQuality,
                                         Action<Stream, string> pictureAvailable, Action assumeCancelled)
        {
            SetCurrentlyActive();
            _maxPixelDimension = maxPixelDimension;
            _percentQuality = percentQuality;
            _pictureAvailable = pictureAvailable;
            _assumeCancelled = assumeCancelled;

            _modalHost.PresentModalViewController(_picker, true);
        }

        private void HandleImagePick(UIImage image, string name)
        {
            ClearCurrentlyActive();
            if (image != null)
            {
                if (_maxPixelDimension > 0 && (image.Size.Height > _maxPixelDimension || image.Size.Width > _maxPixelDimension))
                {
                    // resize the image
                    image = image.ImageToFitSize(new CGSize(_maxPixelDimension, _maxPixelDimension));
                }

                using (NSData data = image.AsJPEG(_percentQuality / 100f))
                {
                    var byteArray = new byte[data.Length];
                    Marshal.Copy(data.Bytes, byteArray, 0, Convert.ToInt32(data.Length));

                    var imageStream = new MemoryStream(byteArray, false);
                    _pictureAvailable?.Invoke(imageStream, name);
                }
            }
            else
            {
                _assumeCancelled?.Invoke();
            }

            _picker.DismissViewController(true, () => { });
            _modalHost.NativeModalViewControllerDisappearedOnItsOwn();
        }

        private void Picker_FinishedPickingMedia(object sender, UIImagePickerMediaPickedEventArgs e)
        {
            NSUrl referenceURL = e.Info[new NSString("UIImagePickerControllerReferenceURL")] as NSUrl;
            var image = e.EditedImage ?? e.OriginalImage;
            HandleImagePick(image, referenceURL != null ? referenceURL.AbsoluteString : string.Empty);
        }

        private void Picker_FinishedPickingImage(object sender, UIImagePickerImagePickedEventArgs e)
        {
            NSUrl referenceURL = e.EditingInfo["UIImagePickerControllerReferenceURL"] as NSUrl;
            var image = e.Image;
            HandleImagePick(image, referenceURL != null ? referenceURL.AbsoluteString : string.Empty);
        }

        private void Picker_Canceled(object sender, EventArgs e)
        {
            ClearCurrentlyActive();
            _assumeCancelled?.Invoke();
            _picker.DismissViewController(true, () => { });
            _picker.Delegate = null;
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
    }
}